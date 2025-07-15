namespace ControlBoard.Domain.Dto;

public class ProcessStateAdvDto
{
    public int StateId { get; set; }
    private string? _value;

    public string? Value
    {
        get => _value;

        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _value = value;
            }
            else
            {
                _value = value.PadLeft(3, '0');
            }
        }
    }
}