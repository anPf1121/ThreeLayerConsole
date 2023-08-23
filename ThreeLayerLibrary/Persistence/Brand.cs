namespace Model;
public class Brand
{
    public int BrandID { get; set; }
    public string BrandName { get; set; }
    public string Website { get; set; }
    public Brand(int brandID, string brandName, string website){
        this.BrandID = brandID;
        this.BrandName = brandName;
        this.Website = website;
    }
}