using USER.API.Models;

namespace USER.API.Repositories
{
    public interface IAuthUser
    {
        Task<User> Login(string email, string password);
    }
}
