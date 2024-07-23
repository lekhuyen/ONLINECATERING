using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public PaymentController(DatabaseContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAllPayments()
        {
            try
            {
                var payments = await _dbContext.Payments.ToListAsync();

                var paymentDTOs = payments.Select(payment => new PaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    TotalPrice = payment.TotalPrice,
                    Method = payment.Method,
                    Date = payment.Date,
                    Type = payment.Type
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get payments successfully",
                    Data = paymentDTOs
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetPaymentById(int id)
        {
            try
            {
                var payment = await _dbContext.Payments.FindAsync(id);

                if (payment == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Payment not found",
                    });
                }

                var paymentDTO = new PaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    TotalPrice = payment.TotalPrice,
                    Method = payment.Method,
                    Date = payment.Date,
                    Type = payment.Type
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get payment successfully",
                    Data = paymentDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Retrieve payment failed"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> CreatePayment(PaymentDTO paymentDTO)
        {
            try
            {
                // Check if the associated order exists
                var order = await _dbContext.Orders.FindAsync(paymentDTO.OrderId);
                if (order == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Order not found",
                        Data = null
                    });
                }

                // Map DTO to entity
                var newPayment = new Payment
                {
                    OrderId = paymentDTO.OrderId,
                    TotalPrice = paymentDTO.TotalPrice,
                    Method = paymentDTO.Method,
                    Date = paymentDTO.Date,
                    Type = paymentDTO.Type
                };

                // Add to DbContext
                await _dbContext.Payments.AddAsync(newPayment);
                await _dbContext.SaveChangesAsync();

                var updatedPaymentDTO = new PaymentDTO
                {
                    Id = newPayment.Id,
                    OrderId = newPayment.OrderId,
                    TotalPrice = newPayment.TotalPrice,
                    Method = newPayment.Method,
                    Date = newPayment.Date,
                    Type = newPayment.Type
                };

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Payment added successfully",
                    Data = updatedPaymentDTO
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error creating payment",
                    Data = e.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentDTO paymentDTO)
        {
            try
            {
                var existingPayment = await _dbContext.Payments.FindAsync(id);

                if (existingPayment == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Payment not found",
                    });
                }

                if (id != paymentDTO.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in object and parameter",
                        Data = null
                    });
                }

                // Update scalar properties
                existingPayment.OrderId = paymentDTO.OrderId;
                existingPayment.TotalPrice = paymentDTO.TotalPrice;
                existingPayment.Method = paymentDTO.Method;
                existingPayment.Date = paymentDTO.Date;
                existingPayment.Type = paymentDTO.Type;

                // Update entity in DbContext
                _dbContext.Payments.Update(existingPayment);
                await _dbContext.SaveChangesAsync();

                var updatedPaymentDTO = new PaymentDTO
                {
                    Id = existingPayment.Id,
                    OrderId = existingPayment.OrderId,
                    TotalPrice = existingPayment.TotalPrice,
                    Method = existingPayment.Method,
                    Date = existingPayment.Date,
                    Type = existingPayment.Type
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Payment updated successfully",
                    Data = updatedPaymentDTO
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error updating payment",
                    Data = e.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var paymentToDelete = await _dbContext.Payments.FindAsync(id);

                if (paymentToDelete == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Payment not found",
                    });
                }

                // Remove from DbContext and save changes
                _dbContext.Payments.Remove(paymentToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Payment deleted successfully",
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error deleting payment",
                    Data = e.Message
                });
            }
        }

        private bool PaymentExists(int id)
        {
            return _dbContext.Payments.Any(e => e.Id == id);
        }
    }
}
