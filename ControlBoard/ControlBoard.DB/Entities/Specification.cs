namespace ControlBoard.DB.Entities
{
    public class Specification
    {
        public int Id { get; set; }
        public string SequenceNumber { get; set; }
        public string VinNumber { get; set; }
        public string? SpecificationStr { get; set; }
        public string? ChassisAssemblyStartDate { get; set; }
        public string? DateInstallationCabinOnСhassis { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
