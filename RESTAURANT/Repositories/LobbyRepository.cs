using Microsoft.EntityFrameworkCore;
using REDISCLIENT;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
	public class LobbyRepository : ILobbyRepository
	{
		private readonly RedisClient _redis;
		private readonly DatabaseContext _dbContext;
		public LobbyRepository(DatabaseContext dbContext, RedisClient redisClient)
		{
			_dbContext = dbContext;
			_redis = redisClient;
		}

		public async Task<Lobby> CreateLobby(Lobby lobby)
		{
			await _dbContext.Lobbies.AddAsync(lobby);
			await _dbContext.SaveChangesAsync();
			return lobby;
		}

		public async Task<Lobby> DeleteLobby(int id)
		{
			var lobby = await _dbContext.Lobbies.FindAsync(id);
			if (lobby != null)
			{
				_dbContext.Lobbies.Remove(lobby);
				await _dbContext.SaveChangesAsync();
			}
			return lobby;
		}

        public async Task<IEnumerable<Lobby>> GetAllLobbies()
        {
            var lobbies = await _dbContext.Lobbies
                .Include(l => l.LobbyImages) // Eager loading LobbyImages
                .ToListAsync();

            return lobbies;
        }

        public async Task<Lobby> GetLobbyById(int id)
        {
            var lobby = await _dbContext.Lobbies
                .Include(l => l.LobbyImages) // Eager loading LobbyImages
                .FirstOrDefaultAsync(l => l.Id == id);

            return lobby;
        }

        public async Task<Lobby> UpdateLobby(Lobby lobby)
		{
			_dbContext.Entry(lobby).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
			return lobby;
		}
	}
}
