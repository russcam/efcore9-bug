using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EFCore9Bug.Encryption;

public class EncryptedConvention : IModelFinalizingConvention
{
    private readonly EncryptionConverter _encryptionConverter;

    public EncryptedConvention(IEncryptor encryptor) =>
        _encryptionConverter = new EncryptionConverter(encryptor);

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var property in entityType.GetDeclaredProperties())
            {
                var encryptedAttribute = property.PropertyInfo?.GetCustomAttribute<EncryptedAttribute>();
                if (encryptedAttribute != null)
                    property.SetValueConverter(_encryptionConverter);
            }
        }
    }
}