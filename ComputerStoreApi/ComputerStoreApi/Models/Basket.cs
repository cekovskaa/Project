namespace ComputerStoreApi.Models
{
    public class Basket
    {
        public List<BasketItem> Items { get; set; }

        public Basket()
        {
            Items = new List<BasketItem>();
        }

        public void AddProduct(Product product, int quantity)
        {
            var existingProduct = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
            }
            else
            {
                Items.Add(new BasketItem
                {
                    Product = product,
                    Quantity = quantity,
                    ProductName = product.Name,  
                    CategoryName = product.Category.Name  
                });
            }
        }
    }
}

