using AutoMapper;
using ComputerStoreApi.DTOs;
using ComputerStoreApi.Models;
using ComputerStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputerStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound("The product you're trying to access does not exist within the database.");
            }

            return _mapper.Map<ProductDTO>(product);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDto)
        {
            var category = await _productRepository.GetCategoryByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "The CategoryId does not exist." });
            }

            var product = _mapper.Map<Product>(productDto);
            product.Id = id;

            try
            {
                await _productRepository.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Product with the given ID not found.")
                {
                    return NotFound(new { message = "Product with the given ID not found." });
                }
                else
                {
                    return StatusCode(500, "An error occurred while trying to update the product.");
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDto)
        {
            if (string.IsNullOrEmpty(productDto.Name) || productDto.Price <= 0)
            {
                return BadRequest("The Name of the product is required and the Price must be greater than zero.");
            }

            var product = _mapper.Map<Product>(productDto);
            try
            {
                await _productRepository.CreateProductAsync(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while trying to create the product. ");
            }

            return CreatedAtAction("GetProduct", new { id = product.Id }, _mapper.Map<ProductDTO>(product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Product with the given ID not found.")
                {
                    return NotFound(new { message = "Product with the given ID not found." });
                }
                else
                {
                    return StatusCode(500, "An error occurred while trying to delete the product.");
                }
            }

            return NoContent();
        }


        [HttpPost("import")]
        public async Task<IActionResult> ImportStock([FromBody] List<StockImportDTO> stockData)
        {
            foreach (var item in stockData)
            {
               
                var product = await _productRepository.GetProductByNameAsync(item.Name);
                if (product == null)
                {
                    product = new Product
                    {
                        Name = item.Name,
                        Description = "", 
                        Price = item.Price,
                        
                    };

                    
                    foreach (var categoryName in item.Categories)
                    {
                        var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
                        if (category == null)
                        {
                            category = new Category
                            {
                                Name = categoryName,
                                Description = "Default description" 
                            };
                            await _categoryRepository.CreateCategoryAsync(category);
                        }
                        product.CategoryId = category.Id;
                    }

                    await _productRepository.CreateProductAsync(product);
                }

                // Update the stock of the product
                await _productRepository.UpdateProductStockAsync(product.Name, item.Quantity);
            }

            return Ok();
        }


    }
}
