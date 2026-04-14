using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class HyperGraph(
    IHyperGraphRepresentationConverter converter,
    Array signature,
    int vertexCount, 
    int uniformityDegree) : 
    HyperGraphBase(
        converter,
        signature,
        vertexCount,
        uniformityDegree)
{
    #region Fabric Methods
    
    public static HyperGraph FromIncidenceMatrix(
        IHyperGraphRepresentationConverter converter,
        Array incidenceMatrix)
    {
        var vertexCount = incidenceMatrix.GetLength(0);
        var uniformityDegree = incidenceMatrix.Rank;
        
        return new HyperGraph(
            converter,
            converter.ComputeSignatureFromIncidenceMatrix(incidenceMatrix),
            vertexCount,
            uniformityDegree);
    }
    
    public static HyperGraph FromAdjacencyMatrix(
        IHyperGraphRepresentationConverter converter,
        Array adjacencyMatrix)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        
        return new HyperGraph(
            converter,
            converter.ComputeSignatureFromAdjacencyMatrix(adjacencyMatrix),
            vertexCount,
            uniformityDegree);
    }
    
    public static HyperGraph FromSignature(
        IHyperGraphRepresentationConverter converter,
        Array signature)
    {
        var signatureLength = signature.GetLength(0);
        var signatureRank = signature.Rank;
        var uniformityDegree = signatureRank + 2;
        var vertexCount = signatureRank;
        
        if (signatureRank == 1 &&
            signatureLength == 1)
        {
            //todo: count vertex from signature size
            vertexCount = 1;
            uniformityDegree = 2;
        }
        
        return new HyperGraph(
            converter,
            signature, 
            vertexCount, 
            uniformityDegree);   
    }
    
    #endregion
}