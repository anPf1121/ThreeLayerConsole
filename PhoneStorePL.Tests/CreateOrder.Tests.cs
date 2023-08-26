using BusinessEnum;
using Model;
using Ults;
using BL;
using DAL;
using Xunit;

namespace PhoneStorePL.Tests;

public class OrderTests
{
    // [Fact]
    // public void Create_Order_False()
    // {
    //     Order order = new();
    //     Customer customer = new(3, "321#@#123%", "12345123123", null!);
    //     Staff seller = new(4, "Tran Tien Anh", "0902126092", "seller01", "abc123", "Hanoi", StaffEnum.Role.Seller, StaffEnum.Status.Active);
    //     PhoneDetail iphone14 = new(14, new Phone(0, "Iphone XXXXXXYZ", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, "1000TB"), new PhoneColor(0, "No Color"), 0, 0, PhoneEnum.Status.Type1, new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime());
    //     Imei imei1InOrder = new(iphone14, "klnflaskndfoiqwn", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
    //     Imei imei2InOrder = new(iphone14, "klnflaskndfoiqdsdsds", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
    //     order.ListImeiInOrder.Add(imei1InOrder);
    //     order.ListImeiInOrder.Add(imei2InOrder);
    //     order.Customer = customer;
    //     order.Seller = seller;
    //     order.OrderID = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
    //     Assert.False(new OrderBL().CreateOrder(order));
    // }
    [Fact]
    public void Create_Order_Success()
    {
        Order order = new("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<Imei>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
        Customer customer = new CustomerDAL().GetCustomerByID(3);
        Staff seller = new StaffDAL().GetStaffByID(4);
        PhoneDetail iphone14 = new PhoneDetailsDAL().GetPhoneDetailByID(46);
        Imei imei1InOrder = new(iphone14, "378532210210266", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
        Imei imei2InOrder = new(iphone14, "378521012121023", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
        order.ListImeiInOrder.Add(imei1InOrder);
        order.ListImeiInOrder.Add(imei2InOrder);
        order.Customer = customer;
        order.Seller = seller;
        order.OrderID = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
        Assert.True(new OrderBL().CreateOrder(order));
    }
}