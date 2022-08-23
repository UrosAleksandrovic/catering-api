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
                case "UID": if (string.IsNullOrEmpty(this.Uid)) { this.Uid = values.First(); } break;
                case "USERPASSWORD": if (string.IsNullOrEmpty(this.UserPassword)) { this.UserPassword = values.First(); } break;
                case "GIVENNAME": if (string.IsNullOrEmpty(this.FirstName)) { this.FirstName = values.First(); } break;
                case "SN": if (string.IsNullOrEmpty(this.LastName)) { this.LastName = values.First(); } break;
                case "TITLE": if (string.IsNullOrEmpty(this.Title)) { this.Title = values.First(); } break;
                case "OBJECTCLASS": if (string.IsNullOrEmpty(this.Ojectclass)) { this.Ojectclass = values.First(); } break;
                case "MOBILE": if (string.IsNullOrEmpty(this.Mobile)) { this.Mobile = values.First(); } break;
                case "MAIL": if (string.IsNullOrEmpty(this.Mail)) { this.Mail = values.First(); } break;
                case "CN": if (string.IsNullOrEmpty(this.CN)) { this.CN = values.First(); } break;
                case "DEPARTMENTNUMBER": if (string.IsNullOrEmpty(this.CN)) { this.DepartmentNumber = values.First(); } break;
                case "L": if (string.IsNullOrEmpty(this.CN)) { this.L = values.First(); } break;

            }
        }
    }
}

