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
    public bool UpdateImeiStatusToInOrder(string imei) {
        return phoneDetailDAL.UpdateImeiStatus(imei, PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_INORDER);
    }
    public bool UpdateImeiStatusToExport(string imei) {
        return phoneDetailDAL.UpdateImeiStatus(imei, PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_EXPORT);
    }
    public bool UpdateImeiStatusToNotExport(string imei) {
        return phoneDetailDAL.UpdateImeiStatus(imei, PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_NOTEXPORT);
    }
    public List<Imei> GetImeis(int phoneDetailId) {
        return phoneDetailDAL.GetImeis(phoneDetailId);
    }
    public List<Phone> GetPhonesByInformation(string phoneInformation)
    {
        if (phoneInformation == "") return GetAllPhone();
        List<Phone> tempList = phoneDAL.GetPhones(PhoneFilter.FILTER_BY_PHONE_INFORMATION, phoneInformation);
        return tempList; 
    }
    public List<PhoneDetail> GetPhoneDetailsByPhoneID(int id)
    {
        return phoneDetailDAL.GetPhoneDetails(id.ToString(), PhoneDetailFilter.GET_PHONE_DETAIL_BY_PHONE_ID);
    }
    public PhoneDetail GetPhoneDetailByID(int phonedetailid)
    {
        return phoneDetailDAL.GetPhoneDetailByID(phonedetailid);
    }
    public List<PhoneDetail> GetPhonesCanTradeIn(){
        return phoneDetailDAL.GetPhoneDetails(PhoneDetailFilter.NULL_PARAMETER.ToString(), PhoneDetailFilter.GET_PHONE_DETAIL_FOR_TRADEIN);
    }
}