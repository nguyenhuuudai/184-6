using MyBizApplication.controller;
using MyBizApplication.model;
using MyBizApplication.service;
using Org.BouncyCastle.Crypto.Engines;

namespace MyBizApplication
{
    class Program
    {
        public static void Main(string[] args)
        {
            string connetionString = "server=localhost;database=prodb;user=root;password=";
            //Model CRUD product
            IProductRepository productServivce = new ProductService(connetionString);
            ProductController productController = new ProductController(productServivce);
            //Model Order management
            IOrderRepository orderService = new OrderService(connetionString);
            OrderController orderController = new OrderController(orderService);


            while (true)
            {
                Console.WriteLine("My Biz Application");
                Console.WriteLine("1. Add product");
                Console.WriteLine("2. Display all products");
                Console.WriteLine("3. Add order");
                Console.WriteLine("4. Display all orders");
                Console.WriteLine("5. Exit");

                Console.WriteLine("Enter your name: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter product name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter price: ");
                        decimal price = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Decription: ");
                        string description = Console.ReadLine();
                        Product newProduct = new Product { Name = name, Price = price, Description = description };
                        // Product newProduct = new Product();
                        // newProduct.Name = name;
                        // newProduct.Price = price;
                        productController.AddProduct(newProduct);
                        Console.WriteLine("Product added successfully!!!");
                        break;
                    case 2:
                        List<Product> products = productController.GetAllProducts();
                        foreach (var product in products)
                        {
                            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Description: {product.Description}");
                        }

                        break;
                    case 3: //add order 
                        Console.WriteLine("Enter customer id: ");
                        int orderCustomerId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter numbder of products");
                        int numberOfProducts = Convert.ToInt32(Console.ReadLine());
                        List<OrderDetail> orderDetails = new List<OrderDetail>();
                        for(int i =0; i< numberOfProducts; i++){
                            Console.WriteLine($"Enter product Id for product {i+1}: ");
                            int orderIdProduct = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter quantity: ");
                            int quantity = Convert.ToInt32(Console.ReadLine());
                            orderDetails.Add(new OrderDetail{ProductId = orderIdProduct,Quantity = quantity });
                        }
                        Order newOrder = new Order{
                            CustomerId = orderCustomerId,
                            OrderDate = DateTime.Now,
                            OrderDetails = orderDetails
                        };
                        orderController.AddOrder(newOrder);
                        Console.WriteLine("Order added succeesfully!!!");
                        break;
                    case 4: //display alll orders 
                        List<Order> orders = orderController.GetAllOrdres();
                        foreach(var order in orders){
                            //List ra các đơn hàng đang có
                            Console.WriteLine($"Order id: {order.Id}, Customer id: {order.CustomerId}, Order date: {order.OrderDate}");

                            foreach(var detail in order.OrderDetails){
                                //List ra các chi tiết đơn hàng của mỗi đơn hàng trong foreach ở trên
                                Console.WriteLine($"Product id: {detail.ProductId}, Quantity: {detail.Quantity}");

                            }


                        }

                        break;
                    case 5: return;
                    default:
                        Console.WriteLine("Invalid choice! pls try again");
                        break;
                }



            }

        }
    }
}