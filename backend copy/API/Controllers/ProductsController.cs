using backend.Models;
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

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }


        [HttpGet]
        [Authorize(Roles =
            "Admin,Operator,ProductionManager,ProductionPlanner,PlantManager,QualityInspector")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productServices.GetProducts();

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
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var createdProduct = await _productServices.CreateProduct(product);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = createdProduct.ProductId },
                createdProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles =
            "Admin,ProductionPlanner,ProductionManager,PlantManager")]
        public async Task<ActionResult<Product>> PutProduct(int id, Product product)
        {
            var updatedProduct = await _productServices.UpdateProduct(id, product);

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles =
            "Admin,PlantManager")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var deletedProduct = await _productServices.DeleteProduct(id);

            return Ok(deletedProduct);
        }
    }
}