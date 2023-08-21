using System;
using DAL;
using Model;
using BusinessEnum;
using System.Globalization;

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
    public List<Order> GetOrdersConfirmedInDay()
    {
        return orderDAL.GetOrders(BusinessEnum.OrderEnum.Status.Confirmed);

    }
    public bool Payment(Order order)
    {
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Confirmed, order);
    }
    public bool CancelPayment(Order order)
    {
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Canceled, order);
    }
    public bool TradeIn(Order order)
    {
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Pending, order);
    }
    public decimal CalculateTotalRevenue(List<Order> ordersCompleted)
    {
        if (ordersCompleted == null || ordersCompleted.Count() == 0) return 0;
        decimal totalRevenue = 0;
        foreach (var order in ordersCompleted)
        {
            foreach (var imei in order.ListImeiInOrder)
            {
                totalRevenue += CalculateTotalRevenueOnModel(imei.PhoneDetail);
            }
        }
        return totalRevenue;
    }
    public decimal CalculateTotalRevenueOnModel(PhoneDetail phoneDetail)
    {
        return phoneDetail.Quantity * phoneDetail.Price;
    }
}