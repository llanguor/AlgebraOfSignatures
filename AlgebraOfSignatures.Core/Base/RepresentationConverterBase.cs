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

    public Array ComputeSignatureFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        return ComputeSignatureFromAdjacency(
            ComputeAdjacencyFromIncidence(
                incidenceMatrix, uniformityDegree));
    }

    public Array ComputeIncidenceFromSignature(
        Array signature,
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

    protected void ThrowIfIllegalSignature(Array signature, int vertexCount)
    {
        //todo: throw if illegal signature 
        //todo: throw if x > 2^(v-1)  // (signature for 2-ranked) >= 2^(vertexCount-1)  // (signature for 2-ranked) > (2<<(n-1))
        //throw new NotImplementedException();
    }

    protected void ThrowIfIllegalIncidence(Array incidenceMatrix)
    {
        throw new NotImplementedException();
    }
    
    protected void ThrowIfIllegalAdjacency(Array incidenceMatrix)
    {
        // throw new NotImplementedException();
    }
    
    #endregion
    
    
    #region Abstract Methods

    public abstract Array ComputeSignatureFromAdjacency(
        Array adjacencyMatrix);
    
    public abstract Array ComputeAdjacencyFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree);
    
    public abstract Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix);
    
    public abstract Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);
    
    #endregion
}