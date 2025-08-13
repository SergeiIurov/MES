using ControlBoard.DB.Entities.Enums;

namespace ControlBoard.Domain.Dto
{
    public class StationDto
    {
        public int Id { get; set; }
        public int ChartElementId { get; set; }
        public string? Name { get; set; }
        public int AreaId { get; set; }
        public string? AreaName { get; set; }
        public ProductTypes? ProductType { get; set; }
    }
}
