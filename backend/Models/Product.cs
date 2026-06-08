using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public enum ProductStatus
    {
        Active,
        Discontinued,
        OutOfStock
    }
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductCode { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
