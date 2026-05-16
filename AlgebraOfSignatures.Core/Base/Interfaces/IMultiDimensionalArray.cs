namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IMultiDimensionalArray
{
    public Array? Value { get; }
    
    public object? GetValue(
        params int[] indices);
    
    public void SetValue(
        object? value,
        params int[] indices);
}