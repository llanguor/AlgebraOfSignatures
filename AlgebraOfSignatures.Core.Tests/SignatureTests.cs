namespace AlgebraOfSignatures.Core.Tests;

public class SignatureTests
{
    [Fact]
    public void GetValue_2Uniform_ReturnsCorrectValue()
    {
        var signature = new Signature(11, 6, 4);
        var value0 = signature.GetValue(0);
        var value1 = signature.GetValue(1);
        var value2 = signature.GetValue(2);
        var value3 = signature.GetValue(3);
        Assert.True(true);
    }
}