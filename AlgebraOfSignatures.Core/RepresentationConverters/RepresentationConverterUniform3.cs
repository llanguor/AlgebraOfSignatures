using AlgebraOfSignatures.Core.Base;
namespace AlgebraOfSignatures.Core.RepresentationConverters;

internal sealed class RepresentationConverterUniform3 :
    RepresentationConverterBase
{
    public override Array ComputeSignatureFromIncidence(Array incidenceMatrix, int uniformityDegree)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeSignatureFromAdjacency(Array adjacencyMatrix)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeIncidenceFromSignature(Array signature, int vertexCount, int uniformityDegree)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeIncidenceFromAdjacency(Array adjacencyMatrix)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeAdjacencyFromSignature(Array signature, int vertexCount, int uniformityDegree)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeAdjacencyFromIncidence(Array incidenceMatrix, int uniformityDegree)
    {
        throw new NotImplementedException();
    }
}