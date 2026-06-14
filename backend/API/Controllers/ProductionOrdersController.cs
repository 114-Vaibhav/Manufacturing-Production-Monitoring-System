using API.Services;
using backend.Models;
using backend.Models.DTOs;
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
        private readonly IAuditLogService _auditLogService;

        public ProductionOrdersController(
            IProductionOrderServices productionOrderServices,
            IAuditLogService auditLogService)
        {
            _productionOrderServices = productionOrderServices;
            _auditLogService = auditLogService;
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
            PostProductionOrder(ProductionOrderRequest productionOrder)
        {
            var createdOrder =
                await _productionOrderServices
                    .CreateProductionOrder(productionOrder);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(ProductionOrder), createdOrder.OrderId);

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
                ProductionOrderRequest productionOrder)
        {
            var updatedOrder =
                await _productionOrderServices
                    .UpdateProductionOrder(id, productionOrder);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(ProductionOrder), id);

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
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(ProductionOrder), id);

            if (deletedOrder == null)
            {
                return NotFound();
            }

            return Ok(deletedOrder);
        }
    }
}
