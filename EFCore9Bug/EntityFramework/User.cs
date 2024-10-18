using EFCore9Bug.Encryption;

namespace EFCore9Bug.EntityFramework;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    
    [Encrypted]
    public string Encrypted { get; set; }
    
    public List<UserRole> UserRoles { get; set; }
}