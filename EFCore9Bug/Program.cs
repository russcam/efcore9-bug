using EFCore9Bug.Encryption;
using EFCore9Bug.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
        
serviceCollection.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

serviceCollection.AddSingleton<IEncryptor>(_ =>
    new AesEncryptor("a/N1cqW/Gopd81Z1cPQy/ReTgH04RTMld+DiKQrZi+M="));
        
var serviceProvider = serviceCollection.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    
    var alice = await db.Users
        .AsNoTracking()
        .Where(u => u.UserId == 1)
        .FirstAsync();
    
    Console.WriteLine($"Username: {alice.Username} encrypted value: {alice.Encrypted}");
    
    var users = await db.Users
        .AsNoTracking()
        .Select(u => new 
        {
            Username = u.Username,
            Encrypted = u.Encrypted,
        })
        .ToListAsync();
    
    foreach (var user in users)
    {
        Console.WriteLine($"Username: {user.Username} encrypted value: {user.Encrypted}");
    }
}