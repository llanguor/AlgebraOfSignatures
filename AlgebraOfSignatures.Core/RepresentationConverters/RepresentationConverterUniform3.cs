using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniform3 :
    RepresentationConverterBase
{
    public override Array ComputeSignatureFromAdjacency(Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);

        var signature = CreateRankedArray<int>(
            vertexCount-2, 
            uniformityDegree-2);
        
        //todo: k-2 в общем случае??? Проверить
        for (var k = 0;
             k < vertexCount - 2;
             ++k)
        {
            var i = k + 1;
            var j = vertexCount - 1;
            
            for (var byteNumber = vertexCount - 3 - k;
                 byteNumber >= 0;
                 --byteNumber)
            {
                var value = Convert.ToInt32(
                    adjacencyMatrix.GetValue(k, i, j));

                if (value == 0)
                {
                    --j;
                }
                else
                {
                    ++i;
                    var toWrite =
                        Convert.ToInt32(
                            signature.GetValue(k)) | (1 << byteNumber);
                    signature.SetValue(toWrite, k);
                }
            }
        }
        
        return signature;
    }

    public override Array ComputeAdjacencyFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalSignature(signature, vertexCount);
  
        var adjacencyMatrix =  
            CreateRankedArray<bool>(
            vertexCount,
            uniformityDegree);
        
        //var value = Convert.ToInt32(
        //    signature.GetValue(0));

        var indices = new int[uniformityDegree];
        
        for (indices[0] = 0;
             indices[0] < vertexCount - 2;
             ++indices[0])
        {
            indices[1] = indices[0] + 1;
            indices[2] = vertexCount - 1;
            var currentSignature = Convert.ToInt32(
                signature.GetValue(indices[0]));
            
            //не vertexCount-3 в данном случае
            for (var byteNumber = vertexCount - 3 - indices[0];
                 byteNumber >= 0;
                 --byteNumber)
            {
                var currentBit = (currentSignature >> byteNumber) & 1;
                if (currentBit == 0)
                {
                    --indices[2];
                }
                else
                {
                    for (var columnNumber = indices[1] + 1;
                         columnNumber <= indices[2];
                         ++columnNumber)
                    {
                        var toPermute = (int[])indices.Clone();
                        
                        toPermute[2] = columnNumber;
                        
                        ForEachPermutation(
                            toPermute,
                            array =>
                            {
                                adjacencyMatrix.SetValue(true, array);
                            });
                    }

                    ++indices[1];
                }
            }
        }

        return adjacencyMatrix;
    }
    
    public override Array ComputeIncidenceFromAdjacency(Array adjacencyMatrix)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeAdjacencyFromIncidence(Array incidenceMatrix, int uniformityDegree)
    {
        throw new NotImplementedException();
    }
}