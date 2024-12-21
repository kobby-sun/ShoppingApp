// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using ShoppingApp;

Console.WriteLine("Hello, Shopping App!");

IProductRepository repo = new ProductRepository();
var products = repo.GetProducts();

var cart = new ShoppingCart();
foreach (var item in products)
{
    cart.Add(item);
}

Console.WriteLine("Cart Info: " + cart.Display());

Console.WriteLine("Cart Search by name: " + JsonConvert.SerializeObject(cart.FilterProducts(new ProductFilter { Name = "jig"})));

Console.WriteLine("Cart Search by price: " + JsonConvert.SerializeObject(cart.FilterProducts(new ProductFilter { Range = new PriceRange { Min = 200, Max = 299 } })));


Console.WriteLine(JsonConvert.SerializeObject(cart));

Console.WriteLine("Total Price: " + cart.GetTotalPrice());



Console.WriteLine("Create Order " );
var order = new Order(cart);


Console.WriteLine("Add Quantity ");
order.AddProduct(products.First(x => x.Id == "111"), 1);

DiscountManager.ApplyDiscounts(order);

Console.WriteLine("Order Info: " + order.DisplayDetails());

Console.WriteLine("Order Total Price: " + order.GetTotalPrice());
Console.WriteLine("Order Final Price: " + order.GetFinalPrice());

var name = Console.ReadLine();