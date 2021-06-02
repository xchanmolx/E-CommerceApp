using Core.Entities;

namespace Core.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}