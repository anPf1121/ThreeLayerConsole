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
                staffDAL.GetStaff(reader),
                staffDAL.GetStaff(reader),
                customerDAL.GetCustomer(reader),
                new List<PhoneDetail>(),
                (OrderEnum.Status)Enum.ToObject(typeof(OrderEnum.Status), reader.GetInt32("Status")),
                new List<DiscountPolicy>()
            );
            return order;
        }
        // + lay id cua staff cho order tu phuong thuc GetOrderByID(int id)
        public Order? GetOrderByID(int id)
        {
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            Order? order = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"SELECT * FROM orders O
                INNER JOIN staffs S ON O.seller_id = S.staff_id
                INNER JOIN customers C ON O.customer_id = C.customer_id
                WHERE O.order_id = @orderid;";
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
            if (order != null)
            {
                // Get Phone Details In Order 
                if (order.PhoneDetails.Count() > 0)
                {
                    foreach (PhoneDetail phoneDetail in order.PhoneDetails)
                    {
                        try
                        {
                            if (connection.State == System.Data.ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            query = @"SELECT * FROM phones P
                            INNER JOIN phonedetails PD ON P.phone_id = PD.phone_id
                            INNER JOIN imeis I ON PD.phone_detail_id = I.phone_detail_id
                            INNER JOIN orderdetails OD ON OD.phone_imei = I.phone_imei
                            INNER JOIN brands B ON P.brand_id = B.brand_id
                            INNER JOIN romsizes RS ON PD.rom_size_id = RS.rom_size_id
                            INNER JOIN colors C ON PD.color_id = C.color_id
                            INNER JOIN staffs S ON P.create_by = S.staff_id
                            WHERE OD.order_id = @orderID";
                            MySqlCommand command = new MySqlCommand(query, connection);
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@orderID", order.OrderID);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                order.PhoneDetails.Add(phoneDetailsDAL.GetPhoneDetail(reader));
                                // phoneDetail.PhoneDetails
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
                        if (order.PhoneDetails.Count() != 0)
                        {
                            foreach (PhoneDetail item in order.PhoneDetails)
                            {
                                try
                                {
                                    if (connection.State == System.Data.ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    query = @"SELECT * FROM imeis I 
                                            INNER JOIN orderdetails OD ON OD.phone_imei = I.phone_Imei
                                            INNER JOIN orders O ON O.order_id = OD.order_id
                                            WHERE O.order_id=@orderID";
                                    MySqlCommand command = new MySqlCommand(query, connection);
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@orderID", order.OrderID);
                                    MySqlDataReader reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        item.ListImei.Add(phoneDetailsDAL.GetImei(reader));
                                    }
                                    reader.Close();
                                }
                                catch (MySqlException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (connection.State == System.Data.ConnectionState.Open)
                            {
                                connection.Close();
                            }
                        }
                    }
                }
            }
            return order;
        }

        public List<Order> GetOrdersInDay(int orderFilter)
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
                switch (orderFilter)
                {
                    case OrderFilter.GET_ORDER_PENDING_IN_DAY:
                        query = @"select order_id, customer_id, seller_id, accountant_id, create_at, order_status, S.*
                        from orders O
                        INNER JOIN staffs S ON O.seller_id = S.staff_id
                        where O.order_status = " + (int)OrderEnum.Status.Pending + " and date(O.create_at) = date(current_time());";
                        break;
                    case OrderFilter.GET_ORDER_CONFIRMED_IN_DAY:
                        query = @"select order_id, customer_id, seller_id, accountant_id, create_at, order_status, S.*
                        from orders O
                        INNER JOIN staffs S ON O.seller_id = S.staff_id
                        where O.order_status = " + (int)OrderEnum.Status.Confirmed + " and date(O.create_at) = date(current_time());";
                        break;
                    case OrderFilter.GET_ORDER_COMPLETED_IN_DAY:
                        query = @"select order_id, customer_id, seller_id, accountant_id, create_at, order_status, S.*
                        from orders O
                        INNER JOIN staffs S ON O.seller_id = S.staff_id
                        where O.order_status = " + (int)OrderEnum.Status.Completed + " and date(O.create_at) = date(current_time());";
                        break;
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
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            if (orders.Count() != 0)
            {
                foreach (var order in orders)
                {
                    try
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        query = @"SELECT * FROM orderdetails O
                            INNER JOIN imeis I ON O.phone_imei = I.phone_imei
                            INNER JOIN phonedetails PD ON I.phone_detail_id = PD.phone_detail_id
                            INNER JOIN phones P ON PD.phone_id = P.phone_id
                            INNER JOIN brands B ON B.brand_id = P.brand_id
                            INNER JOIN staffs S ON P.create_by = S.staff_id
                            INNER JOIN romsizes RS ON PD.rom_size_id = RS.rom_size_id
                            INNER JOIN colors C ON C.color_id = PD.color_id
                            WHERE O.order_id = @orderid;";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            order.PhoneDetails.Add(phoneDetailsDAL.GetPhoneDetail(reader));
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
                    // Get Phone Details In Order 
                    if (order.PhoneDetails.Count() > 0)
                    {
                        foreach (PhoneDetail phoneDetail in order.PhoneDetails)
                        {
                            try
                            {
                                if (connection.State == System.Data.ConnectionState.Closed)
                                {
                                    connection.Open();
                                }
                                query = @"SELECT * FROM phones P
                            INNER JOIN phonedetails PD ON P.phone_id = PD.phone_id
                            INNER JOIN imeis I ON PD.phone_detail_id = I.phone_detail_id
                            INNER JOIN orderdetails OD ON OD.phone_imei = I.phone_imei
                            INNER JOIN brands B ON P.brand_id = B.brand_id
                            INNER JOIN romsizes RS ON PD.rom_size_id = RS.rom_size_id
                            INNER JOIN colors C ON PD.color_id = C.color_id
                            INNER JOIN staffs S ON P.create_by = S.staff_id
                            WHERE OD.order_id = @orderID";
                                MySqlCommand command = new MySqlCommand(query, connection);
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@orderID", order.OrderID);
                                MySqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    order.PhoneDetails.Add(phoneDetailsDAL.GetPhoneDetail(reader));
                                    // phoneDetail.PhoneDetails
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
                            if (order.PhoneDetails.Count() != 0)
                            {
                                foreach (PhoneDetail item in order.PhoneDetails)
                                {
                                    try
                                    {
                                        if (connection.State == System.Data.ConnectionState.Closed)
                                        {
                                            connection.Open();
                                        }
                                        query = @"SELECT * FROM imeis I 
                                            INNER JOIN orderdetails OD ON OD.phone_imei = I.phone_Imei
                                            INNER JOIN orders O ON O.order_id = OD.order_id
                                            WHERE O.order_id=@orderID";
                                        MySqlCommand command = new MySqlCommand(query, connection);
                                        command.Parameters.Clear();
                                        command.Parameters.AddWithValue("@orderID", order.OrderID);
                                        MySqlDataReader reader = command.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            item.ListImei.Add(phoneDetailsDAL.GetImei(reader));
                                        }
                                        reader.Close();
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                if (connection.State == System.Data.ConnectionState.Open)
                                {
                                    connection.Close();
                                }
                            }
                        }
                    }
                }
            }
            return orders;
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
                        command.Parameters.AddWithValue("@orderstatus", (int)order.OrderStatus);
                        command.Parameters.AddWithValue("@orderid", order.OrderID);
                        command.ExecuteNonQuery();
                        break;
                    case OrderEnum.Status.Confirmed:
                        query = @"update orders set order_status = @orderstatus where order_id = @orderid;";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@orderstatus", (int)order.OrderStatus);
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
