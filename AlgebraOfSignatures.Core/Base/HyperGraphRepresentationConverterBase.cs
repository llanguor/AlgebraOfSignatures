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

    public abstract Array ComputeSignatureFromIncidenceMatrix(
        Array incidenceMatrix);
    
    public abstract Array ComputeSignatureFromAdjacencyMatrix(
        Array adjacencyMatrix);
    
    public abstract Array ComputeIncidenceMatrixFromSignature(
        Array signature);

    public abstract Array ComputeIncidenceMatrixFromAdjacencyMatrix(
        Array adjacencyMatrix);

    public abstract Array ComputeAdjacencyMatrixFromSignature(
        Array signature);

    public abstract Array ComputeAdjacencyMatrixFromIncidenceMatrix(
        Array incidenceMatrix);
}