using ComputerStoreApi.Models;
using System.Threading.Tasks;
namespace ComputerStoreApi.Services
{
    public interface IDiscountService
    {
        Task<(decimal TotalPrice, decimal Discount)> CalculateDiscount(Basket basket);
    }
}




