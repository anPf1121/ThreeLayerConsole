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
            foreach (var phoneDetail in order.PhoneDetails)
            {
                totalRevenue += CalculateTotalRevenueOnModel(phoneDetail);
            }
        }
        return totalRevenue;
    }
    public decimal CalculateTotalRevenueOnModel(PhoneDetail phoneDetail)
    {
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
    public List<Order> GetCompletedOrderInDay()
    {
        DateTime currentDate = DateTime.Now;
        List<Order> orders = orderDAL.GetOrders(OrderEnum.Status.Completed);
        List<Order> ordersInDay = new List<Order>();
        int dayNumber = 0;
        foreach (Order order in orders)
        {
            dayNumber = order.CreateAt.Day;
            if (dayNumber == currentDate.Day - 1)
            {
                ordersInDay.Add(order);
            }
        }
        return ordersInDay;
    }
    public List<Order> GetCompletedOrderInDay(Staff accountant)
    {
        DateTime currentDate = DateTime.Now;
        List<Order> orders = orderDAL.GetOrders(OrderEnum.Status.Completed);
        List<Order> ordersInDay = new List<Order>();
        foreach (Order order in orders)
        {
            if (order.CreateAt.Day == currentDate.Day && order.Accountant.StaffID == accountant.StaffID)
            {
                ordersInDay.Add(order);
            }
        }
        return ordersInDay;
    }
    public List<Order> GetCompletedOrderInWeek()
    {
        DateTime currentDate = DateTime.Now;
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        Calendar calendar = cultureInfo.Calendar;
        int currentWeekNumber = calendar.GetWeekOfYear(currentDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        int weekNumber = 0;
        List<Order> orders = orderDAL.GetOrders(OrderEnum.Status.Completed);
        List<Order> ordersInWeek = new List<Order>();
        foreach (Order order in orders)
        {
            if (order.CreateAt.Year == currentDate.Year)
            {
                weekNumber = calendar.GetWeekOfYear(order.CreateAt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                if (weekNumber == currentWeekNumber - 1)
                {
                    ordersInWeek.Add(order);
                }
            }
        }
        return ordersInWeek;
    }
    public List<Order> GetCompletedOrderInWeek(Staff accountant)
    {
        DateTime currentDate = DateTime.Now;
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        Calendar calendar = cultureInfo.Calendar;
        int currentWeekNumber = calendar.GetWeekOfYear(currentDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        int weekNumber = 0;
        List<Order> orders = orderDAL.GetOrders(OrderEnum.Status.Completed);
        List<Order> ordersInWeek = new List<Order>();
        foreach (Order order in orders)
        {
            if (order.CreateAt.Year == currentDate.Year)
            {
                weekNumber = calendar.GetWeekOfYear(order.CreateAt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                if (weekNumber == currentWeekNumber - 1)
                {
                    if (order.Accountant.StaffID == accountant.StaffID)
                        ordersInWeek.Add(order);
                }
            }
        }
        return ordersInWeek;
    }
    public List<Order> GetCompletedOrderInMonth()
    {
        DateTime currentDate = DateTime.Now;
        int currentMonthNumber = currentDate.Month;
        List<Order> orders = orderDAL.GetOrders(OrderEnum.Status.Completed);
        List<Order> ordersInMonth = new List<Order>();
        foreach (Order order in orders)
        {
            if (order.CreateAt.Year == currentDate.Year)
            {
                if (order.CreateAt.Month == currentMonthNumber - 1)
                {
                    ordersInMonth.Add(order);
                }
            }
        }
        return ordersInMonth;
    }
    public List<Order> GetCompletedOrderInMonth(Staff accountant)
    {
        DateTime currentDate = DateTime.Now;
        int currentMonthNumber = currentDate.Month;
        List<Order> orders = orderDAL.GetOrders(OrderEnum.Status.Completed);
        List<Order> ordersInMonth = new List<Order>();
        foreach (Order order in orders)
        {
            if (order.CreateAt.Year == currentDate.Year)
            {
                if (order.CreateAt.Month == currentMonthNumber - 1 && order.Accountant.StaffID == accountant.StaffID)
                {
                    ordersInMonth.Add(order);
                }
            }
        }
        return ordersInMonth;
    }
}