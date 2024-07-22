using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private readonly IServiceRepository _repository;
		public ServiceController(IServiceRepository repository)
		{
			_repository = repository;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllService()
		{
			try
			{
				var services = await _repository.GetAllServiceAsync();
				return Ok(new
				{
					status = 200,
					message = "Get all service successfully",
					data = services
				});
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
