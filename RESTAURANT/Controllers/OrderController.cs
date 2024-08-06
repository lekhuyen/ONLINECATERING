using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
using Order = RESTAURANT.API.Models.Order;

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
                var orders = await _dbContext.Orders
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboDesserts)
                        .ThenInclude(c => c.Dessert)
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboAppetizers)
                        .ThenInclude(c => c.Appetizer)
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboDishes)
                        .ThenInclude(c => c.Dish)
                    .Include(c => c.User)
                    .Include(c => c.Lobby)
                    .ToListAsync();

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
                    ComboId = o.ComboId,
                    Status = o.Status,
                    User = new UserDTO
                    {
                        UserEmail = o.User.UserEmail,
                        UserName = o.User.UserName,
                        Phone = o.User.Phone
                    },
                    Combo = o.Combo != null ? new ComboDTO
                    {
                        Id = o.Combo.Id,
                        Name = o.Combo.Name,
                        Price = o.Combo.Price,
                        ComboDesserts = o.Combo.ComboDesserts.Select(x => new ComboDessertDTO
                        {
                            DessertName = x.Dessert?.DessertName,
                            DessertPrice = x.Dessert.Price,
                            DessertImage = x.Dessert.DessertImage
                        }).ToList(),
                        ComboAppetizers = o.Combo?.ComboAppetizers?.Select(ca => new ComboAppetizerDTO
                        {
                            AppetizerName = ca?.Appetizer.AppetizerName,
                            AppetizerPrice = ca.Appetizer.Price,
                            AppetizerImage = ca.Appetizer.AppetizerImage
                        }).ToList(),
                        ComboDishes = o.Combo.ComboDishes?.Select(x => new ComboDishDTO
                        {
                            DishName = x.Dish.Name,
                            DishPrice = x.Dish.Price,
                            DishImagePath = x.Dish.ImagePath
                        }).ToList()
                    } : null,
                    Lobby = o.Lobby != null ? new LobbyDTO
                    {
                        LobbyName = o.Lobby.LobbyName,
                        Price = o.Lobby.Price
                    } : null

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
                var order = await _dbContext.Orders
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboDesserts)
                        .ThenInclude(c => c.Dessert)
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboAppetizers)
                        .ThenInclude(c => c.Appetizer)
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboDishes)
                        .ThenInclude(c => c.Dish)

                    .Include(c => c.Lobby)
                    .FirstOrDefaultAsync(c => c.Id == id);

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
                    ComboId = order.ComboId,
                    Status = order.Status,
                    Combo = order.Combo != null ? new ComboDTO
                    {
                        Id = order.Combo.Id,
                        Name = order.Combo.Name,
                        Price = order.Combo.Price,
                        ComboDesserts = order.Combo.ComboDesserts.Select(x => new ComboDessertDTO
                        {
                            DessertName = x.Dessert?.DessertName,
                            DessertPrice = x.Dessert.Price,
                            DessertImage = x.Dessert.DessertImage
                        }).ToList(),
                        ComboAppetizers = order.Combo?.ComboAppetizers?.Select(ca => new ComboAppetizerDTO
                        {
                            AppetizerName = ca?.Appetizer.AppetizerName,
                            AppetizerPrice = ca.Appetizer.Price,
                            AppetizerImage = ca.Appetizer.AppetizerImage
                        }).ToList(),
                        ComboDishes = order.Combo.ComboDishes?.Select(x => new ComboDishDTO
                        {
                            DishName = x.Dish.Name,
                            DishPrice = x.Dish.Price,
                            DishImagePath = x.Dish.ImagePath
                        }).ToList()
                    } : null,
                    Lobby = order.Lobby != null ? new LobbyDTO
                    {
                        LobbyName = order.Combo.Name,
                        Price = order.Combo.Price
                    } : null
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

        [HttpGet("booked/{userId}")]
        public async Task<IActionResult> GetOrderByUserId(int userId)
        {
            try
            {
                var orders = await _dbContext.Orders
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboDesserts)
                        .ThenInclude(c => c.Dessert)
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboAppetizers)
                        .ThenInclude(c => c.Appetizer)
                    .Include(c => c.Combo)
                        .ThenInclude(c => c.ComboDishes)
                        .ThenInclude(c => c.Dish)
                    .Include(c => c.Lobby)
                    .Where(o => o.UserId == userId).ToListAsync();
                if (orders == null || !orders.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Order not found",
                    });
                }

                var orderDTOs = orders.Select(order => new OrderDTO
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
                    ComboId = order.ComboId,
                    Status = order.Status,
                    Combo = order.Combo != null ? new ComboDTO
                    {
                        Id = order.Combo.Id,
                        Name = order.Combo.Name,
                        Price = order.Combo.Price,
                        ComboDesserts = order.Combo?.ComboDesserts?.Select(x => new ComboDessertDTO
                        {
                            DessertName = x.Dessert?.DessertName,
                            DessertPrice = x.Dessert.Price,
                            DessertImage = x.Dessert.DessertImage
                        }).ToList(),
                        ComboAppetizers = order.Combo?.ComboAppetizers?.Select(ca => new ComboAppetizerDTO
                        {
                            AppetizerName = ca?.Appetizer.AppetizerName,
                            AppetizerPrice = ca.Appetizer.Price,
                            AppetizerImage = ca.Appetizer.AppetizerImage
                        }).ToList(),
                        ComboDishes = order.Combo.ComboDishes?.Select(x => new ComboDishDTO
                        {
                            DishName = x.Dish.Name,
                            DishPrice = x.Dish.Price,
                            DishImagePath = x.Dish.ImagePath
                        }).ToList()
                    } : null,
                    Lobby = order.Lobby != null ? new LobbyDTO
                    {
                        LobbyName = order.Combo.Name,
                        Price = order.Combo.Price
                    } : null
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get orders successfully",
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



        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            try
            {
                // Check if CustomComboId exists
                var customCombo = await _dbContext.CustomCombos
                    .Include(cc => cc.Dish)
                    .Include(cc => cc.User)
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
                    ComboId = order.ComboId,
                   
                    

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
