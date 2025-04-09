using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }


        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
