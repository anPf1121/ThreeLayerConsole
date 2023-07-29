using Model;
using MySqlConnector;
using BusinessEnum;

namespace DAL
{
    public static class OrderFilter
    {
        public const int GET_ORDER_PENDING_IN_DAY = 0;
        public const int GET_ORDER_CONFIRMED_IN_DAY = 1;
        public const int GET_ORDER_COMPLETED_IN_DAY = 2;
    }
    public static class StatusFilter
    {
        public const int Paid = 0;
        public const int Export = 1;
    }
    public class OrderDAL
    {
        private MySqlConnection connection = DbConfig.GetConnection();
        private string query = "";
        public Order GetOrder(MySqlDataReader reader)
        {
            StaffDAL staffDAL = new StaffDAL();
            CustomerDAL customerDAL = new CustomerDAL();
            Order order = new Order(
                reader.GetInt32("Order_ID"),
                reader.GetDateTime("Create_At"),
                new Staff(reader.GetInt32("seller_id"), "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active),
                new Staff(reader.GetInt32("accountant_id"), "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active),
                new Customer(reader.GetInt32("customer_id"), "", "", ""),
                new List<PhoneDetail>(),
                (OrderEnum.Status)Enum.ToObject(typeof(OrderEnum.Status), reader.GetInt32("order_status")),
                new List<DiscountPolicy>(),
                reader.GetString("payment_method"),
                0
            );
            return order;
        }
        // + lay id cua staff cho order tu phuong thuc GetOrderByID(int id)
        public Order GetOrderByID(int id)
        {
            StaffDAL staffDAL = new StaffDAL();
            CustomerDAL customerDAL = new CustomerDAL();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            Order order = new Order(0, new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);

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

            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            // trong order chi chua thuoc tinh id cua customer, accountant, seller
            // de lay toan bo thong tin cua 3 actor tren ta can phai get tat ca gia tri thong qua id nhu sau: 

            order.Accountant = staffDAL.GetStaffByID(order.Accountant.StaffID);
            order.Seller = staffDAL.GetStaffByID(order.Seller.StaffID);
            order.Customer = customerDAL.GetCustomerByID(order.Customer.CustomerID);
            order.PhoneDetails = phoneDetailsDAL.GetListPhoneDetailInOrder(order.OrderID);

            //Lay danh sach Phonedetails co trong order

            return order;
        }

        public List<Order> GetOrdersInDay(OrderEnum.Status status)
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
                        where order_status = 3 and date(create_at) = date(current_time());";
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
        public bool InsertOrder(Order order)
        {
            bool result = false;
            int countphone = 0;
            CustomerDAL cdl = new CustomerDAL();
            MySqlTransaction? tr = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                tr = connection.BeginTransaction();
                MySqlCommand command = new MySqlCommand(connection, tr);
                MySqlDataReader? reader = null;
                if (cdl.InsertCustomer(order.Customer))
                {
                    query = @"select customer_id from customers order by customer_id desc limit 1;";
                    command.CommandText = query;
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        order.Customer.CustomerID = reader.GetInt32("customer_id");
                    }
                    reader.Close();
                }
                else
                {
                    query = @"select customer_id where Phone_Number = @phonenumber;";
                    command.CommandText = query;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@phonenumber", order.Customer.PhoneNumber);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            order.Customer.CustomerID = reader.GetInt32("customer_id");
                        }
                        reader.Close();
                    }
                    else return false;
                }
                query = @"insert into orders(customer_id, seller_id) 
        value (@cusid, @sellerid);";
                command.CommandText = query;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@cusid", order.Customer.CustomerID);
                command.Parameters.AddWithValue("@sellerid", order.Seller.StaffID);
                command.ExecuteNonQuery();
                query = @"select Order_ID from orders order by Order_ID desc limit 1; ";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    order.OrderID = reader.GetInt32("Order_ID");
                }
                reader.Close();
                foreach (var phone in order.PhoneDetails)
                {
                    int phoneDetailID = 0;
                    int quantity = 0;
                    bool mySqlReader = false;
                    try
                    {
                        query = @"select phone_detail_id, quantity from phonedetails where price = @price;";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@price", phone.Price);
                        reader = command.ExecuteReader();
                        mySqlReader = reader.Read();
                        if (mySqlReader)
                        {
                            phoneDetailID = reader.GetInt32("phone_detail_id");
                            quantity = reader.GetInt32("quantity");
                        }
                        reader.Close();
                        if (mySqlReader)
                        {
                            if (phone.Quantity > quantity) break;
                            else
                            {
                                query = @"insert into orderdetails(order_id, phone_imei) 
                                value(@orderid, @phoneimei);";
                                command.CommandText = query;
                                foreach (Imei imei in phone.ListImei)
                                {
                                    if (phone.PhoneDetailID == phoneDetailID)
                                    {
                                        command.Parameters.Clear();
                                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                                        command.Parameters.AddWithValue("@phoneimei", imei.PhoneImei);
                                        command.ExecuteNonQuery();
                                    }
                                }
                                countphone++;
                            }
                        }
                        else break;
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (countphone == order.PhoneDetails.Count() && order.PhoneDetails.Count() > 0) result = true;
                else result = false;
                if (result == true) tr.Commit();
                else tr.Rollback();
            }
            catch (MySqlException ex)
            {
                try
                {
                    if (tr != null)
                        tr.Rollback();
                    result = false;
                }
                catch (MySqlException ex1)
                {
                    Console.WriteLine(ex1.Message);
                }
                Console.WriteLine(ex.Message);
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
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
                MySqlCommand command = new MySqlCommand("", connection);
                switch (orderStatus)
                {
                    case OrderEnum.Status.Pending:
                        query = @"update orders set accountant_id = @accountantid, order_status = @orderstatus 
                        where order_id = @orderid;";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@accountantid", order.Accountant.StaffID);
                        command.Parameters.AddWithValue("@orderstatus", (int)OrderEnum.Status.Confirmed);
                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                        command.ExecuteNonQuery();
                        break;
                    case OrderEnum.Status.Confirmed:
                        query = @"update orders set order_status = @orderstatus where order_id = @orderid;";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@orderstatus", (int)OrderEnum.Status.Completed);
                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                        command.ExecuteNonQuery();
                        break;
                    case OrderEnum.Status.Canceled:
                        query = @"update orders set accountant_id = @accountantid, order_status = @orderstatus 
                        where order_id = @orderid;";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@accountantid", order.Accountant.StaffID);
                        command.Parameters.AddWithValue("@orderstatus", (int)OrderEnum.Status.Canceled);
                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                        command.ExecuteNonQuery();
                        break;
                    default:
                        return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            return true;
        }
    }
}
