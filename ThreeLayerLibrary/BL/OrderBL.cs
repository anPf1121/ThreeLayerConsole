using System;
using DAL;
using Model;

public class OrderBL
{
    private OrderDAL orderDAL = new OrderDAL();
    public Order? GetOrderById(int orderID)
    {
        Order? order = orderDAL.GetOrderByID(orderID);
        if (order == null) return null;
        return order;
    }

}