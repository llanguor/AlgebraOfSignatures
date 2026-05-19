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
        Signature signature);
    
    Matrix<bool> ComputeIncidenceFromAdjacency(
        Matrix<bool> adjacencyMatrix);

    Matrix<bool> ComputeAdjacencyFromSignature(
        Signature signature);
    
    Matrix<bool> ComputeAdjacencyFromIncidence(
        Matrix<bool> incidenceMatrix,
        int uniformityDegree);

    public Signature ComputeSignatureFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector);

    public Matrix<int> ComputeVertexDegreeVectorFromSignature(
        Signature signature);

    public Matrix<int> ComputeVertexDegreeVectorFromIncidence(
        Matrix<bool> incidenceMatrix,
        int uniformityDegree);

    public Matrix<bool> ComputeIncidenceFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector);
    
    public Matrix<int> ComputeVertexDegreeVectorFromAdjacency(
        Matrix<bool> adjacencyMatrix);

    public Matrix<bool> ComputeAdjacencyFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector);

    #endregion
}