using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;



        public OrderController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _dbContext.Orders.ToListAsync();

                var orderDTOs = orders.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    CustomComboId = o.CustomComboId,
                    TotalPrice = o.TotalPrice,
                    QuantityTable = o.QuantityTable,
                    StatusPayment = o.StatusPayment,
                    Deposit = o.Deposit,
                    Oganization = o.Oganization,
                    LobbyId = o.LobbyId,
                    
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get all orders successfully",
                    Data = orderDTOs
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = ex.Message
                });
            }
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Order not found",
                    });
                }

                var orderDTO = new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    CustomComboId = order.CustomComboId,
                    TotalPrice = order.TotalPrice,
                    QuantityTable = order.QuantityTable,
                    StatusPayment = order.StatusPayment,
                    Deposit = order.Deposit,
                    Oganization = order.Oganization,
                    LobbyId = order.LobbyId,
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get order successfully",
                    Data = orderDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = ex.Message
                });
            }
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            try
            {
                // Check if CustomComboId exists
                var customCombo = await _dbContext.CustomCombos
                    .Include(cc => cc.User) // Include related User information
                    .Include(cc => cc.Dish) // Include related Dish information if needed
                    .FirstOrDefaultAsync(cc => cc.Id == orderDTO.CustomComboId);

                // Create new Order entity and set properties
                var order = new Order
                {
                    UserId = orderDTO?.UserId,
                    ComboId = orderDTO.ComboId,
                    TotalPrice = orderDTO.TotalPrice,
                    QuantityTable = orderDTO.QuantityTable,
                    Deposit = orderDTO.Deposit,
                    Oganization = orderDTO.Oganization,
                    LobbyId = orderDTO.LobbyId,
                };

                // Add to database
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                // Prepare detailed response data
                var responseData = new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    CustomComboId = order.CustomComboId,
                    TotalPrice = order.TotalPrice,
                    QuantityTable = order.QuantityTable,
                    StatusPayment = order.StatusPayment,
                    Deposit = order.Deposit,
                    Oganization = order.Oganization,

                    // Include other properties as needed
                };

                // Return created order DTO
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Order created successfully",
                    Data = responseData
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error occurred while creating order",
                    Data = ex.Message
                });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO orderDTO)
        {
            try
            {
                var order = await _dbContext.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 404, // Not Found status code
                        Message = "Order not found",
                    });
                }

                // Update specific fields from orderDTO
                order.TotalPrice = orderDTO.TotalPrice;
                order.QuantityTable = orderDTO.QuantityTable;
                order.StatusPayment = (bool)orderDTO.StatusPayment;
                order.Deposit = orderDTO.Deposit;
                order.Oganization = orderDTO.Oganization;

                // Save changes
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();

                // Prepare updated order DTO response
                var updatedOrderDTO = new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    CustomComboId = order.CustomComboId,
                    TotalPrice = order.TotalPrice,
                    QuantityTable = order.QuantityTable,
                    StatusPayment = order.StatusPayment,
                    Deposit = order.Deposit,
                    Oganization = order.Oganization,
                    // Include other properties as needed
                };

                // Return updated order DTO
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 200, // OK status code
                    Message = "Order updated successfully",
                    Data = updatedOrderDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 400, // Bad Request status code
                    Message = "Error occurred while updating order",
                    Data = ex.Message
                });
            }
        }


        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Order not found",
                        Data = null
                    });
                }

                // Remove order from database
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Order deleted successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error occurred while deleting order",
                    Data = ex.Message
                });
            }
        }
    }
}
