namespace ControlBoard.Domain.Dto
{
    public class SpecificationDto
    {
        public int Id { get; set; }
        public string SequenceNumber { get; set; }
        public string VinNumber { get; set; }
        public string? SpecificationStr { get; set; }
        public string? ChassisAssemblyStartDate { get; set; }
        public string? DateInstallationCabin { get; set; }
    }
}
