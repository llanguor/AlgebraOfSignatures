namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IGraphRepresentation :
    ICloneable
{
    public object? GetValue(
        params int[] indices);
    
    public void SetValue(
        object? value,
        params int[] indices);
    
    public int Size { get; }
    
    public int Rank { get; }
    
    public Type ElementType { get; }
}