using System;
using DAL;
using Model;
using BusinessEnum;

public class OrderBL
{
    private OrderDAL orderDAL = new OrderDAL();
    public Order GetOrderById(string orderID)
    {
        Order order = orderDAL.GetOrderByID(orderID);
        return order;
    }
    public bool CreateOrder(Order order)
    {
        return orderDAL.SaveOrder(order);
    }
    public List<Order>? GetOrdersInDay(OrderEnum.Status status)
    {
        List<Order> orders = orderDAL.GetOrders(status);
        if (orders == null) return null;
        else
            return orders;
    }
    public bool UpdateOrder(OrderEnum.Status orderStatus, Order order)
    {
        return orderDAL.UpdateOrder(orderStatus, order);
    }
    public List<Order> GetOrdersPendingInday()
    {
        return orderDAL.GetOrders(BusinessEnum.OrderEnum.Status.Pending);
    }
    public bool Payment(Order order)
    {
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Confirmed, order);
    }
    public bool CancelPayment(Order order)
    {
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Canceled, order);
    }
    public List<Order>? GetOrdersCompletedInMonth()
    {
        return orderDAL.GetOrders(OrderEnum.Status.CompletedInMonth);
    }
    public decimal CalculateTotalRevenue(List<Order> ordersCompleted)
    {
        if(ordersCompleted == null || ordersCompleted.Count() == 0) return 0;  
        decimal totalRevenue = 0;
        foreach (var order in ordersCompleted)
        {
            foreach (var phoneDetail in order.PhoneDetails)
            {
                totalRevenue += CalculateTotalRevenueOnModel(phoneDetail);
            }
        }
        return totalRevenue;
    }
    public decimal CalculateTotalRevenueOnModel(PhoneDetail phoneDetail) {
        return phoneDetail.Quantity * phoneDetail.Price;
    }
    public List<PhoneDetail> GetPhoneDetailInOrder(List<Order> ordersCompleted)
    {
        List<PhoneDetail> phoneDetail = new List<PhoneDetail>();
        foreach (var order in ordersCompleted)
        {
            foreach (var pd in order.PhoneDetails)
            {
                phoneDetail.Add(pd);
            }
        }
        return phoneDetail;
    }
    public List<Order> GetCompletedOrderByDate(DateTime startDate, DateTime endDate) {
        return orderDAL.GetOrdersByDateTime(startDate, endDate);
    }
    public int GetCustomersRepeatTime(List<Order> orders) {
        Dictionary<int, int> customerIDWithRepeatTime = new Dictionary<int, int>();
        int count = 0;
        foreach (var item in orders)
        {
            if(customerIDWithRepeatTime.ContainsKey(item.Customer.CustomerID)) {
                customerIDWithRepeatTime[item.Customer.CustomerID]++;
                Console.WriteLine("bp bl" + item.Customer.CustomerID);
                Console.ReadLine();
            } else {
                customerIDWithRepeatTime[item.Customer.CustomerID] = 1;
            }
        }
        foreach (var item in customerIDWithRepeatTime)
        {
            if(item.Value > 1) count++;
        }
        return count;
    }
}