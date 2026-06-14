using backend.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductionOrdersController : ControllerBase
    {
        private readonly IProductionOrderServices _productionOrderServices;

        public ProductionOrdersController(
            IProductionOrderServices productionOrderServices)
        {
            _productionOrderServices = productionOrderServices;
        }

        [HttpGet]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager," +
            "ProductionPlanner,PlantManager")]
        public async Task<ActionResult<IEnumerable<ProductionOrder>>>
            GetProductionOrders()
        {
            var orders =
                await _productionOrderServices.GetProductionOrder();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager," +
            "ProductionPlanner,PlantManager")]
        public async Task<ActionResult<ProductionOrder>>
            GetProductionOrder(int id)
        {
            var order =
                await _productionOrderServices.GetProductionOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles =
            "Admin,ProductionManager,PlantManager")]
        public async Task<ActionResult<ProductionOrder>>
            PostProductionOrder(ProductionOrder productionOrder)
        {
            var createdOrder =
                await _productionOrderServices
                    .CreateProductionOrder(productionOrder);

            return CreatedAtAction(
                nameof(GetProductionOrder),
                new { id = createdOrder.OrderId },
                createdOrder);
        }

        [HttpPut("{id}")]
        [Authorize(Roles =
            "Admin,ProductionManager,PlantManager")]
        public async Task<ActionResult<ProductionOrder>>
            PutProductionOrder(
                int id,
                ProductionOrder productionOrder)
        {
            var updatedOrder =
                await _productionOrderServices
                    .UpdateProductionOrder(id, productionOrder);

            if (updatedOrder == null)
            {
                return NotFound();
            }

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager")]
        public async Task<ActionResult<ProductionOrder>>
            DeleteProductionOrder(int id)
        {
            var deletedOrder =
                await _productionOrderServices
                    .DeleteProductionOrder(id);

            if (deletedOrder == null)
            {
                return NotFound();
            }

            return Ok(deletedOrder);
        }
    }
}