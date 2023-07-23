using FindMyTeddy.API.JwtMenager.Models;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Models;
using System.Security.Claims;

namespace FindMyTeddy.API.JwtMenager
{
    public interface IJwtAuthManager
    {
        JwtAuthResultViewModel GenerateTokens(UserDomainModel user, DateTime now);
    }
}