using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core.Base;

public class Matrix<T> :
    IMatrix
{
    #region Actions
    public event Action<int[], T?>? OnSetValue;
    
    #endregion
    
    
    #region Constructors
    
    public Matrix(
        Array value)
    {
        if (value.GetType().GetElementType() != typeof(T))
            throw new ArgumentException(
                $"Expected array of type {typeof(T).Name}, " +
                $"but got {value.GetType().GetElementType()?.Name}.");
        
        Value = value;
    }
    
    public Matrix(
        int size,
        int rank)
    {
        Value = ArrayExtensions.CreateRankedArray<T>(size, rank);
    }
    
    #endregion
    
    
    #region Properties
    
    public int Rank => 
        Value.Rank;
    
    public int Size =>
        Value.GetLength(0);
    
    public Type ElementType
        => typeof(T);

    public Array Value
    {
        get;
        private init;
    }
    
    #endregion
    
    #region Methods

    public T? GetValue(
        params int[] indices)
        => (T?)Value.GetValue(indices);
    
    public void SetValue(
        T? value,
        params int[] indices)
    {
        Value?.SetValue(value, indices);
        OnSetValue?.Invoke(indices, value);
    }
    
    #endregion
    
    #region IGraphRepresentation implementation
    
    object? IGraphRepresentation.GetValue(params int[] indices)
        => GetValue(indices);

    void IGraphRepresentation.SetValue(object? value, params int[] indices)
        => SetValue((T?)value, indices);
    
    #endregion

    #region ICloneable implementation
    
    public object Clone()
        => new Matrix<T>((Array)Value.Clone());
    
    #endregion
}