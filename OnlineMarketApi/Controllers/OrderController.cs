using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMarket.Api.Helpers;
using OnlineMarket.Api.Models;
using OnlineMarket.Application.Features.Orders.Commands;
using OnlineMarket.Application.Features.Orders.Queries;
using System.Security.Claims;

namespace OnlineMarket.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;

        public OrderController(ILogger<OrderController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _mediator.Send(new GetOrdersQuery());
            return this.FromResultCode(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            return this.FromResultCode(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(
            List<AddOrderApiModel> productIds)
        {
            _logger.LogInformation("Add order called");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var result = await _mediator.Send(new CreateOrderCommand(productIds, userId));
            return this.FromResultCode(result);
        }
    }
}
