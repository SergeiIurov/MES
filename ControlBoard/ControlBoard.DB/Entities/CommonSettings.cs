namespace ControlBoard.DB.Entities
{
    /// <summary>
    /// Общие настройки MES системы
    /// </summary>
    public class CommonSettings
    {
        public int Id { get; set; }
        public int LineCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}