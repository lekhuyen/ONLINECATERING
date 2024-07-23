using System.Threading.Tasks;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
	public interface ILobbyImagesRepository
	{
		Task<LobbyImages> CreateLobbyImage(LobbyImages lobbyImage);
		Task<LobbyImages> UpdateLobbyImage(LobbyImages lobbyImage);
		Task<LobbyImages> DeleteLobbyImage(int id);
	}
}
