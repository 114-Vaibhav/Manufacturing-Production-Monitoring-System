using backend.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefectsController : ControllerBase
    {
        private IDefectServices _defectServices;

        public DefectsController(IDefectServices defectServices)
        {
            _defectServices = defectServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Defect>>> GetDefects()
        {
            var defects = await _defectServices.GetDefects();

            return Ok(defects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Defect>> GetDefect(int id)
        {
            var defect = await _defectServices.GetDefect(id);

            if (defect == null)
            {
                return NotFound();
            }

            return defect;
        }

        [HttpPost]
        public async Task<ActionResult<Defect>> PostDefect(Defect defect)
        {
            var createdDefect = await _defectServices.CreateDefect(defect);

            return CreatedAtAction(
                nameof(GetDefect),
                new { id = createdDefect.DefectId },
                createdDefect);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Defect>> PutDefect(int id, Defect defect)
        {
            var updatedDefect = await _defectServices.UpdateDefect(id, defect);

            return Ok(updatedDefect);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Defect>> DeleteDefect(int id)
        {
            var deletedDefect = await _defectServices.DeleteDefect(id);

            return Ok(deletedDefect);
        }
    }
}
