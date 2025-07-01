namespace ControlBoard.Domain.Dto
{
    public class ProcessStateDto
    {
        public string? StationName { get; set; }
        private string? _value;

        public string? Value
        {
            get => _value;

            set => _value = value?.PadLeft(3, '0');
        }
        public string? ProductTypeName { get; set; }
    }
}
