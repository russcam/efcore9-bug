namespace EFCore9Bug.EntityFramework;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<UserRole> UserRoles { get; set; }
}