namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IHyperGraphRepresentationConverter
{
    Array ComputeSignatureFromIncidenceMatrix(
        Array incidenceMatrix);

    Array ComputeSignatureFromAdjacencyMatrix(
        Array adjacencyMatrix);

    Array ComputeIncidenceMatrixFromSignature(
        Array signature);
    
    Array ComputeIncidenceMatrixFromAdjacencyMatrix(
        Array adjacencyMatrix);

    Array ComputeAdjacencyMatrixFromSignature(
        Array signature);
    
    Array ComputeAdjacencyMatrixFromIncidenceMatrix(
        Array incidenceMatrix);
}