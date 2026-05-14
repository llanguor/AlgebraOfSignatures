using System.Runtime.InteropServices;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class UniformHyperGraph :
    IUniformHyperGraph, 
    ICloneable
{
    #region Fabric Methods

    public static UniformHyperGraph FromFile(
        string path)
    {
        return Empty(3, 2);
    }

    public static UniformHyperGraph FromIncidenceMatrix(
        Array incidenceMatrix,
        int uniformityDegree,
        IRepresentationConverter? converter = null)
    {
        if (incidenceMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array", 
                nameof(incidenceMatrix));
        
        converter ??= new RepresentationConverter();
        var vertexCount = incidenceMatrix.GetLength(0);
        
        return new UniformHyperGraph(
            converter,
            converter.ComputeSignatureFromIncidence(
                incidenceMatrix, 
                uniformityDegree),
            vertexCount,
            uniformityDegree);
    }
    
    public static UniformHyperGraph FromAdjacencyMatrix(
        Array adjacencyMatrix,
        IRepresentationConverter? converter = null)
    {
        if (adjacencyMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array",
                nameof(adjacencyMatrix));
        
        converter ??= new RepresentationConverter();
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        
        return new UniformHyperGraph(
            converter,
            converter.ComputeSignatureFromAdjacency(adjacencyMatrix),
            vertexCount,
            uniformityDegree);
    }

    public static UniformHyperGraph FromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree,
        IRepresentationConverter? converter = null)
    {
        converter ??= new RepresentationConverter();
        return new UniformHyperGraph(
            converter,
            signature, 
            vertexCount, 
            uniformityDegree);   
    }
    
    public static UniformHyperGraph FromSignature(
        Array signatureValue,
        int vertexCount,
        int uniformityDegree,
        IRepresentationConverter? converter = null)
    {
        return FromSignature(
            new Signature(
                signatureValue,
                vertexCount,
                uniformityDegree),
            vertexCount,
            uniformityDegree,
            converter); 
    }
    
    public static UniformHyperGraph Empty(
        int vertexCount,
        int uniformityDegree,
        IRepresentationConverter? converter = null)
    {
        return FromSignature(
                Core.Signature.Empty(vertexCount, uniformityDegree),
                vertexCount,
                uniformityDegree,
                converter); 
    }

    #endregion
    
    
    #region Fields

    private readonly int _uniformityDegree;

    private readonly int _vertexCount;

    private readonly IRepresentationConverter _converter;

    #endregion
    
    
    #region Properties

    private Array? _cachedIncidenceMatrix = null;
    private Array? _cachedAdjacencyMatrix = null;

    public Array IncidenceMatrix
    {
        get
        {
            _cachedIncidenceMatrix ??= _converter.ComputeIncidenceFromSignature(
                Signature,
                VertexCount,
                UniformityDegree);

            return _cachedIncidenceMatrix!;
        }
    }

    public Array AdjacencyMatrix
    {
        get
        {
            _cachedAdjacencyMatrix ??= _converter.ComputeAdjacencyFromSignature(
                Signature,
                VertexCount,
                UniformityDegree);

            return _cachedAdjacencyMatrix!;
        }
    }

    public Signature Signature { get; private init; }

    public int UniformityDegree
    {
        get => _uniformityDegree;
        private init
        {
            if (value < 2)
                throw new ArgumentException(
                    "Uniformity edge cannot be less than 2.",
                    nameof(UniformityDegree));
            
            _uniformityDegree = value;
        }
    }

    public int VertexCount
    {
        get => _vertexCount;
        private init
        {
            if (value < 2)
                throw new ArgumentException(
                    "Vertex count cannot be less than 2.", 
                    nameof(VertexCount));
            
            _vertexCount = value;
        }
    }
    
    #endregion
    
    
    #region Constructors

    protected UniformHyperGraph(
        IRepresentationConverter converter,
        Signature signature,
        int vertexCount,
        int uniformityDegree)
    {
        _converter = converter;
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
        Signature = signature;
    }
    
    #endregion
    
    
    #region Operators Methods

    public UniformHyperGraph Intersect(UniformHyperGraph other)
    {
        this.Signature.Intersect(other.Signature);
        return this;
    }

    public UniformHyperGraph Union(UniformHyperGraph other)
    {
        this.Signature.Union(other.Signature);
        return this;
    }

    public UniformHyperGraph Mod2N(int n)
    {
        this.Signature.Mod2N(n);
        return this;
    }

    public UniformHyperGraph Add(UniformHyperGraph other)
    {
        this.Signature.Add(other.Signature);
        return this;
    }

    public UniformHyperGraph Add(long constant)
    {
        this.Signature.Add(constant);
        return this;
    }

    public UniformHyperGraph Multiply(UniformHyperGraph other)
    {
        this.Signature.Multiply(other.Signature);
        return this;
    }

    public UniformHyperGraph Multiply(long constant)
    {
        this.Signature.Multiply(constant);
        return this;
    }
    
    #endregion
    
    
    #region Static Operators Methods
    
    public static UniformHyperGraph Intersect(UniformHyperGraph a, UniformHyperGraph b) => 
        a.Clone().Intersect(b);

    public static UniformHyperGraph Union(UniformHyperGraph a, UniformHyperGraph b) => 
        a.Clone().Union(b);
    
    public static UniformHyperGraph Add(UniformHyperGraph a, UniformHyperGraph b) =>
        a.Clone().Add(b);

    public static UniformHyperGraph Add(UniformHyperGraph a, long constant) =>
        a.Clone().Add(constant);

    public static UniformHyperGraph Multiply(UniformHyperGraph a, UniformHyperGraph b) =>
        a.Clone().Multiply(b);

    public static UniformHyperGraph Multiply(UniformHyperGraph a, long constant) =>
        a.Clone().Multiply(constant);
    
    #endregion
    
    
    #region Operators
    
    public static UniformHyperGraph operator &(UniformHyperGraph a, UniformHyperGraph b) => 
        UniformHyperGraph.Intersect(a, b);

    public static UniformHyperGraph operator |(UniformHyperGraph a, UniformHyperGraph b) => 
        UniformHyperGraph.Union(a, b);
    
    public static UniformHyperGraph operator +(UniformHyperGraph a, UniformHyperGraph b) =>
        UniformHyperGraph.Add(a, b);

    public static UniformHyperGraph operator +(UniformHyperGraph a, long constant) =>
        UniformHyperGraph.Add(a, constant);

    public static UniformHyperGraph operator *(UniformHyperGraph a, UniformHyperGraph b) =>
        UniformHyperGraph.Multiply(a, b);

    public static UniformHyperGraph operator *(UniformHyperGraph a, long constant) =>
        UniformHyperGraph.Multiply(a, constant);
    
    #endregion
    
    
    #region ICloneable Implementation

    public UniformHyperGraph Clone()
    {
        return new UniformHyperGraph(
            _converter,
            Signature.Clone(),
            VertexCount,
            UniformityDegree);
    }

    object ICloneable.Clone() => 
        Clone();

    #endregion

    
    #region Nested

    public enum RepresentationTypes
    {
        Signature = 0,
        AdjacencyMatrix = 1,
        IncidenceMatrix = 2
    }
    
    public enum OperationsTypes
    {
        Union = 0,
        Intersection = 1,
        Addition = 2,
        AdditionConst = 3,
        Multiply = 4,
        MultiplyConst = 5
    }

    #endregion
}