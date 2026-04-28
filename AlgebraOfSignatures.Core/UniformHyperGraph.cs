using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Contexts;

namespace AlgebraOfSignatures.Core;

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
            converter.ComputeSignatureFromIncidence(
                incidenceMatrix, 
                uniformityDegree),
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

    public static IUniformHyperGraph FromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree)
    {
        var converter = new RepresentationConverterContext(
            uniformityDegree);

        return new UniformHyperGraphContext(
            converter,
            signature, 
            vertexCount, 
            uniformityDegree);   
    }
    
    public static IUniformHyperGraph FromSignature(
        long signatureValue,
        int vertexCount,
        int uniformityDegree)
    {
        return FromSignature(
            new Signature(
                signatureValue,
                vertexCount,
                uniformityDegree),
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

    public Signature Signature { get; private init; }
    
    public int UniformityDegree { get; private init; }
    
    public int VertexCount { get; protected set; }
    
    #endregion
    
    
    #region Constructors

    protected UniformHyperGraph(
        IRepresentationConverter converter,
        Signature signature,
        int vertexCount,
        int uniformityDegree)
    {
        if (vertexCount < 2)
            throw new ArgumentException("Vertex count cannot be less than 2.", nameof(vertexCount));
        
        if (uniformityDegree < 2)
            throw new ArgumentException("Uniformity edge cannot be less than 2.", nameof(uniformityDegree));
        
        Converter = converter;
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
        Signature = signature;

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