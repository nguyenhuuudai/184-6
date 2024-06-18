using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBizApplication.model;
using MySql.Data.MySqlClient;

namespace MyBizApplication.service
{
    public class OrderService : IOrderRepository
    {
        private readonly string connectionString;
        public OrderService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void AddOrder(Order order)
        {
            //throw new NotImplementedException();
            //Insert đồng thời 2 Entities: Order và OrderDetail
            //Đưa và 1 Transaction để đảm bảo các giao dịch diễn ra.
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {//start Transaction
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = "insert into orders(customer_id, order_date) values(@customerId, @orderDate)";
                    cmd.Parameters.AddWithValue("@customerId", order.CustomerId);
                    cmd.Parameters.AddWithValue("@orderDate", order.OrderDate);
                    cmd.ExecuteNonQuery();
                    int orderId = (int)cmd.LastInsertedId;//Lay Id cua bang Orders 

                    foreach (var detail in order.OrderDetails)
                    {
                        MySqlCommand detailCmd = conn.CreateCommand();
                        detailCmd.Transaction = transaction;
                        detailCmd.CommandText = "insert into order_details(order_id,product_id,quantity) values (@orderId,@productId, @quantity)";
                        detailCmd.Parameters.AddWithValue("@orderId", orderId);
                        detailCmd.Parameters.AddWithValue("@productId", detail.ProductId);
                        detailCmd.Parameters.AddWithValue("@quantity", detail.Quantity);
                        detailCmd.ExecuteNonQuery();

                    }
                    transaction.Commit();//Finish Transaction
                }

            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from orders";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = reader.GetInt32("id"),
                            CustomerId = reader.GetInt32("customer_id"),
                            OrderDate = reader.GetDateTime("order_date"),
                            OrderDetails = new List<OrderDetail>()

                        };
                        orders.Add(order);
                    }
                }
                //Sau khi load được order từ bảng Orders. Cần lấy ra chi tiết của Order trong Order_Details
                foreach(var order in orders){
                    MySqlCommand detailCmd = conn.CreateCommand();
                    detailCmd.CommandText = "select * from order_details where order_id = @orderId";
                    detailCmd.Parameters.AddWithValue("@orderId",order.Id);
                    using(MySqlDataReader detailReader = detailCmd.ExecuteReader()){
                        while(detailReader.Read()){
                            OrderDetail detail = new OrderDetail
                            {
                                ProductId = detailReader.GetInt32("product_id"),
                                Quantity = detailReader.GetInt32("quantity")
                            };
                            order.OrderDetails.Add(detail);
                        }
                    }        
                }
            }
            return orders;
        }

        public Order GetOrderById(int id)
        {
            throw new NotImplementedException();
        }
    }
}