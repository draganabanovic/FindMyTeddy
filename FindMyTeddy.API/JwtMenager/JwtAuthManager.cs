using FindMyTeddy.API.JwtMenager.Models;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FindMyTeddy.API.JwtMenager
{
    public class JwtAuthManager : IJwtAuthManager
    {
        public UserManager<User> userManager { get; }
        private readonly byte[] secret;
        private readonly AppSettings appSettings;

        public JwtAuthManager( UserManager<User> userManager, IConfiguration configuration)
        {
            this.appSettings = configuration.Get<AppSettings>();
            secret = Encoding.ASCII.GetBytes(appSettings.AuthSettings.JwtSecret);
            this.userManager = userManager;

        }

        public JwtAuthResultViewModel GenerateTokens(UserDomainModel user, DateTime now)
        {
            var claims = GetUserClaims(user);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                appSettings.AuthSettings.Issuer,
                shouldAddAudienceClaim ? appSettings.AuthSettings.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(appSettings.AuthSettings.AccessTokenExpiryInMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature));

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            string refreshTokenstring = "";
          
            return new JwtAuthResultViewModel
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenstring
            };
        }

        public IEnumerable<Claim> GetUserClaims(UserDomainModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("name", $"{user.FirstName ?? string.Empty} {user.LastName ?? string.Empty}".Trim()),
                new Claim("givenName", (user.FirstName ?? string.Empty).Trim()),
                new Claim("surname", (user.LastName ?? string.Empty).Trim()),
                new Claim(ClaimTypes.Role, (user.Role ?? string.Empty).Trim()),
                new Claim("email", user.Email),
                new Claim("userId", user.Id.ToString())
            };

            return claims;
        }
    }
}
