using ComputerStoreApi.Models;

namespace ComputerStoreApi.Models
{
    public class BasketItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        
    }



}



