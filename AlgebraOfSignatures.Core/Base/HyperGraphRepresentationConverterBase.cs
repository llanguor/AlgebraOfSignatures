using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core.Base;

public abstract class HyperGraphRepresentationConverterBase :
    IHyperGraphRepresentationConverter
{
    protected Array CreateRankedArray<T>(
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

    public abstract Array ComputeSignatureFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);
    
    public abstract Array ComputeSignatureFromAdjacency(
        Array adjacencyMatrix);

    public abstract Array ComputeIncidenceFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree);

    public abstract Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix);

    public abstract Array ComputeAdjacencyFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree);

    public abstract Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);
}