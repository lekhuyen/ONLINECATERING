using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
	public class GetLobbyDTO
	{
		public int Id { get; set; }
		public string LobbyName { get; set; }
		public string Description { get; set; }
		public string Area { get; set; }
		public int Type { get; set; }
		public List<LobbyImagesDTO>? LobbyImages { get; set; }
	}
}
