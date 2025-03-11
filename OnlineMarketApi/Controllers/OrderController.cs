using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMarket.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        public OrderController()
        {
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            return Ok("GetOrders called");
        }
    }
}
