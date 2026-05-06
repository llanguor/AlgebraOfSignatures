using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

public class Signature :
    ISignature,
    ICloneable
{
    #region Fields

    private readonly int _vertexCount;

    private readonly int _uniformityDegree;

    private readonly Array _value = null!;
    
    #endregion
    
    
    #region Properties

    public Array Value
    {
        get => _value;
        private init
        {
            if (value.GetFinalElementType() != typeof(long))
                throw new ArgumentException($"{nameof(value)} elements must be of type long");
            
            _value = value;
        }
    }

    public int VertexCount
    {
        get => _vertexCount;
        private init
        {
            if (value < 2)
                throw new ArgumentException(
                    "Vertex count cannot be less than 2.", 
                    nameof(VertexCount));
            
            _vertexCount = value;
        }
    }

    public int UniformityDegree
    {
        get => _uniformityDegree;
        private init
        {
            if (value < 2)
                throw new ArgumentException(
                    "Uniformity edge cannot be less than 2.",
                    nameof(UniformityDegree));
            
            _uniformityDegree = value;
        }
    }
    
    #endregion
    
    
    #region Constructors

    public Signature(
        Array value,
        int vertexCount,
        int uniformityDegree)
    {
        VertexCount = vertexCount;
        UniformityDegree = uniformityDegree;
        
        ThrowIfIncorrectSignature(value);
        Value = value;
    }
    
    public Signature(
        long value,
        int vertexCount)
    {
        if (value < 0)
            throw new ArgumentException("Value cannot be negative.", nameof(value));
        
        Value = new [] { value };
        VertexCount = vertexCount;
        UniformityDegree = 2;
    }
    
    #endregion
    
    
    #region Methods
    
    public void SetValue(
        long value, 
        params int[] indices)
    {
        if (indices.Length == 0)
            indices = [0];

        Value.SetValue(value, indices);
        
        //todo: replace with ThrowIfIncorrectSetValue
        ThrowIfIncorrectSignature(Value); 
    }

    /// <inheritdoc/>
    public long GetValue(params int[] indices)
    {
        return Convert.ToInt64(
            indices.Length == 0 ? 
                Value.GetValue(0) :
                Value.GetValue(indices));
    }

    private int CalculateBitLength(params int[] indices)
    {
        return VertexCount - 2 - indices[^1];
    }

    private long TruncateValue(long value)
    {
        var msb = (int)Math.Floor(Math.Log2(value));
        return value & ((1L << msb) - 1);
    }

    private int SignatureValueCompare(       
        long lastValue,
        long currValue,
        params int[] indices)
    {
        var totalBits = CalculateBitLength(indices);
        if (currValue >> totalBits != 0)
            throw new ArgumentException(
                $"Such a signature cannot exist. The number is too large for this position in the array.",
                nameof(currValue));
        
        lastValue = TruncateValue(lastValue);
        var lastCount = 0;
        var currCount = 0;
        
        for (var bit = totalBits-1; bit >= 0; --bit)
        {
            lastCount += (int)((lastValue >> bit) & 1L);
            currCount += (int)((currValue >> bit) & 1L);

            if (currCount > lastCount)
                return 1;
        }
        
        return currCount == lastCount ? 0 : -1;
    }

    #endregion
    
          
    #region ThrowIf Methods

    protected void ThrowIfIncorrectSignature(Array value)
    {
        if (value.GetFinalElementType() != typeof(long))
            throw new ArgumentException($"{nameof(value)} elements must be of type long");
        
        if (UniformityDegree == 2)
            return;

        if (UniformityDegree != 3) 
            return;
        
        //todo: k-signature
        for (var i = 1; i < VertexCount - 2; ++i)
        {
            ThrowIfIncorrectSignatureNextValue(
                Convert.ToInt64(value.GetValue(i-1)),
                Convert.ToInt64(value.GetValue(i)),
                i);
        }
    }

    protected void ThrowIfIncorrectSignatureNextValue(
        long lastValue,
        long currValue,
        params int[] indices)
    {
        if (SignatureValueCompare(lastValue, currValue, indices) == 1)
            throw new ArgumentException(
                "Such a signature cannot exist. A \"0\" cannot be followed by a \"1\" by the definition of extremity.");
    }

    protected void ThrowIfVertexCountMismatch( 
        int vertexCount1,
        int vertexCount2)
    {
        if(vertexCount1 != vertexCount2)
            throw new Exception("Cannot operate on signatures with different vertex counts");
    }
    
    protected void ThrowIfUniformityDegreeMismatch( 
        int uniformityDegree1,
        int uniformityDegree2)
    {
        if(uniformityDegree1 != uniformityDegree2)
            throw new ArgumentException("Uniformity degree mismatch");
    }
    
    #endregion
    
    
    #region Operators Methods

    public Signature Intersect(Signature other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        ThrowIfVertexCountMismatch(
            this.VertexCount,
            other.VertexCount);
        
        Value.TraverseSignature(VertexCount, UniformityDegree, state =>
        {
            var signatureValue1 = this.GetValue(state.SignatureIndices);
            var signatureValue2 = other.GetValue(state.SignatureIndices);
            var signatureValueResult = 0L;
            
            if (signatureValue1 == 0 && signatureValue2 == 0)
                return;

            var bitsCount = Convert.ToInt32(
                Math.Floor(Math.Log2(Math.Max(signatureValue1, signatureValue2))));

            int onesCount1 = 0,
                onesCount2 = 0;

            for (var currentBitNumber = bitsCount;
                 currentBitNumber >= 0;
                 --currentBitNumber)
            {
                var bit1 =
                    (signatureValue1 >> currentBitNumber) & 1L;
                var bit2 =
                    (signatureValue2 >> currentBitNumber) & 1L;

                if (bit2 < bit1 &&
                    onesCount2 == onesCount1)
                {
                    (signatureValue1, signatureValue2) =
                        (signatureValue2, signatureValue1);
                    (bit1, bit2) =
                        (bit2, bit1);
                }

                if (bit1 == 1)
                    ++onesCount1;

                if (bit2 == 1)
                    ++onesCount2;

                signatureValueResult =
                    (signatureValueResult & ~(1L << currentBitNumber)) |
                    (bit1 << currentBitNumber);
            }
            
            this.Value.SetValue(signatureValueResult, state.SignatureIndices);
        });

        return this;
    }

    public Signature Union(Signature other)
    {
        ThrowIfUniformityDegreeMismatch(
            this.UniformityDegree, 
            other.UniformityDegree);
        
        ThrowIfVertexCountMismatch(
            this.VertexCount,
            other.VertexCount);
        
        Value.TraverseSignature(VertexCount, UniformityDegree, state =>
        {
            var signatureValue1 = this.GetValue(state.SignatureIndices);
            var signatureValue2 = other.GetValue(state.SignatureIndices);
            var signatureValueResult = 0L;
            
            if (signatureValue1 == 0 && signatureValue2 == 0)
                return;

            var bitsCount = Convert.ToInt32(
                Math.Floor(Math.Log2(Math.Max(signatureValue1, signatureValue2))));

            int onesCount1 = 0,
                onesCount2 = 0;

            for (var currentBitNumber = bitsCount;
                 currentBitNumber >= 0;
                 --currentBitNumber)
            {
                var bit1 =
                    (signatureValue1 >> currentBitNumber) & 1L;
                var bit2 =
                    (signatureValue2 >> currentBitNumber) & 1L;

                if (bit2 > bit1 &&
                    onesCount2 == onesCount1)
                {
                    (signatureValue1, signatureValue2) =
                        (signatureValue2, signatureValue1);
                    (bit1, bit2) =
                        (bit2, bit1);
                }

                if (bit1 == 1)
                    ++onesCount1;

                if (bit2 == 1)
                    ++onesCount2;

                signatureValueResult =
                    (signatureValueResult & ~(1L << currentBitNumber)) |
                    (bit1 << currentBitNumber);
            }
            
            this.Value.SetValue(signatureValueResult, state.SignatureIndices);
        });

        return this;
    }

    public Signature Mod2N(int n)
    {
        /*
        if (n <= 0) 
            return this;
        
        Value &= (1L << n) - 1;
        */
        return this;
    }

    public Signature Add(Signature other)
    {
        /*
        ThrowIfVertexCountMismatch(
            this.VertexCount, 
            other.VertexCount);

        this.Value += other.Value;
        this.Mod2N(VertexCount-1);
        */
        return this;
    }

    public Signature Add(long constant)
    {
        //todo: mod n. For Signature object or for long value?
        //this.Value += constant;
        //this.Mod2N(VertexCount-1);
        
        Value.TraverseSignature(VertexCount, UniformityDegree, state =>
        { 
            this.Value.SetValue(
                constant + this.GetValue(state.SignatureIndices), 
                state.SignatureIndices);
        });

        return this;
    }

    public Signature Multiply(Signature other)
    {
        /*
        ThrowIfVertexCountMismatch(
            this.VertexCount, 
            other.VertexCount);
                   
        this.Value *= other.Value;
        this.Mod2N(VertexCount-1);
        */
        return this;
    }

    public Signature Multiply(long constant)
    {
        //todo: mod n. For Signature object or for long value?
        //this.Value += constant;
        //this.Mod2N(VertexCount-1);
        
        Value.TraverseSignature(VertexCount, UniformityDegree, state =>
        { 
            this.Value.SetValue(
                constant * this.GetValue(state.SignatureIndices), 
                state.SignatureIndices);
        });

        return this;
    }
    
    #endregion
    
    
    #region Static Operator Methods
    
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
            (Array)Value.Clone(),
            VertexCount,
            UniformityDegree);
    }

    object ICloneable.Clone() => 
        Clone();

    #endregion
}