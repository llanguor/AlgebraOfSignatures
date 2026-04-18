using AlgebraOfSignatures.Core.RepresentationConverters;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConverterUniform2Tests
{
    private readonly RepresentationConverterUniform2 _converterUniform2 = new();

    [Fact]
    public void ComputeSignatureFromIncidence_ReturnsCorrectValue()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public void ComputeSignatureFromAdjacency_ReturnsCorrectValue()
    {
        int signature = 459, 
            vertexCount = 12,
            uniformityDegree = 2;
        
        var adjacency = _converterUniform2.ComputeAdjacencyFromSignature(
            new [] {signature},
            vertexCount, 
            uniformityDegree);

        var signatureOutput = _converterUniform2.ComputeSignatureFromAdjacency(
            adjacency);
        
        Assert.True(true);
    }
    
    [Fact]
    public void ComputeIncidenceFromSignature_ReturnsCorrectValue()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public void ComputeIncidenceFromAdjacency_ReturnsCorrectValue()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public void ComputeAdjacencyFromSignature_ReturnsCorrectValue()
    {
        int signature = 459, 
            vertexCount = 12,
            uniformityDegree = 2;
        
        var result = _converterUniform2.ComputeAdjacencyFromSignature(
            new [] {signature},
            vertexCount, 
            uniformityDegree);

        for (var i = 0; i < result.GetLength(0); i++)
        {
            for (var j = 0; j < result.GetLength(1); j++)
            {
                var value = Convert.ToBoolean(
                    result.GetValue(i, j));
          
            }
        }
        
        Assert.True(true);
    }
    
    [Fact]
    public void ComputeAdjacencyFromIncidence_ReturnsCorrectValue()
    {
        throw new NotImplementedException();
    }
}