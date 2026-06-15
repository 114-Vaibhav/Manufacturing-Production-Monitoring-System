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
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly IAuditLogService _auditLogService;

        public ProductsController(
            IProductServices productServices,
            IAuditLogService auditLogService)
        {
            _productServices = productServices;
            _auditLogService = auditLogService;
        }


        [HttpGet]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager,ProductionPlanner,PlantManager,QualityInspector")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var products = await _productServices.GetProducts(pageNumber, pageSize);

            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager,ProductionPlanner,PlantManager,QualityInspector")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productServices.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles =
            "Admin,ProductionPlanner,ProductionManager,PlantManager")]
        public async Task<ActionResult<Product>> PostProduct(ProductRequest product)
        {
            var createdProduct = await _productServices.CreateProduct(product);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(Product), createdProduct.ProductId);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = createdProduct.ProductId },
                createdProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles =
            "Admin,ProductionPlanner,ProductionManager,PlantManager")]
        public async Task<ActionResult<Product>> PutProduct(int id, ProductRequest product)
        {
            var updatedProduct = await _productServices.UpdateProduct(id, product);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(Product), id);

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var deletedProduct = await _productServices.DeleteProduct(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(Product), id);

            return Ok(deletedProduct);
        }
    }
}
