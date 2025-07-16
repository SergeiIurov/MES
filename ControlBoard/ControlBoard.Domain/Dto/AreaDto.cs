namespace ControlBoard.Domain.Dto
{
    public class AreaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StationDto> Stations { get; set; }
    }
}