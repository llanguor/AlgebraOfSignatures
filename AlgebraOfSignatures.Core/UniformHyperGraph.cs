using System.Runtime.InteropServices;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class UniformHyperGraph :
    IUniformHyperGraph, 
    ICloneable,
    IEquatable<UniformHyperGraph>
{
    #region Fabric Methods

    public static UniformHyperGraph FromFile(
        string path)
    {
        Array ToArray(System.Text.Json.Nodes.JsonArray node)
        {
            if (node[0] is System.Text.Json.Nodes.JsonValue)
            {
                var flat = Array.CreateInstance(typeof(long), node.Count);
                for (var i = 0; i < node.Count; i++)
                    flat.SetValue(node[i]!.GetValue<long>(), i);
                return flat;
            }
 
            var subs    = node.Select(n => ToArray(n!.AsArray())).ToArray();
            var lengths = new[] { subs.Length }.Concat(Enumerable.Range(0, subs[0].Rank).Select(subs[0].GetLength)).ToArray();
            var result  = Array.CreateInstance(typeof(long), lengths);
            var idx     = new int[lengths.Length];
 
            void Copy(Array src, int dim)
            {
                for (var i = 0; i < src.GetLength(dim - 1); i++)
                {
                    idx[dim] = i;
                    if (dim == src.Rank)
                    {
                        var srcIdx = idx[1..];
                        result.SetValue(src.GetValue(srcIdx), idx);
                    }
                    else Copy(src, dim + 1);
                }
            }
 
            for (var i = 0; i < subs.Length; i++) { idx[0] = i; Copy(subs[i], 1); }
 
            return result;
        }
 
        var doc = System.Text.Json.Nodes.JsonNode.Parse(File.ReadAllText(path))!;
 
        return FromSignature(
            new Matrix<long>(ToArray(doc["Array"]!.AsArray())),
            doc["VertexCount"]!.GetValue<int>(),
            doc["UniformityDegree"]!.GetValue<int>());
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
    
    public static UniformHyperGraph FromVertexDegreeVector(
        Matrix<int> vertexDegreeVector,
        IRepresentationConverter? converter = null)
    {
        if (vertexDegreeVector.ElementType != typeof(int))
            throw new ArgumentException(
                $"Expected {typeof(int)} array",
                nameof(vertexDegreeVector));
        
        converter ??= new RepresentationConverter();
        var vertexCount = vertexDegreeVector.Size;
        var uniformityDegree = vertexDegreeVector.Rank;
        
        return new UniformHyperGraph(
            converter,
            converter.ComputeSignatureFromVertexDegreeVector(vertexDegreeVector),
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
    
    private Matrix<int>? _cachedVertexDegreeVector = null;

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
                Signature);

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
                Signature);

            return _cachedAdjacencyMatrix!;
        }
    }
    
    /// <summary>
    /// Incidence matrix lazily computed from <see cref="Signature"/> and cached until invalidated.
    /// Mutating the returned value has no effect — modify <see cref="Signature"/> instead, then call <see cref="InvalidateCache"/>.
    /// </summary>
    public Matrix<int> VertexDegreeVector
    {
        get
        {
            _cachedVertexDegreeVector ??= _converter.ComputeVertexDegreeVectorFromSignature(
                Signature);

            return _cachedVertexDegreeVector!;
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
        _cachedVertexDegreeVector = null;
    }

    public void SaveToFile(string path)
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("RepresentationType=Signature");
        sb.AppendLine($"UniformityDegree={UniformityDegree}");
        sb.AppendLine($"VertexCount={VertexCount}");
        sb.AppendLine("Array=");
        AppendArray(sb, Signature.Value.Value, new int[Signature.Value.Rank], 0);

        File.WriteAllText(path, sb.ToString());
    }

    private void AppendArray(
        System.Text.StringBuilder sb,
        Array array, 
        int[] indices,
        int dimension)
    {
        var size = array.GetLength(dimension);
        var isLast = dimension == array.Rank - 1;

        sb.Append('{');

        for (var i = 0; i < size; i++)
        {
            indices[dimension] = i;

            if (isLast)
            {
                sb.Append(array.GetValue(indices));
                if (i < size - 1)
                    sb.Append(", ");
            }
            else
            {
                AppendArray(sb, array, indices, dimension + 1);
                if (i < size - 1)
                    sb.AppendLine(",");
            }
        }

        sb.Append('}');
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

    public UniformHyperGraph Add(UniformHyperGraph other, Signature.AddType type)
    {
        this.Signature.Add(other.Signature, type);
        return this;
    }

    public UniformHyperGraph Add(long constant, Signature.AddType type)
    {
        this.Signature.Add(constant, type);
        return this;
    }
    
    #endregion
    
    
    #region Static Operators Methods
    
    public static UniformHyperGraph Intersect(UniformHyperGraph a, UniformHyperGraph b) => 
        a.Clone().Intersect(b);

    public static UniformHyperGraph Union(UniformHyperGraph a, UniformHyperGraph b) => 
        a.Clone().Union(b);
    
    public static UniformHyperGraph Add(UniformHyperGraph a, UniformHyperGraph b, Signature.AddType type) =>
        a.Clone().Add(b, type);

    public static UniformHyperGraph Add(UniformHyperGraph a, long constant, Signature.AddType type) =>
        a.Clone().Add(constant, type);

    public static UniformHyperGraph Mod2N(UniformHyperGraph a, int n) =>
        a.Clone().Mod2N(n);

    #endregion
    
    
    #region Operators
    
    public static UniformHyperGraph operator &(UniformHyperGraph a, UniformHyperGraph b) => 
        UniformHyperGraph.Intersect(a, b);

    public static UniformHyperGraph operator |(UniformHyperGraph a, UniformHyperGraph b) => 
        UniformHyperGraph.Union(a, b);
    
    public static UniformHyperGraph operator +(UniformHyperGraph a, UniformHyperGraph b) =>
        UniformHyperGraph.Add(a, b, Signature.AddType.Vertical);

    public static UniformHyperGraph operator +(UniformHyperGraph a, long constant) =>
        UniformHyperGraph.Add(a, constant, Signature.AddType.Vertical);
    
    public static bool operator ==(UniformHyperGraph? a, UniformHyperGraph? b) =>
        a?.Equals(b) ?? b is null;

    public static bool operator !=(UniformHyperGraph? a, UniformHyperGraph? b) =>
        !(a == b);

    public static bool operator <(UniformHyperGraph a, UniformHyperGraph b) =>
        a._signature.CompareTo(b._signature) < 0;

    public static bool operator >(UniformHyperGraph a, UniformHyperGraph b) =>
        a._signature.CompareTo(b._signature) > 0;

    public static bool operator <=(UniformHyperGraph a, UniformHyperGraph b) =>
        a._signature.CompareTo(b._signature) <= 0;

    public static bool operator >=(UniformHyperGraph a, UniformHyperGraph b) =>
        a._signature.CompareTo(b._signature) >= 0;
    
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
        IncidenceMatrix = 2,
        VertexDegreeVector = 3
    }

    #endregion
    
    
    #region IEquatable<UniformHyperGraph> Implementation

    public bool Equals(UniformHyperGraph? other)
    {
        if (other is null) 
            return false;
        
        if (ReferenceEquals(this, other)) 
            return true;
        
        return
            _uniformityDegree == other._uniformityDegree &&
            _vertexCount == other._vertexCount &&
            _signature.Equals(other._signature);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        
        if (ReferenceEquals(this, obj)) 
            return true;
        
        if (obj.GetType() != GetType())
            return false;
        
        return Equals((UniformHyperGraph)obj);
    }

    public override int GetHashCode()
    {
        return 
            HashCode.Combine(
                _signature,
                _uniformityDegree, 
                _vertexCount);
    }
    
    #endregion
}