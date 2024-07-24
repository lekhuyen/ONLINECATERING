using RESTAURANT.API.Models;
using System.Threading.Tasks;

namespace RESTAURANT.API.Repositories
{
	public class LobbyImagesRepository : ILobbyImagesRepository
	{
		private readonly DatabaseContext _dbContext;

		public LobbyImagesRepository(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<LobbyImages> CreateLobbyImage(LobbyImages lobbyImage)
		{
			await _dbContext.LobbiesImages.AddAsync(lobbyImage);
			await _dbContext.SaveChangesAsync();
			return lobbyImage;
		}

		public async Task<LobbyImages> DeleteLobbyImage(int id)
		{
			var lobbyImage = await _dbContext.LobbiesImages.FindAsync(id);
			if (lobbyImage != null)
			{
				_dbContext.LobbiesImages.Remove(lobbyImage);
				await _dbContext.SaveChangesAsync();
			}
			return lobbyImage;
		}

		public async Task<LobbyImages> UpdateLobbyImage(LobbyImages lobbyImage)
		{
			var existingLobbyImage = await _dbContext.LobbiesImages.FindAsync(lobbyImage.Id);

			if (existingLobbyImage == null)
			{
				return null;
			}

			_dbContext.Entry(existingLobbyImage).CurrentValues.SetValues(lobbyImage);
			await _dbContext.SaveChangesAsync();

			return existingLobbyImage;
		}
	}
}
