using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core.Base;

public abstract class HyperGraphBase : 
    IHyperGraph
{
    #region Fields
    
    private readonly Lazy<Array> _incidenceMatrix;

    private readonly Lazy<Array> _adjacencyMatrix;

    private readonly IHyperGraphRepresentationConverter _converter;

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
        IHyperGraphRepresentationConverter converter,
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
        Signature = signature;
        _converter = converter;

        _incidenceMatrix =
            new Lazy<Array>(() =>
                _converter.ComputeIncidenceMatrixFromSignature(
                    Signature));
            
        _adjacencyMatrix =
            new Lazy<Array>(() => 
                _converter.ComputeAdjacencyMatrixFromSignature(
                    Signature));
    }
    
    #endregion
}