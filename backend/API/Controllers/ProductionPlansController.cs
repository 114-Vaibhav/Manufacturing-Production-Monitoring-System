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
    public class ProductionPlansController : ControllerBase
    {
        private readonly IProductionPlanServices _productionPlanServices;
        private readonly IAuditLogService _auditLogService;

        public ProductionPlansController(
            IProductionPlanServices productionPlanServices,
            IAuditLogService auditLogService)
        {
            _productionPlanServices = productionPlanServices;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager,ProductionPlanner")]
        public async Task<ActionResult<IEnumerable<ProductionPlan>>>
            GetProductionPlans()
        {
            var plans =
                await _productionPlanServices.GetProductionPlan();

            return Ok(plans);
        }

        [HttpGet("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager,ProductionPlanner")]
        public async Task<ActionResult<ProductionPlan>>
            GetProductionPlan(int id)
        {
            var plan =
                await _productionPlanServices.GetProductionPlan(id);

            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        [HttpPost]
        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager,ProductionPlanner")]
        public async Task<ActionResult<ProductionPlan>>
            PostProductionPlan(ProductionPlanRequest productionPlan)
        {
            var createdPlan =
                await _productionPlanServices
                    .CreateProductionPlan(productionPlan, this.GetCurrentUserId());
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(ProductionPlan), createdPlan.PlanId);

            return CreatedAtAction(
                nameof(GetProductionPlan),
                new { id = createdPlan.PlanId },
                createdPlan);
        }

        [HttpPut("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager,ProductionPlanner")]
        public async Task<ActionResult<ProductionPlan>>
            PutProductionPlan(
                int id,
                ProductionPlanRequest productionPlan)
        {
            var updatedPlan =
                await _productionPlanServices
                    .UpdateProductionPlan(id, productionPlan, this.GetCurrentUserId());
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(ProductionPlan), id);

            if (updatedPlan == null)
            {
                return NotFound();
            }

            return Ok(updatedPlan);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager")]
        public async Task<ActionResult<ProductionPlan>>
            DeleteProductionPlan(int id)
        {
            var deletedPlan =
                await _productionPlanServices
                    .DeleteProductionPlan(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(ProductionPlan), id);

            if (deletedPlan == null)
            {
                return NotFound();
            }

            return Ok(deletedPlan);
        }
    }
}
