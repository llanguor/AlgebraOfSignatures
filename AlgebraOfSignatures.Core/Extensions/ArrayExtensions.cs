namespace AlgebraOfSignatures.Core.Extensions;

public static class ArrayExtensions
{
    #region Nested
    
    public sealed class TraverseState
    {
        public required int[] SignatureIndices;
        public required int[] AdjacencyIndices;
    }
    
    #endregion
    
    #region Static Methods
    
    public static Array CreateRankedArray<T>(
        int size,
        int rank)
    {
        if (size < 1)
            throw new ArgumentException($"{nameof(size)} must be greater than 0");
        
        if (rank < 1)
            throw new ArgumentException($"{nameof(rank)} must be greater than 0");
        
        var shape = Enumerable
            .Repeat(size, rank)
            .ToArray();
        
        return Array.CreateInstance(
            typeof(T),
            shape);
    }
    
    #endregion
    

    #region Extensions

    public static void TraverseSignature(
        this Array array,
        int vertexCount,
        int uniformityDegree,
        Action<TraverseState> delegateAction)
    {
        var signatureLength = vertexCount - uniformityDegree + 1;
        var state = new TraverseState
        {
            SignatureIndices = new int[uniformityDegree == 2 ? 1 : uniformityDegree-2],
            AdjacencyIndices = new int[uniformityDegree]
        };

        for (var i = 1; i < state.AdjacencyIndices.Length - 2; ++i)
        {
            state.AdjacencyIndices[i] = i;
        }
        
        while (state.SignatureIndices[0] != 
               signatureLength)
        {
            //if we have reached the edge of dimension 'i+1', move to the next index in dimension 'i'
            for (var i = uniformityDegree - 3;
                 i > 0;
                 --i)
            {
                //неверно что то. Были индексы 3, 3 при размере 3 в целом
                if (state.SignatureIndices[i] != signatureLength) 
                    break;
                
                ++state.SignatureIndices[i-1];
                state.SignatureIndices[i] = state.SignatureIndices[i-1];
                if (i+1 != state.SignatureIndices.Length)
                    state.SignatureIndices[i + 1] = state.SignatureIndices[i];
                
                ++state.AdjacencyIndices[i-1];
                state.AdjacencyIndices[i] = state.AdjacencyIndices[i-1] + 1;
                state.AdjacencyIndices[i + 1] = state.AdjacencyIndices[i] + 1;
                
                if (state.SignatureIndices[0] == signatureLength) 
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
    
    public static Type GetFinalElementType(this Array array)
    {
        var type = array.GetType();
        while (type.IsArray)
            type = type.GetElementType()!;
        return type;
    }
    
    public static void ForEachPermutation(
        this int[] array,
        Action<int[]> handler)
    {
        var len = array.Length;
        Span<int> depth = stackalloc int[array.Length]; 

        handler(array);

        var i = 0;
        while (i < len)
        {
            if (depth[i] < i)
            {
                if ((i & 1) == 0)
                    (array[0], array[i]) = (array[i], array[0]);
                else
                    (array[depth[i]], array[i]) = (array[i], array[depth[i]]);
                
                handler(array);

                ++depth[i];
                i = 0;
            }
            else
            {
                depth[i] = 0;
                ++i;
            }
        }
    }
    
    #endregion

}