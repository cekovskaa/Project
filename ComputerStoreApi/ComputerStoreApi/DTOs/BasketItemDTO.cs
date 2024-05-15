namespace ComputerStoreApi.DTOs
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
    }
}
