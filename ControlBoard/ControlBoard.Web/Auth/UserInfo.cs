namespace ControlBoard.Web.Auth
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public MesRoles Role { get; set; }
        public bool IsActive { get; set; }
    }
}
