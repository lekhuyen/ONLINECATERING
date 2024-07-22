using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
	public interface ILobbyImagesRepository
	{
		Task CreateLobbyImage(LobbyImages lobbyImage);
	}
}
