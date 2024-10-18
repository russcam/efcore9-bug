using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore9Bug.Encryption;

public class EncryptionConverter : ValueConverter<string, string>
{
    public EncryptionConverter(IEncryptor encryptor)
        : base(x => Convert.ToBase64String(encryptor.Encrypt(x)),
            x => encryptor.Decrypt(Convert.FromBase64String(x)))
    {
    }
}