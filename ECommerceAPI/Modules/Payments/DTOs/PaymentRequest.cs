﻿namespace ECommerceAPI.Modules.Payments.DTOs
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
