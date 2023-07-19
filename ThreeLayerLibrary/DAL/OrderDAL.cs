using Model;
using MySqlConnector;
namespace DAL
{
    public static class OrderFilter
    {
        public const int GET_ORDER_PROCESSING_IN_DAY = 0;
        public const int GET_ORDER_PAID_IN_DAY = 1;
        public const int GET_ORDER_EXPORT_IN_DAY = 2;
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
            Order order = new Order();
            return order;
        }
        public Customer GetOrderCustomer(int id)
        {
            Customer customer = new Customer();
            return customer;
        }

        // + lay id cua staff cho order tu phuong thuc GetOrderByID(int id)
        public Order GetOrderByID(int id)
        {
            Order order = new Order();
            return order;
        }
        public List<Phone> GetItemsInOrderByID(int id)
        {
            List<Phone> lstPhone = new List<Phone>();
            return lstPhone;
        }
        public List<Order> GetOrdersInDay(int orderFilter)
        {
            List<Order> lstOrder = new List<Order>();
            return lstOrder;
        }
        public bool InsertOrder(Order order)
        {
            bool result = false;
            return result;
        }
        public bool UpdateOrder(int statusFilter, Order order)
        {
            return true;
        }
    }
}