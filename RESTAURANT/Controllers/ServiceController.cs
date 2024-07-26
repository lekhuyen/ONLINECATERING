using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;
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
					success = true,
					status = 0,
					message = "Get all service successfully",
					data = services
				});
			}
			catch (Exception e)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from service",
					Data = null
				});
			}
		}
		[HttpPost]
		public async Task<IActionResult> CreateService([FromForm] Service service, IFormFile formFile)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "Create service failed"
					});
				}
				//service.ImagePath = await FileUploader.SaveImage("images", formFile);
				var serviceCreated = await _repository.AddServiceAsync(service);
				return Created("success", new
				{
					success = true,
					status = 201,
					message = "Create service successfully",
					data = serviceCreated
				});
			}
			catch (Exception e)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Create service failed"
				});
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateService(int id, [FromForm] Service service, IFormFile formFile)
		{
			try
			{
				var serviceExisted = await _repository.GetServiceByIdAsync(id);
				if (serviceExisted == null)
				{
					ModelState.AddModelError("ID", "ID is not existed");
				}
				if (id != service.Id)
				{
					ModelState.AddModelError("ID", "ID does not match");
				}
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}
				if (formFile != null)
				{
					if (!string.IsNullOrEmpty(serviceExisted.ImagePath))
					{
						//FileUpload.DeleteImage(serviceExisted.ImagePath);
					}
					//service.ImagePath = await FileUpload.SaveImage("images", formFile);
				}
				else
				{
					service.ImagePath = serviceExisted.ImagePath;
				}
				var serviceUpdated = await _repository.UpdateServiceAsync(service);
				return Ok(new
				{
					success = true,
					status = 0,
					message = "Update service successfully",
					data = serviceUpdated
				});
			}
			catch (Exception e)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Create service failed"
				});
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteService(int id)
		{
			try
			{
				var serviceExisted = await _repository.GetServiceByIdAsync(id);
				if (serviceExisted == null)
				{
					return NotFound(new
					{
						Success = false,
						status = 1,
						message = "Service is not found",
						data = serviceExisted
					});
				}
				if (!string.IsNullOrEmpty(serviceExisted.ImagePath))
				{
					//FileUpload.DeleteImage(serviceExisted.ImagePath);
				}
				var serviceDelete = await _repository.DeleteServiceAsync(id);
				return Ok(new
				{
					success = true,
					status = 0,
					message = "Delete service successfully",
					data = serviceDelete
				});
			}
			catch (Exception e)
			{
				return BadRequest(new
				{
					Success = false,
					status = 1,
					message = "Service is not found",
				});
			}
		}
	}
}
