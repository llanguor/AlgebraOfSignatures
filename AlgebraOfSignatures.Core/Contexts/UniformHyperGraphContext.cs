using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.UniformHyperGraphs;
namespace AlgebraOfSignatures.Core.Contexts;

internal sealed class UniformHyperGraphContext :
    IUniformHyperGraph
{
    #region Fields

    private readonly IUniformHyperGraph _uniformHyperGraph;
    
    #endregion


    #region Constructors

    public UniformHyperGraphContext(
        IRepresentationConverter converter,
        Signature signature,
        int vertexCount,
        int uniformityDegree)
    {
        if(uniformityDegree < 2)
            throw new ArgumentException("Uniformity Degree must be at least 2.");
 
        _uniformHyperGraph = uniformityDegree switch
        {
            2 =>
                new Uniform2HyperGraph(converter, signature, vertexCount, uniformityDegree),
            3 =>
                new Uniform3HyperGraph(converter, signature, vertexCount, uniformityDegree),
            _ =>
                new UniformNHyperGraph(converter, signature, vertexCount, uniformityDegree),
        };
        
    }

    #endregion
    
    
    #region Properties Implementation from IUniformHyperGraph
    
    public Array IncidenceMatrix => 
        _uniformHyperGraph.IncidenceMatrix; 
    
    public Array AdjacencyMatrix =>
        _uniformHyperGraph.AdjacencyMatrix;
    
    public Signature Signature =>
        _uniformHyperGraph.Signature;
    
    public int VertexCount =>
        _uniformHyperGraph.VertexCount;
    
    public int UniformityDegree =>
        _uniformHyperGraph.UniformityDegree;
    
    #endregion
    
    
    #region Methods Implementation from IUniformHyperGraph
    
    public IUniformHyperGraph Intersect(IUniformHyperGraph other) =>
        _uniformHyperGraph.Intersect(other);

    public IUniformHyperGraph Union(IUniformHyperGraph other) =>
        _uniformHyperGraph.Union(other);

    public IUniformHyperGraph Mod2N(int n) =>
        _uniformHyperGraph.Mod2N(n);

    public IUniformHyperGraph Add(IUniformHyperGraph other) =>
        _uniformHyperGraph.Add(other);
    
    public IUniformHyperGraph Add(int constant) =>
        _uniformHyperGraph.Add(constant);

    public IUniformHyperGraph Multiply(IUniformHyperGraph other) => 
        _uniformHyperGraph.Multiply(other);

    public IUniformHyperGraph Multiply(int constant) =>
        _uniformHyperGraph.Multiply(constant);
    
    #endregion
}