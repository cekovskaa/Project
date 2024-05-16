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


            var categories = basket.Items.GroupBy(i => i.Product.CategoryId);

            foreach (var category in categories)
            {
                for (int i = 0; i < category.Count(); i++)
                {
                    var item = category.ElementAt(i);
                    var price = item.Product.Price * item.Quantity;

                    totalPrice += price;
                }

                var totalItems = 0;
                for (int i = 0; i < category.Count(); i++) {
                    var item = category.ElementAt(i);
                    totalItems += item.Quantity;
                }

                if (totalItems > 1) {
                    var eligibleItems = category.Distinct();

                    for (int i = 0; i < eligibleItems.Count(); i++)
                    {
                        var item = eligibleItems.ElementAt(i);
                        var discountAmount = item.Product.Price * (decimal)0.05;
                        totalDiscount += discountAmount;
                    }
                }
            }

            return (totalPrice-totalDiscount, totalDiscount);
        }

    }
}



