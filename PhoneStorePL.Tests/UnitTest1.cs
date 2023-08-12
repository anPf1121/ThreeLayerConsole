using Ults;
using Xunit;

namespace PhoneStorePL.Tests;

// cách chạy test dotnet test    
public class OrderTests
{
    [Fact] //[Fact] là một thuộc tính (attribute) được sử dụng để đánh dấu một phương thức là một test method. Khi bạn đánh dấu một phương thức bằng [Fact], xUnit sẽ biết rằng phương thức đó cần được chạy như một test và sẽ thực hiện kiểm tra kết quả của nó.
    public void CreateValidOrder_Success()
    {
        int a = 5;
        int b = 7;
        int result = 12;
        Assert.Equal(12, result);
    }

}
