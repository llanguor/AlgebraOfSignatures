using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniform2:
    RepresentationConverterBase
{
    public override Array ComputeSignatureFromAdjacency(
        Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        int i = 0,
            j = vertexCount - 1,
            signature = 0;

        for (var k = vertexCount - 3; k >= 0; --k)
        {
            var value = Convert.ToInt32(
                adjacencyMatrix.GetValue(i, j));

            if (value == 0)
            {
                --j;
            }
            else
            {
                ++i;
                signature |= (1 << k);
            }
        }
        
        return new[] { signature };
    }

    public override Array ComputeAdjacencyFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalSignature(signature, vertexCount);
  
        var adjacencyMatrix =  CreateRankedArray<bool>(
            vertexCount,
            uniformityDegree);
        
        var value = Convert.ToInt32(
            signature.GetValue(0));

        int i = 0,
            j = vertexCount;

        for (var k = vertexCount - 3; k >= 0; --k)
        {
            var currentBit = (value >> k) & 1;
            if (currentBit == 0)
            {
                --j;
            }
            else
            {
                for (var q = 0; q < j; ++q)
                {
                    adjacencyMatrix.SetValue(true, i, q);
                    adjacencyMatrix.SetValue(true,q, i);
                }
                ++i;
            }
        }
        
        return adjacencyMatrix;
    }
    
    public override Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix)
    {
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        throw new NotImplementedException();
    }

    public override Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        ThrowIfIllegalUniformityDegree(uniformityDegree);
        ThrowIfIllegalIncidence(incidenceMatrix);
        throw new NotImplementedException();
    }
}