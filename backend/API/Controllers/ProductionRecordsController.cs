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
    public class ProductionRecordsController : ControllerBase
    {
        private readonly IProductionRecordServices _productionRecordServices;
        private readonly IAuditLogService _auditLogService;

        public ProductionRecordsController(
            IProductionRecordServices productionRecordServices,
            IAuditLogService auditLogService)
        {
            _productionRecordServices = productionRecordServices;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager," +
            "ProductionPlanner,PlantManager," +
            "QualityInspector")]
        public async Task<ActionResult<IEnumerable<ProductionRecord>>>
            GetProductionRecords()
        {
            var records =
                await _productionRecordServices.GetProductionRecord();

            return Ok(records);
        }

        [HttpGet("{id}")]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager," +
            "ProductionPlanner,PlantManager," +
            "QualityInspector")]
        public async Task<ActionResult<ProductionRecord>>
            GetProductionRecord(int id)
        {
            var record =
                await _productionRecordServices.GetProductionRecord(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager,PlantManager")]
        public async Task<ActionResult<ProductionRecord>>
            PostProductionRecord(ProductionRecordRequest productionRecord)
        {
            var createdRecord =
                await _productionRecordServices
                    .CreateProductionRecord(productionRecord);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(ProductionRecord), createdRecord.Id);

            return CreatedAtAction(
                nameof(GetProductionRecord),
                new { id = createdRecord.Id },
                createdRecord);
        }

        [HttpPut("{id}")]
        [Authorize(Roles =
            "Admin,ProductionManager,PlantManager")]
        public async Task<ActionResult<ProductionRecord>>
            PutProductionRecord(
                int id,
                ProductionRecordRequest productionRecord)
        {
            var updatedRecord =
                await _productionRecordServices
                    .UpdateProductionRecord(id, productionRecord);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(ProductionRecord), id);

            if (updatedRecord == null)
            {
                return NotFound();
            }

            return Ok(updatedRecord);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager")]
        public async Task<ActionResult<ProductionRecord>>
            DeleteProductionRecord(int id)
        {
            var deletedRecord =
                await _productionRecordServices
                    .DeleteProductionRecord(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(ProductionRecord), id);

            if (deletedRecord == null)
            {
                return NotFound();
            }

            return Ok(deletedRecord);
        }
    }
}
