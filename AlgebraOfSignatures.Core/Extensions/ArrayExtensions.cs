namespace AlgebraOfSignatures.Core.Extensions;

public static class ArrayExtensions
{
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