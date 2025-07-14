using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.DB.Repositories.Concrete;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
using ControlBoard.Web;
using ControlBoard.Web.AutoMapperProfiles;
using ControlBoard.Web.Data;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//    .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = options.DefaultPolicy;
//});

builder.Services.AddDbContext<MesDbContext>(ctx =>
{
    ctx.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies();

});

builder.Services.AddScoped<IProcessStateRepository, ProcessStateRepository>();
builder.Services.AddTransient<IProcessStateService, ProcessStateService>();
builder.Services.AddScoped<StationRepository>();
builder.Services.AddTransient<IStationService, StationService>();
builder.Services.AddScoped<BoardConstructorRepository>();
builder.Services.AddTransient<IBoardConstructorService, BoardConstructorService>();
builder.Services.AddScoped<AreaRepository>();
builder.Services.AddTransient<IAreaService, AreaService>();


builder.Services.AddAutoMapper(configAction =>
{
    configAction.AddProfile<StationMapperProfile>();
    configAction.AddProfile<AreaMapperProfile>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()//.WithOrigins("http://localhost:4200").AllowCredentials()
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
});
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();



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

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MesHub>("/api/mes-hub");

app.MapFallbackToFile("/index.html");
//TestData.LoadData(app.Services);

app.Run();

