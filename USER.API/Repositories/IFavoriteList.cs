using USER.API.DTOs;
using USER.API.Models;

namespace USER.API.Repositories
{
    public interface IFavoriteList
    {
        Task<IEnumerable<FavoriteListDTO>> GetFavoriteListAsync();
        Task AddFavorite(FavoriteListDTO item);
        Task DeleteFavorite(int id);
    }
}
