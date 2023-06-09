﻿namespace Notification.API.Models
{
    public class PriceChangeRequest
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal NewPrice { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
    }
}
