using Model;
using MySqlConnector;
using BusinessEnum;
using MySqlConnector.Logging;
using System.Runtime.InteropServices;

namespace DAL
{
    public class OrderDAL
    {
        private MySqlConnection connection = DbConfig.GetConnection();
        private string query = "";
        public Order GetOrder(MySqlDataReader reader)
        {
            StaffDAL staffDAL = new StaffDAL();
            CustomerDAL customerDAL = new CustomerDAL();
            Order order = new Order(
                reader.GetString("order_id"),
                reader.GetDateTime("Create_At"),
                new Staff(reader.GetInt32("seller_id"), "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active),
                new Staff(reader.GetInt32("accountant_id"), "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active),
                new Customer(reader.GetInt32("customer_id"), "", "", ""),
                new List<Imei>(),
                (OrderEnum.Status)Enum.ToObject(typeof(OrderEnum.Status), reader.GetInt32("order_status")),
                new List<DiscountPolicy>(),
                reader.GetString("payment_method"),
                0
            );
            return order;
        }
        // + lay id cua staff cho order tu phuong thuc GetOrderByID(int id)
        public Order GetOrderByID(string id)
        {
            StaffDAL staffDAL = new StaffDAL();
            CustomerDAL customerDAL = new CustomerDAL();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            Order order = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<Imei>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            // Dau tien lay ra thong tin cua Customer, Seller, Accountant theo order id
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"SELECT * FROM orders where order_id = @orderid";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@orderid", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    order = GetOrder(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            // trong order chi chua thuoc tinh id cua customer, accountant, seller
            // de lay toan bo thong tin cua 3 actor tren ta can phai get tat ca gia tri thong qua id nhu sau: 

            order.Accountant = staffDAL.GetStaffByID(order.Accountant.StaffID);
            order.Seller = staffDAL.GetStaffByID(order.Seller.StaffID);
            order.Customer = customerDAL.GetCustomerByID(order.Customer.CustomerID);
            order.ListImeiInOrder = phoneDetailsDAL.GetListImeisInOrder(order.OrderID);
            order.TotalDue = order.GetTotalDue();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select dp.* from discountpolicies dp
                inner join discountpolicydetails dpd on dp.policy_id = dpd.policy_id
                where dpd.order_id = @orderid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@orderid", order.OrderID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    order.DiscountPolicies.Add(new DiscountPolicyDAL().GetDiscountPolicy(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }


            return order;
        }

        public List<Order> GetOrders(OrderEnum.Status status)
        {
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            PhoneDAL phoneDAL = new PhoneDAL();
            List<Order> orders = new List<Order>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                switch (status)
                {
                    case OrderEnum.Status.Pending:
                        query = @"select * from orders
                        where order_status = 0 and date(create_at) = date(current_time());";
                        break;
                    case OrderEnum.Status.Confirmed:
                        query = @"select * from orders
                        where order_status = 1 and date(create_at) = date(current_time());";
                        break;
                    case OrderEnum.Status.Canceled:
                        query = @"select * from orders
                        where order_status = 2 and date(create_at) = date(current_time());";
                        break;
                    case OrderEnum.Status.Completed:
                        query = @"select * from orders
                        where order_status = 3;";
                        break;
                    default: break;
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(GetOrder(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            List<Order> output = new List<Order>();
            foreach (var o in orders)
            {
                output.Add(GetOrderByID(o.OrderID));
            }
            return output;
        }
        public bool SaveOrder(Order order)
        {
            bool result = false;
            int countphone = 0;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (MySqlTransaction tr = connection.BeginTransaction())
                {
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = tr;
                        MySqlDataReader? reader = null;
                        if (order.Customer.CustomerID == 0)
                        {
                            command.CommandText = $@"insert into Customers(name, address, phone_number) values ('{order.Customer.CustomerName}','{order.Customer.Address}','{order.Customer.PhoneNumber}');";
                            command.ExecuteNonQuery();
                            command.CommandText = "select customer_id from customers order by customer_id desc limit 1;";
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                order.Customer.CustomerID = reader.GetInt32("customer_id");
                            }
                            reader.Close();
                        }
                        else
                        {
                            command.CommandText = "select * from customers where customer_id=@customerId;";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@customerId", order.Customer.CustomerID);
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                order.Customer = new CustomerDAL().GetCustomer(reader);
                            }
                            reader.Close();
                        }
                        if (order.Seller.StaffID != 0)
                        {
                            command.CommandText = @"insert into orders(customer_id, seller_id, order_id) values (@cusid, @sellerid, @orderid);";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@cusid", order.Customer.CustomerID);
                            command.Parameters.AddWithValue("@sellerid", order.Seller.StaffID);
                            command.Parameters.AddWithValue("@orderid", order.OrderID);
                        }
                        else
                        {
                            command.CommandText = @"insert into orders(customer_id, order_id) values (@cusid, @orderid);";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@cusid", order.Customer.CustomerID);
                            command.Parameters.AddWithValue("@orderid", order.OrderID);
                        }
                        command.ExecuteNonQuery();

                        foreach (var imei in order.ListImeiInOrder)
                        {
                            int quantity = 0;
                            bool mySqlReader = false;
                            try
                            {
                                query = @"select pd.quantity from imeis i
                        inner join phonedetails pd on i.phone_detail_id = pd.phone_detail_id
                        where pd.phone_detail_id = @phonedetailid;";
                                command.CommandText = query;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@phonedetailid", imei.PhoneDetail.PhoneDetailID);
                                reader = command.ExecuteReader();
                                mySqlReader = reader.Read();
                                if (mySqlReader)
                                {
                                    quantity = reader.GetInt32("quantity");
                                }
                                reader.Close();
                                if (mySqlReader)
                                {
                                    if (imei.PhoneDetail.Quantity > quantity) break;
                                    else
                                    {
                                        query = @"insert into orderdetails(order_id, phone_imei) value (@orderid, @phoneimei);";
                                        command.CommandText = query;
                                        foreach (Imei imei2 in order.ListImeiInOrder)
                                        {
                                            command.Parameters.Clear();
                                            command.Parameters.AddWithValue("@orderid", order.OrderID);
                                            command.Parameters.AddWithValue("@phoneimei", imei2.PhoneImei);
                                            command.ExecuteNonQuery();
                                            countphone++;
                                        }
                                    }
                                }
                                if (countphone == order.ListImeiInOrder.Count() && order.ListImeiInOrder.Count() > 0) result = true;
                                else result = false;
                                if (result == true) tr.Commit();
                                else tr.Rollback();
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("err: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return result;
        }
        public bool UpdateOrder(OrderEnum.Status orderStatus, Order order)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    switch (orderStatus)
                    {
                        case OrderEnum.Status.Confirmed:
                            query = @"update orders set accountant_id = @accountantid, order_status = @orderstatus, 
                        update_at = current_timestamp(), payment_method = @paymentmethod 
                        where order_id = @orderid;";
                            command.CommandText = query;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@accountantid", order.Accountant.StaffID);
                            command.Parameters.AddWithValue("@orderstatus", (int)OrderEnum.Status.Confirmed);
                            command.Parameters.AddWithValue("@orderid", order.OrderID);
                            command.Parameters.AddWithValue("@paymentmethod", order.PaymentMethod);
                            command.ExecuteNonQuery();
                            break;
                        case OrderEnum.Status.Canceled:
                            query = @"update orders set order_status = @orderstatus,update_at = current_timestamp(),
                        accountant_id = @accountantid where order_id = @orderid;";
                            command.CommandText = query;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@accountantid", order.Accountant.StaffID);
                            command.Parameters.AddWithValue("@orderstatus", (int)OrderEnum.Status.Canceled);
                            command.Parameters.AddWithValue("@orderid", order.OrderID);
                            command.ExecuteNonQuery();
                            break;
                        case OrderEnum.Status.Completed:
                            query = @"update orders set order_status = @orderstatus, update_at = current_timestamp() 
                        where order_id = @orderid;";
                            command.CommandText = query;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@orderstatus", (int)OrderEnum.Status.Completed);
                            command.Parameters.AddWithValue("@orderid", order.OrderID);
                            command.ExecuteNonQuery();
                            break;
                        case OrderEnum.Status.Pending:
                            break;
                        default:
                            return false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (orderStatus == OrderEnum.Status.Canceled)
            {
                query = @"update imeis set status = 0 where phone_imei = @phoneimei;";
                MySqlCommand command = new MySqlCommand(query, connection);
                foreach (var imei in order.ListImeiInOrder)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@phoneimei", imei.PhoneImei);
                    command.ExecuteNonQuery();
                }
                query = @"delete from discountpolicydetails where order_id = @orderid;";
                command.CommandText = query;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@orderid", order.OrderID);
                command.ExecuteNonQuery();
            }
            if (orderStatus == OrderEnum.Status.Confirmed || orderStatus == OrderEnum.Status.Pending)
            {
                if (order.DiscountPolicies.Count() != 0 || order.DiscountPolicies != null)
                {
                    query = @"insert into discountpolicydetails(order_id, policy_id) value(@orderid, @policyid);";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    foreach (var dc in order.DiscountPolicies)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                        command.Parameters.AddWithValue("@policyid", dc.PolicyID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            return true;
        }
    }
}