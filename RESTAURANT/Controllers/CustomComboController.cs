using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public CustomComboController(DatabaseContext context)
        {
            _dbContext = context;
        }


    }
}
