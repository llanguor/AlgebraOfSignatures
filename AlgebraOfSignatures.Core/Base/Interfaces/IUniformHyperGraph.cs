namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IUniformHyperGraph
{
    #region Properties
    
    public Array IncidenceMatrix { get; }

    public Array AdjacencyMatrix { get; }
        
    public Signature Signature { get; }
    
    public int VertexCount { get; }
    
    public int UniformityDegree { get; }
    
    #endregion
    
    
    #region Methods
    
    public IUniformHyperGraph Intersect(
        IUniformHyperGraph other);

    public IUniformHyperGraph Union(
        IUniformHyperGraph other);

    public IUniformHyperGraph Mod2N(
        int n);

    public IUniformHyperGraph Add(
        IUniformHyperGraph other);

    public IUniformHyperGraph Add(
        int constant);

    public IUniformHyperGraph Multiply(
        IUniformHyperGraph other);

    public IUniformHyperGraph Multiply(
        int constant);
    
    #endregion
}