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

		public async Task CreateLobbyImage(LobbyImages lobbyImage)
		{
			await _dbContext.LobbiesImages.AddAsync(lobbyImage);
			await _dbContext.SaveChangesAsync();
		}
	}
}
