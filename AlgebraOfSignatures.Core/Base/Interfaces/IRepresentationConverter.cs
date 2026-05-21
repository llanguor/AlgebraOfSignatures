namespace AlgebraOfSignatures.Core.Base.Interfaces;

public interface IRepresentationConverter
{
    #region Methods

    Signature ComputeSignatureFromAdjacency(
        Matrix<bool> adjacencyMatrix,
        bool isThrowIfIncorrectAdjacencyMatrix = false);
    
    Matrix<bool> ComputeAdjacencyFromSignature(
        Signature signature);
    
    public Signature ComputeSignatureFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector);

    public Matrix<int> ComputeVertexDegreeVectorFromSignature(
        Signature signature);
    
    public Matrix<int> ComputeVertexDegreeVectorFromAdjacency(
        Matrix<bool> adjacencyMatrix);

    public Matrix<bool> ComputeAdjacencyFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector);

    #endregion
}