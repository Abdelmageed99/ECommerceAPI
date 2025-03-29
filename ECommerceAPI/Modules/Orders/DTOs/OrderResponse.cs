namespace ECommerceAPI.Modules.Orders.DTOs
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        //public string CustomerId { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public decimal TotalPrice { get; set; } 
        public List<OrderItemResponse> Items { get; set; }
    }
}
