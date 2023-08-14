using DAL;
using BusinessEnum;
using System;
using Model;

namespace BL;
public class DiscountPolicyBL{
    private DiscountPolicyDAL discountPolicyDAL = new DiscountPolicyDAL();
    public List<DiscountPolicy> GetDiscountValidToOrder(Order order){
        PhoneBL phoneBL = new PhoneBL();

        List<DiscountPolicy> lst = new List<DiscountPolicy>();
        List<DiscountPolicy> discountPoliciesValidated = discountPolicyDAL.GetDiscountValidated();
        foreach(var dc in discountPoliciesValidated){
            //GetDiscount for payment method
            if(order.PaymentMethod == dc.PaymentMethod){
                if(order.PaymentMethod == "VNPay"|| order.PaymentMethod == "Banking"|| order.PaymentMethod == "Cash"){
                    if(order.TotalDue >= dc.MinimumPurchaseAmount && order.TotalDue <=dc.MaximumPurchaseAmount)lst.Add(dc);
                }
            }
            //GetDiscount for order total due
            if(dc.MinimumPurchaseAmount >0){
                if(order.TotalDue >= dc.MinimumPurchaseAmount && order.TotalDue <=dc.MaximumPurchaseAmount)lst.Add(dc);
            }
            //GetDiscount for phone have discount
                if(dc.PhoneDetail.PhoneDetailID!=0 && dc.MoneySupported !=0){
                    foreach(var phone in order.PhoneDetails){
                        if(phone.PhoneDetailID == dc.PhoneDetail.PhoneDetailID)lst.Add(dc);
                    }
                }

        }
        List<DiscountPolicy> output = new List<DiscountPolicy>();
        foreach(var dc in lst){
            int count = 0;
            foreach(var o in output){
                if(o.Title == dc.Title)count++;
            }
            if(count == 0)output.Add(dc);
        }

        return lst;
    }
    public DiscountPolicy GetDiscountPolicyByID(int discountid){
        return discountPolicyDAL.GetDiscountPolicyById(discountid);
    }
    public DiscountPolicy GetDiscountTradeInForPhone(PhoneDetail phoneDetail){
        return discountPolicyDAL.GetDiscountPolicyForPhoneTradeIn(phoneDetail);
    }

}