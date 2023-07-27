using System;
using BusinessEnum;
using DAL;
using Model;

namespace BL;

public class PhoneBL
{
    private PhoneDAL phoneDAL = new PhoneDAL();
    private PhoneDetailsDAL phoneDetailDAL = new PhoneDetailsDAL();
    public Phone GetPhoneById(int phoneID)
    {
        return phoneDAL.GetPhoneById(phoneID);
    }
    public List<Phone> GetAllPhone()
    {
        List<Phone> tempList = phoneDAL.GetPhones(PhoneFilter.GET_ALL, null);
        return tempList;
    }
    
    public List<Phone> GetPhonesByInformation(string phoneInformation)
    {
        if (phoneInformation == "") return GetAllPhone();
        
            List<Phone> tempList = phoneDAL.GetPhones(PhoneFilter.FILTER_BY_PHONE_INFORMATION, phoneInformation);

            return tempList;
        
    }
    public List<PhoneDetail> GetPhoneDetailsByPhoneID(int id){
        return phoneDetailDAL.GetPhoneDetailsByPhoneID(id);
    }
    public PhoneDetail GetPhoneDetailByID(int phonedetailid){
        return phoneDetailDAL.GetPhoneDetailByID(phonedetailid);
    }
    public bool CheckImeisExits(PhoneDetail phonedetail, List<Imei> imeiWantToCheck){
        
    int count = 0;
    foreach(var imeisofphone in phonedetail.ListImei){
        foreach(var imeiwanttocheck in imeiWantToCheck){
            if(imeisofphone.PhoneImei == imeiwanttocheck.PhoneImei){
                count++;
                break;
            }
        }
    }
    if(count == imeiWantToCheck.Count()){
        return true;
    }
    return false;
    
    }
    public Dictionary<PhoneDetail, decimal> GetListPhoneDetailHaveDiscountByDiscountPolicy(int policyid){
        return phoneDetailDAL.GetListPhoneDetailHaveDiscountByDiscountPolicy(policyid);
    }
}