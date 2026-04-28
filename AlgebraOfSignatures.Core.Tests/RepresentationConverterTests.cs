using AlgebraOfSignatures.Core.RepresentationConverters;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConverterTests
{
    //todo: tests for 2,3,4, 5(?) uh. Data from excel
    
    private readonly RepresentationConverterUniform2 _converterUniform2 = new();
    private readonly RepresentationConverterUniform3 _converterUniform3 = new();
    private readonly RepresentationConverterUniformN _converterUniformN = new();
    
    [Fact]
    public void ComputeUniform3_ReturnsCorrectValue()
    {
        int vertexCount = 6,
            uniformityDegree = 3;
      
        var matrix = _converterUniform3.CreateRankedArray<bool>(vertexCount, uniformityDegree);
        matrix.SetValue(true, 0,1,2);
        matrix.SetValue(true, 0,1,3);
        matrix.SetValue(true, 0,1,4);
        matrix.SetValue(true, 0,1,5);
        matrix.SetValue(true, 0,2,1);
        matrix.SetValue(true, 0,2,3);
        matrix.SetValue(true, 0,2,4);
        matrix.SetValue(true, 0,3,1);
        matrix.SetValue(true, 0,3,2);
        matrix.SetValue(true, 0,3,4);
        matrix.SetValue(true, 0,4,1);
        matrix.SetValue(true, 0,4,2);
        matrix.SetValue(true, 0,4,3);
        matrix.SetValue(true, 0,5,1);
        matrix.SetValue(true, 1,0,2);
        matrix.SetValue(true, 1,0,3);
        matrix.SetValue(true, 1,0,4);
        matrix.SetValue(true, 1,0,5);
        matrix.SetValue(true, 1,2,0);
        matrix.SetValue(true, 1,2,3);
        matrix.SetValue(true, 1,2,4);
        matrix.SetValue(true, 1,3,0);
        matrix.SetValue(true, 1,3,2);
        matrix.SetValue(true, 1,3,4);
        matrix.SetValue(true, 1,4,0);
        matrix.SetValue(true, 1,4,2);
        matrix.SetValue(true, 1,4,3);
        matrix.SetValue(true, 1,5,0);
        matrix.SetValue(true, 2,0,1);
        matrix.SetValue(true, 2,0,3);
        matrix.SetValue(true, 2,0,4);
        matrix.SetValue(true, 2,1,0);
        matrix.SetValue(true, 2,1,3);
        matrix.SetValue(true, 2,1,4);
        matrix.SetValue(true, 2,3,0);
        matrix.SetValue(true, 2,3,1);
        matrix.SetValue(true, 2,3,4);
        matrix.SetValue(true, 2,4,0);
        matrix.SetValue(true, 2,4,1);
        matrix.SetValue(true, 2,4,3);
        matrix.SetValue(true, 3,0,1);
        matrix.SetValue(true, 3,0,2);
        matrix.SetValue(true, 3,0,4);
        matrix.SetValue(true, 3,1,0);
        matrix.SetValue(true, 3,1,2);
        matrix.SetValue(true, 3,1,4);
        matrix.SetValue(true, 3,2,0);
        matrix.SetValue(true, 3,2,1);
        matrix.SetValue(true, 3,2,4);
        matrix.SetValue(true, 3,4,0);
        matrix.SetValue(true, 3,4,1);
        matrix.SetValue(true, 3,4,2);
        matrix.SetValue(true, 4,0,1);
        matrix.SetValue(true, 4,0,2);
        matrix.SetValue(true, 4,0,3);
        matrix.SetValue(true, 4,1,0);
        matrix.SetValue(true, 4,1,2);
        matrix.SetValue(true, 4,1,3);
        matrix.SetValue(true, 4,2,0);
        matrix.SetValue(true, 4,2,1);
        matrix.SetValue(true, 4,2,3);
        matrix.SetValue(true, 4,3,0);
        matrix.SetValue(true, 4,3,1);
        matrix.SetValue(true, 4,3,2);
        matrix.SetValue(true, 5,0,1);
        matrix.SetValue(true, 5,1,0);
       
        
        var signature3 = _converterUniform3.ComputeSignatureFromAdjacency(matrix);
        var signatureN = _converterUniformN.ComputeSignatureFromAdjacency(matrix);
        Assert.True(signature3.Value == 11);
        Assert.True(signatureN.Value == 11);
        
        var matrix3 = _converterUniform3.ComputeAdjacencyFromSignature(
            new Signature(11, vertexCount, uniformityDegree),
            vertexCount, 
            uniformityDegree);
        var matrixN = _converterUniformN.ComputeAdjacencyFromSignature(
            new Signature(11, vertexCount, uniformityDegree),
            vertexCount, 
            uniformityDegree);
        
        Assert.Equal(matrix.Length, matrix3.Length);
        for (var i = 0; i < vertexCount; ++i)
        {
            for (var j = 0; j < vertexCount; ++j)
            {
                for (var k = 0; k < vertexCount; ++k)
                {
                    var value = matrix.GetValue(i, j, k);
                    var value3 = matrix3.GetValue(i, j, k);
                    var valueN = matrixN.GetValue(i, j, k);
                    Assert.Equal(value, value3);
                    Assert.Equal(value, valueN);
                }
            }
        }
        
    }

    [Fact]
    public void ComputeUniform2_ReturnsCorrectValue()
    {
        int vertexCount = 12,
            uniformityDegree = 2;
        var signature = new Signature(459, vertexCount, uniformityDegree);
        
        var matrix2 = _converterUniform2.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);
        var matrixN = _converterUniformN.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);

        Assert.Equal(matrix2.Length, matrixN.Length);
        for (var i = 0; i < matrix2.GetLength(0); i++)
        {
            for (var j = 0; j < matrix2.GetLength(1); j++)
            {
                var value2 = Convert.ToBoolean(
                    matrix2.GetValue(i, j));
                var valueN = Convert.ToBoolean(
                    matrixN.GetValue(i, j));
                Assert.Equal(value2, valueN);
            }
        }

        var signature2 = _converterUniform2.ComputeSignatureFromAdjacency(matrix2);
        var signatureN = _converterUniformN.ComputeSignatureFromAdjacency(matrixN);
        Assert.Equal(signature2.Value, signatureN.Value);
        
        Assert.True(true);
    }
}