using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniform3 :
    RepresentationConverterBase
{
    public override Signature ComputeSignatureFromAdjacency(Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);

        long signature = 0; 
        var indices = new int[uniformityDegree];
        indices[0] = 0;
        indices[1] = indices[0] + 1;
        indices[2] = vertexCount - 1;
        
        for (var bitNumber = vertexCount - 3 - indices[0];
             bitNumber >= 0;
             --bitNumber)
        {
            var value = Convert.ToBoolean(
                adjacencyMatrix.GetValue(indices));

            if (!value)
            {
                --indices[2];
            }
            else
            {
                ++indices[1];
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
  
        var adjacencyMatrix =  
            CreateRankedArray<bool>(
            vertexCount,
            uniformityDegree);

        var indices = new int[uniformityDegree];
        
        for (indices[0] = 0;
             indices[0] < vertexCount - 2;
             ++indices[0])
        {
            indices[1] = indices[0] + 1;
            indices[2] = vertexCount - 1;
            var currentSignature = 
                signature.GetValue(indices[0]);
            
            for (var bitNumber = vertexCount - 3 - (uniformityDegree == 2 ? 0 : indices[0]);
                 bitNumber >= 0;
                 --bitNumber)
            {
                var currentBit = (currentSignature >> bitNumber) & 1;
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