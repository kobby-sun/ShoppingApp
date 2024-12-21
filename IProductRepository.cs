using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp
{
    public interface IProductRepository
    {
        public List<Product> GetProducts();
    }
}
