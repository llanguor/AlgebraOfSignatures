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
    /// Retrieves a value from a  cell of the signature prism.
    /// The indices passed as parameters must be prism indices.
    /// 
    ///  The signatures prism for 3-uniform graph has the following appearance
    /// (where the numbers are indices). Here, each number stored in a cell
    /// is a signature. In each subsequent cell,
    /// the number is one digit less than the previous value.:
    /// <code>
    /// 00 01 02 03
    /// </code>
    /// For example, these cells could store signatures such as:
    /// <code>
    /// 1011, 011, 01, 0
    /// </code> 
    /// The prism for 4-uniform graph has the following appearance:
    /// <code>
    /// 00 01 02 03
    /// -- 11 12 13
    /// -- -- 22 23
    /// -- -- -- 33
    /// </code>
    /// The prism for a 5-uniform graph is three-dimensional. And so on.
    /// <para>
    /// For example, for a 4-homogeneous graph, you can retrieve the prism layer
    /// at cell "12" by passing indices (1,2).
    /// The result will be the signature corresponding to that layer.
    /// If the graph is 5-homogeneous, 3 indices are expected as input,
    /// since the prism is essentially three-dimensional. And so on.
    /// </para>
    /// <para>
    /// If there is only one layer (which is possible for a 2-homogeneous graph),
    /// the method returns the <see cref="Value"/> describing its adjacency matrix.
    /// </para>
    /// </summary>
    /// <param name="indices">
    /// The prism indices used to access the target cell.
    /// The number of indices must match the dimensionality of the prism,
    /// which is <c>VertexCount - 2</c>. If no indices are provided,
    /// the raw <see cref="Value"/> is returned directly.
    /// </param>
    /// <returns>
    /// The <see cref="long"/> value stored at the specified position in the signature prism.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the number of provided indices does not match
    /// the expected dimensionality of the prism.
    /// </exception>
    public long GetValue(
        params int[] indices);
    
    #endregion
}