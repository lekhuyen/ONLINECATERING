using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
	public interface IServiceRepository
	{
		Task<IEnumerable<Service>> GetAllServiceAsync();
		Task<Service> AddServiceAsync(Service service);
		Task<Service> UpdateServiceAsync(Service service);
		Task<Service> DeleteServiceAsync(int id);
		Task<Service> GetServiceByIdAsync(int id);
	}
}
