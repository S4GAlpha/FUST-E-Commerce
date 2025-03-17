using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        [Column("category_id")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
