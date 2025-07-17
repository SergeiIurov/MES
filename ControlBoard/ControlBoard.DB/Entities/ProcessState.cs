namespace ControlBoard.DB.Entities
{
    public class ProcessState
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public int? StationId { get; set; }
        public int? ProductTypeId { get; set; }
        public Guid GroupId { get; set; }
        public virtual Station Station { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}