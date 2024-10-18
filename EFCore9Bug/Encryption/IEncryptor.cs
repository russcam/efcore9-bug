namespace EFCore9Bug.Encryption;

public interface IEncryptor
{
    string Key { get; }
    
    string Decrypt(byte[] bytes);
    
    byte[] Encrypt(string value);
}