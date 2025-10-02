namespace ControlBoard.Domain.Dto
{
    /// <summary>
    /// Точка сканирования
    /// </summary>
    public class ScanningPointDto
    {
        public int Id { get; set; }
        //Наименование ТС(точки сканирования)
        public string Name { get; set; }
        //Порядковый номер
        public string OrderNum { get; set; }
        //Номер линии
        public int LineNumber { get; set; }
        //Код ТС(точки сканирования)
        public string CodeTs { get; set; }
    }
}
