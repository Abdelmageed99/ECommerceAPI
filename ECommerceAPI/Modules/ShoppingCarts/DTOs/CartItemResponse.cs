﻿namespace ECommerceAPI.Modules.ShoppingCarts.DTOs
{
    public class CartItemResponse
    {
        public int CartItemId { get; set; }
        
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        

        
    }

        
}
