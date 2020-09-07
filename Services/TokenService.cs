using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CJ.Identity.Api.Models.UsuarioViewModels;

namespace CJ.Identity.Api.Services
{
    public static class TokenService
    {
        private const string SECRET = "UmSegredoBemSecreto@123";
        public const int TEMPO_EXPIRACAO_EM_HORAS = 2;
        public const string EMISSOR = "Carlos Barreto";
        public static byte[] SECURITY_KEY = Encoding.ASCII.GetBytes(SECRET);

        public static LoginResponse GerarToken(IdentityUser user, IList<Claim> claims = null, IList<string> roles = null)
        {
            var email = user.Email;
            var idUsuario = user.Id;

            claims = claims ?? new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, idUsuario));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

            if (roles != null)
                roles.ToList().ForEach(userRole => claims.Add(new Claim("role", userRole)));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, claims, email, idUsuario);
        }



        private static string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = EMISSOR,
                Audience = "MinhaApi",
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(TEMPO_EXPIRACAO_EM_HORAS),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SECURITY_KEY), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private static LoginResponse ObterRespostaToken(string encodedToken, IEnumerable<Claim> claims, string email, string idUsuario)
        {
            return new LoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(TEMPO_EXPIRACAO_EM_HORAS).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = idUsuario,
                    Email = email,
                    Claims = claims.ToDictionary(c => c.Type, c => c.Value)
                }
            };
        }

    }
}
