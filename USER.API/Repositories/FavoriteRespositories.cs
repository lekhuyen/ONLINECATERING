using Microsoft.EntityFrameworkCore;
using USER.API.DTOs;
using USER.API.Models;

namespace USER.API.Repositories
{
    public class FavoriteRespositories: IFavoriteList
    {
        private readonly DatabaseContext _databaseContext;
        public FavoriteRespositories(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddFavorite(FavoriteListDTO item)
        {
            var favorite = new FavoriteList
            {
                RestaurantName = item.RestaurantName,
                Image = item.Image,
                Address = item.Address,
                Rating = item.Rating,
                UserId = (int)item.UserId
            };
            await _databaseContext.FavoriteLists.AddAsync(favorite);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteFavorite(int id)
        {
            var favarite = await _databaseContext.FavoriteLists.FindAsync(id);
            if (favarite != null)
            {
                 _databaseContext.FavoriteLists.Remove(favarite);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FavoriteListDTO>> GetFavoriteListAsync()
        {
            var favorites = await _databaseContext.FavoriteLists.ToListAsync();
            var favorDTO = favorites.Select(f => new FavoriteListDTO
            {
                RestaurantName = f.RestaurantName,
                Image = f.Image,
                Address = f.Address,
                Rating = f.Rating,
            }).ToList();
            return favorDTO;
        }
    }
}
