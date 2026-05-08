using System.Runtime.InteropServices;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

internal sealed class RepresentationConverter :
    RepresentationConverterBase
{
    public enum RepresentationType
    {
        Signature = 0,
        AdjacencyMatrix = 1,
        IncidenceMatrix = 2,
    }

    public void Convert(
        RepresentationType from,
        RepresentationType to,
        object input, 
        out object? output,
        int? uniformityDegree,
        int? vertexCount)
    {
        switch (from)
        {
            case RepresentationType.Signature:
            {
                ArgumentNullException.ThrowIfNull(uniformityDegree);
                ArgumentNullException.ThrowIfNull(vertexCount);
                
                if (input is not Signature signature)
                    throw new ArgumentException("Expected Signature", nameof(input));
                
                output = to switch
                {
                    RepresentationType.AdjacencyMatrix
                        => ComputeAdjacencyFromSignature(signature, vertexCount!.Value, uniformityDegree!.Value),
              
                    RepresentationType.IncidenceMatrix 
                        => ComputeIncidenceFromSignature(signature, vertexCount!.Value, uniformityDegree!.Value),
                
                    RepresentationType.Signature
                        => input,
                
                    _ => throw new NotSupportedException($"Unsupported type for convert: {to}")
                };
                break;
            }
            case RepresentationType.AdjacencyMatrix:
            {
                if (input is not Array adjacency)
                    throw new ArgumentException("Expected Array", nameof(input));
            
                output = to switch
                {
                    RepresentationType.AdjacencyMatrix
                        => input,
                
                    RepresentationType.IncidenceMatrix 
                        => ComputeIncidenceFromAdjacency(adjacency),
                
                    RepresentationType.Signature
                        => ComputeSignatureFromAdjacency(adjacency),
                
                    _ => throw new NotSupportedException($"Unsupported type for convert: {to}")
                };
                break;
            }
            case RepresentationType.IncidenceMatrix:
            {
                ArgumentNullException.ThrowIfNull(uniformityDegree);
                
                if (input is not Array incidence)
                    throw new ArgumentException("Expected Array", nameof(input));
            
                output = to switch
                {
                    RepresentationType.AdjacencyMatrix
                        => ComputeAdjacencyFromIncidence(incidence, uniformityDegree!.Value),
                
                    RepresentationType.IncidenceMatrix 
                        => input,
                
                    RepresentationType.Signature
                        => ComputeSignatureFromIncidence(incidence, uniformityDegree!.Value),
                
                    _ => throw new NotSupportedException($"Unsupported type for convert: {to}")
                };
                break;
            }
            
            default:
                throw new NotSupportedException(
                    $"Unsupported type for convert: {from}");
        }
    }
    
    
    public override Signature ComputeSignatureFromAdjacency(
        Array adjacencyMatrix,
        bool isThrowIfIncorrectAdjacencyMatrix = false)
    {
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        var signatureLength = vertexCount - uniformityDegree + 1;
        long currentSignatureValue = 0;
        
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        var signatureArray = 
            uniformityDegree == 2 ? 
            ArrayExtensions.CreateRankedArray<long>(1,1) : 
            ArrayExtensions.CreateRankedArray<long>(signatureLength, uniformityDegree-2);
        
        signatureArray.TraverseSignature(vertexCount, uniformityDegree, state =>
        {
            currentSignatureValue = System.Convert.ToInt64(
                signatureArray.GetValue(state.SignatureIndices));
            
            var bitsCount = 
                signatureLength - state.SignatureIndices[^1];
            
            for (var bitNumber = bitsCount - 1;
                 bitNumber >= 0;
                 --bitNumber)
            {
                var value = System.Convert.ToBoolean(
                    adjacencyMatrix.GetValue(state.AdjacencyIndices));

                if (!value)
                {
                    --state.AdjacencyIndices[^1];
                }
                else
                {
                    currentSignatureValue |= 1L << bitNumber;
                    ++state.AdjacencyIndices[^2];
                    
                    if (isThrowIfIncorrectAdjacencyMatrix)
                        ThrowIfIllegalAdjacencyValues(
                            vertexCount,
                            state.AdjacencyIndices[^2],
                            state.AdjacencyIndices[^1],
                            state.AdjacencyIndices,
                            adjacencyMatrix,
                            value);
                }
            }
            
            signatureArray.SetValue(
                currentSignatureValue,
                state.SignatureIndices);
        });

        return new Signature(
            signatureArray,
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
  
        var signatureLength = vertexCount - uniformityDegree + 1;
        long currentSignatureValue = 0;
        var adjacencyMatrix =  
            ArrayExtensions.CreateRankedArray<bool>(
                vertexCount,
                uniformityDegree);
        
        Action<int[]> setValueAction = 
            array => adjacencyMatrix.SetValue(true, array);
        
        adjacencyMatrix.TraverseSignature(vertexCount, uniformityDegree, state =>
        {
            currentSignatureValue = System.Convert.ToInt64(
                signature.GetValue(state.SignatureIndices));
            
            var bitsCount = signatureLength - state.SignatureIndices[^1];
            
            for (var bitNumber = bitsCount - 1;
                 bitNumber >= 0;
                 --bitNumber)
            {
                var currentBit = (currentSignatureValue >> bitNumber) & 1;
                if (currentBit == 0)
                {
                    --state.AdjacencyIndices[^1];
                }
                else
                {
                    //filling the left side of the matrix relative to the domain separation boundary
                    for (var columnIndex = state.AdjacencyIndices[^2] + 1;
                         columnIndex <= state.AdjacencyIndices[^1];
                         ++columnIndex)
                    {
                        var toPermute = (int[])state.AdjacencyIndices.Clone();
                        toPermute[^1] = columnIndex;
                        toPermute.ForEachPermutation(setValueAction);
                    }

                    ++state.AdjacencyIndices[^2];
                }
            }
        });

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