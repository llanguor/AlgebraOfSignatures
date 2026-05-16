using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core.Tests;

public class RepresentationConverterTests
{
    //todo: тесты: ошибка при неверном заполнении незначащих и справа/слева от сигнатуры для к-матриц. Ошибка при неверном вводе. Проверка всех методов и тд
    //todo: tests for 2,3,4, 5(?) uh. Data from excel
    private readonly RepresentationConverter _converter = new();
    
    [Fact]
    public void ComputeUniform3_ReturnsCorrectValue()
    {
        int vertexCount = 6,
            uniformityDegree = 3;
      
        var inputMatrix = new Matrix<bool>(
            vertexCount, 
            uniformityDegree);
        
        inputMatrix.SetValue(true, 0,1,2);
        inputMatrix.SetValue(true, 0,1,3);
        inputMatrix.SetValue(true, 0,1,4);
        inputMatrix.SetValue(true, 0,1,5);
        inputMatrix.SetValue(true, 0,2,1);
        inputMatrix.SetValue(true, 0,2,3);
        inputMatrix.SetValue(true, 0,2,4);
        inputMatrix.SetValue(true, 0,3,1);
        inputMatrix.SetValue(true, 0,3,2);
        inputMatrix.SetValue(true, 0,3,4);
        inputMatrix.SetValue(true, 0,4,1);
        inputMatrix.SetValue(true, 0,4,2);
        inputMatrix.SetValue(true, 0,4,3);
        inputMatrix.SetValue(true, 0,5,1);
        inputMatrix.SetValue(true, 1,0,2);
        inputMatrix.SetValue(true, 1,0,3);
        inputMatrix.SetValue(true, 1,0,4);
        inputMatrix.SetValue(true, 1,0,5);
        inputMatrix.SetValue(true, 1,2,0);
        inputMatrix.SetValue(true, 1,2,3);
        inputMatrix.SetValue(true, 1,2,4);
        inputMatrix.SetValue(true, 1,3,0);
        inputMatrix.SetValue(true, 1,3,2);
        inputMatrix.SetValue(true, 1,3,4);
        inputMatrix.SetValue(true, 1,4,0);
        inputMatrix.SetValue(true, 1,4,2);
        inputMatrix.SetValue(true, 1,4,3);
        inputMatrix.SetValue(true, 1,5,0);
        inputMatrix.SetValue(true, 2,0,1);
        inputMatrix.SetValue(true, 2,0,3);
        inputMatrix.SetValue(true, 2,0,4);
        inputMatrix.SetValue(true, 2,1,0);
        inputMatrix.SetValue(true, 2,1,3);
        inputMatrix.SetValue(true, 2,1,4);
        inputMatrix.SetValue(true, 2,3,0);
        inputMatrix.SetValue(true, 2,3,1);
        inputMatrix.SetValue(true, 2,3,4);
        inputMatrix.SetValue(true, 2,4,0);
        inputMatrix.SetValue(true, 2,4,1);
        inputMatrix.SetValue(true, 2,4,3);
        inputMatrix.SetValue(true, 3,0,1);
        inputMatrix.SetValue(true, 3,0,2);
        inputMatrix.SetValue(true, 3,0,4);
        inputMatrix.SetValue(true, 3,1,0);
        inputMatrix.SetValue(true, 3,1,2);
        inputMatrix.SetValue(true, 3,1,4);
        inputMatrix.SetValue(true, 3,2,0);
        inputMatrix.SetValue(true, 3,2,1);
        inputMatrix.SetValue(true, 3,2,4);
        inputMatrix.SetValue(true, 3,4,0);
        inputMatrix.SetValue(true, 3,4,1);
        inputMatrix.SetValue(true, 3,4,2);
        inputMatrix.SetValue(true, 4,0,1);
        inputMatrix.SetValue(true, 4,0,2);
        inputMatrix.SetValue(true, 4,0,3);
        inputMatrix.SetValue(true, 4,1,0);
        inputMatrix.SetValue(true, 4,1,2);
        inputMatrix.SetValue(true, 4,1,3);
        inputMatrix.SetValue(true, 4,2,0);
        inputMatrix.SetValue(true, 4,2,1);
        inputMatrix.SetValue(true, 4,2,3);
        inputMatrix.SetValue(true, 4,3,0);
        inputMatrix.SetValue(true, 4,3,1);
        inputMatrix.SetValue(true, 4,3,2);
        inputMatrix.SetValue(true, 5,0,1);
        inputMatrix.SetValue(true, 5,1,0);
       
        
        var signature = _converter.ComputeSignatureFromAdjacency(inputMatrix);
        
        Assert.Equal(11, signature.GetValue(0));
        Assert.Equal(3, signature.GetValue(1));
        Assert.Equal(1, signature.GetValue(2));
        Assert.Equal(0, signature.GetValue(3));
       
        var matrix = _converter.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);
        
        Assert.Equal(inputMatrix.Size, matrix.Size);
        for (var i = 0; i < vertexCount; ++i)
        {
            for (var j = 0; j < vertexCount; ++j)
            {
                for (var k = 0; k < vertexCount; ++k)
                {
                    var inputValue = inputMatrix.GetValue(i, j, k);
                    var value = matrix.GetValue(i, j, k);
                    Assert.Equal(value, inputValue);
                }
            }
        }
        
        Assert.True(true);
    }

    [Fact]
    public void ComputeUniform2_ReturnsCorrectValue()
    {
        int vertexCount = 12,
            uniformityDegree = 2;
        var inputSignature = new Signature(459, vertexCount);
        inputSignature.SetValue(459);
        
        var matrix = _converter.ComputeAdjacencyFromSignature(
            inputSignature,
            vertexCount, 
            uniformityDegree);
        
        var signature = _converter.ComputeSignatureFromAdjacency(matrix);
        Assert.Equal(inputSignature.GetValue(), signature.GetValue());
        
        Assert.True(true);
    }
    
    [Fact]
    public void ComputeUniform4_ReturnsCorrectValue()
    {
        int vertexCount = 6,
            uniformityDegree = 4;
      
        var inputMatrix = new Matrix<bool>(
            vertexCount, 
            uniformityDegree);

        Action<int[]> action = ints =>
        {
            inputMatrix.SetValue(true, ints);
        };
        
        int [] indices = [0, 1, 2, 3];
        indices.ForEachPermutation(action);
        indices = [0, 1, 2, 4];
        indices.ForEachPermutation(action);
        indices = [0, 1, 2, 5];
        indices.ForEachPermutation(action);
        indices = [0, 1, 3, 4];
        indices.ForEachPermutation(action);
        indices = [0, 1, 3, 5];
        indices.ForEachPermutation(action);
        indices = [0, 2, 3, 4];
        indices.ForEachPermutation(action);
        indices = [0, 2, 3, 5];
        indices.ForEachPermutation(action);
        indices = [1, 2, 3, 4];
        indices.ForEachPermutation(action);
        
        var signature = _converter.ComputeSignatureFromAdjacency(inputMatrix);
        Assert.Equal(6, signature.GetValue(0, 0));
        Assert.Equal(2, signature.GetValue(0, 1));
        Assert.Equal(0, signature.GetValue(0, 2));
        Assert.Equal(0, signature.GetValue(1, 0));
        Assert.Equal(1, signature.GetValue(1, 1));
        Assert.Equal(0, signature.GetValue(1, 2));
        Assert.Equal(0, signature.GetValue(2, 0));
        Assert.Equal(0, signature.GetValue(2, 1));
        Assert.Equal(0, signature.GetValue(2, 2));
        
        var matrix = _converter.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);
        
        for (var i = 0; i < vertexCount; ++i)
        {
            for (var j = 0; j < vertexCount; ++j)
            {
                for (var k = 0; k < vertexCount; ++k)
                {
                    for (var l = 0; l < vertexCount; ++l)
                    {
                        var inputValue = inputMatrix.GetValue(i, j, k, l);
                        var value = matrix.GetValue(i, j, k, l);
                        Assert.Equal(value, inputValue);
                    }
                }
            }
        }
        
        Assert.True(true);

    }
    
     [Fact]
    public void ComputeUniform5_ReturnsCorrectValue()
    {
        int vertexCount = 6,
            uniformityDegree = 5;
      
        var inputMatrix = new Matrix<bool>(
            vertexCount, 
            uniformityDegree);

        Action<int[]> action = ints =>
        {
            inputMatrix.SetValue(true, ints);
        };
        
        int [] indices = [0, 1, 2, 3, 4];
        indices.ForEachPermutation(action);
        indices = [0, 1, 2, 3, 5];
        indices.ForEachPermutation(action);
        indices = [0, 1, 2, 4, 5];
        indices.ForEachPermutation(action);
        indices = [0, 1, 3, 4, 5];
        indices.ForEachPermutation(action);
        indices = [0, 2, 3, 4, 5];
        indices.ForEachPermutation(action);

        
        var signature = _converter.ComputeSignatureFromAdjacency(inputMatrix);
        Assert.Equal(3, signature.GetValue(0, 0, 0));
        Assert.Equal(1, signature.GetValue(0, 0, 1));
        Assert.Equal(1, signature.GetValue(0, 1, 1));
        Assert.Equal(0, signature.GetValue(1, 1, 1));
        
        var matrix = _converter.ComputeAdjacencyFromSignature(
            signature,
            vertexCount, 
            uniformityDegree);
        
        for (var i = 0; i < vertexCount; ++i)
        {
            for (var j = 0; j < vertexCount; ++j)
            {
                for (var k = 0; k < vertexCount; ++k)
                {
                    for (var l = 0; l < vertexCount; ++l)
                    {
                        for (var d = 0; d < vertexCount; ++d)
                        {
                            var inputValue = inputMatrix.GetValue(i, j, k, l, d);
                            var value = matrix.GetValue(i, j, k, l, d);
                            Assert.Equal(value, inputValue);
                        }
                    }
                }
            }
        }
        
        Assert.True(true);

    }
}