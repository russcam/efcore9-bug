namespace EFCore9Bug.EntityFramework;

public class UserRole
{
    public int UserRoleId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}