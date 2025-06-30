using FileWatcherService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddWindowsService();
var handler = new HttpClientHandler
{
    // Credentials = CredentialCache.DefaultNetworkCredentials,
    //PreAuthenticate = true
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
};
builder.Services.AddTransient<HttpClient>((sp) => new HttpClient(handler));

var host = builder.Build();

host.Run();
