namespace FileWatcherService
{
    public class Worker(ILogger<Worker> logger, HttpClient httpClient, IConfiguration config)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (logger.IsEnabled(LogLevel.Information))
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            bool isSending = false;
            FileSystemWatcher watcher = new(config["directoryName"] ?? string.Empty, "*.jpg");

            watcher.Changed += async (o, e) =>
            {
                isSending = !isSending;
                if (isSending)
                {

                    await SendPictureAsync();
                    logger.LogInformation("Обновление");

                }
            };

            watcher.Created += async (o, e) =>
            {
                isSending = true;
                await SendPictureAsync();
                Console.WriteLine("Создан");
            };

            watcher.EnableRaisingEvents = true;


        }

        private async Task SendPictureAsync()
        {
            try
            {
                string fileName = $"{config?["directoryName"]}{config?["filename"]}";
                if (!File.Exists(fileName))
                {
                    return;
                }

                string serverAddress = config?["url"] ?? string.Empty;
                await Task.Delay(1000);
                using StreamContent c = new(File.Open(fileName, FileMode.Open));
                c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                MultipartFormDataContent content = new() { { c, "file", fileName } };
                await httpClient.PostAsync(serverAddress, content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
