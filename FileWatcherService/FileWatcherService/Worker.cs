using System.Text;
using System.Text.Json;

namespace FileWatcherService
{
    public class Worker(ILogger<Worker> logger, HttpClient httpClient, IConfiguration config)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WatchJpgFiles();
            //WatchExcelFiles();
        }

        private void WatchJpgFiles()
        {
            bool isSending = false;
            FileSystemWatcher jpgWatcher = new(config["jpegDirectoryName"] ?? string.Empty, "*.jpg");
            jpgWatcher.Changed += async (o, e) =>
            {
                isSending = !isSending;
                if (isSending)
                {
                    logger.LogInformation("Зафиксировано обновление файла-скриншота JPG.");
                    await SendPictureAsync();
                    await SendDataAsync();
                }
            };

            jpgWatcher.Created += async (o, e) =>
            {
                isSending = true;
                logger.LogInformation("Зафиксировано создание файла-скриншота JPG.");
                await SendPictureAsync();
                await SendDataAsync();
            };

            jpgWatcher.EnableRaisingEvents = true;
        }

        private async Task SendPictureAsync()
        {
            try
            {
                string fileName = $"{config?["jpegDirectoryName"]}{config?["jpgFileName"]}";
                if (!File.Exists(fileName))
                {
                    logger.LogInformation($"Не найден файл скриншота '{fileName}' для отправки на сервер");
                    return;
                }
                string serverAddress = config?["url"] ?? string.Empty;
                await Task.Delay(2000);
                using StreamContent c = new(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None));
                c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                MultipartFormDataContent content = new() { { c, "file", fileName } };
                logger.LogInformation("файл скриншота подготовлен к отправке.");
                await httpClient.PostAsync(serverAddress, content);
                logger.LogInformation("файл скриншота отправлен.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        private async Task SendDataAsync()
        {
            try
            {
                List<ProcessStateDto> list = new List<ProcessStateDto>();

                string fileName = $"{config?["csvDirectoryName"]}{config?["csvFileName"]}";
                if (!File.Exists(fileName))
                {
                    logger.LogInformation($"Не найден файл CSV '{fileName}' для отправки на сервер.");
                    return;
                }

                string serverAddress = $"{config?["url"]}/csv-data";
                await Task.Delay(2000);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                foreach (var line in File.ReadLines(fileName, Encoding.GetEncoding(1251)))
                {
                    string[] str = line.Split(';');
                    list.Add(new()
                    {
                        StationName = str[0],
                        Value = str[1],
                        ProductTypeName = str[2]
                    });
                }

                string s = JsonSerializer.Serialize(list);
                StringContent content = new StringContent(s, Encoding.UTF8, "application/json");
                logger.LogInformation("файл CSV подготовлен к отправке.");
                await httpClient.PostAsync(serverAddress, content);
                logger.LogInformation("файл CSV отправлен.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }


    }
}
