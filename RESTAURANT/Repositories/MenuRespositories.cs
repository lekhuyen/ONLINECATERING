using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public class MenuRespositories : IMenu
    {
        private readonly DatabaseContext _databaseContext;
        public MenuRespositories(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddMenu(CreateMenuDTO menuDTO)
        {
            var menu = new Menu
            {
                MenuName = menuDTO.MenuName,
                Price = menuDTO.Price,
                Ingredient = menuDTO.Ingredient,
                RestaurantId = menuDTO.RestaurantId,
            };
            await _databaseContext.AddAsync(menu);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteMenu(int id)
        {
             var menu = await _databaseContext.Menus.FindAsync(id);
            if(menu != null)
            {
                 _databaseContext.Menus.Remove(menu);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task<Menu> GetMenuById(int id)
        {
            var menu = await _databaseContext.Menus.FindAsync(id);
            if (menu != null)
            {
                return menu;    
            }
            return null;
        }
        public async Task<GetMenuDTO> GetOneMenuById(int id)
        {
            var menu = await _databaseContext.Menus.FindAsync(id);
            
            if (menu != null)
            {
                var menuDTO = new GetMenuDTO
                {
                    Id = menu.Id,
                    MenuName = menu.MenuName,
                    Price = menu.Price,
                    Ingredient = menu.Ingredient,
                };
                return menuDTO;
            }
            return null;
        }

        public async Task UpdateMenu(Menu menu)
        {
            _databaseContext.Menus.Update(menu);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
