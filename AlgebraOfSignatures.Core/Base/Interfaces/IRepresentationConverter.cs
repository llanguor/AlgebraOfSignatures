namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IRepresentationConverter
{
    #region Methods
    
    Signature ComputeSignatureFromIncidence(
        Matrix<bool> incidenceMatrix,
        int uniformityDegree);

    Signature ComputeSignatureFromAdjacency(
        Matrix<bool> adjacencyMatrix,
        bool isThrowIfIncorrectAdjacencyMatrix = false);

    Matrix<bool> ComputeIncidenceFromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree);
    
    Matrix<bool> ComputeIncidenceFromAdjacency(
        Matrix<bool> adjacencyMatrix);

    Matrix<bool> ComputeAdjacencyFromSignature(
        Signature signature,
        int vertexCount,
        int uniformityDegree);
    
    Matrix<bool> ComputeAdjacencyFromIncidence(
        Matrix<bool> incidenceMatrix,
        int uniformityDegree);
    
    #endregion
}