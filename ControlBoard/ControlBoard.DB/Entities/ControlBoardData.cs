namespace ControlBoard.DB.Entities
{
    public class ControlBoardData
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
