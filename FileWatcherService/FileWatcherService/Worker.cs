using System.Text;
using System.Text.Json;
using ControlBoard.Domain.Dto;
using OfficeOpenXml;

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
            FileSystemWatcher excelWatcher = new(config["excelDirectoryName"] ?? string.Empty, "*.xlsx");

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
                ExcelPackage.License.SetNonCommercialPersonal("grey");
                ExcelPackage package = new ExcelPackage(new FileInfo(fileName));

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                for (var row = 1; row <= worksheet.Rows.Count(); row++)
                {
                    list.Add(new()
                    {
                        StationName = worksheet.Cells[row, 1].Value.ToString(),
                        Value = worksheet.Cells[row, 2].Value.ToString(),
                        ProductTypeName = worksheet.Cells[row, 3].Value.ToString()
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
