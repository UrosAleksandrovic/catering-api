using Ardalis.GuardClauses;
using Catering.Application.Security;
using Catering.Infrastructure.Security.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Catering.Infrastructure.Security;

internal class DataProtector : IDataProtector
{
    private readonly SecurityAesOptions _aesOptions;
    private readonly SecurityShaOptions _shaOptions;

    public DataProtector(IOptions<SecurityAesOptions> aesOptions, IOptions<SecurityShaOptions> shaOptions)
    {
        _aesOptions = aesOptions.Value;
        _shaOptions = shaOptions.Value;
    }

    public string Decrypt(string data)
    {
        Guard.Against.NullOrWhiteSpace(data);

        using var aesProvider = Aes.Create();

        aesProvider.Key = GetByteArray(_aesOptions.Key);
        var decryptedData = aesProvider.DecryptCbc(GetByteArray(data), GetByteArray(_aesOptions.IV));

        return Encoding.UTF8.GetString(decryptedData);
    }

    public string Encrypt(string data)
    {
        Guard.Against.NullOrWhiteSpace(data);

        using var aesProvider = Aes.Create();

        aesProvider.Key = GetByteArray(_aesOptions.Key);
        var encryptedData = aesProvider.EncryptCbc(GetByteArray(data), GetByteArray(_aesOptions.IV));

        return Encoding.UTF8.GetString(encryptedData);
    }

    public string Hash(string data)
    {
        using var hasher = new HMACSHA512(GetByteArray(_shaOptions.Key));

        var hashedBytes = hasher.ComputeHash(GetByteArray(data));

        return Encoding.UTF8.GetString(hashedBytes);
    }

    private byte[] GetByteArray(string value)
        => Encoding.UTF8.GetBytes(value);
}
