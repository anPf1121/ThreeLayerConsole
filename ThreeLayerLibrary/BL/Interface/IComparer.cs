using Model;
namespace Interface; 

public interface IComparer
{
    int Compare<T>(T x, T y);
}
