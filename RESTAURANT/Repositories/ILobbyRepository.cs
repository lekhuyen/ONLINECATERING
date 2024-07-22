using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
	public interface ILobbyRepository
	{
		Task<IEnumerable<Lobby>> GetAllLobbies();
		Task<Lobby> GetLobbyById(int id);
		Task<Lobby> CreateLobby(Lobby lobby);
		Task<Lobby> UpdateLobby(Lobby lobby);
		Task<Lobby> DeleteLobby(int id);
	}
}
