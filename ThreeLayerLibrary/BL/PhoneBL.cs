using System;
using BusinessEnum;
using DAL;
using Model;

namespace BL;

public class PhoneBL
{
    private PhoneDAL phoneDAL = new PhoneDAL();
    public Phone? GetPhoneById(int phoneID)
    {
        return phoneDAL.GetPhoneById(phoneID);
    }
    public List<Phone>? GetAllPhone()
    {
        List<Phone> tempList = phoneDAL.GetPhones(PhoneFilter.GET_ALL, null);
        if (tempList.Count() == 0) return null;
        else return tempList;
    }
    public List<Phone>? GetPhonesByInformation(string? phoneInformation)
    {
        if (phoneInformation == "") return GetAllPhone();
        else if (phoneInformation == null) return null;
        else
        {
            List<Phone> tempList = phoneDAL.GetPhones(PhoneFilter.FILTER_BY_PHONE_INFORMATION, phoneInformation);
            if (tempList.Count() == 0) return null;
            else return tempList;
        }
    }
}