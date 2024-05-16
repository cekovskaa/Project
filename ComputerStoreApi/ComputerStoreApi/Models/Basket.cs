namespace ComputerStoreApi.Models
{
    public class Basket
    {
        public List<BasketItem> Items { get; set; }

        public Basket()
        {
            Items = new List<BasketItem>();
        }

        public int AddProduct(Product product, int quantity)
        {
            var existingProduct = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            var stock = product.Quantity;

            if (quantity > stock)
            {
                return quantity - stock;
            }

            if (existingProduct != null)
            {
                var new_quantity = existingProduct.Quantity + quantity;
                if (new_quantity > stock) {
                    return new_quantity - stock;
                }
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

            return 0;
        }
    }
}

