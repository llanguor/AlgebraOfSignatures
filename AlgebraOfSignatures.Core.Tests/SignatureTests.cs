namespace AlgebraOfSignatures.Core.Tests;

public class SignatureTests
{
    [Fact]
    public void GetValue_2Uniform_ReturnsCorrectValue()
    {
        var signature = new Signature(11, 6, 4);
        Assert.Equal(11, signature.GetValue(0));
        Assert.Equal(3, signature.GetValue(1));
        Assert.Equal(1, signature.GetValue(2));
        Assert.Equal(0, signature.GetValue(3));
        
    }
}