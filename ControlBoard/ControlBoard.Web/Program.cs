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
using ControlBoard.Web.ServiceExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllControlBoardServices(builder.Configuration);

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
    public const string Issuer = "MyAuthServer"; // �������� ������
    public const string Audience = "MyAuthClient"; // ����������� ������
    private const string Key = "mysupersecret_secretsecretsecretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}