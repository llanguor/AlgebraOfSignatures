namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface ISignature
{
    #region Properties
    
    public int VertexCount { get; }
    
    public int UniformityDegree { get; }
    
    #endregion
    
    
    #region Methods
    
    public void SetValue(
        long value,
        params int[] indices);
    
    public long GetValue(
        params int[] indices);
    
    #endregion
}