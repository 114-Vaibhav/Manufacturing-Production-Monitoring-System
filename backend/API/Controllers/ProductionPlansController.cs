using backend.Models;
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

        public ProductionPlansController(
            IProductionPlanServices productionPlanServices)
        {
            _productionPlanServices = productionPlanServices;
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
            PostProductionPlan(ProductionPlan productionPlan)
        {
            var createdPlan =
                await _productionPlanServices
                    .CreateProductionPlan(productionPlan);

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
                ProductionPlan productionPlan)
        {
            var updatedPlan =
                await _productionPlanServices
                    .UpdateProductionPlan(id, productionPlan);

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

            if (deletedPlan == null)
            {
                return NotFound();
            }

            return Ok(deletedPlan);
        }
    }
}