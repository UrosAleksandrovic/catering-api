using LdapForNet;

namespace Catering.Infrastructure.Security.Ldap;

internal class LdapUser
{
    public string Uid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Ojectclass { get; set; }
    public string UserPassword { get; set; }
    public string Mobile { get; set; }
    public string Mail { get; set; }
    public string CN { get; set; }
    public string DepartmentNumber { get; set; }
    public string L { get; set; }

    internal LdapUser(LdapEntry entry)
    {
        foreach (var attr in entry.DirectoryAttributes)
        {
            var values = attr.GetValues<string>();
            switch (attr.Name.ToUpperInvariant())
            {
                case "UID": if (string.IsNullOrEmpty(Uid)) { Uid = values.First(); } break;
                case "USERPASSWORD": if (string.IsNullOrEmpty(UserPassword)) { UserPassword = values.First(); } break;
                case "GIVENNAME": if (string.IsNullOrEmpty(FirstName)) { FirstName = values.First(); } break;
                case "SN": if (string.IsNullOrEmpty(LastName)) { LastName = values.First(); } break;
                case "TITLE": if (string.IsNullOrEmpty(Title)) { Title = values.First(); } break;
                case "OBJECTCLASS": if (string.IsNullOrEmpty(Ojectclass)) { Ojectclass = values.First(); } break;
                case "MOBILE": if (string.IsNullOrEmpty(Mobile)) { Mobile = values.First(); } break;
                case "MAIL": if (string.IsNullOrEmpty(Mail)) { Mail = values.First(); } break;
                case "CN": if (string.IsNullOrEmpty(CN)) { CN = values.First(); } break;
                case "DEPARTMENTNUMBER": if (string.IsNullOrEmpty(CN)) { DepartmentNumber = values.First(); } break;
                case "L": if (string.IsNullOrEmpty(CN)) { L = values.First(); } break;
            }
        }
    }
}

