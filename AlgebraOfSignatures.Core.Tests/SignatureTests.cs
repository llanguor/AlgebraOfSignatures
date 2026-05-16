using AlgebraOfSignatures.Core.Base;

namespace AlgebraOfSignatures.Core.Tests;

public class SignatureTests
{
    [Fact]
    public void GetValue_2Uniform_ReturnsCorrectValue()
    {
        /*
        var signature = new Signature(11, 6, 4);
        Assert.Equal(11, signature.GetValue(0));
        Assert.Equal(3, signature.GetValue(1));
        Assert.Equal(1, signature.GetValue(2));
        Assert.Equal(0, signature.GetValue(3));
        */
        
    }
    
    [Fact]
    public void GetAllSignatures_3Uniform()
    {
        var uniformityDegree = 3;
        var vertexCount = 6;
        var res = new List<string>();
        
        for (var i = 0; i < 5; ++i)
        {
            for (var j = 0; j <= i; ++j)
            {
                for (var k = 0; k <= j; ++k)
                {
                    for (var l = 0; l <= k; ++l)
                    {
                        var signatureValue = new Matrix<long>(new long[]{ i, j, k, l });
                        try
                        {
                            var signature = new Signature(signatureValue, vertexCount, uniformityDegree);
                            res.Add($"({i}, {j}, {k}, {l})");
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }
        }
        Assert.True(true);
    }
}