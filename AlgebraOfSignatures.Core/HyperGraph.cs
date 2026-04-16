using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class HyperGraph(
    IHyperGraphRepresentationConverter converter,
    Array signature,
    int vertexCount, 
    int uniformityDegree) : 
    HyperGraphBase(
        converter,
        signature,
        vertexCount,
        uniformityDegree)
{
    #region Fabric Methods
    
    public static HyperGraph FromIncidenceMatrix(
        IHyperGraphRepresentationConverter converter,
        Array incidenceMatrix,
        int uniformityDegree)
    {
        if (incidenceMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array", 
                nameof(incidenceMatrix));
        
        var vertexCount = incidenceMatrix.GetLength(0);
        
        return new HyperGraph(
            converter,
            converter.ComputeSignatureFromIncidence(incidenceMatrix, uniformityDegree),
            vertexCount,
            uniformityDegree);
    }
    
    public static HyperGraph FromAdjacencyMatrix(
        IHyperGraphRepresentationConverter converter,
        Array adjacencyMatrix)
    {
        if (adjacencyMatrix.GetType().GetElementType() != typeof(bool))
            throw new ArgumentException(
                $"Expected {typeof(bool)} array",
                nameof(adjacencyMatrix));
        
        var vertexCount = adjacencyMatrix.GetLength(0);
        var uniformityDegree = adjacencyMatrix.Rank;
        
        return new HyperGraph(
            converter,
            converter.ComputeSignatureFromAdjacency(adjacencyMatrix),
            vertexCount,
            uniformityDegree);
    }
    
    // todo: vertexCount and uniformityDegree can be derived from signature directly?
    // todo: add if(signature array is just int-value) ...      (for api-consistent) 
    public static HyperGraph FromSignature(
        IHyperGraphRepresentationConverter converter,
        Array signature,
        int vertexCount,
        int uniformityDegree)
    {
        if (signature.GetType().GetElementType() != typeof(int))
            throw new ArgumentException(
                $"Expected {typeof(int)} array", 
                nameof(signature));

        return new HyperGraph(
            converter,
            signature, 
            vertexCount, 
            uniformityDegree);   
    }
    
    // todo: vertexCount can be derived from signature directly?
    public static HyperGraph FromSignature(
        IHyperGraphRepresentationConverter converter,
        int signature,
        int vertexCount)
    {
        var uniformityDegree = 2;

        return new HyperGraph(
            converter,
            new[] { signature }, 
            vertexCount, 
            uniformityDegree);   
    }
    
    #endregion
    
    
    #region ThrowIf Methods
    
    private void ThrowIfVertexCountMismatch( 
        int vertexCount1,
        int vertexCount2)
    {
        if(vertexCount1 != vertexCount2)
            throw new ArgumentException("Vertex count mismatch");
    }
    
    private void ThrowIfUniformityDegreeMismatch( 
        int uniformityDegree1,
        int uniformityDegree2)
    {
        if(uniformityDegree1 != uniformityDegree2)
            throw new ArgumentException("Uniformity degree mismatch");
    }
    
    #endregion
    
    
    #region Operations Methods
        
    public HyperGraph Intersect(HyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        if (UniformityDegree == 2)
        {
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
            
            return new HyperGraph(
                this._converter,
                new int[] {result},
                this.VertexCount,
                this.UniformityDegree);
        }
        
        throw new NotImplementedException();
    }

    public HyperGraph Union(HyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        if (UniformityDegree == 2)
        {
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
            
            return new HyperGraph(
                this._converter,
                new int[] {result},
                this.VertexCount,
                this.UniformityDegree);
        }
        
        throw new NotImplementedException();
    }

    //todo: rename?
    public HyperGraph Mod2N(int n)
    {
        if (UniformityDegree == 2)
        {
            var resultSignature = new[]
            {
                Convert.ToInt32(this.Signature.GetValue(0)!) & 
                    ((1 << n) - 1)
            };
                
            return new HyperGraph(
                this._converter,
                resultSignature,
                this.VertexCount,
                this.UniformityDegree);
        }
        
        throw new NotImplementedException();
    }
    
    public HyperGraph Add(HyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);

        if (UniformityDegree == 2)
        {
            var resultSignature = new[]
            {
                Convert.ToInt32(this.Signature.GetValue(0)!) + 
                Convert.ToInt32(other.Signature.GetValue(0)!)
            };
                
            return new HyperGraph(
                this._converter,
                resultSignature,
                this.VertexCount,
                this.UniformityDegree);
        }
            
        throw new NotImplementedException();
    }
    
    public HyperGraph Add(int constant)
    {
        if (UniformityDegree == 2)
        {
            var resultSignature = new[]
            {
                Convert.ToInt32(this.Signature.GetValue(0)!) + 
                constant
            };
                
            return new HyperGraph(
                this._converter,
                resultSignature,
                this.VertexCount,
                this.UniformityDegree);
        }
        
        throw new NotImplementedException();
    }
    
    public HyperGraph Multiply(HyperGraph other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        if (UniformityDegree == 2)
        {
            var resultSignature = new[]
            {
                Convert.ToInt32(this.Signature.GetValue(0)!) *
                Convert.ToInt32(other.Signature.GetValue(0)!)
            };
                
            return new HyperGraph(
                this._converter,
                resultSignature,
                this.VertexCount,
                this.UniformityDegree);
        }
            
        throw new NotImplementedException();
    }
    
    public HyperGraph Multiply(int constant)
    {
        if (UniformityDegree == 2)
        {
            var resultSignature = new[]
                {
                    Convert.ToInt32(this.Signature.GetValue(0)!) *
                    constant
                };
            
            return new HyperGraph(
                this._converter,
                resultSignature,
                this.VertexCount,
                this.UniformityDegree);
        }
            
        throw new NotImplementedException();
    }
    
    #endregion

    
    #region Operations Override
    
    public static HyperGraph operator &(HyperGraph a, HyperGraph b) => 
        a.Intersect(b);

    public static HyperGraph operator |(HyperGraph a, HyperGraph b) => 
        a.Union(b);
    
    public static HyperGraph operator +(HyperGraph a, HyperGraph b) =>
        a.Add(b).Mod2N(a.VertexCount-1);

    public static HyperGraph operator +(HyperGraph a, int constant) =>
        a.Add(constant).Mod2N(a.VertexCount-1);

    public static HyperGraph operator *(HyperGraph a, HyperGraph b) =>
        a.Multiply(b).Mod2N(a.VertexCount-1);

    public static HyperGraph operator *(HyperGraph a, int constant) =>
        a.Multiply(constant).Mod2N(a.VertexCount-1);
    
    #endregion
}