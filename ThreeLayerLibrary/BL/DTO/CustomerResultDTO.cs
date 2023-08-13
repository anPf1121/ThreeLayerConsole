namespace CustomerDTO;
public class CustomerResultDTO
{
    public int CustomerId { get; set; }
    public bool IsDuplicate { get; set; }
    public CustomerResultDTO(int customerID, bool isDuplicate) {
        this.CustomerId = customerID;
        this.IsDuplicate = isDuplicate;
    } 
}