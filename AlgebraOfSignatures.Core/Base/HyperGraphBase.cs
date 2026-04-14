using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core.Base;

public abstract class HyperGraphBase : 
    IHyperGraph
{
    #region Fields
    
    private readonly Lazy<Array> _incidenceMatrix;

    private readonly Lazy<Array> _adjacencyMatrix;

    #endregion
    
    
    #region Properties

    public Array IncidenceMatrix => _incidenceMatrix.Value;
    
    public Array AdjacencyMatrix => _adjacencyMatrix.Value;
        
    public Array Signature { get; init; }
    
    public int UniformityDegree { get; init; }
    
    public int VertexCount { get; init; }
    
    #endregion
    
    
    #region Constructors

    protected HyperGraphBase(
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
        Signature = signature;

        _incidenceMatrix =
            new Lazy<Array>(() =>
                ComputeIncidenceMatrixFromSignature(
                    Signature,
                    VertexCount, 
                    UniformityDegree));
            
        _adjacencyMatrix =
            new Lazy<Array>(() => 
                ComputeAdjacencyMatrixFromSignature(
                    Signature,
                    VertexCount,
                    UniformityDegree));
    }
    
    #endregion
    
    
    #region Abstract methods

    protected abstract Array ComputeIncidenceMatrixFromSignature(
        Array array,
        int vertexCount,
        int uniformityDegree);

    protected abstract Array ComputeAdjacencyMatrixFromSignature(
        Array array,
        int vertexCount,
        int uniformityDegree);

    #endregion
}