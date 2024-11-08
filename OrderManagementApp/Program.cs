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

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine();
            Console.WriteLine("Wybierz opcję:");
            Console.WriteLine("1. Dodaj produkt do zamówienia");
            Console.WriteLine("2. Usuń produkt z zamówienia");
            Console.WriteLine("3. Wyświetl wartość zamówienia");
            Console.WriteLine("4. Dodaj nowy produkt");
            Console.WriteLine("5. Wyjdź");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddProductToOrder(productController, orderController);
                    break;
                case "2":
                    RemoveProductFromOrder(orderController,productController);
                    break;
                case "3":
                    DisplayOrderTotal(orderController);
                    break;
                case "4":
                    AddNewProduct(productController);
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Wybrano nieprawidłową opcję.");
                    break;
            }
        }
    }

    private static void SeedProducts(ProductController productController)
    {
        productController.AddProduct("Laptop", 2500m);
        productController.AddProduct("Klawiatura", 120m);
        productController.AddProduct("Mysz", 90m);
        productController.AddProduct("Monitor", 1000m);
        productController.AddProduct("Kaczka debuggująca", 66m);
    }

    private static void AddProductToOrder(ProductController productController, OrderController orderController)
    {
        Console.WriteLine("Dostępne produkty:");
        var products = productController.GetProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id}. {product.Name} - {product.Price:F2} PLN");
        }

        Console.Write("Wpisz ID produktu do dodania: ");
        string inputProductId = Console.ReadLine();

        if (!int.TryParse(inputProductId, out int productId) ||
            !products.Any(p => p.Id == productId))
        {
            Console.WriteLine("ID jest niepoprawne.");
            return;
        }

        Console.Write("Podaj ilość: ");
        string inputQuantity = Console.ReadLine();

        if (!int.TryParse(inputQuantity, out int quantity) || quantity <= 0)
        {
            Console.WriteLine("ID jest niepoprawne.");
            return;
        }
        orderController.AddProductToOrder(productId, quantity);
    }

    private static void RemoveProductFromOrder(OrderController orderController, ProductController productController)
    {
        Console.WriteLine("Dostępne produkty:");
        var products = productController.GetProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id}. {product.Name} - {product.Price:F2} PLN");
        }
        Console.Write("Wpisz ID produktu do usunięcia z zamówienia: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            orderController.RemoveProductFromOrder(productId);
            Console.WriteLine("Produkt usunięty z zamówienia.");
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID produktu.");
        }
    }

    private static void DisplayOrderTotal(OrderController orderController)
    {
        var orderItems = orderController.GetOrderItems();

        if (orderItems.Count == 0)
        {
            Console.WriteLine("Zamówienie jest puste.");
            return;
        }

        Console.WriteLine("Zamówione produkty:");
        foreach (var item in orderItems)
        {
            Console.WriteLine($"- {item.ProductName}: {item.Quantity} szt. x {item.Price} PLN");
        }

        decimal total = orderController.GetOrderTotal();
        Console.WriteLine($"Łączna wartość zamówienia (po zniżkach): {total:F2} PLN");
    }

    private static void AddNewProduct(ProductController productController)
    {
        Console.Write("Wpisz nazwę produktu: ");
        var name = Console.ReadLine();

        Console.Write("Wpisz cenę produktu: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            bool isAdded = productController.AddProduct(name, price);

            if (isAdded)
            {
                Console.WriteLine("Produkt dodany pomyślnie.");
            }
            else
            {
                Console.WriteLine("Produkt o tej nazwie już istnieje. Nie można dodać duplikatu.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowa cena.");
        }
    }
}