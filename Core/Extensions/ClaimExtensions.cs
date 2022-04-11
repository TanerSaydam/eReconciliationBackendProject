using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddNameIdentitfier(this ICollection<Claim> claims, string nameIdentitfier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentitfier));
        }

        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
        public static void AddCompany(this ICollection<Claim> claims, string company)
        {
            claims.Add(new Claim(ClaimTypes.Anonymous, company));
        }

        public static void AddCompanyName(this ICollection<Claim> claims, string companyName)
        {
            claims.Add(new Claim(ClaimTypes.IsPersistent, companyName));
        }

    }
}
