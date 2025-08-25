using ControlBoard.DB;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.DB.Repositories.Concrete;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
using ControlBoard.Web;
using ControlBoard.Web.AutoMapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;
using ControlBoard.Domain.ChartConverts;
using ControlBoard.Web.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookieAuthenticationOptions>(o =>
{
    o.LoginPath = PathString.Empty;
});

builder.Services.AddDbContext<MesDbContext>(ctx =>
{
    ctx.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies();
});

builder.Services.AddDbContext<AppDbContext>(ctx =>
{
    ctx.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection"))
        .UseLazyLoadingProxies();
});

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 4;
    o.Password.RequireUppercase = false;
    o.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddScoped<IProcessStateRepository, ProcessStateRepository>();
builder.Services.AddTransient<IProcessStateService, ProcessStateService>();
builder.Services.AddScoped<StationRepository>();
builder.Services.AddTransient<IStationService, StationService>();
builder.Services.AddScoped<BoardConstructorRepository>();
builder.Services.AddTransient<IBoardConstructorService, BoardConstructorService>();
builder.Services.AddScoped<AreaRepository>();
builder.Services.AddTransient<IAreaService, AreaService>();
builder.Services.AddTransient<IProcessStateAdvService, ProcessStateAdvService>();
builder.Services.AddTransient<IChartConvertService, ChartConvertService>();
builder.Services.AddScoped<ProductTypeRepository>();
builder.Services.AddTransient<IProductTypeService, ProductTypeService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();
builder.Services.AddTransient<ICarExecutionService, CarExecutionService>();
builder.Services.AddTransient<IChartServices, ChartServices>();
builder.Services.AddTransient<DataChartConverter>();
builder.Services.AddTransient<DisabledChartConverter>();

builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(configAction =>
{
    configAction.AddProfile<StationMapperProfile>();
    configAction.AddProfile<AreaMapperProfile>();
    configAction.AddProfile<ProductTypeMapperProfile>();
    configAction.AddProfile<ProcessStateAdvMapper>();
    configAction.AddProfile<SpecificationProfile>();
    configAction.AddProfile<CarExecutionProfile>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // указывает, будет ли валидироваться издатель при валидации токена
        ValidateIssuer = true,
        // строка, представляющая издателя
        ValidIssuer = AuthOptions.Issuer,
        // будет ли валидироваться потребитель токена
        ValidateAudience = true,
        // установка потребителя токена
        ValidAudience = AuthOptions.Audience,
        // будет ли валидироваться время существования
        ValidateLifetime = true,
        // установка ключа безопасности
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        // валидация ключа безопасности
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod() //.WithOrigins("http://localhost:4200").AllowCredentials()
                .AllowAnyHeader();
        });
});
//builder.Services.AddHttpLogging(opt => opt.LoggingFields = HttpLoggingFields.Request);
builder.Services.AddSignalR();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MES API",
        Description = "An ASP.NET Core Web API for managing MES App",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();
app.MapIdentityApi<ApplicationUser>();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

    //app.UseHttpLogging();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MesHub>("/api/mes-hub");

app.MapFallbackToFile("/index.html");
//TestData.LoadData(app.Services);

app.Run();

public class AuthOptions
{
    public const string Issuer = "MyAuthServer"; // издатель токена
    public const string Audience = "MyAuthClient"; // потребитель токена
    private const string Key = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}