namespace AlgebraOfSignatures.Core.Extensions;

public static class ArrayExtensions
{
    #region Nested
    
    public sealed class TraverseState
    {
        public int RowIndex;
        public int ColumnIndex;
        public int BitNumber;
        public int BitsCount;
        public required int[] TwoDimensionalIndices;
        public required int[] FullIndices;
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
        var state = new TraverseState
        {
            TwoDimensionalIndices = new int[uniformityDegree == 2 ? 1 : uniformityDegree-2],
            FullIndices = new int[uniformityDegree]
        };

        for (var i = 1; i < state.TwoDimensionalIndices.Length; ++i)
        {
            state.TwoDimensionalIndices[i] = i;
        }
        
        while (state.TwoDimensionalIndices[0] != 
               vertexCount - 2)
        {
            //if we have reached the edge of dimension 'i+1', move to the next index in dimension 'i'
            for (var i = uniformityDegree - 3;
                 i > 0;
                 --i)
            {
                if (state.TwoDimensionalIndices[i] != vertexCount - 2) 
                    break;
                
                ++state.TwoDimensionalIndices[i-1];
                state.TwoDimensionalIndices[i] = state.TwoDimensionalIndices[i-1] + 1;
                if (i+1 != state.TwoDimensionalIndices.Length)
                    state.TwoDimensionalIndices[i + 1] = state.TwoDimensionalIndices[i] + 1;
            }
            
            state.RowIndex = uniformityDegree == 2 ? 0 : state.TwoDimensionalIndices[^1] + 1;
            state.ColumnIndex = vertexCount - 1;
            
            delegateAction(state);
            
            if (uniformityDegree == 2)
                break;
            
            ++state.TwoDimensionalIndices[^1];
        }
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