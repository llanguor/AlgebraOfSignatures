namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IHyperGraphRepresentationConverter
{
    //todo: generalize?
    
    Array ComputeSignatureFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);

    Array ComputeSignatureFromAdjacency(
        Array adjacencyMatrix);

    Array ComputeIncidenceFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree);
    
    Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix);

    Array ComputeAdjacencyFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree);
    
    Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);
}