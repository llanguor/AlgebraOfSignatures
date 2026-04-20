using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;
namespace AlgebraOfSignatures.Core.UniformHyperGraphs;

internal sealed class Uniform2HyperGraph(
    IRepresentationConverter converter, 
    Array signature, 
    int vertexCount, 
    int uniformityDegree) : 
    UniformHyperGraph(
        converter, 
        signature,
        vertexCount, 
        uniformityDegree)
{
    public override IUniformHyperGraph Intersect(IUniformHyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        var signature1 = Convert.ToInt32(this.Signature.GetValue(0)!);
        var signature2 = Convert.ToInt32(other.Signature.GetValue(0)!);
        var bitsCount =  Convert.ToInt32(
            Math.Floor(Math.Log2(Math.Max(signature1, signature2))));

        int onesCount1 = 0,
            onesCount2 = 0,
            result = 0;
        
        for(var currentBitNumber = bitsCount; 
            currentBitNumber >= 0; 
            --currentBitNumber)
        {
            var bit1 = 
                (signature1 >> currentBitNumber) & 1;
            var bit2 = 
                (signature2 >> currentBitNumber) & 1;

            if (bit2 < bit1 &&
                onesCount2 == onesCount1)
            {
                (signature1, signature2) =
                    (signature2, signature1);
                (bit1, bit2) =
                    (bit2, bit1);
            }
            
            if(bit1==1)
                ++onesCount1;
            
            if(bit2==1)
                ++onesCount2;
            
            result =
                (result & ~(1 << currentBitNumber)) |
                (bit1 << currentBitNumber);
        }
        
        return new Uniform2HyperGraph(
            this.Converter,
            new[] {result},
            this.VertexCount,
            this.UniformityDegree);
    }

    public override IUniformHyperGraph Union(IUniformHyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        var signature1 = Convert.ToInt32(this.Signature.GetValue(0)!);
        var signature2 = Convert.ToInt32(other.Signature.GetValue(0)!);
        var bitsCount =  Convert.ToInt32(
            Math.Floor(Math.Log2(Math.Max(signature1, signature2))));

        int onesCount1 = 0,
            onesCount2 = 0,
            result = 0;
        
        for(var currentBitNumber = bitsCount; 
            currentBitNumber >= 0; 
            --currentBitNumber)
        {
            var bit1 = 
                (signature1 >> currentBitNumber) & 1;
            var bit2 = 
                (signature2 >> currentBitNumber) & 1;

            if (bit2 > bit1 &&
                onesCount2 == onesCount1)
            {
                (signature1, signature2) =
                    (signature2, signature1);
                (bit1, bit2) =
                    (bit2, bit1);
            }
            
            if(bit1==1)
                ++onesCount1;
            
            if(bit2==1)
                ++onesCount2;
            
            result =
                (result & ~(1 << currentBitNumber)) |
                (bit1 << currentBitNumber);
        }
        
        return new Uniform2HyperGraph(
            this.Converter,
            new[] {result},
            this.VertexCount,
            this.UniformityDegree);
    }

    //todo: rename?
    public override IUniformHyperGraph Mod2N(int n)
    {
        var resultSignature = new[]
        {
            Convert.ToInt32(this.Signature.GetValue(0)!) & 
                ((1 << n) - 1)
        };
            
        return new Uniform2HyperGraph(
            this.Converter,
            resultSignature,
            this.VertexCount,
            this.UniformityDegree);
    }
    
    public override IUniformHyperGraph Add(IUniformHyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        var resultSignature = new[]
        {
            Convert.ToInt32(this.Signature.GetValue(0)!) + 
            Convert.ToInt32(other.Signature.GetValue(0)!)
        };
            
        return new Uniform2HyperGraph(
            this.Converter,
            resultSignature,
            this.VertexCount,
            this.UniformityDegree);
    }
    
    public override IUniformHyperGraph Add(int constant)
    {
        var resultSignature = new[]
        {
            Convert.ToInt32(this.Signature.GetValue(0)!) + 
            constant
        };
            
        return new Uniform2HyperGraph(
            this.Converter,
            resultSignature,
            this.VertexCount,
            this.UniformityDegree);
    }
    
    public override IUniformHyperGraph Multiply(IUniformHyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        var resultSignature = new[]
        {
            Convert.ToInt32(this.Signature.GetValue(0)!) *
            Convert.ToInt32(other.Signature.GetValue(0)!)
        };
            
        return new Uniform2HyperGraph(
            this.Converter,
            resultSignature,
            this.VertexCount,
            this.UniformityDegree);
    }
    
    public override IUniformHyperGraph Multiply(int constant)
    {
        var resultSignature = new[]
            {
                Convert.ToInt32(this.Signature.GetValue(0)!) *
                constant
            };
        
        return new Uniform2HyperGraph(
            this.Converter,
            resultSignature,
            this.VertexCount,
            this.UniformityDegree);
    }
}