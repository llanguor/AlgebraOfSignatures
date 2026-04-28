namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IUniformHyperGraph
{
    public Array IncidenceMatrix { get; }

    public Array AdjacencyMatrix { get; }
        
    public Signature Signature { get; }
    
    public int VertexCount { get; }
    
    public int UniformityDegree { get; }
}