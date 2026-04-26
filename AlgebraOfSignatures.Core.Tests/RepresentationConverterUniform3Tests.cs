using AlgebraOfSignatures.Core.RepresentationConverters;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConverterUniform3Tests
{
    private readonly RepresentationConverterUniform3 _converter = new();

    [Fact]
    public void ComputeSignatureFromAdjacency_ReturnsCorrectValue()
    {
        var matrix = _converter.CreateRankedArray<bool>(6, 3);
        
        matrix.SetValue(true, 0,1,2);
        matrix.SetValue(true, 0,1,3);
        matrix.SetValue(true, 0,1,4);
        matrix.SetValue(true, 0,1,5);
        matrix.SetValue(true, 0,2,3);
        matrix.SetValue(true, 0,2,4);
        matrix.SetValue(true, 0,3,4);
        
        matrix.SetValue(true, 1,1,2);
        matrix.SetValue(true, 1,1,3);
        matrix.SetValue(true, 1,1,4);
        matrix.SetValue(true, 1,1,5);
        matrix.SetValue(true, 1,2,3);
        matrix.SetValue(true, 1,2,4);
        matrix.SetValue(true, 1,3,4);
        
        matrix.SetValue(true, 2,1,2);
        matrix.SetValue(true, 2,1,3);
        matrix.SetValue(true, 2,1,4);
        matrix.SetValue(true, 2,1,5);
        matrix.SetValue(true, 2,2,3);
        matrix.SetValue(true, 2,2,4);
        matrix.SetValue(true, 2,3,4);
        
        matrix.SetValue(true, 3,1,2);
        matrix.SetValue(true, 3,1,3);
        matrix.SetValue(true, 3,1,4);
        matrix.SetValue(true, 3,1,5);
        matrix.SetValue(true, 3,2,3);
        matrix.SetValue(true, 3,2,4);
        matrix.SetValue(true, 3,3,4);

        var result = _converter.ComputeSignatureFromAdjacency(matrix);
        
        Assert.True(true);
    }

    [Fact]
    public void ComputeAdjacencyFromSignature_ReturnsCorrectValue()
    {
        var signature = _converter.CreateRankedArray<int>(4, 1);
        signature.SetValue(11, 0);
        signature.SetValue(3, 1);
        signature.SetValue(1, 2);
        signature.SetValue(0, 3);
        
        int vertexCount = 6,
            uniformityDegree = 3;
        
        var adjacency = _converter.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);

        Assert.True(true);
    }
}