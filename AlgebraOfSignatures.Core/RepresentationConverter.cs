using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

internal sealed class RepresentationConverter :
    RepresentationConverterBase
{
    public override Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix,
        bool throwIfIncorrectAdjacencyMatrix = false)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        long currentSignatureValue = 0;
        
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        var signatureArray = 
            uniformityDegree == 2 ? 
            ArrayExtensions.CreateRankedArray<long>(1,1) : 
            ArrayExtensions.CreateRankedArray<long>(vertexCount-2, uniformityDegree-2);
        
        signatureArray.TraverseSignature(vertexCount, uniformityDegree, state =>
        {
            currentSignatureValue = Convert.ToInt64(
                signatureArray.GetValue(state.TwoDimensionalIndices[^1]));
            
            state.BitsCount = 
                vertexCount - 2 - state.TwoDimensionalIndices[^1];
            
            for (state.BitNumber = state.BitsCount - 1;
                 state.BitNumber >= 0;
                 --state.BitNumber)
            {
                state.TwoDimensionalIndices.CopyTo(state.FullIndices, 0);
                state.FullIndices[^2] = state.RowIndex;
                state.FullIndices[^1] = state.ColumnIndex;

                var value = Convert.ToBoolean(
                    adjacencyMatrix.GetValue(state.FullIndices));

                if (!value)
                {
                    --state.ColumnIndex;
                }
                else
                {
                    currentSignatureValue |= 1L << state.BitNumber;
                    ++state.RowIndex;

                    if (state.BitNumber == 0)
                        signatureArray.SetValue(
                            currentSignatureValue,
                            state.TwoDimensionalIndices);

                    if (throwIfIncorrectAdjacencyMatrix)
                        ThrowIfIllegalAdjacencyValues(
                            vertexCount,
                            state.RowIndex,
                            state.ColumnIndex,
                            state.FullIndices,
                            adjacencyMatrix,
                            value);
                }
            }
        });

        return new Signature(
            signatureArray,
            vertexCount, 
            uniformityDegree);
    }
    
    public override Array ComputeAdjacencyFromSignature(
        Signature signature, 
        int vertexCount,
        int uniformityDegree)
    {
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalSignature(signature, vertexCount);
  
        long currentSignatureValue = 0;
        var adjacencyMatrix =  
            ArrayExtensions.CreateRankedArray<bool>(
                vertexCount,
                uniformityDegree);
        
        Action<int[]> setValueAction = 
            array => adjacencyMatrix.SetValue(true, array);
        
        adjacencyMatrix.TraverseSignature(vertexCount, uniformityDegree, state =>
        {
            currentSignatureValue = Convert.ToInt64(
                signature.GetValue(state.TwoDimensionalIndices[^1]));
            
            state.BitsCount = vertexCount - 2 - state.TwoDimensionalIndices[^1];
            
            for (state.BitNumber = state.BitsCount - 1;
                 state.BitNumber >= 0;
                 --state.BitNumber)
            {
                var currentBit = (currentSignatureValue >> state.BitNumber) & 1;
                if (currentBit == 0)
                {
                    --state.ColumnIndex;
                }
                else
                {
                    //filling the left side of the matrix relative to the domain separation boundary
                    for (var currentRowColumnIndex = state.RowIndex + 1;
                         currentRowColumnIndex <= state.ColumnIndex;
                         ++currentRowColumnIndex)
                    {
                        state.TwoDimensionalIndices.CopyTo(state.FullIndices, 0);
                        state.FullIndices[^2] = state.RowIndex;
                        state.FullIndices[^1] = currentRowColumnIndex;
                        
                        state.FullIndices.ForEachPermutation(
                            setValueAction);
                    }

                    ++state.RowIndex;
                }
            }
        });

        return adjacencyMatrix;
    }
    
    public override Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        throw new NotImplementedException();
    }
}