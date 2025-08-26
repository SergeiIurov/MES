namespace ControlBoard.DB.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        //Диапазон допустимых значений ID для станций(для свойства ChartElementId)
        public string Range { get; set; }
        public bool IsDisabled { get; set; }
        public string DisabledColor { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
    }
}
