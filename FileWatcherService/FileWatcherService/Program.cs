using FileWatcherService;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddWindowsService();
var handler = new HttpClientHandler
{
    // Credentials = CredentialCache.DefaultNetworkCredentials,
    //PreAuthenticate = true
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
};

builder.Logging.ClearProviders();
builder.UseNLog();

builder.Services.AddTransient<HttpClient>((sp) => new HttpClient(handler));

var host = builder.Build();

host.Run();
