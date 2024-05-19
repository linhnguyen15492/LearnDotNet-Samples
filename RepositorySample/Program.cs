using Microsoft.Data.SqlClient;
using RepositorySample.Entities;
using RepositorySample.Repository;
using RepositorySample.Repository.InMemory;
using RepositorySample.Repository.SqlServer;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RepositorySample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ""; // get from ConnectionStrings

            if (connectionString.Length > 0 )
            {
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                var trans = sqlConnection.BeginTransaction();

                var orderRepository = new SqlServerOrderRepository(sqlConnection, trans);
                var productRepository = new SqlServerProductRepository(sqlConnection, trans);

                InsertSampleProducts(productRepository);
                QueryProducts(productRepository);
                CreateOrders(orderRepository);
                QueryOrders(orderRepository);

                trans.Commit();
            }
            else
            {
                var orderRepository = new InMemoryOrderRepository();
                var productRepository = new InMemoryProductRepository();

                InsertSampleProducts(productRepository);
                QueryProducts(productRepository);
                CreateOrders(orderRepository);
                QueryOrders(orderRepository);
            }
        }

        private static void QueryOrders(IOrderRepository orderRepository)
        {
        }

        private static void CreateOrders(IOrderRepository orderRepository)
        {
        }

        private static void QueryProducts(IProductRepository productRepository)
        {
            Console.WriteLine("Product.Price >= 1000");
            var products = productRepository.Find(new ProductFindCreterias()
            {
                MinPrice = 1000,
            });
            PrintProducts(products);

            Console.WriteLine("Product.Price <= 1000");
            products = productRepository.Find(new ProductFindCreterias()
            {
                MaxPrice = 1000,
            });
            PrintProducts(products);

            Console.WriteLine("Product.Name contains iPad");
            products = productRepository.Find(new ProductFindCreterias()
            {
                Name = "iPad",
            });
            PrintProducts(products);
        }

        private static void PrintProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id, 20} {product.Name, 40} {product.Price, 10}");
            }
        }

        private static void InsertSampleProducts(IProductRepository productRepository)
        {
            productRepository.Add(new Product() { 
                Id = Guid.NewGuid(),
                Name = "Apple iPhone",
                Price = 999
            });

            productRepository.Add(new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Apple iPad",
                Price = 799
            });

            productRepository.Add(new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Apple Macbook",
                Price = 1399
            });
        }
    }
}
