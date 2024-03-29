using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreationDate { get; set; }
        public double PriceWithTax { get; set; }
        public List<inventory>? Inventories { get; set; }
        
    }
}
