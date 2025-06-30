using ControlBoard.DB;
using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.Web.Data
{
    static public class TestData
    {
        public static void LoadData(IServiceProvider appServices)
        {
            using (IServiceScope scope = appServices.CreateScope())
            {
                MesDbContext context = scope.ServiceProvider.GetRequiredService<MesDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Areas.Any())
                {
                    context.Areas.AddRange(new List<Area>()
            {
                new()
                {
                    Name = "Сварка", Created = DateTime.Now, LastUpdated = DateTime.Now, Stations = new List<Station>()
                    {
                        new(){ Name = "Сварка 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                        new(){ Name = "Сварка 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                        new(){ Name = "Сварка 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                        new(){ Name = "Сварка 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                        new(){ Name = "Сварка 5", Created = DateTime.Now, LastUpdated = DateTime.Now },
                        new(){ Name = "Сварка 6", Created = DateTime.Now, LastUpdated = DateTime.Now }
                    }
                },
                new() { Name = "Окраска", Created = DateTime.Now, LastUpdated = DateTime.Now , Stations = new List<Station>()
                {
                    new(){ Name = "Ожидание маскировки", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Маскировка каркаса", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Ожидание пескоструя 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Ожидание пескоструя 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Пескоструйная обработка 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Пескоструйная обработка 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Ожидание окраски 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Ожидание окраски 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Окраска каркаса", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Буфер после окраски 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Буфер после окраски 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 5", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 6", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 7", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 8", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 9", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 10", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 11", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 12", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 13", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 14", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 15", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 16", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Герметик, пластик 17", Created = DateTime.Now, LastUpdated = DateTime.Now }
                }},
                new() { Name = "Сборка кабин", Created = DateTime.Now, LastUpdated = DateTime.Now , Stations = new List<Station>()
                {
                    new(){ Name = "Сборка кабин 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 5", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 6", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 7", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Сборка кабин 8", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Буфер кабин 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Буфер кабин 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Проверка кабины ОТК", Created = DateTime.Now, LastUpdated = DateTime.Now },

                }},
                new() { Name = "Сборка шасси", Created = DateTime.Now, LastUpdated = DateTime.Now , Stations = new List<Station>()
                {
                    new(){ Name = "Шасси 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 5", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 6", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 7", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 8", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Окраска рамы 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Окраска рамы 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Окраска рамы 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Окраска рамы 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 9", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 10", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 11", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 12", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 13", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 14", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 15", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Шасси 16", Created = DateTime.Now, LastUpdated = DateTime.Now }

                }},
                new() { Name = "Оси", Created = DateTime.Now, LastUpdated = DateTime.Now, Stations = new List<Station>()
                    {
                    new(){ Name = "Подсборка осей 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Подсборка осей 2", Created = DateTime.Now, LastUpdated = DateTime.Now }

            }},
                new() { Name = "Качество", Created = DateTime.Now, LastUpdated = DateTime.Now, Stations = new List<Station>()
                {
                    new(){ Name = "Тормозной стенд", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Настройка схождения", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Инспекция", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Аудит", Created = DateTime.Now, LastUpdated = DateTime.Now }

                }},
                new() { Name = "Ремзона", Created = DateTime.Now, LastUpdated = DateTime.Now, Stations = new List<Station>()
                {
                    new(){ Name = "Установка надстройки", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Участок ремонта 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Участок ремонта 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Участок ремонта 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Участок ремонта 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Ремонт окраски 1", Created = DateTime.Now, LastUpdated = DateTime.Now }
                  }},
                new() { Name = "Автомобили на улице", Created = DateTime.Now, LastUpdated = DateTime.Now, Stations = new List<Station>()
                {
                    new(){ Name = "Улица 1", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 2", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 3", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 4", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 5", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 6", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 7", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 8", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 9", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 10", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 11", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 12", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 13", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 14", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 15", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 16", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 17", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 18", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 19", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 20", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 21", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 22", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 23", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 24", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 25", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 26", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 27", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 28", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 29", Created = DateTime.Now, LastUpdated = DateTime.Now },
                    new(){ Name = "Улица 30", Created = DateTime.Now, LastUpdated = DateTime.Now },
                } }
            });
                }

                if (!context.ProductTypes.Any())
                {
                    context.ProductTypes.AddRange(new List<ProductType>()
            {
                new() { Name = "ДКС", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new() { Name = "ККС", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new() { Name = "ДКВ", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new() { Name = "ККН", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new() { Name = "Тягач", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new() { Name = "Самосвал", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new() { Name = "Шасси", Created = DateTime.Now, LastUpdated = DateTime.Now },
            });
                }

                context.SaveChanges();

            }
        }
    }
}
