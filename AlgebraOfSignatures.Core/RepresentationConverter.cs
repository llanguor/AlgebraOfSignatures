using AlgebraOfSignatures.Core.Base;

namespace AlgebraOfSignatures.Core;

internal sealed class RepresentationConverter :
    RepresentationConverterBase
{
    public override Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        var signature = 
            uniformityDegree == 2 ? 
            CreateRankedArray<long>(1,1) : 
            CreateRankedArray<long>(vertexCount-2, uniformityDegree-2);
        
        var indices = new int[uniformityDegree == 2 ? 1 :uniformityDegree-2];
        var adjacencyIndices = new int[uniformityDegree];
        for (var i = 0; i < indices.Length; ++i)
        {
            indices[i] = i;
        }

        while (uniformityDegree == 2 ||
               indices[0] != vertexCount - 2)
        {
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

            long currentSignatureValue = 0;
           
            for (var bitNumber = vertexCount - 3 - (uniformityDegree == 2 ? 0 : indices[^1]);
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
                    --columnIndex;
                }
                else
                {
                    ++rowIndex;
                    currentSignatureValue |= 1L << bitNumber;
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

        var indices = new int[uniformityDegree-2];
        var adjacencyIndices = new int[uniformityDegree];
        for (var i = 1; i < indices.Length; ++i)
        {
            indices[i] = i;
        }
        
        while (uniformityDegree == 2 || 
               indices[0] != vertexCount - 2)
        {
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

            var currentSignature = 
                signature.GetValue(uniformityDegree == 2 ? 0 : indices[^1]); 
            
            for (var bitNumber = vertexCount - 3 - (uniformityDegree == 2 ? 0 : indices[^1]);
                 bitNumber >= 0;
                 --bitNumber)
            {
                var currentBit = (currentSignature >> bitNumber) & 1;
                if (currentBit == 0)
                {
                    --columnIndex;
                }
                else
                {
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