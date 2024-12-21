using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp
{
    public interface IDiscount
    {
        decimal Apply(Order order);

        bool IsValid(Order order);

        bool IsStackable { get; }
    }

    public class FixAmountDiscount : IDiscount
    {
        const decimal discountFixed = 10m;

        public decimal Apply(Order order)
        {

            if (IsValid(order))
            {
                order.Discount = discountFixed;
                return discountFixed;
            }

            return 0;
        }

        public bool IsValid(Order order)
        {
            return order.GetTotalPrice() > 500;
        }

        public bool IsStackable { get { return true; } }
    }

    public class PercentageDiscount : IDiscount
    {

        const decimal discountPercent = 5 / 100m;

        public decimal Apply(Order order)
        {
            decimal totalDiscount = 0;
            if (IsValid(order))
            {
                foreach (var item in order.OrderLineItems)
                {
                    var discount = (item.Product.Price * discountPercent) * item.Quantity;
                    totalDiscount += discount;

                    Console.WriteLine("item.Product.Price " + item.Product.Price );
                    Console.WriteLine("item.Quantity " + item.Quantity);
                    Console.WriteLine("item.TotalPrice " + item.Product.Price * item.Quantity);
                    Console.WriteLine("item.TotalDiscount " + discount);
                    item.Discount += discount;
                }
            }
            return totalDiscount;
        }

        public bool IsValid(Order order)
        {
            Console.WriteLine("Order.GetTotalProductPrice() " + order.GetTotalPrice());
            return order.GetTotalPrice() > 200;
        }

        public bool IsStackable
        {
            get { return true; }
        }
    }

    public static class DiscountManager
    {
        private static readonly List<IDiscount> DiscountRules = new List<IDiscount>
        {
            new PercentageDiscount(),
            new FixAmountDiscount(),
        };

        public static void ApplyDiscounts(Order order)
        {
            foreach (var item in order.OrderLineItems)
            {
                item.Discount = 0;
            }
            foreach (var rule in DiscountRules)
            {
                if (rule.IsValid(order)) {
                    rule.Apply(order);
                }

            }

            Console.WriteLine("order.Discount " + order.Discount);
        }
    }
}
