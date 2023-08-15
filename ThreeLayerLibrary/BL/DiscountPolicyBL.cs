using DAL;
using BusinessEnum;
using System;
using Model;

namespace BL;
public class DiscountPolicyBL{
    private DiscountPolicyDAL discountPolicyDAL = new DiscountPolicyDAL();
    public List<DiscountPolicy> GetDiscountForPaymentmethod(Order order){
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
    public List<DiscountPolicy> GetDiscountValidated(){
        return discountPolicyDAL.GetDiscountValidated();
    }
    public List<DiscountPolicy> GetDiscountTradeIn(List<PhoneDetail> phoneDetails){
        List<DiscountPolicy> lst = new List<DiscountPolicy>();
        List<DiscountPolicy> discountPoliciesValidated = discountPolicyDAL.GetDiscountValidated();
        foreach(var dc in discountPoliciesValidated){
            foreach(var phone in phoneDetails){
                if(dc.PhoneDetail.PhoneDetailID == phone.PhoneDetailID && dc.MoneySupported !=0)lst.Add(dc);
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
        return output;
        
    }
    public DiscountPolicy GetDiscountForOrder(Order order){
        PhoneBL phoneBL = new PhoneBL();
        DiscountPolicy lst = new DiscountPolicy(0, "", new DateTime(), new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime(), 0, 0, 0, "", new PhoneDetail(0, new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "",  new DateTime(), "",new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new List<Imei>(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime()), new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), 0, "");
        List<DiscountPolicy> discountPoliciesValidated = discountPolicyDAL.GetDiscountValidated();
        foreach(var dc in discountPoliciesValidated){
            //GetDiscount for order totaldue
            if(dc.DiscountPrice != 0 && dc.PaymentMethod == "Not Have"){
                if(order.TotalDue >= dc.MinimumPurchaseAmount && order.TotalDue <=dc.MaximumPurchaseAmount){
                    lst = dc;
                    break;
                }
            }
        }
        return lst;

}
}