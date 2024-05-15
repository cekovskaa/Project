using ComputerStoreApi.Models;
using ComputerStoreApi.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreApi.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IProductRepository _productRepository;

        public DiscountService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<(decimal TotalPrice, decimal Discount)> CalculateDiscount(Basket basket)
        {
            decimal totalPrice = 0;
            decimal totalDiscount = 0;

            
            var groupedItems = basket.Items.GroupBy(i => i.Product.CategoryId);

            foreach (var group in groupedItems)
            {
                for (int i = 0; i < group.Count(); i++)
                {
                    var item = group.ElementAt(i);
                    var price = item.Product.Price * item.Quantity;

                    
                    if (i == 0 && group.Count() > 1)
                    {
                        var discount = price * 0.05m; 
                        price -= discount; 
                        totalDiscount += discount; 
                    }

                    totalPrice += price;
                }
            }

            return (totalPrice, totalDiscount);
        }

    }
}



