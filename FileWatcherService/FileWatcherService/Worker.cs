using System.Text;
using System.Text.Json;
using ClosedXML.Excel;
using ControlBoard.Domain.Dto;

namespace FileWatcherService
{
    public class Worker(ILogger<Worker> logger, HttpClient httpClient, IConfiguration config)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WatchJpgFiles();
            WatchExcelFiles();

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

                    await SendPictureAsync();
                    logger.LogInformation("Обновление");

                }
            };

            jpgWatcher.Created += async (o, e) =>
            {
                isSending = true;
                await SendPictureAsync();
                Console.WriteLine("Создан");
            };

            jpgWatcher.EnableRaisingEvents = true;
        }

        private void WatchExcelFiles()
        {
            bool isSending = false;
            FileSystemWatcher excelWatcher = new(config["excelDirectoryName"] ?? string.Empty, "*.csv");

            excelWatcher.Changed += async (o, e) =>
            {
                isSending = !isSending;
                if (isSending)
                {

                    await SendDataAsync();
                    logger.LogInformation("Обновление");

                }
            };

            excelWatcher.Created += async (o, e) =>
            {
                isSending = true;
                await SendDataAsync();
                Console.WriteLine("Создан");
            };

            excelWatcher.EnableRaisingEvents = true;
        }

        private async Task SendPictureAsync()
        {
            try
            {
                string fileName = $"{config?["jpegDirectoryName"]}{config?["jpgFileName"]}";
                if (!File.Exists(fileName))
                {
                    return;
                }

                string serverAddress = config?["url"] ?? string.Empty;
                await Task.Delay(2000);
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

        private async Task SendDataAsync()
        {
            try
            {
                List<ProcessStateDto> list = new List<ProcessStateDto>();

                string fileName = $"{config?["excelDirectoryName"]}{config?["excelFileName"]}";
                if (!File.Exists(fileName))
                {
                    return;
                }
                string serverAddress = $"{config?["url"]}/excel-data";
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
                await httpClient.PostAsync(serverAddress, content);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }


    }
}
