namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface ISignature
{
    #region Properties
    
    public long Value { get; }
    
    public int VertexCount { get; }
    
    public int UniformityDegree { get; }
    
    #endregion
    
    
    #region Methods
    
    public void SetValue(
        long value,
        int vertexCount,
        int uniformityDegree);
    
    /// <summary>
    /// Truncates the signature by the number of bits corresponding to one upper row of the adjacency matrix.
    /// The provided index coincides with the prism cut index of the k-signature counted from its lower layer.
    /// </summary>
    /// <param name="degreeOfTruncation">The degree of truncation.</param>
    /// <returns>The truncated signature.</returns>
    public long GetValue(
        int degreeOfTruncation);
    
    public long GetValue();
    
    #endregion
}