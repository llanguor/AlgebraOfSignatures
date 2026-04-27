using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core;

public class Signature :
    ISignature,
    ICloneable
{
    #region Properties
    
    public long Value { get; private set; }
    public int VertexCount { get; private set; }
    public int UniformityDegree { get; private set; }
    
    #endregion
    
    
    #region Constructors

    public Signature(
        long value,
        int vertexCount,
        int uniformityDegree)
    {
        Value = value;
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
    }
    
    #endregion
    
    
    #region Methods
    
    public void SetValue(long value, int vertexCount, int uniformityDegree)
    {
        Value = value;
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
    }

    /// <inheritdoc/>
    public long GetValue(params int[] indices)
    {
        if (indices.Length == 0)
            return Value;
        
        if (indices.Length != UniformityDegree-2)
            throw new ArgumentException(
                $"Incorrect number of indices for array. Expected {UniformityDegree-2} indices.");

        var lastIndex = indices[0];
        foreach (var index in indices)
        {
            if (index >= VertexCount-2)
                throw new ArgumentException(
                    $"Incorrect index. Each index must be less than {VertexCount-2}.");
            
            if (index < lastIndex)
                throw new ArgumentException(
                    $"Incorrect index. Each index must be greater than or equal to previous index. This is justified by the fact that the prism narrows from bottom to top relative to large indices");
            
            lastIndex = index;
        }
        
        var i = indices[^1];
        var onesCount = 0;
        
        var bytesNumber = VertexCount - 2;
        while (onesCount != i &&
               --bytesNumber >= 0)
        {
            if ((Value & (1<<bytesNumber)) != 0)
                ++onesCount;
        }
        
        return Value & ((1 << bytesNumber)-1);
    }
    
    #endregion
    
    
    #region Operators Methods

    public Signature Intersect(Signature other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        var signature1 = this.Value;
        var signature2 = other.Value;
        var bitsCount =  Convert.ToInt32(
            Math.Floor(Math.Log2(Math.Max(signature1, signature2))));

        int onesCount1 = 0,
            onesCount2 = 0;
        
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
            
            Value =
                (Value & ~(1 << currentBitNumber)) |
                (bit1 << currentBitNumber);
        }

        return this;
    }

    public Signature Union(Signature other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        var signature1 = this.Value;
        var signature2 = other.Value;
        var bitsCount =  Convert.ToInt32(
            Math.Floor(Math.Log2(Math.Max(signature1, signature2))));

        int onesCount1 = 0,
            onesCount2 = 0;
        
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
            
            Value =
                (Value & ~(1 << currentBitNumber)) |
                (bit1 << currentBitNumber);
        }

        return this;
    }

    public Signature Mod2N(int n)
    {
        if (n <= 0) 
            return this;
        
        Value &= (1 << n) - 1;
        return this;
    }

    public Signature Add(Signature other)
    {
        ThrowIfVertexCountMismatch(
            this.VertexCount, 
            other.VertexCount);

        this.Value += other.Value;
        this.Mod2N(VertexCount-1);
        return this;
    }

    public Signature Add(long constant)
    {
        this.Value += constant;
        this.Mod2N(VertexCount-1);
        return this;
    }

    public Signature Multiply(Signature other)
    {
        ThrowIfVertexCountMismatch(
            this.VertexCount, 
            other.VertexCount);
                   
        this.Value *= other.Value;
        this.Mod2N(VertexCount-1);
        return this;
    }

    public Signature Multiply(long constant)
    {
        this.Value *= constant;
        this.Mod2N(VertexCount-1);
        return this;
    }
    
    #endregion
    
    
    #region Static Methods
    
    public static Signature Intersect(Signature a, Signature b) => 
        a.Clone().Intersect(b);

    public static Signature Union(Signature a, Signature b) => 
        a.Clone().Union(b);
    
    public static Signature Add(Signature a, Signature b) =>
        a.Clone().Add(b);

    public static Signature Add(Signature a, long constant) =>
        a.Clone().Add(constant);

    public static Signature Multiply(Signature a, Signature b) =>
        a.Clone().Multiply(b);

    public static Signature Multiply(Signature a, long constant) =>
        a.Clone().Multiply(constant);
    
    #endregion
    
    
    #region Operators
    
    public static Signature operator &(Signature a, Signature b) => 
        Signature.Intersect(a, b);

    public static Signature operator |(Signature a, Signature b) => 
        Signature.Union(a, b);
    
    public static Signature operator +(Signature a, Signature b) =>
        Signature.Add(a, b);

    public static Signature operator +(Signature a, long constant) =>
        Signature.Add(a, constant);

    public static Signature operator *(Signature a, Signature b) =>
        Signature.Multiply(a, b);

    public static Signature operator *(Signature a, long constant) =>
        Signature.Multiply(a, constant);
    
    #endregion
    

    #region ICloneable Implementation

    public Signature Clone()
    {
        return new Signature(
            Value,
            VertexCount,
            UniformityDegree);
    }

    object ICloneable.Clone() => 
        Clone();

    #endregion
    
        
    #region ThrowIf Methods
    
    protected void ThrowIfVertexCountMismatch( 
        int vertexCount1,
        int vertexCount2)
    {
        if(vertexCount1 != vertexCount2)
            throw new Exception("Cannot multiply signatures with different vertex counts");
    }
    
    protected void ThrowIfUniformityDegreeMismatch( 
        int uniformityDegree1,
        int uniformityDegree2)
    {
        if(uniformityDegree1 != uniformityDegree2)
            throw new ArgumentException("Uniformity degree mismatch");
    }
    
    #endregion
}