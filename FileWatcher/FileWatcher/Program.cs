using Microsoft.Extensions.Configuration;

namespace FileWatcher;

public class Program
{
    private static IConfiguration? _config;
    public static async Task SendPictureAsync()
    {
        try
        {
            //var handler = new HttpClientHandler
            //{
            //    Credentials = CredentialCache.DefaultNetworkCredentials,
            //    PreAuthenticate = true
            //};
            HttpClient client = new(/*handler*/);

            string fileName = $"{_config?["directoryName"]}{_config?["filename"]}";
            if (!File.Exists(fileName))
            {
                return;
            }

            string serverAddress = _config?["url"] ?? string.Empty;
            await Task.Delay(1000);
            using StreamContent c = new(File.Open(fileName, FileMode.Open));
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
            MultipartFormDataContent content = new() { { c, "file", fileName } };
            await client.PostAsync(serverAddress, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public static Task Main()
    {
        _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        bool isSending = false;
        FileSystemWatcher watcher = new(_config["directoryName"] ?? string.Empty, "*.jpg");

        watcher.Changed += async (o, e) =>
        {
            isSending = !isSending;
            if (isSending)
            {

                await SendPictureAsync();
                Console.WriteLine("Обновление");

            }
        };

        watcher.Created += async (o, e) =>
        {
            isSending = true;
            await SendPictureAsync();
            Console.WriteLine("Создан");
        };

        watcher.EnableRaisingEvents = true;
        Console.ReadLine();
        return Task.CompletedTask;
    }

}