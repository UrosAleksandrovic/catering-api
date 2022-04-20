namespace Catering.Application.Security;

public interface IDataProtector
{
    string Encrypt(string data);
    string Decrypt(string data);
    string Hash(string data);
}
