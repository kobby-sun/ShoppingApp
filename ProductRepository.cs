using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository()
        {
            using (var context = new ShoppingDbContext())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Id = "111",
                        Name = "Chainsaw",
                        Price = 299
                    },

                    new Product
                    {
                        Id = "222",
                        Name = "Jigsaw",
                        Price = 199
                    },

                    new Product
                    {
                        Id = "333",
                        Name = "Circular Saw",
                        Price = 399
                    }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

        public List<Product> GetProducts()
        {
            using (var context = new ShoppingDbContext())
            {
                var list = context.Products
                    .ToList();
                return list;
            }
        }
    }
}
