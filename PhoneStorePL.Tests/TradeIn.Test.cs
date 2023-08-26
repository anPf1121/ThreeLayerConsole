using Xunit;
using Model;
using BL;
using DAL;

namespace PhoneStorePL.Tests;

public class TradeInTests
{
    [Fact]
    public void TradeIn_Success1()
    {
        int count = 0;
        // Arrange
        OrderBL orderBL = new OrderBL();
        PhoneBL phoneBL = new PhoneBL();
        DiscountPolicyBL discountPolicyBL = new DiscountPolicyBL();
        string orderId = "5FBB22AF4D9F";
        Order order = orderBL.GetOrderById(orderId);
        PhoneDetail iphone11 = new PhoneDetailsDAL().GetPhoneDetailByID(1);
        PhoneDetail iphone12 = new PhoneDetailsDAL().GetPhoneDetailByID(2);
        List<PhoneDetail> list = new List<PhoneDetail>();
        list.Add(iphone11);
        list.Add(iphone12);
        List<DiscountPolicy> result1 = new DiscountPolicyBL().GetDiscountTradeIn(list);
        // So sanh so luong dien thoai trong order cua tradein function voi so luong dien thoai trong DB
        foreach (var item in result1)
        {
            if (item.PhoneDetail == iphone11 || item.PhoneDetail == iphone12) count++;
        }
        Assert.Equal(count, result1.Count());

        // ACT

        // Test Change Tradein status 
        bool result = orderBL.TradeIn(order);
        
        // Assert
        Assert.True(result);


    }
}