using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core.Extensions;

public static class MultiDimensionalArrayExtensions
{
    #region Nested
    
    public sealed class TraverseState
    {
        public required int[] SignatureIndices;
        public required int[] AdjacencyIndices;
        public required int SignatureArraySize;
    }
    
    #endregion
    
    
    #region Extensions

    public static void TraverseSignature(
        this IMatrix array,
        int vertexCount,
        int uniformityDegree,
        Action<TraverseState> delegateAction)
    {
        var state = new TraverseState
        {
            SignatureIndices = new int[uniformityDegree == 2 ? 1 : uniformityDegree-2],
            AdjacencyIndices = new int[uniformityDegree],
            SignatureArraySize = uniformityDegree == 2 ? 1 : vertexCount - uniformityDegree + 1,
        };

        for (var i = 1; i < state.AdjacencyIndices.Length - 2; ++i)
        {
            state.AdjacencyIndices[i] = i;
        }
        
        while (state.SignatureIndices[0] != 
               state.SignatureArraySize)
        {
            //if we have reached the edge of dimension 'i+1', move to the next index in dimension 'i'
            for (var i = uniformityDegree - 3;
                 i > 0;
                 --i)
            {
                if (state.SignatureIndices[i] != state.SignatureArraySize) 
                    break;
                
                ++state.SignatureIndices[i-1];
                state.SignatureIndices[i] =
                    state.SignatureIndices[i - 1] == state.SignatureArraySize
                        ? state.SignatureIndices[i - 1] - 1
                        : state.SignatureIndices[i - 1];
                
                ++state.AdjacencyIndices[i-1];
                state.AdjacencyIndices[i] = state.AdjacencyIndices[i-1] + 1;
                state.AdjacencyIndices[i + 1] = state.AdjacencyIndices[i] + 1;
                
                if (state.SignatureIndices[0] == state.SignatureArraySize) 
                    return;
            }
            
            state.AdjacencyIndices[^2] = uniformityDegree == 2 ? 0 : state.AdjacencyIndices[^3] + 1;
            state.AdjacencyIndices[^1] = vertexCount - 1;
            
            delegateAction(state);
            
            if (uniformityDegree == 2)
                break;
            
            ++state.SignatureIndices[^1];
            ++state.AdjacencyIndices[^3];
        }
    }
    
    #endregion
}