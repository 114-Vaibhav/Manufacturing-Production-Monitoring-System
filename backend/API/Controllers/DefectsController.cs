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
    public class DefectsController : ControllerBase
    {
        private readonly IDefectServices _defectServices;
        private readonly IAuditLogService _auditLogService;

        public DefectsController(
            IDefectServices defectServices,
            IAuditLogService auditLogService)
        {
            _defectServices = defectServices;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Authorize(Roles =
            "Admin,QualityInspector,ProductionManager,PlantManager")]
        public async Task<ActionResult<IEnumerable<Defect>>> GetDefects()
        {
            var defects = await _defectServices.GetDefects();

            return Ok(defects);
        }

        [HttpGet("{id}")]
        [Authorize(Roles =
            "Admin,QualityInspector,ProductionManager,PlantManager,Operator")]
        public async Task<ActionResult<Defect>> GetDefect(int id)
        {
            var defect = await _defectServices.GetDefect(id);

            if (defect == null)
            {
                return NotFound();
            }

            return Ok(defect);
        }

        [HttpPost]
        [Authorize(Roles =
            "Admin,QualityInspector,Operator")]
        public async Task<ActionResult<Defect>> PostDefect(DefectRequest defect)
        {
            var createdDefect = await _defectServices.CreateDefect(defect, this.GetCurrentUserId());
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(Defect), createdDefect.DefectId);

            return CreatedAtAction(
                nameof(GetDefect),
                new { id = createdDefect.DefectId },
                createdDefect);
        }

        [HttpPut("{id}")]
        [Authorize(Roles =
            "Admin,QualityInspector,ProductionManager")]
        public async Task<ActionResult<Defect>> PutDefect(int id, DefectRequest defect)
        {
            var updatedDefect =
                await _defectServices.UpdateDefect(id, defect, this.GetCurrentUserId());
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(Defect), id);

            return Ok(updatedDefect);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager")]
        public async Task<ActionResult<Defect>> DeleteDefect(int id)
        {
            var deletedDefect =
                await _defectServices.DeleteDefect(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(Defect), id);

            return Ok(deletedDefect);
        }
    }
}
