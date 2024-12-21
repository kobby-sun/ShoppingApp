using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingApp
{
    public class PriceRange
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

    public class ProductFilter
    {
        public string Name { get; set; }
        public PriceRange Range { get; set; }
    }

    public class ShoppingCartItem
    {
        public Product Product { get; set; }
    }

    public class ShoppingCart : IEnumerable<ShoppingCartItem>
    {
        List<ShoppingCartItem> items { get; set; } = new List<ShoppingCartItem>();

        public IEnumerator<ShoppingCartItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Product product)
        {
            if (product == null) throw new ArgumentNullException();
            if (this.Any(x => x.Product.Id == product.Id))  throw new DuplicateProductException();
            items.Add(new ShoppingCartItem { Product = product});
        }

        public decimal GetTotalPrice()
        {
            return items.Sum(x => x.Product.Price);

        }

        public List<ShoppingCartItem> FilterProducts(ProductFilter filter)
        {
            if (filter == null) throw new ArgumentNullException();


            var query = items.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Product.Name.ToLower().IndexOf(filter.Name.ToLower()) > -1);
            }
            if (filter.Range != null)
            {
                query = query.Where(x => (filter.Range.Min == 0 || x.Product.Price >= filter.Range.Min) && (filter.Range.Max == 0 || x.Product.Price <= filter.Range.Max));
            }

            return query.ToList();
        }

        public string Display()
        {
            return string.Join(", ", items.Select(x => x.Product.DisplayDetails()));
        }

        public static void Save(ShoppingCart card)
        {
            string json = JsonConvert.SerializeObject(card);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "shoppingcard.json",
                  json);
        }

        public static ShoppingCart Load()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + "shoppingcard.json")) throw new FileNotFoundException();
            try
            {
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "shoppingcard.json");
                return JsonConvert.DeserializeObject<ShoppingCart>(json);
            }
            catch (Exception ex) { throw new Exception("Unable to load shopping card"); }

        }
    }
}
