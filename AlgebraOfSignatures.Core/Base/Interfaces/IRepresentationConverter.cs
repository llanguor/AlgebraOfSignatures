namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IRepresentationConverter
{
    #region Methods
    
    Signature ComputeSignatureFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);

    Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix);

    Array ComputeIncidenceFromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree);
    
    Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix);

    Array ComputeAdjacencyFromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree);
    
    Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree);
    
    #endregion
}