using AlgebraOfSignatures.Core.Base.Interfaces;
namespace AlgebraOfSignatures.Core.Base;

public abstract class RepresentationConverterBase :
    IRepresentationConverter
{
    #region Methods
    
    protected internal Array CreateRankedArray<T>(
        int size,
        int rank)
    {
        if (size < 1)
            throw new ArgumentException($"{nameof(size)} must be greater than 0");
        
        if (rank < 1)
            throw new ArgumentException($"{nameof(rank)} must be greater than 0");
        
        var shape = Enumerable
            .Repeat(size, rank)
            .ToArray();
        
        return Array.CreateInstance(
            typeof(T),
            shape);
    }
    
    /// <summary>
    /// Generates all permutations of the input array using Heap's algorithm.
    /// The array is modified in-place, and each permutation is passed to the handler.
    /// </summary>
    /// <param name="indices">Array being permuted (modified in-place).</param>
    /// <param name="handler">Called for each permutation. The same array instance is reused.</param>
    protected void ForEachPermutation(
        int[] indices,
        Action<int[]> handler)
    {
        var len = indices.Length;
        Span<int> depth = stackalloc int[indices.Length]; 

        handler(indices);

        var i = 0;
        while (i < len)
        {
            if (depth[i] < i)
            {
                if ((i & 1) == 0)
                    (indices[0], indices[i]) = (indices[i], indices[0]);
                else
                    (indices[depth[i]], indices[i]) = (indices[i], indices[depth[i]]);
                
                handler(indices);

                ++depth[i];
                i = 0;
            }
            else
            {
                depth[i] = 0;
                ++i;
            }
        }
    }

    public Signature ComputeSignatureFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        return ComputeSignatureFromAdjacency(
            ComputeAdjacencyFromIncidence(
                incidenceMatrix, uniformityDegree));
    }

    public Array ComputeIncidenceFromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree)
    {
        return ComputeIncidenceFromAdjacency(
            ComputeAdjacencyFromSignature(
                signature,
                vertexCount,
                uniformityDegree));
    }
    
    #endregion

    
    #region ThrowIf Methods
    
    protected void ThrowIfIllegalGraphParameters( 
        int vertexCount,
        int uniformityDegree)
    {
        ThrowIfIllegalVertexCount(vertexCount);
        ThrowIfIllegalUniformityDegree(uniformityDegree);
    }
    
    protected void ThrowIfIllegalVertexCount( 
        int vertexCount)
    {
        if (vertexCount < 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(vertexCount)} must be greater than or equal to 1.");
    }
    
    protected void ThrowIfIllegalUniformityDegree( 
        int uniformityDegree)
    {
        if (uniformityDegree < 2)
            throw new ArgumentOutOfRangeException(
                $"{nameof(uniformityDegree)} must be greater than or equal to 2.");
    }

    protected void ThrowIfIllegalSignature(Signature signature, int vertexCount)
    {
        //todo: throw if illegal signature 
        //todo: throw if x > 2^(v-1) 
        //throw new NotImplementedException();
    }

    protected void ThrowIfIllegalIncidence(Array incidenceMatrix)
    {
        throw new NotImplementedException();
    }
    
    protected void ThrowIfIllegalAdjacency(Array incidenceMatrix)
    {
        // throw new NotImplementedException();
        
        if (incidenceMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException($"{nameof(incidenceMatrix)} elements must be of type bool");

    }
    
    #endregion
    
    
    #region Abstract Methods

    public abstract Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix);
    
    public abstract Array ComputeAdjacencyFromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree);
    
    public abstract Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix);
    
    public abstract Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);
    
    #endregion
    
}