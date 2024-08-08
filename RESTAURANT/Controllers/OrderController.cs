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
                    .Include(c => c.OOrderDishes)
                        .ThenInclude(c => c.Dish)
                    .Include(c => c.OrderAppetizers)
                        .ThenInclude(c => c.Appetizer)
                    .Include(c => c.OrderDesserts)
                        .ThenInclude(c => c.Dessert)
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
                    User = o.User != null ? new UserDTO
                    {
                        Id = o.User.Id,
                        UserEmail = o.User.UserEmail,
                        UserName = o.User.UserName,
                        Phone = o.User.Phone
                    } : null,
                    Combo = (o.OOrderDishes.Any() || o.OrderDesserts.Any() || o.OrderAppetizers.Any())
                        ? null
                        : new ComboDTO
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
                            ComboAppetizers = o.Combo.ComboAppetizers.Select(ca => new ComboAppetizerDTO
                            {
                                AppetizerName = ca.Appetizer?.AppetizerName,
                                AppetizerPrice = ca.Appetizer.Price,
                                AppetizerImage = ca.Appetizer.AppetizerImage
                            }).ToList(),
                            ComboDishes = o.Combo.ComboDishes.Select(x => new ComboDishDTO
                            {
                                DishName = x.Dish.Name,
                                DishPrice = x.Dish.Price,
                                DishImagePath = x.Dish.ImagePath
                            }).ToList()
                        },
                    Lobby = o.Lobby != null ? new LobbyDTO
                    {
                        LobbyName = o.Lobby.LobbyName,
                        Price = o.Lobby.Price
                    } : null,
                    GetOrderDishes = o.OOrderDishes.Select(c => new GetOrderDishDTO
                    {
                        Quantity = c.Quantity,
                        DishDTO = new DishDTO
                        {
                            Name = c.Dish?.Name,
                            Price = c.Dish?.Price ?? 0,
                            Image = c.Dish?.ImagePath
                        }
                    }).ToList(),
                    GetOrderDesserts = o.OrderDesserts.Select(c => new GetOrderDessertDTO
                    {
                        Quantity = c.Quantity,
                        Dessert = new DessertDTO
                        {
                            Name = c.Dessert?.DessertName,
                            Price = c.Dessert?.Price ?? 0,
                            Image = c.Dessert?.DessertImage
                        }
                    }).ToList(),
                    GetOrderAppetizers = o.OrderAppetizers.Select(c => new GetOrderAppetizerDTO
                    {
                        Quantity = c.Quantity,
                        Appetizer = new AppetizerDTO
                        {
                            Name = c.Appetizer?.AppetizerName,
                            Price = c.Appetizer?.Price ?? 0,
                            Image = c.Appetizer?.AppetizerImage
                        }
                    }).ToList()
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
                    .Include(c => c.OOrderDishes)
                        .ThenInclude(c => c.Dish)
                    .Include(c => c.OrderAppetizers)
                        .ThenInclude(c => c.Appetizer)
                    .Include(c => c.OrderDesserts)
                        .ThenInclude(c => c.Dessert)
                    .Include(c => c.User)
                    .Include(c => c.Lobby)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (order == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Order not found"
                    });
                }

                var hasOrderDetails = order.OOrderDishes.Any() || order.OrderDesserts.Any() || order.OrderAppetizers.Any();

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
                    User = order.User != null ? new UserDTO
                    {
                        Id = order.User.Id,
                        UserEmail = order.User.UserEmail,
                        UserName = order.User.UserName,
                        Phone = order.User.Phone
                    } : null,
                    Combo = !hasOrderDetails && order.Combo != null ? new ComboDTO
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
                        ComboAppetizers = order.Combo.ComboAppetizers.Select(ca => new ComboAppetizerDTO
                        {
                            AppetizerName = ca.Appetizer?.AppetizerName,
                            AppetizerPrice = ca.Appetizer.Price,
                            AppetizerImage = ca.Appetizer.AppetizerImage
                        }).ToList(),
                        ComboDishes = order.Combo.ComboDishes.Select(x => new ComboDishDTO
                        {
                            DishName = x.Dish.Name,
                            DishPrice = x.Dish.Price,
                            DishImagePath = x.Dish.ImagePath
                        }).ToList()
                    } : null,
                    GetOrderDishes = order.OOrderDishes.Any() ? order.OOrderDishes.Select(c => new GetOrderDishDTO
                    {
                        Quantity = c.Quantity,
                        DishDTO = new DishDTO
                        {
                            Name = c.Dish?.Name,
                            Price = c.Dish?.Price ?? 0,
                            Image = c.Dish?.ImagePath
                        }
                    }).ToList() : null,
                    GetOrderDesserts = order.OrderDesserts.Any() ? order.OrderDesserts.Select(c => new GetOrderDessertDTO
                    {
                        Quantity = c.Quantity,
                        Dessert = new DessertDTO
                        {
                            Name = c.Dessert?.DessertName,
                            Price = c.Dessert?.Price ?? 0,
                            Image = c.Dessert?.DessertImage
                        }
                    }).ToList() : null,
                    GetOrderAppetizers = order.OrderAppetizers.Any() ? order.OrderAppetizers.Select(c => new GetOrderAppetizerDTO
                    {
                        Quantity = c.Quantity,
                        Appetizer = new AppetizerDTO
                        {
                            Name = c.Appetizer?.AppetizerName,
                            Price = c.Appetizer?.Price ?? 0,
                            Image = c.Appetizer?.AppetizerImage
                        }
                    }).ToList() : null,
                    Lobby = order.Lobby != null ? new LobbyDTO
                    {
                        LobbyName = order.Lobby.LobbyName,
                        Price = order.Lobby.Price
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
                    .Include(c => c.OOrderDishes)
                        .ThenInclude(c => c.Dish)
                    .Include(c => c.OrderAppetizers)
                        .ThenInclude(c => c.Appetizer)
                    .Include(c => c.OrderDesserts)
                        .ThenInclude(c => c.Dessert)
                    .Include(c => c.Lobby)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "No orders found for the given user ID"
                    });
                }

                var orderDTOs =  orders.Select(order => new OrderDTO
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
                    Combo = (order.OOrderDishes.Any() || order.OrderDesserts.Any() || order.OrderAppetizers.Any())
                        ? null
                        : new ComboDTO
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
                            ComboAppetizers = order.Combo.ComboAppetizers.Select(ca => new ComboAppetizerDTO
                            {
                                AppetizerName = ca.Appetizer?.AppetizerName,
                                AppetizerPrice = ca.Appetizer.Price,
                                AppetizerImage = ca.Appetizer.AppetizerImage
                            }).ToList(),
                            ComboDishes = order.Combo.ComboDishes.Select(x => new ComboDishDTO
                            {
                                DishName = x.Dish.Name,
                                DishPrice = x.Dish.Price,
                                DishImagePath = x.Dish.ImagePath
                            }).ToList()
                        },
                    GetOrderDishes = order.OOrderDishes.Any() ? order.OOrderDishes.Select(c => new GetOrderDishDTO
                    {
                        Quantity = c.Quantity,
                        DishDTO = new DishDTO
                        {
                            Name = c.Dish?.Name,
                            Price = c.Dish?.Price ?? 0,
                            Image = c.Dish?.ImagePath
                        }
                    }).ToList() : null,
                    GetOrderDesserts = order.OrderDesserts.Any() ? order.OrderDesserts.Select(c => new GetOrderDessertDTO
                    {
                        Quantity = c.Quantity,
                        Dessert = new DessertDTO
                        {
                            Name = c.Dessert?.DessertName,
                            Price = c.Dessert?.Price ?? 0,
                            Image = c.Dessert?.DessertImage
                        }
                    }).ToList() : null,
                    GetOrderAppetizers = order.OrderAppetizers.Any() ? order.OrderAppetizers.Select(c => new GetOrderAppetizerDTO
                    {
                        Quantity = c.Quantity,
                        Appetizer = new AppetizerDTO
                        {
                            Name = c.Appetizer?.AppetizerName,
                            Price = c.Appetizer?.Price ?? 0,
                            Image = c.Appetizer?.AppetizerImage
                        }
                    }).ToList() : null,
                    Lobby = order.Lobby != null ? new LobbyDTO
                    {
                        LobbyName = order.Lobby.LobbyName,
                        Price = order.Lobby.Price
                    } : null
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Orders retrieved successfully",
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


        [HttpGet("lobby/{lobbyId}")]
        public async Task<IActionResult> GetLobby(int lobbyId)
        {
            try
            {
                var lobies = await _dbContext.Orders.Where(c => c.LobbyId == lobbyId && c.StatusPayment == true).ToListAsync();
                if (lobies != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Get order successfully",
                        Data = lobies
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Order nofound",
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
        //k sử dụng CreateOrder nữa, sử dụng CreateOrderCombo
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

        [HttpPost("combo")]
        public async Task<IActionResult> CreateOrderCombo(CreateOrderDTO orderDTO)
        {
            try
            {
                
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
                
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();


                if (orderDTO.OrderDish != null)
                {
                    foreach (var dish in orderDTO.OrderDish)
                    {
                        var orderDish = new OOrderDish
                        {
                            DishId = dish.DishId,
                            OrderId = order.Id,
                            Quantity = dish.Quantity
                        };
                        await _dbContext.OOrderDishes.AddAsync(orderDish);
                        await _dbContext.SaveChangesAsync();

                    };
                };

                if (orderDTO.OrderDessert != null)
                {
                    foreach (var dessert in orderDTO.OrderDessert)
                    {
                        var orderDessert = new OrderDessert
                        {
                            DessertId = dessert.DessertId,
                            OrderId = order.Id,
                            Quantity = dessert.Quantity
                        };
                        await _dbContext.OrderDesserts.AddAsync(orderDessert);
                        await _dbContext.SaveChangesAsync();

                    };
                };

                if (orderDTO.OrderAppetizer != null)
                {
                    foreach (var dessert in orderDTO.OrderAppetizer)
                    {
                        var orderAppetizer = new OrderAppetizer
                        {
                            AppetizerId = dessert.AppetizerId,
                            OrderId = order.Id,
                            Quantity = dessert.Quantity
                        };
                        await _dbContext.OrderAppetizers.AddAsync(orderAppetizer);
                        await _dbContext.SaveChangesAsync();

                    };
                };

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

        [HttpDelete("{userId}/{orderId}")]
        public async Task<IActionResult> DeleteOrderByUser(int userId, int orderId)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(c => c.Id == orderId && c.UserId == userId);

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
