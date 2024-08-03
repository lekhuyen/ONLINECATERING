using Microsoft.EntityFrameworkCore;
//using RESTAURANT.API.Data;
using RESTAURANT.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Repositories
{
    public class LobbyRepository : ILobbyRepository
    {
        private readonly DatabaseContext _dbContext;

        public LobbyRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<IEnumerable<Lobby>> GetAllLobbies()
        {
            return await _dbContext.Lobbies
                .Include(l => l.LobbyImages) // Eager load LobbyImages
                .ToListAsync();
        }

        public async Task<Lobby> GetLobbyById(int id)
        {
            return await _dbContext.Lobbies
                .Include(l => l.LobbyImages)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lobby> CreateLobby(Lobby lobby, IEnumerable<LobbyImages> images)
        {
            // Initialize LobbyImages collection if it's null
            lobby.LobbyImages ??= new List<LobbyImages>();

            foreach (var image in images)
            {
                lobby.LobbyImages.Add(image);
            }

            _dbContext.Lobbies.Add(lobby);
            await _dbContext.SaveChangesAsync();

            return lobby;
        }

        public async Task<Lobby> UpdateLobby(int id, Lobby lobby, IEnumerable<LobbyImages> images)
        {
            var existingLobby = await _dbContext.Lobbies
                .Include(l => l.LobbyImages)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (existingLobby != null)
            {
                // Update properties of the lobby entity
                existingLobby.LobbyName = lobby.LobbyName;
                existingLobby.Description = lobby.Description;
                existingLobby.Area = lobby.Area;
                existingLobby.Type = lobby.Type;
                existingLobby.Price = lobby.Price;

                // Handle images
                if (images != null && images.Any())
                {
                    // Clear existing images
                    existingLobby.LobbyImages.Clear();

                    // Add new images
                    foreach (var image in images)
                    {
                        existingLobby.LobbyImages.Add(image);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }

            return existingLobby;
        }


        public async Task DeleteLobby(int id)
        {
            var lobby = await _dbContext.Lobbies.FindAsync(id);
            if (lobby != null)
            {
                _dbContext.Lobbies.Remove(lobby);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
