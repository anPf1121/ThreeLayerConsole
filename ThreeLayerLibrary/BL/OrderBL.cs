using System;
using DAL;
using Model;

public class OrderBL
{
    private OrderDAL orderDAL = new OrderDAL();
    public Order GetOrderById(int orderID)
    {
        Order order = orderDAL.GetOrderByID(orderID);
        return order;
    }
    public bool CreateOrder(Order order) {
        return orderDAL.InsertOrder(order);
    }
    public List<Order> GetOrdersPendingInday(){
        return orderDAL.GetOrdersInDay(BusinessEnum.OrderEnum.Status.Pending);
    }
    public bool Payment(Order order){
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Pending, order);
    }
    public bool CancelPayment(Order order){
        return orderDAL.UpdateOrder(BusinessEnum.OrderEnum.Status.Confirmed, order);
    }
}