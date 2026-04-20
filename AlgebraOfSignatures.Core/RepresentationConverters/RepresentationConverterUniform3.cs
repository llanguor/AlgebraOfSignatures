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

        int i = 0, j = 0;
        
        var signature = CreateRankedArray<int>(
            vertexCount-2, 
            uniformityDegree-2);
        
        for (var x = 0; x < vertexCount - 2; ++x)
        {
            i = x + 1;
            j = vertexCount - 1;
            
            for (var k = vertexCount - 3 - x; k >= 0; --k)
            {
                var value = Convert.ToInt32(
                    adjacencyMatrix.GetValue(x, i, j));

                if (value == 0)
                {
                    --j;
                }
                else
                {
                    ++i;
                    var toWrite =
                        Convert.ToInt32(
                            signature.GetValue(x)) | (1 << k);
                    signature.SetValue(toWrite, x);
                }
            }
        }
        
        return signature;
    }

    public override Array ComputeAdjacencyFromSignature(Array signature, int vertexCount, int uniformityDegree)
    {
        throw new NotImplementedException();
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