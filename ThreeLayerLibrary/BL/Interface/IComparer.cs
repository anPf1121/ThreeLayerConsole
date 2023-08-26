using Model;
namespace Interface; 

public interface IComparer
{
    int IntCompare<T>(T x, T y);
    bool BoolCompare<T>(T x, T y);
    
}
