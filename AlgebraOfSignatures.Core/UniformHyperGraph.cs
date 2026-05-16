using System.Runtime.InteropServices;
using AlgebraOfSignatures.Core.Base;
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
        Matrix<bool> incidenceMatrix,
        int uniformityDegree,
        IRepresentationConverter? converter = null)
    {
        if (incidenceMatrix.ElementType != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array", 
                nameof(incidenceMatrix));
        
        converter ??= new RepresentationConverter();
        var vertexCount = incidenceMatrix.Size;
        
        return new UniformHyperGraph(
            converter,
            converter.ComputeSignatureFromIncidence(
                incidenceMatrix, 
                uniformityDegree),
            vertexCount,
            uniformityDegree);
    }
    
    public static UniformHyperGraph FromAdjacencyMatrix(
        Matrix<bool> adjacencyMatrix,
        IRepresentationConverter? converter = null)
    {
        if (adjacencyMatrix.ElementType != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array",
                nameof(adjacencyMatrix));
        
        converter ??= new RepresentationConverter();
        var vertexCount = adjacencyMatrix.Size;
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
        Matrix<long> signatureValue,
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

    private readonly Signature _signature;
    
    private readonly int _uniformityDegree;

    private readonly int _vertexCount;

    private readonly IRepresentationConverter _converter;

    #endregion
    
    
    #region Fields

    private Matrix<bool>? _cachedIncidenceMatrix = null;
    
    private Matrix<bool>? _cachedAdjacencyMatrix = null;

    #endregion
    
    
    #region Properties
    
    
    /// <summary>
    /// Incidence matrix lazily computed from <see cref="Signature"/> and cached until invalidated.
    /// Mutating the returned value has no effect — modify <see cref="Signature"/> instead, then call <see cref="InvalidateCache"/>.
    /// </summary>
    public Matrix<bool> IncidenceMatrix
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

    /// <summary>
    /// Adjacency matrix lazily computed from <see cref="Signature"/> and cached until invalidated.
    /// Mutating the returned value has no effect — modify <see cref="Signature"/> instead, then call <see cref="InvalidateCache"/>.
    /// </summary>
    public Matrix<bool> AdjacencyMatrix
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

    public Signature Signature
    {
        get => _signature;
        private init
        {
            _signature = value;
            UniformityDegree = value.UniformityDegree;
            VertexCount = value.VertexCount;
            InvalidateCache();
        }
    }

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
        Signature.Value.OnSetValue += SignatureOnOnSetValue;
    }

    #endregion
    
        
    #region Event Handlers
    
    private void SignatureOnOnSetValue(int[] indices, long value)
    {
        InvalidateCache();
    }
    
    #endregion


    #region Methods

    private void InvalidateCache()
    {
        _cachedIncidenceMatrix = null;
        _cachedAdjacencyMatrix = null;
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

    public static UniformHyperGraph Mod2N(UniformHyperGraph a, int n) =>
        a.Clone().Mod2N(n);

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
        AdditionConst = 3
    }

    #endregion
}