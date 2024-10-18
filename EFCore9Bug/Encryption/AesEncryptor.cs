using System.Security.Cryptography;

namespace EFCore9Bug.Encryption;

public class AesEncryptor : IEncryptor
{
    private const int VectorSize = 16;
    private byte[]? _bytes;

    public AesEncryptor(string key) =>
        Key = key;
    
    public string Key { get; }

    public string Decrypt(byte[] bytes)
    {
        var keyBytes = GetKeyBytes();
        var cipherBytes = bytes.Take(bytes.Length - VectorSize).ToArray();
        var iv = bytes.Skip(cipherBytes.Length).Take(VectorSize).ToArray();
        using var memoryStream = new MemoryStream(cipherBytes);
        using var aes = Aes.Create();
        using var decryptor = aes.CreateDecryptor(keyBytes, iv);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);
        return reader.ReadToEnd();
    }

    public byte[] Encrypt(string value)
    {
        var keyBytes = GetKeyBytes();
        using var memoryStream = new MemoryStream();
        using var aes = Aes.Create();
        using var encryptor = aes.CreateEncryptor(keyBytes, aes.IV);
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var writer = new StreamWriter(cryptoStream);
        writer.Write(value);
        writer.Flush();
        cryptoStream.FlushFinalBlock();
        memoryStream.Write(aes.IV, 0, aes.IV.Length);
        return memoryStream.ToArray();
    }

    private byte[] GetKeyBytes() => _bytes ??= Convert.FromBase64String(Key);
}