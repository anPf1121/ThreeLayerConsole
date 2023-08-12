- Assert.Equal:
So sánh hai giá trị và đảm bảo chúng bằng nhau.
Assert.Equal(expected, actual);

- Assert.True:
Đảm bảo giá trị là true.
Assert.True(condition);

- Assert.False:
Đảm bảo giá trị là false.
Assert.False(condition);

- Assert.NotNull:
Đảm bảo giá trị không phải là null.
Assert.NotNull(value);

- Assert.Null:
Đảm bảo giá trị là null.
Assert.Null(value);

- Assert.Empty:
Đảm bảo một danh sách (collection) hoặc chuỗi là trống.
Assert.Empty(collection);

- Assert.Contains:
Đảm bảo một phần tử xuất hiện trong danh sách hoặc chuỗi.
Assert.Contains(expectedElement, collection);

- Assert.Throws:
Kiểm tra việc ném ra một loại exception cụ thể.
Assert.Throws<ExpectedExceptionType>(() => { /* Code ném exception */ });

- Assert.ThrowsAny:
Kiểm tra việc ném ra bất kỳ exception nào.
Assert.ThrowsAny<Exception>(() => { /* Code ném exception */ });

- TheoryAttribute:
Đánh dấu một phương thức là một test method kiểm thử dựa trên các trường hợp dữ liệu khác nhau.
[Theory]
[InlineData(data1, data2, ...)]
public void TestMethod(params)
{
    // Test code
}

- [Fact] là một thuộc tính (attribute) được sử dụng để đánh dấu một phương thức là một test method. Khi bạn đánh dấu một phương thức bằng [Fact], xUnit sẽ biết rằng phương thức đó cần được chạy như một test và sẽ thực hiện kiểm tra kết quả của nó.
- [Theory] là một thuộc tính (attribute) trong thư viện kiểm thử xUnit, được sử dụng để đánh dấu một phương thức là một test method kiểm thử dựa trên các trường hợp dữ liệu khác nhau. Điều này cho phép bạn viết một test method và chạy nó với nhiều bộ dữ liệu đầu vào khác nhau để kiểm tra các trường hợp khác nhau mà không cần viết nhiều phương thức kiểm thử riêng biệt.


- cách chạy test dotnet test