using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class HyperGraphRepresentationConverter:
    HyperGraphRepresentationConverterBase
{
    //передавать параметры или считать на ходу?
    public override Array ComputeSignatureFromIncidenceMatrix(
        Array incidenceMatrix)
    {
        //var vertexCount = incidenceMatrix.GetLength(0);
        //var uniformityDegree = incidenceMatrix.Rank;
        
        throw new NotImplementedException();
        /*
        return CreateRankedArray<int>(
            uniformityDegree == 2 ? 1 : vertexCount, 
            uniformityDegree);
            */
    }
    
    public override Array ComputeSignatureFromAdjacencyMatrix(
        Array adjacencyMatrix)
    {
        throw new NotImplementedException();
        /*
        return CreateRankedArray<int>(
            uniformityDegree == 2 ? 1 : vertexCount, 
            uniformityDegree);
            */
    }
    
    public override Array ComputeIncidenceMatrixFromSignature(
        Array signature)
    {
        throw new NotImplementedException();
        /*
        return CreateRankedArray<bool>(
            vertexCount, 
            uniformityDegree);
            */
    }

    public override Array ComputeIncidenceMatrixFromAdjacencyMatrix(
        Array adjacencyMatrix)
    {
        throw new NotImplementedException();
    }

    public override Array ComputeAdjacencyMatrixFromSignature(
        Array signature)
    {
        throw new NotImplementedException();
        /*
        return CreateRankedArray<bool>(
            vertexCount, 
            uniformityDegree);
            */
    }

    public override Array ComputeAdjacencyMatrixFromIncidenceMatrix(
        Array incidenceMatrix)
    {
        throw new NotImplementedException();
    }
}