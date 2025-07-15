namespace ControlBoard.Domain.Dto
{
    public class AreaDto
    {
        public string Name { get; set; }
        public IEnumerable<StationDto> Stations { get; set; }
    }
}