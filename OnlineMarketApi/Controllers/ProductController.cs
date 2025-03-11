using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMarket.Api.Helpers;
using OnlineMarket.Application.Features.Products.Commands;
using OnlineMarket.Application.Features.Products.Queries;

namespace OnlineMarket.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator _mediator;

        public ProductController(ILogger<ProductController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogInformation("GetProducts called");
            var result = await _mediator.Send(new GetAllProductsQuery());
            return this.FromResultCode(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"GetProductById called for ID: {id}");
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return this.FromResultCode(result);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
        {
            _logger.LogInformation($"UpdateProduct called for ID: {command.ProductId}");
            var result = await _mediator.Send(command);
            return this.FromResultCode(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(CreateProductCommand command)
        {
            _logger.LogInformation("AddProduct called");
            var result = await _mediator.Send(command);
            return this.FromResultCode(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"DeleteProduct called for ID: {id}");
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return this.FromResultCode(result);
        }
    }
}
