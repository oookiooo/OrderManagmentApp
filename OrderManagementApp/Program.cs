using OrderManagementApp.Interfaces;
using OrderManagementApp.Services;
using OrderManagementApp;
using OrderManagementApp.Controllers;

class Program
{
    static void Main(string[] args)
    {
        IProductService productService = new ProductService();
        IOrderService orderService = new OrderService();

        var productController = new ProductController(productService);
        var orderController = new OrderController(orderService, productService);
        SeedProducts(productController);
    private static void SeedProducts(ProductController productController)
    {
        productController.AddProduct("Laptop", 2500m);
        productController.AddProduct("Klawiatura", 120m);
        productController.AddProduct("Mysz", 90m);
        productController.AddProduct("Monitor", 1000m);
        productController.AddProduct("Kaczka debuggująca", 66m);
    }
    }
}