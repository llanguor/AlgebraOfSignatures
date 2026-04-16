using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class HyperGraphRepresentationConverter:
    HyperGraphRepresentationConverterBase
{
    #region ThrowIf Methods
    
    private void ThrowIfIllegalGraphParameters( 
        int vertexCount,
        int uniformityDegree)
    {
        ThrowIfIllegalVertexCount(vertexCount);
        ThrowIfIllegalUniformityDegree(uniformityDegree);
    }
    
    private void ThrowIfIllegalVertexCount( 
        int vertexCount)
    {
        if (vertexCount < 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(vertexCount)} must be greater than or equal to 1.");
    }
    
    private void ThrowIfIllegalUniformityDegree( 
        int uniformityDegree)
    {
        if (uniformityDegree < 2)
            throw new ArgumentOutOfRangeException(
                $"{nameof(uniformityDegree)} must be greater than or equal to 2.");
    }

    private void ThrowIfIllegalSignature(Array signature, int vertexCount)
    {
        //todo: throw if illegal signature 
        //todo: throw if x > 2^(v-1)  // (signature for 2-ranked) >= 2^(vertexCount-1)  // (signature for 2-ranked) > (2<<(n-1))
        //throw new NotImplementedException();
    }

    private void ThrowIfIllegalIncidence(Array incidenceMatrix)
    {
        throw new NotImplementedException();
    }
    
    private void ThrowIfIllegalAdjacency(Array incidenceMatrix)
    {
       // throw new NotImplementedException();
    }
    
    #endregion


    #region Override Methods
    
    public override Array ComputeSignatureFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        return ComputeSignatureFromAdjacency(
            ComputeAdjacencyFromIncidence(
                incidenceMatrix, uniformityDegree));
    }
    
    public override Array ComputeSignatureFromAdjacency(
        Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        if (uniformityDegree == 2)
        {
            int i = 0,
                j = vertexCount - 1,
                k = vertexCount - 3,
                signature = 0;

            while (k >= 0)
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

                --k;
            }
            
            return new[] { signature };
        }
        
        return CreateRankedArray<int>(
            uniformityDegree == 2 ? 1 : vertexCount-2, 
            uniformityDegree);
    }
    
    public override Array ComputeIncidenceFromSignature(
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        return ComputeIncidenceFromAdjacency(
            ComputeAdjacencyFromSignature(
                signature,
                vertexCount,
                uniformityDegree));
    }

    public override Array ComputeIncidenceFromAdjacency(
        Array adjacencyMatrix)
    {
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        throw new NotImplementedException();
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
        
        if (uniformityDegree == 2)
        {
            var value = Convert.ToInt32(
                signature.GetValue(0));
            
            int i = 0,
                j = vertexCount,
                k = vertexCount - 3;

            while (k >= 0)
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
                
                --k;
            }
        }

        return adjacencyMatrix;
    }

    public override Array ComputeAdjacencyFromIncidence(
        Array incidenceMatrix,
        int uniformityDegree)
    {
        ThrowIfIllegalUniformityDegree(uniformityDegree);
        ThrowIfIllegalIncidence(incidenceMatrix);
        throw new NotImplementedException();
    }
    
    #endregion
}