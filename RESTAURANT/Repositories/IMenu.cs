using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public interface IMenu
    {
        Task AddMenu(CreateMenuDTO menu);
        Task DeleteMenu(int id);
        Task UpdateMenu( Menu menu);
        Task<Menu> GetMenuById(int id);
        Task<GetMenuDTO> GetOneMenuById(int id);
    }
}
