using ControlBoard.Web;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using ControlBoard.Web.Auth;
using ControlBoard.Web.ServiceExtensions;

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
    public const string Issuer = "MyAuthServer"; // издатель токена
    public const string Audience = "MyAuthClient"; // потребитель токена
    private const string Key = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}