using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemQuote
{
   public class ItemQuote
    {
        public long ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public bool Discounted { get; set; }
        public bool InStock { get; set; }

        public ItemQuote() { }
        public ItemQuote(long itemNumber, string itemDescription, int quantity,
            int unitPrice, bool discounted, bool inStock)
        {
            ItemNumber = itemNumber;
            ItemDescription = itemDescription;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discounted = discounted;
            InStock = inStock;
        }

        public override string ToString()
        {
            string value = "Item# = " + ItemNumber + "\n" + "Description = " + ItemDescription + "\n" +
                "Quantity = " + Quantity + "\n" + "Price(each) = " + UnitPrice + "\n" + "Total Price = " + (Quantity * UnitPrice);
            if (Discounted)
                value += " (discounted)";
            if (InStock)
                value += "\n" + "In Stock" + "\n";
            else
                value += "\n" + "Out Of Stock" + "\n";

            return value;
        }  
    }
}
