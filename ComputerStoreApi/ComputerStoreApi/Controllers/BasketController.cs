using ComputerStoreApi.Models;
using ComputerStoreApi.Services;
using ComputerStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerStoreApi.DTOs;

namespace ComputerStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IProductRepository _productRepository;  

        public BasketController(IDiscountService discountService, IProductRepository productRepository)  
        {
            _discountService = discountService;
            _productRepository = productRepository;  
        }

        private static Basket _basket = new Basket();

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToBasket([FromBody] ProductInputModel productInput, [FromQuery] int quantity)
        {
            
            var product = await _productRepository.GetProductByIdAsync(productInput.Id, includeCategory: true);

            _basket.AddProduct(product, quantity);
            return Ok();
        }



        [HttpGet("get-basket")]
        public IActionResult GetBasket()
        {
            var basketDto = new BasketDto
            {
                Items = _basket.Items.Select(i => new BasketItemDto
                {
                    ProductId = i.Product.Id,
                    ProductName = i.Product.Name,
                    CategoryName = i.Product.Category.Name,
                    Quantity = i.Quantity
                }).ToList()
            };

            return Ok(basketDto);
        }


        [HttpGet("calculate-discount")]
        public async Task<IActionResult> CalculateDiscount()
        {
            var (totalPrice, discount) = await _discountService.CalculateDiscount(_basket);
            return Ok(new { TotalPrice = totalPrice, Discount = discount });
        }



    }
}



