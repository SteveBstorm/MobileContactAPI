using DataAccessLayer.Models;

namespace DemoAPI.Tools
{
    public interface ITokenManager
    {
        string GenerateJWT(User user);
    }
}