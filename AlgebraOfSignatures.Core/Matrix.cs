using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

public class Matrix<T> :
    IMatrix,
    IEquatable<Matrix<T>>
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

    #region IEquatable<Matrix<T>> implementation
    
    public bool Equals(Matrix<T>? other)
    {
        if (other is null) 
            return false;
    
        if (ReferenceEquals(this, other))
            return true;

        if (Value.Rank != other.Value.Rank)
            return false;

        for (var i = 0; i < Value.Rank; i++)
            if (Value.GetLength(i) != other.Value.GetLength(i))
                return false;

        var xEnum = Value.GetEnumerator();
        var yEnum = other.Value.GetEnumerator();

        while (xEnum.MoveNext())
        {
            yEnum.MoveNext();

            if (!EqualityComparer<T>.Default.Equals((T?)xEnum.Current, (T?)yEnum.Current))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(Value.Rank);

        for (var i = 0; i < Value.Rank; i++)
            hash.Add(Value.GetLength(i));

        var enumerator = Value.GetEnumerator();
        while (enumerator.MoveNext())
            hash.Add(enumerator.Current);

        return hash.ToHashCode();
    }
    
    #endregion
}