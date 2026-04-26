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

        for (var byteNumber = vertexCount - 3;
             byteNumber >= 0; 
             --byteNumber)
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
                signature |= (1 << byteNumber);
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
            j = vertexCount - 1;

        for (var byteNumber = vertexCount - 3; 
             byteNumber >= 0; 
             --byteNumber)
        {
            var currentBit = (value >> byteNumber) & 1;
            if (currentBit == 0)
            {
                --j;
            }
            else
            {
                for (var columnNumber = i + 1;
                     columnNumber <= j;
                     ++columnNumber)
                {
                    var toPermute = new[] { i, columnNumber };
                    ForEachPermutation(
                        (int[]) toPermute.Clone(), 
                        array => adjacencyMatrix.SetValue(true, array));
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