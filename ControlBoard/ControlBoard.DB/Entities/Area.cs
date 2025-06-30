namespace ControlBoard.DB.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Station> Stations { get; set; }
    }
}
