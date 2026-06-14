using backend.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductionAnalyticsController : ControllerBase
    {
        private readonly IProductionAnalyticsServices _productionAnalyticsServices;

        public ProductionAnalyticsController(
            IProductionAnalyticsServices productionAnalyticsServices)
        {
            _productionAnalyticsServices = productionAnalyticsServices;
        }

        // 1. KEEP: Get all production analytics for dashboard visualization
        [HttpGet]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager," +
            "ProductionPlanner,PlantManager," +
            "QualityInspector,MaintenanceTechnician")]
        public async Task<ActionResult<IEnumerable<ProductionAnalytics>>>
            GetProductionAnalytics()
        {
            var analytics =
                await _productionAnalyticsServices.GetProductionAnalytics();

            return Ok(analytics);
        }

    }
}