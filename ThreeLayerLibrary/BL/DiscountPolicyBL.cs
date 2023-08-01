using DAL;
using BusinessEnum;
using System;
using Model;

namespace BL;
public class DiscountPolicyBL{
    private DiscountPolicyDAL discountPolicyDAL = new DiscountPolicyDAL();
    public List<DiscountPolicy> GetDiscountValidToOrder(Order order) {
        PhoneBL phoneBL = new PhoneBL();
        decimal totaldue = 0;
        foreach(var phone in order.PhoneDetails){
            totaldue+=phone.Price*phone.Quantity;
        }

        List<DiscountPolicy> lst = new List<DiscountPolicy>();
        List<DiscountPolicy> discountPoliciesValidated = discountPolicyDAL.GetDiscountValidated();
        foreach(var dc in discountPoliciesValidated){
            if(totaldue > dc.MinimumPurchaseAmount && totaldue <dc.MaximumPurchaseAmount && order.PaymentMethod == dc.PaymentMethod)lst.Add(dc);
               
        }
        List<DiscountPolicy> output = new List<DiscountPolicy>();
        foreach(var dc in lst){
            int count = 0;
            foreach(var o in output){
                if(o.Title == dc.Title)count++;
            }
            if(count == 0)output.Add(dc);
        }
        return output;
    }
}