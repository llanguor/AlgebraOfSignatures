namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IUniformHyperGraph
{
    public Matrix<bool> IncidenceMatrix { get; }

    public Matrix<bool> AdjacencyMatrix { get; }
        
    public Signature Signature { get; }
    
    public int VertexCount { get; }
    
    public int UniformityDegree { get; }
}