using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace InventoryApp.Models
{
    public class inventory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double PriceWithoutTax { get; set; }
        public int Tax { get; set; }
        public double PriceWithTax {
            get
            {
                return PriceWithoutTax + PriceWithoutTax * Tax;
            }
            set { }
        }

        public int Quantity { get; set; }
        public double TotalPrice
        {
            get
            {
                return (PriceWithoutTax + PriceWithoutTax * Tax) * Quantity;
            }
            set { }        }
        public int CompanyId { get; set; }
        public Company? CompanyName { get; set; }


    }


}
