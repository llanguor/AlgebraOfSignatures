using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniform2:
    RepresentationConverterBase
{
    public override Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);

        int i = 0,
            j = vertexCount - 1;
        long signature = 0;

        for (var bitNumber = vertexCount - 3;
             bitNumber >= 0; 
             --bitNumber)
        {
            var value = Convert.ToBoolean(
                adjacencyMatrix.GetValue(i, j));

            if (!value)
            {
                --j;
            }
            else
            {
                ++i;
                signature |= 1L << bitNumber;
            }
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
  
        var adjacencyMatrix =  CreateRankedArray<bool>(
            vertexCount,
            uniformityDegree);
        
        var value = 
            signature.GetValue(0);
        var bitsCount = vertexCount - 3;

        int i = 0,
            j = vertexCount - 1;

        for (var bitNumber = bitsCount; 
             bitNumber >= 0; 
             --bitNumber)
        {
            var currentBit = (value >> bitNumber) & 1;
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