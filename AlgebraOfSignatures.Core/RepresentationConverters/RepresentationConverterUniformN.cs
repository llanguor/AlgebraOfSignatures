using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniformN :
    RepresentationConverterBase
{
    public override Array ComputeSignatureFromAdjacency(Array adjacencyMatrix)
    {
        throw new NotImplementedException();
        
        /*
        return CreateRankedArray<int>(
            uniformityDegree == 2 ? 1 : vertexCount-2, 
            uniformityDegree);
        */
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