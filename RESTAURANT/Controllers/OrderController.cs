/*using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public OrderController(DatabaseContext dbContext)
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
                    .Include(o => o.User)
                    .Include(o => o.CustomCombo)
                    .Include(o => o.Promotions)
                    .Include(o => o.Payment)
                    .ToListAsync();

                var orderDTOs = orders.Select(o => new OrderDTO
                {
                    Id = o.Id,

                    TotalPrice = o.TotalPrice,
                    QuantityTable = o.QuantityTable,
                    StatusPayment = o.StatusPayment,
                    Deposit = o.Deposit,
                    Oganization = o.Oganization,

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

        [HttpPost("Create")]
        public async Task<IActionResult> Create(OrderDTO orderDTO)
        {
            try
            {
                // Validate other properties of orderDTO as needed

                // Create new Order entity and set properties
                var order = new Order
                {
                    TotalPrice = orderDTO.TotalPrice,
                    QuantityTable = orderDTO.QuantityTable,
                    StatusPayment = orderDTO.StatusPayment,
                    Deposit = orderDTO.Deposit,
                    Oganization = orderDTO.Oganization,
                    PromotionId = orderDTO.PromotionId  // Assign PromotionId here
                };

                // Add to database
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                // Return created order DTO with associated PromotionId
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Order created successfully",
                    Data = new OrderDTO
                    {
                        Id = order.Id,
                        TotalPrice = order.TotalPrice,
                        QuantityTable = order.QuantityTable,
                        StatusPayment = order.StatusPayment,
                        Deposit = order.Deposit,
                        Oganization = order.Oganization,
                        PromotionId = order.PromotionId  // Return the associated PromotionId
                    }
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
    }
}
*/