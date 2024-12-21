using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingApp
{
    public class OrderLineItem
    {

        public string LineNumber { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public string DisplayDetails()
        {
            return Product.DisplayDetails() + " *" + Quantity + " -" + Discount + " = $" + ((Product.Price * Quantity) - Discount)  ;
        }
    }

    public class Order
    {
        public Order(ShoppingCart cart)
        {
            this.OrderLineItems = cart.Select(x => new OrderLineItem
            {
                LineNumber = Guid.NewGuid().ToString(),
                Product = x.Product,
                Quantity = 1,
            }).ToList();
        }

        public List<OrderLineItem> OrderLineItems { get; set; } =  new List<OrderLineItem>();


        public decimal Discount { get; set; }


        public decimal GetFinalPrice()
        {
            return OrderLineItems.Sum(x => x.Product.Price * x.Quantity) - OrderLineItems.Sum(x => x.Discount) - Discount;
        }

        public void AddProduct(Product product, int quantity)
        {
            if (product == null) throw new ArgumentNullException();

            if (this.OrderLineItems.Any(x => x.Product.Id == product.Id))
            {
                var item = this.OrderLineItems.First(x => x.Product.Id == product.Id);
                item.Quantity += quantity;
            } 
            else
            {
                this.OrderLineItems.Add(new OrderLineItem { Product = product, Quantity = quantity, LineNumber = Guid.NewGuid().ToString() });
            }

        }

        public decimal GetTotalPrice()
        {
            return OrderLineItems.Sum(x => x.Product.Price * x.Quantity);
        }

        public string DisplayDetails()
        {
            return string.Join("; ", OrderLineItems.Select(x => x.DisplayDetails()));
        }
    }
}
