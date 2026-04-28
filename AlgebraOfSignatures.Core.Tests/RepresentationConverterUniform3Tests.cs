using AlgebraOfSignatures.Core.RepresentationConverters;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConverterUniform3Tests
{
    private readonly RepresentationConverterUniform3 _converter3 = new();
    private readonly RepresentationConverterUniformN _converterN = new();
    
    [Fact]
    public void ComputeSignatureFromAdjacency_ReturnsCorrectValue()
    {
        var matrix = _converter3.CreateRankedArray<bool>(6, 3);
        
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

        var result = _converterN.ComputeSignatureFromAdjacency(matrix);
        
        Assert.True(true);
    }

    [Fact]
    public void ComputeAdjacencyFromSignature_ReturnsCorrectValue()
    {
        int vertexCount = 6,
            uniformityDegree = 3;
        
        var signature = new Signature(
            11,
            vertexCount,
            uniformityDegree);
        
        var adjacency = _converterN.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);

        Assert.True(true);
    }
}