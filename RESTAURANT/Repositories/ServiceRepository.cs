using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
	public class ServiceRepository : IServiceRepository
	{
		private readonly DatabaseContext _dbContext;
		public ServiceRepository(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Service> AddServiceAsync(Service service)
		{
			await _dbContext.AddAsync(service);		
			await _dbContext.SaveChangesAsync();
			return service;
		}

		public async Task<Service> DeleteServiceAsync(int id)
		{
			var service = await GetServiceByIdAsync(id);
			_dbContext.Services.Remove(service);
			await _dbContext.SaveChangesAsync();
			return service;
		}

		public async Task<IEnumerable<Service>> GetAllServiceAsync()
		{
			var services = await _dbContext.Services.ToListAsync();
			return services;
		}

		public async Task<Service> GetServiceByIdAsync(int id)
		{
			var service = await _dbContext.Services.FirstOrDefaultAsync(s=>s.Id == id);
			return service;
		}

		public async Task<Service> UpdateServiceAsync(Service service)
		{
			var serviceExisted = await GetServiceByIdAsync(service.Id);
			_dbContext.Entry(serviceExisted).CurrentValues.SetValues(service);
			await _dbContext.SaveChangesAsync();
			return service;
		}
	}
}

