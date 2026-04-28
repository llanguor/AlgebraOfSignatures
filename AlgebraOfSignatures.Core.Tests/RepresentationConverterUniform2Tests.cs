using AlgebraOfSignatures.Core.RepresentationConverters;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConverterUniform2Tests
{
    private readonly RepresentationConverterUniform2 _converterUniform2 = new();
    private readonly RepresentationConverterUniformN _converterUniformN = new();

    [Fact]
    public void ComputeSignatureFromIncidence_ReturnsCorrectValue()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public void ComputeSignatureFromAdjacency_ReturnsCorrectValue()
    {
        int vertexCount = 12,
            uniformityDegree = 2;
        var signature = new Signature(459, vertexCount, uniformityDegree);
        
        var adjacency = _converterUniform2.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);

        var signatureOutput = _converterUniform2.ComputeSignatureFromAdjacency(
            adjacency);
        
        Assert.True(true);
        
        
        /*
          var converter = new RepresentationConverterUniform3();
        
         
         */
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
        int vertexCount = 12,
            uniformityDegree = 2;
        var signature = new Signature(459, vertexCount, uniformityDegree);
        
        var result = _converterUniform2.ComputeAdjacencyFromSignature(
            signature,
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