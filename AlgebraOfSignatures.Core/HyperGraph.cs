using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
        if (incidenceMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array", 
                nameof(incidenceMatrix));
        
        var vertexCount = incidenceMatrix.GetLength(0);
        var uniformityDegree = incidenceMatrix.Rank;
        
        return new HyperGraph(
            converter,
            converter.ComputeSignatureFromIncidence(incidenceMatrix, uniformityDegree),
            vertexCount,
            uniformityDegree);
    }
    
    public static HyperGraph FromAdjacencyMatrix(
        IHyperGraphRepresentationConverter converter,
        Array adjacencyMatrix)
    {
        if (adjacencyMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array",
                nameof(adjacencyMatrix));
        
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        
        return new HyperGraph(
            converter,
            converter.ComputeSignatureFromAdjacency(adjacencyMatrix),
            vertexCount,
            uniformityDegree);
    }
    
    public static HyperGraph FromSignature(
        IHyperGraphRepresentationConverter converter,
        Array signature)
    {
        if (signature.GetType().GetElementType() != typeof(int))
            throw new ArgumentException(
                $"Expected {typeof(int)} array", 
                nameof(signature));
        
        var signatureLength = signature.GetLength(0);
        var signatureRank = signature.Rank;
        var uniformityDegree = signatureRank + 2;
        var vertexCount = signatureRank;
        
        if (signatureRank == 1 &&
            signatureLength == 1)
        {
            //todo: refact
            return FromSignature(
                converter,
                (int)signature.GetValue(0, 0)!);
        }
        
        return new HyperGraph(
            converter,
            signature, 
            vertexCount, 
            uniformityDegree);   
    }
    
    public static HyperGraph FromSignature(
        IHyperGraphRepresentationConverter converter,
        int signature)
    {
        //todo: count vertex from signature size
        var uniformityDegree = 2;
        var vertexCount = 5;

        return new HyperGraph(
            converter,
            new[] { signature }, 
            vertexCount, 
            uniformityDegree);   
    }
    
    #endregion
}