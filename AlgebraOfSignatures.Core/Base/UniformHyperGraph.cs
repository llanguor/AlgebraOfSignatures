using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Contexts;
namespace AlgebraOfSignatures.Core.Base;

public abstract class UniformHyperGraph :
    IUniformHyperGraph
{
    #region Fabric Methods
    
    public static IUniformHyperGraph FromIncidenceMatrix(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        if (incidenceMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array", 
                nameof(incidenceMatrix));
        
        var vertexCount = incidenceMatrix.GetLength(0);
        
        var converter = new RepresentationConverterContext(
            uniformityDegree);
        
        return new UniformHyperGraphContext(
            converter,
            converter.ComputeSignatureFromIncidence(incidenceMatrix, uniformityDegree),
            vertexCount,
            uniformityDegree);
    }
    
    public static IUniformHyperGraph FromAdjacencyMatrix(
        Array adjacencyMatrix)
    {
        if (adjacencyMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array",
                nameof(adjacencyMatrix));
        
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        var converter = new RepresentationConverterContext(
            uniformityDegree);
        
        return new UniformHyperGraphContext(
            converter,
            converter.ComputeSignatureFromAdjacency(adjacencyMatrix),
            vertexCount,
            uniformityDegree);
    }
    
    // todo: vertexCount and uniformityDegree can be derived from signature directly?
    // todo: add if(signature array is just int-value) ...      (for api-consistent) 
    public static IUniformHyperGraph FromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        if (signature.GetType().GetElementType() != typeof(int))
            throw new ArgumentException(
                $"Expected {typeof(int)} array", 
                nameof(signature));
        
        var converter = new RepresentationConverterContext(
            uniformityDegree);

        return new UniformHyperGraphContext(
            converter,
            signature, 
            vertexCount, 
            uniformityDegree);   
    }
    
    // todo: vertexCount can be derived from signature directly?
    public static IUniformHyperGraph FromSignature(
        int signature,
        int vertexCount)
    {
        const int uniformityDegree = 2;
        var converter = new RepresentationConverterContext(
            uniformityDegree);

        return new UniformHyperGraphContext(
            converter,
            new[] { signature }, 
            vertexCount, 
            uniformityDegree);   
    }
    
    #endregion
    
    
    #region Fields
    
    private readonly Lazy<Array> _incidenceMatrix;

    private readonly Lazy<Array> _adjacencyMatrix;

    protected readonly IRepresentationConverter Converter;

    #endregion
    
    
    #region Properties

    public Array IncidenceMatrix => 
        _incidenceMatrix.Value;
    
    public Array AdjacencyMatrix => 
        _adjacencyMatrix.Value;

    public Array Signature { get; protected set; }
    
    public int UniformityDegree { get; private init; }
    
    public int VertexCount { get; protected set; }
    
    #endregion
    
    
    #region Constructors

    protected UniformHyperGraph(
        IRepresentationConverter converter,
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        if (signature.GetType().GetElementType() != typeof(int)) 
            throw new ArgumentException(
                $"Expected {typeof(int)} array", 
                nameof(signature));
        
        if (vertexCount < 1)
            throw new ArgumentException(
                $"Length of {nameof(vertexCount)} must be greater than 0",
                nameof(vertexCount));
        
        if (uniformityDegree < 2)
            throw new ArgumentException(
                $"Length of {nameof(uniformityDegree)} must be greater than 1",
                nameof(uniformityDegree));
        
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
        Signature = signature;
        Converter = converter;

        _incidenceMatrix =
            new Lazy<Array>(() =>
                Converter.ComputeIncidenceFromSignature(
                    Signature,
                    VertexCount,
                    UniformityDegree));
            
        _adjacencyMatrix =
            new Lazy<Array>(() => 
                Converter.ComputeAdjacencyFromSignature(
                    Signature, 
                    VertexCount,
                    UniformityDegree));
    }
    
    #endregion

    
    #region Abstract Methods
    
    public abstract IUniformHyperGraph Intersect(
        IUniformHyperGraph other);
    
    public abstract IUniformHyperGraph Union(
        IUniformHyperGraph other);
    
    public abstract IUniformHyperGraph Mod2N(
        int n);
    
    public abstract IUniformHyperGraph Add(
        IUniformHyperGraph other);
    
    public abstract IUniformHyperGraph Add(
        int constant);
    
    public abstract IUniformHyperGraph Multiply(
        IUniformHyperGraph other);
    
    public abstract IUniformHyperGraph Multiply(
        int constant);
    
    #endregion
}