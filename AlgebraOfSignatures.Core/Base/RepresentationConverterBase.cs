using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core.Base;

public abstract class RepresentationConverterBase :
    IRepresentationConverter
{
    //todo: The vertex degree vector is calculated from the adjacency matrix due to the lack of time to implement it from the signature. All calculations must be based on the signature.
    
    #region Methods
    
    public Signature ComputeSignatureFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector)
    {
        return ComputeSignatureFromAdjacency(
            ComputeAdjacencyFromVertexDegreeVector(vertexDegreeVector));
    }

    public Matrix<int> ComputeVertexDegreeVectorFromSignature(
        Signature signature)
    {
        return ComputeVertexDegreeVectorFromAdjacency(
            ComputeAdjacencyFromSignature(signature));
    }
    
    #endregion
    
    
    #region ThrowIf Methods
    
    protected void ThrowIfIllegalAdjacencyValues( 
        int vertexCount,
        int rowIndex,
        int columnIndex,
        int [] adjacencyIndices,
        Matrix<bool> adjacencyMatrix,
        bool requiredValue)
    {
        for (var currentRowColumnIndex = rowIndex;
             currentRowColumnIndex <= vertexCount;
             ++currentRowColumnIndex)
        {
            adjacencyIndices[^1] = currentRowColumnIndex;
            var cellValue = Convert.ToBoolean(
                adjacencyMatrix.GetValue(adjacencyIndices));

            if (currentRowColumnIndex > columnIndex &&
                cellValue == true ||
                currentRowColumnIndex <= columnIndex &&
                cellValue == false)
            {
                throw new ArgumentException(
                    "The values in the cells to the left and right of the domain separator must be equal to 1 and 0, respectively.",
                    nameof(adjacencyMatrix));
            }
                        
            if (cellValue == false)
                continue;

            adjacencyIndices.ForEachPermutation(
                _ =>
                {
                    var permutationValue = Convert.ToBoolean(
                        adjacencyMatrix.GetValue(adjacencyIndices));
     
                    if (requiredValue != permutationValue)
                    {
                        throw new ArgumentException(
                            $"The values in the matrix cell with indices [{string.Join(", ", adjacencyIndices)}] do not match the value specified in the significant cells of the signature",
                            nameof(adjacencyMatrix));
                    };
                });
        }
    }

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

    protected void ThrowIfIllegalIncidence(Matrix<bool> incidenceMatrix)
    {
        throw new NotImplementedException();
    }
    
    protected void ThrowIfIllegalAdjacency(Matrix<bool> incidenceMatrix)
    {
        //todo: validate 
        
        if (incidenceMatrix.ElementType != typeof(bool))
            throw new ArgumentException($"{nameof(incidenceMatrix)} elements must be of type bool");

    }
    
    #endregion
    
    
    #region Abstract Methods

    public abstract Signature ComputeSignatureFromAdjacency(
        Matrix<bool> adjacencyMatrix,
        bool isThrowIfIncorrectAdjacencyMatrix = false);
    
    public abstract Matrix<bool> ComputeAdjacencyFromSignature(
        Signature signature);

    public abstract Matrix<int> ComputeVertexDegreeVectorFromAdjacency(
        Matrix<bool> adjacencyMatrix);

    public abstract Matrix<bool> ComputeAdjacencyFromVertexDegreeVector(
        Matrix<int> vertexDegreeVector);

    #endregion

}