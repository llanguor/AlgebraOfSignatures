using System.Runtime.InteropServices;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

internal sealed class RepresentationConverter :
    RepresentationConverterBase
{
    public void Convert(
        UniformHyperGraph.RepresentationTypes from,
        UniformHyperGraph.RepresentationTypes to,
        object input, 
        out object? output,
        int? uniformityDegree,
        int? vertexCount)
    {
        switch (from)
        {
            case UniformHyperGraph.RepresentationTypes.Signature:
            {
                ArgumentNullException.ThrowIfNull(uniformityDegree);
                ArgumentNullException.ThrowIfNull(vertexCount);
                
                if (input is not Signature signature)
                    throw new ArgumentException("Expected Signature", nameof(input));
                
                output = to switch
                {
                    UniformHyperGraph.RepresentationTypes.AdjacencyMatrix
                        => ComputeAdjacencyFromSignature(signature),
                
                    UniformHyperGraph.RepresentationTypes.Signature
                        => input,
                    
                    UniformHyperGraph.RepresentationTypes.VertexDegreeVector
                        => ComputeVertexDegreeVectorFromSignature(signature),
                
                    _ => throw new NotSupportedException($"Unsupported type for convert: {to}")
                };
                break;
            }
            case UniformHyperGraph.RepresentationTypes.AdjacencyMatrix:
            {
                if (input is not Matrix<bool> adjacency)
                    throw new ArgumentException("Expected Array", nameof(input));
            
                output = to switch
                {
                    UniformHyperGraph.RepresentationTypes.AdjacencyMatrix
                        => input,
                
                    UniformHyperGraph.RepresentationTypes.Signature
                        => ComputeSignatureFromAdjacency(adjacency),
                    
                    UniformHyperGraph.RepresentationTypes.VertexDegreeVector
                        => ComputeVertexDegreeVectorFromAdjacency(adjacency),
                
                    _ => throw new NotSupportedException($"Unsupported type for convert: {to}")
                };
                break;
            }
            case UniformHyperGraph.RepresentationTypes.VertexDegreeVector:
            {
                if (input is not Matrix<int> vector)
                    throw new ArgumentException("Expected Array", nameof(input));
            
                output = to switch
                {
                    UniformHyperGraph.RepresentationTypes.AdjacencyMatrix
                        => ComputeAdjacencyFromVertexDegreeVector(vector),
                
                    UniformHyperGraph.RepresentationTypes.Signature
                        => ComputeSignatureFromVertexDegreeVector(vector),
                    
                    UniformHyperGraph.RepresentationTypes.VertexDegreeVector
                        => input,
                
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
        Matrix<bool> adjacencyMatrix,
        bool isThrowIfIncorrectAdjacencyMatrix = false)
    {
        var vertexCount = adjacencyMatrix.Size;
        var uniformityDegree = adjacencyMatrix.Rank;
        var signatureLength = vertexCount - uniformityDegree + 1;
        long currentSignatureValue = 0;
        
        ThrowIfIllegalGraphParameters(vertexCount, uniformityDegree);
        ThrowIfIllegalAdjacency(adjacencyMatrix);
        
        var signatureArray = 
            uniformityDegree == 2 ? 
            new Matrix<long>(1,1) : 
            new Matrix<long>(signatureLength, uniformityDegree-2);
        
        signatureArray.Traverse(vertexCount, uniformityDegree, state =>
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
    
    public override Matrix<bool> ComputeAdjacencyFromSignature(
        Signature signature)
    {
        var signatureLength = signature.VertexCount -  signature.UniformityDegree + 1;
        long currentSignatureValue = 0;
        var adjacencyMatrix =  new Matrix<bool>(
            signature.VertexCount,
            signature.UniformityDegree);
        
        Action<int[]> setValueAction = 
            array => adjacencyMatrix.SetValue(true, array);
        
        adjacencyMatrix.Traverse(signature.VertexCount,  signature.UniformityDegree, state =>
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

    public override Matrix<int> ComputeVertexDegreeVectorFromAdjacency(Matrix<bool> adjacencyMatrix)
    {
        var vertexDegreeVector = new Matrix<int>(
            adjacencyMatrix.Size,
            adjacencyMatrix.Rank - 1);
        var vertexDegreeVectorIndices = 
            new int[vertexDegreeVector.Rank];


        var adjacencyIndices = new int[adjacencyMatrix.Rank];

        while (adjacencyIndices[0] !=
               adjacencyMatrix.Size)
        {
            for (var i = adjacencyMatrix.Rank - 1; i > 0; --i)
            {
                if (adjacencyIndices[i] != adjacencyMatrix.Size)
                    break;

                ++adjacencyIndices[i-1];
                adjacencyIndices[i] = 0;
                
                if (adjacencyIndices[0] == adjacencyMatrix.Size)
                    return vertexDegreeVector;
            }
            
            Array.Copy(adjacencyIndices, 
                vertexDegreeVectorIndices, 
                adjacencyIndices.Length - 1);

            if (adjacencyMatrix.GetValue(adjacencyIndices))
                vertexDegreeVector.SetValue(
                    vertexDegreeVector.GetValue(vertexDegreeVectorIndices) + 1,
                    vertexDegreeVectorIndices);
            
            ++adjacencyIndices[^1];
        }
        
        return vertexDegreeVector;
    }

    public override Matrix<bool> ComputeAdjacencyFromVertexDegreeVector(Matrix<int> vertexDegreeVector)
    {
        throw new NotImplementedException();
    }
}