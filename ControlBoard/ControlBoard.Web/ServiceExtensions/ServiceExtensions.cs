using ControlBoard.DB;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.DB.Repositories.Concrete;
using ControlBoard.Domain.ChartConverts;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
using ControlBoard.Web.Auth;
using ControlBoard.Web.AutoMapperProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ControlBoard.Web.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAllControlBoardServices(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<CookieAuthenticationOptions>(o =>
            {
                o.LoginPath = PathString.Empty;
            });

            services.AddDbContext<MesDbContext>(ctx =>
            {
                ctx.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies();
            });

            services.AddDbContext<AppDbContext>(ctx =>
            {
                ctx.UseNpgsql(config.GetConnectionString("AuthConnection"))
                    .UseLazyLoadingProxies();
            });

            services.AddIdentityApiEndpoints<ApplicationUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppDbContext>();


            services.AddScoped<IProcessStateRepository, ProcessStateRepository>();
            services.AddTransient<IProcessStateService, ProcessStateService>();
            services.AddScoped<StationRepository>();
            services.AddTransient<IStationService, StationService>();
            services.AddScoped<BoardConstructorRepository>();
            services.AddTransient<IBoardConstructorService, BoardConstructorService>();
            services.AddScoped<AreaRepository>();
            services.AddTransient<IAreaService, AreaService>();
            services.AddTransient<IProcessStateAdvService, ProcessStateAdvService>();
            services.AddTransient<IChartConvertService, ChartConvertService>();
            services.AddScoped<ProductTypeRepository>();
            services.AddTransient<IProductTypeService, ProductTypeService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<ICarExecutionService, CarExecutionService>();
            services.AddTransient<IChartServices, ChartServices>();
            services.AddTransient<DataChartConverter>();
            services.AddTransient<DisabledChartConverter>();

            services.AddProblemDetails();

            services.AddAutoMapper(configAction =>
            {
                configAction.AddProfile<StationMapperProfile>();
                configAction.AddProfile<AreaMapperProfile>();
                configAction.AddProfile<ProductTypeMapperProfile>();
                configAction.AddProfile<ProcessStateAdvMapper>();
                configAction.AddProfile<SpecificationProfile>();
                configAction.AddProfile<CarExecutionProfile>();
                configAction.AddProfile<ProcessStateMapperProfile>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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
            services.AddAuthorization();


            services.AddCors(options =>
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
            services.AddSignalR();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
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
            return services;
        }
    }
}
