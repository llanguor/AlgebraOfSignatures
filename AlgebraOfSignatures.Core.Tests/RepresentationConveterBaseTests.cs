using AlgebraOfSignatures.Core.RepresentationConverters;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConveterBaseTests
{
    private readonly RepresentationConverterUniform2 _converterUniform2 = new();

    [Fact]
    public void HeapPermutation_ReturnsCorrectValue()
    {
        var input = new[] { 1, 2, 3 };
        _converterUniform2.ForEachPermutation(input, PermuteAction);
        Console.WriteLine("ads");
    }

    void PermuteAction(int[] values)
    {
        foreach (var value in values)
            Console.Write($"{value} ");
        
        Console.WriteLine();
    }

}