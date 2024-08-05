using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RESTAURANT.API.Models;
using RESTAURANT.API.Servicer;
using StackExchange.Redis;
using System;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentBookController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly DatabaseContext _dbContext;
        public PaymentBookController(IVnPayService vnPayService, DatabaseContext dbContext)
        {
            _vnPayService = vnPayService;
            _dbContext = dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Url = url
            });
        }

        [HttpGet]

        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            var orderDescription = response.OrderDescription;
            var parts = orderDescription.Split(',');
            var desiredValue = parts.Length > 3 ? parts[3] : null;
            if (desiredValue != null && int.TryParse(desiredValue, out int orderId))
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if(order != null)
                {
                    order.StatusPayment = true;

                    _dbContext.Orders.Update(order);
                    await _dbContext.SaveChangesAsync();
                }
                var success = "Payment success";
                var url = $"http://localhost:3000/ordercombo/{success}";
                return Redirect(url);
                //return Redirect("http://localhost:3000/");
            }
            return Ok(response);
            
        }

        
    }
}
