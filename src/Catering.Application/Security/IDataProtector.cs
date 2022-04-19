namespace Catering.Application.Security;

internal interface IDataProtector
{
    string Encrypt(string data);
    string Decrypt(string data);
    string Hash(string data);
}
