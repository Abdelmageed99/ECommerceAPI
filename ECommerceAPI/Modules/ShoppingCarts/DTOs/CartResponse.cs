namespace ECommerceAPI.Modules.ShoppingCarts.DTOs
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public string UserId { get; set; }

        public decimal TotalPrice { get; set; }
        public List<CartItemResponse> Items { get; set; }
    }

    

}
