using AlgebraOfSignatures.Core.Base;

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
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        var signature = 
            uniformityDegree == 2 ? 
            CreateRankedArray<long>(1,1) : 
            CreateRankedArray<long>(vertexCount-2, uniformityDegree-2);
        
        var indices = new int[uniformityDegree == 2 ? 1 : uniformityDegree-2];
        var adjacencyIndices = new int[uniformityDegree];
        for (var i = 1; i < indices.Length; ++i)
        {
            indices[i] = i;
        }

        while (indices[0] != vertexCount - 2)
        {
            //if we have reached the edge of dimension 'i+1', move to the next index in dimension 'i'
            for (var i = uniformityDegree - 3;
                 i > 0;
                 --i)
            {
                if (indices[i] != vertexCount - 2) 
                    break;
                
                ++indices[i-1];
                indices[i] = indices[i-1] + 1;
                if (i+1 != indices.Length)
                    indices[i + 1] = indices[i] + 1;
            }
            
            var rowIndex = uniformityDegree == 2 ? 0 : indices[^1] + 1;
            var columnIndex = vertexCount - 1;

            //start of traversal current row of adjacency matrix
            long currentSignatureValue = 0;
            var bitsCount = vertexCount - 2 - indices[^1];
            for (var bitNumber = bitsCount-1;
                 bitNumber >= 0;
                 --bitNumber)
            {
                indices.CopyTo(adjacencyIndices, 0);
                adjacencyIndices[^2] = rowIndex;
                adjacencyIndices[^1] = columnIndex;
                var value = Convert.ToBoolean(
                    adjacencyMatrix.GetValue(adjacencyIndices));
                
                if (!value)
                {
                    //There is a 0 in the cell.
                    //This means we are to the right of the domain separator.
                    
                    --columnIndex;
                }
                else
                {
                    //The cell contains a 1, which means it's on the domain boundary.
                    //Let's add the number to the signature and move on to the next line.
                    
                    currentSignatureValue |= 1L << bitNumber;
                    ++rowIndex;
                    
                    if (!throwIfIncorrectAdjacencyMatrix)
                        continue;

                    //validate other cells of adjacency matrix 
                    
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
                            
                        ForEachPermutation(
                            adjacencyIndices,
                            array =>
                            {
                                var permutationValue = Convert.ToBoolean(
                                    adjacencyMatrix.GetValue(adjacencyIndices));
         
                                if (value != permutationValue)
                                {
                                    throw new ArgumentException(
                                        $"The values in the matrix cell with indices [{string.Join(", ", adjacencyIndices)}] do not match the value specified in the significant cells of the signature",
                                        nameof(adjacencyMatrix));
                                };
                            });
                    }
                }
            }
            
            signature.SetValue(
                currentSignatureValue,
                indices);
            
            if (uniformityDegree == 2)
                break;
            
            ++indices[^1];
        }

        return new Signature(
            signature,
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
  
        var adjacencyMatrix =  
            CreateRankedArray<bool>(
                vertexCount,
                uniformityDegree);

        var indices = new int[uniformityDegree == 2 ? 1 : uniformityDegree-2];
        var adjacencyIndices = new int[uniformityDegree];
        for (var i = 1; i < indices.Length; ++i)
        {
            indices[i] = i;
        }
        
        while (indices[0] != vertexCount - 2)
        {
            //if we have reached the edge of dimension 'i+1', move to the next index in dimension 'i'
            for (var i = uniformityDegree - 3;
                 i > 0;
                 --i)
            {
                if (indices[i] != vertexCount - 2) 
                    break;
                
                ++indices[i-1];
                indices[i] = indices[i-1] + 1;
                if (i+1 != indices.Length)
                    indices[i + 1] = indices[i] + 1;
            }
            
            var rowIndex = uniformityDegree == 2 ? 0 : indices[^1] + 1;
            var columnIndex = vertexCount - 1;

            var currentSignatureValue = 
                signature.GetValue(indices[^1]); 
            
            //start of traversal current row of adjacency matrix
            var bitsCount = vertexCount - 2 - indices[^1];
            for (var bitNumber = bitsCount-1;
                 bitNumber >= 0;
                 --bitNumber)
            {
                var currentBit = (currentSignatureValue >> bitNumber) & 1;
                if (currentBit == 0)
                {
                    --columnIndex;
                }
                else
                {
                    //filling the left side of the matrix relative to the domain separation boundary
                    for (var currentRowColumnIndex = rowIndex + 1;
                         currentRowColumnIndex <= columnIndex;
                         ++currentRowColumnIndex)
                    {
                        indices.CopyTo(adjacencyIndices, 0);
                        adjacencyIndices[^2] = rowIndex;
                        adjacencyIndices[^1] = currentRowColumnIndex;
                        
                        ForEachPermutation(
                            adjacencyIndices,
                            array =>
                            {
                                adjacencyMatrix.SetValue(true, array);
                            });
                    }

                    ++rowIndex;
                }
            }

            if (uniformityDegree == 2)
                break;
            
            ++indices[^1];
        }

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