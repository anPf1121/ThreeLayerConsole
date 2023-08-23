using System;
using BusinessEnum;
using DAL;
using Model;

namespace BL;

public class PhoneBL
{
    private PhoneDAL phoneDAL = new PhoneDAL();
    private PhoneDetailsDAL phoneDetailDAL = new PhoneDetailsDAL();
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
    public List<PhoneDetail> GetPhoneDetailsByPhoneID(int id)
    {
        return phoneDetailDAL.GetPhoneDetailsByPhoneID(id);
    }
    public bool CheckImeiExist(Imei imei, int phoneDetailID)
    {
        foreach (Imei item in phoneDetailDAL.GetImeisByPhoneDetailsID(phoneDetailID))
        {
            if (item.PhoneImei == imei.PhoneImei && item.Status == imei.Status) return true;
        }
        return false;
    }
    public PhoneDetail GetPhoneDetailByImei(string imei) {
        PhoneDetail phoneDetail = phoneDetailDAL.GetPhoneDetailByImei(imei);
        return phoneDetail;
    }
    public PhoneDetail GetPhoneDetailByID(int phonedetailid)
    {
        return phoneDetailDAL.GetPhoneDetailByID(phonedetailid);
    }
}