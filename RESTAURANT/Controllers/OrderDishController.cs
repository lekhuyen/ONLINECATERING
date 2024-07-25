﻿using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDishController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public OrderDishController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/OrderDish
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDishDTO>>> GetAllOrderDishes()
        {
            var orderDishes = await _dbContext.OrderDishes
                .Include(od => od.Order)
                .Include(od => od.Dish)
                .ToListAsync();

            var orderDishDTOs = orderDishes.Select(od => new OrderDishDTO
            {
                OrderDishId = od.OrderDishId,
                OrderId = od.OrderId,
                DishId = od.DishId,
                Order = new OrderDTO
                {
                    Id = od.Order.Id,
                    UserId = od.Order.UserId,
                    CustomComboId = od.Order.CustomComboId,
                    TotalPrice = od.Order.TotalPrice,
                    QuantityTable = od.Order.QuantityTable,
                    StatusPayment = od.Order.StatusPayment,
                    Deposit = od.Order.Deposit,
                    Oganization = od.Order.Oganization,

                },
                Dish = new DishDTO
                {
                    Id = od.Dish.Id,
                    Name = od.Dish.Name,
                    Price = od.Dish.Price,
                    Status = od.Dish.Status,
                    ImagePath = od.Dish.ImagePath
                }
            }).ToList();

            return orderDishDTOs;
        }

        // GET: api/OrderDish/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDishDTO>> GetOrderDishById(int id)
        {
            var orderDish = await _dbContext.OrderDishes
                .Include(od => od.Order)
                .Include(od => od.Dish)
                .FirstOrDefaultAsync(od => od.OrderDishId == id);

            if (orderDish == null)
            {
                return NotFound();
            }

            var orderDishDTO = new OrderDishDTO
            {
                OrderDishId = orderDish.OrderDishId,
                OrderId = orderDish.OrderId,
                DishId = orderDish.DishId,
                Order = new OrderDTO
                {
                    Id = orderDish.Order.Id,
                    UserId = orderDish.Order.UserId,
                    CustomComboId = orderDish.Order.CustomComboId,
                    TotalPrice = orderDish.Order.TotalPrice,
                    QuantityTable = orderDish.Order.QuantityTable,
                    StatusPayment = orderDish.Order.StatusPayment,
                    Deposit = orderDish.Order.Deposit,
                    Oganization = orderDish.Order.Oganization,

                },
                Dish = new DishDTO
                {
                    Id = orderDish.Dish.Id,
                    Name = orderDish.Dish.Name,
                    Price = orderDish.Dish.Price,
                    Status = orderDish.Dish.Status,
                    ImagePath = orderDish.Dish.ImagePath
                }
            };

            return orderDishDTO;
        }

        // POST: api/OrderDish
        [HttpPost]
        public async Task<ActionResult<OrderDishDTO>> CreateOrderDish([FromBody] OrderDishCreateDTO orderDishCreateDTO)
        {
            try
            {
                var order = await _dbContext.Orders.FindAsync(orderDishCreateDTO.OrderId);
                if (order == null)
                {
                    return BadRequest("Invalid OrderId. Order does not exist.");
                }

                var dish = await _dbContext.Dishes.FindAsync(orderDishCreateDTO.DishId);
                if (dish == null)
                {
                    return BadRequest("Invalid DishId. Dish does not exist.");
                }

                var orderDish = new OrderDish
                {
                    OrderId = orderDishCreateDTO.OrderId,
                    DishId = orderDishCreateDTO.DishId
                    // You can add more properties as needed
                };

                _dbContext.OrderDishes.Add(orderDish);
                await _dbContext.SaveChangesAsync();

                var orderDishDTO = new OrderDishDTO
                {
                    OrderDishId = orderDish.OrderDishId,
                    OrderId = orderDish.OrderId,
                    DishId = orderDish.DishId,
                    Order = new OrderDTO
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        CustomComboId = order.CustomComboId,
                        TotalPrice = order.TotalPrice,
                        QuantityTable = order.QuantityTable,
                        StatusPayment = order.StatusPayment,
                        Deposit = order.Deposit,
                        Oganization = order.Oganization,

                    },
                    Dish = new DishDTO
                    {
                        Id = dish.Id,
                        Name = dish.Name,
                        Price = dish.Price,
                        Status = dish.Status,
                        ImagePath = dish.ImagePath
                    }
                };

                return CreatedAtAction(nameof(GetOrderDishById), new { id = orderDish.OrderDishId }, orderDishDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order dish: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDish(int id)
        {
            try
            {
                var orderDish = await _dbContext.OrderDishes.FirstOrDefaultAsync(od => od.OrderDishId == id);

                if (orderDish == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Order Dish not found",
                    });
                }

                _dbContext.OrderDishes.Remove(orderDish);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Order Dish deleted successfully",
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 400, // Bad Request status code
                    Message = "Error deleting Order Dish",
                    Data = e.Message // Provide exception message as Data
                });
            }

        }
    }
}
