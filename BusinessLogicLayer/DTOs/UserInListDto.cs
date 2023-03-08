namespace BusinessLogicLayer.DTOs
{
    public class UserInListDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public RoleDto Role { get; set; }
    }
}
