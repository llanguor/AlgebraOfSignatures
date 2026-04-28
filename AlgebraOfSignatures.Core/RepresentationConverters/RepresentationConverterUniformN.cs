using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniformN :
    RepresentationConverterBase
{
    public override Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);

        long signature = 0; 
        var indices = new int[uniformityDegree];
        indices[^1] = vertexCount - 1;
        for (var i = 0; i < uniformityDegree - 1; ++i)
        {
            indices[i] = i;
        }

        for (var bitNumber = vertexCount - 3 - (uniformityDegree==2 ? 0 : indices[^3]);
             bitNumber >= 0;
             --bitNumber)
        {
            var value = Convert.ToBoolean(
                adjacencyMatrix.GetValue(indices));

            if (!value)
            {
                --indices[^1];
            }
            else
            {
                ++indices[^2];
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

        var indices= new int[uniformityDegree];
        for (var i = 1; i < uniformityDegree - 2; ++i)
        {
            indices[i] = i;
        }
        
        while (indices[0] != 
               vertexCount - 2)
        {
            for (var i = uniformityDegree - 3;
                 i > 0;
                 --i)
            {
                if (indices[i] != vertexCount - 2) 
                    break;
                
                ++indices[i-1];
                indices[i] = indices[i-1] + 1;
                indices[i + 1] = indices[i] + 1;
            }
            
            indices[^2] = uniformityDegree == 2 ? 0 : indices[^3] + 1;
            indices[^1] = vertexCount - 1;

            var currentSignature = 
                signature.GetValue(uniformityDegree == 2 ? 0 : indices[^3]); 
            
            for (var bitNumber = vertexCount - 3 - (uniformityDegree == 2 ? 0 : indices[^3]);
                 bitNumber >= 0;
                 --bitNumber)
            {
                var currentBit = (currentSignature >> bitNumber) & 1;
                if (currentBit == 0)
                {
                    --indices[^1];
                }
                else
                {
                    for (var columnNumber = indices[^2] + 1;
                         columnNumber <= indices[^1];
                         ++columnNumber)
                    {
                        var toPermute = (int[])indices.Clone();
                        toPermute[^1] = columnNumber;
                        
                        ForEachPermutation(
                            toPermute,
                            array =>
                            {
                                adjacencyMatrix.SetValue(true, array);
                            });
                    }

                    ++indices[^2];
                }
            }

            if (uniformityDegree != 2)
                ++indices[^3];
            else
                break;
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