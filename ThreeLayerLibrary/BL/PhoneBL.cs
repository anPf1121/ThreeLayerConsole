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
    public bool AddPhoneImeiToOrder(string imei) {
        return phoneDetailDAL.UpdateImeiStatus(imei, PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_INORDER);
    }
    public bool ExportPhoneImei(string imei) {
        return phoneDetailDAL.UpdateImeiStatus(imei, PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_EXPORT);
    }
    public bool NotExportPhoneImei(string imei) {
        return phoneDetailDAL.UpdateImeiStatus(imei, PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_NOTEXPORT);
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
    public PhoneDetail GetPhoneDetailByID(int phonedetailid)
    {
        return phoneDetailDAL.GetPhoneDetailByID(phonedetailid);
    }
    public List<Imei> GetImeisByPhoneDetailsID(int phoneDetailID)
    {
        return phoneDetailDAL.GetImeisByPhoneDetailsID(phoneDetailID);
    }
    public bool CheckImeiIsDuplicateInOrder(Imei imei, Order order)
    {
        bool isDuplicate = false;
        foreach (Imei j in order.ListImeiInOrder)
        {
            if (j.PhoneImei == imei.PhoneImei)
            {
                isDuplicate = true;
            }
        }
        return isDuplicate;
    }
    public List<PhoneDetail> GetPhonesCanTradeIn(){
        return phoneDetailDAL.GetListPhoneDetailCanTradeIn();
    }
}