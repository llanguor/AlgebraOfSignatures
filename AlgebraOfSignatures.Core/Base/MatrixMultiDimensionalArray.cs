using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core.Base;

public class MultiDimensionalArray<T> :
    IMultiDimensionalArray,
    ICloneable
{
    public event Action<int[], object?>? ValueChanged;
    
    private MultiDimensionalArray(
        Array value)
    {
        if (value.GetType().GetElementType() != typeof(T))
            throw new ArgumentException(
                $"Expected array of type {typeof(T).Name}, " +
                $"but got {value.GetType().GetElementType()?.Name}.");
        
        Value = value;
    }
    
    public MultiDimensionalArray(
        int size,
        int rank)
    {
        Value = ArrayExtensions.CreateRankedArray<T>(size, rank);
    }
    
    public int Rank => 
        Value.Rank;
    
    public int Size =>
        Value.GetLength(0);
    
    public int Length => 
        Value.Length;

    public Array Value
    {
        get;
        private init;
    }
    
    public object? GetValue(
        params int[] indices)
        => Value.GetValue(indices);
    
    public void SetValue(
        object? value,
        params int[] indices)
    {
        Value?.SetValue(value, indices);
        ValueChanged?.Invoke(indices, value);
    }

    public object Clone()
        => new MultiDimensionalArray<T>((Array)Value.Clone());
}