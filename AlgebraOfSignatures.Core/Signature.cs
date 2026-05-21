using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.Unicode;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

public class Signature :
    ISignature,
    IGraphRepresentation,
    IEquatable<Signature>,
    IComparable<Signature>
{
    #region Fields

    private readonly int _vertexCount;

    private readonly int _uniformityDegree;

    private readonly Matrix<long> _value = null!;

    #endregion


    #region Properties

    public Matrix<long> Value
    {
        get => _value;
        private init
        {
            if (value.ElementType != typeof(long))
                throw new ArgumentException($"{nameof(value)} elements must be of type long");

            _value = value;
        }
    }

    public int VertexCount
    {
        get => _vertexCount;
        private init
        {
            if (value < 1)
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


    #region Static Fabric Methods

    public static Signature FromGraphMatrix(
        Matrix<long> matrix,
        int vertexCount,
        int uniformityDegree)
    {
        return new Signature(
            matrix,
            vertexCount,
            uniformityDegree);
    }

    public static Signature FromLongValue(
        long value,
        int vertexCount)
    {
        return new Signature(
            value,
            vertexCount);
    }

    public static Signature Empty(
        int vertexCount,
        int uniformityDegree)
    {
        if (vertexCount < 1)
            throw new ArgumentException($"{nameof(vertexCount)} must be more than 0");
        if (uniformityDegree < 1)
            throw new ArgumentException($"{nameof(uniformityDegree)} must be more than 1");
        
        var arraySize = vertexCount - uniformityDegree + 1;
        var arrayRank = uniformityDegree - 2;

        if (uniformityDegree == 2)
            arrayRank = arraySize = 1;
        
        if (!(uniformityDegree == 2 && vertexCount == 1) && 
             vertexCount < uniformityDegree)
        {
            throw new ArgumentException($"{nameof(vertexCount)} must be more or equal to {nameof(uniformityDegree)}");
        }
            
        return new Signature(
            new Matrix<long>(arraySize, arrayRank),
            vertexCount,
            uniformityDegree);
    }

    #endregion
    
    
    #region Constructors

    public Signature(
        Matrix<long> value,
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
        : this(value < 0 ? 
                throw new ArgumentException("Value cannot be negative.", nameof(value)) :
                new Matrix<long>(new [] { value }),
            vertexCount, 
            2)
    {
    }
    
    #endregion
    
    
    #region Methods
    
    public void SetValue(
        long value, 
        params int[] indices)
    {
        if (indices.Length == 0)
            indices = [0];

        ThrowIfSetIncorrectValueToSignature(value, indices); 
        Value.SetValue(value, indices);
    }
    

    /// <inheritdoc/>
    public long GetValue(params int[] indices)
    {
        return Convert.ToInt64(
            indices.Length == 0 ? 
                Value.GetValue(0) :
                Value.GetValue(indices));
    }

    //note: can be moved to iterator
    public int CalculateBitLengthFromIndices(params int[] indices)
    {
        return VertexCount - UniformityDegree + 1 - indices[^1];
    }

    private long TruncateValue(long value)
    {
        var msb = (int)Math.Floor(Math.Log2(value));
        return value & ((1L << msb) - 1);
    }

    private int SignatureValueCompare(       
        long lastValue,
        long currValue,
        int currValueBitLength)
    {
        ThrowIfLargeValue(currValue, currValueBitLength);
        
        
        lastValue = TruncateValue(lastValue);
        var lastCount = 0;
        var currCount = 0;
        
        for (var bit = currValueBitLength-1; bit >= 0; --bit)
        {
            lastCount += (int)((lastValue >> bit) & 1L);
            currCount += (int)((currValue >> bit) & 1L);

            if (currCount > lastCount)
                return 1;
        }
        
        return currCount == lastCount ? 0 : -1;
    }
   
    protected long GetNextLongValueFromLeftToRight(
        long value,
        int maxBitLength)
    {
        if (value == (1L << maxBitLength) - 1)
            throw new ArgumentException(
                "No next value: current value is the maximum for the given bit length.",
                nameof(value));
        
        var onesIndex = -1;
        var passedLeadingOnes = false;
        
        for (var i = maxBitLength - 1; i >= 0; i--)
        {
            bool bitIsSet = ((value >> i) & 1L) == 1L;
            
            if (!passedLeadingOnes)
            {
                if (!bitIsSet) 
                    passedLeadingOnes = true;
            }
            else if (bitIsSet)
            {
                onesIndex = i;
                break;
            }
        }

        if (onesIndex == -1)
        {
            value |= 1L;
        }
        else
        {
            value &= ~(1L << onesIndex);
            value |= 1L << (onesIndex + 1);
        }
        
        return value;
    }

    protected long GetNextLongValueFromTopToBottom(
        long value,
        int maxBitLength)
    {
        if (value == (1L << maxBitLength) - 1)
            throw new ArgumentException(
                "No next value: current value is the maximum for the given bit length.",
                nameof(value));
        
        var minimalOnesCount = BitOperations.PopCount((ulong)value);

        do
        {
            ++value;
        }
        while(BitOperations.PopCount((ulong)value) < minimalOnesCount);
        
        return value;
    }
    
    #endregion
    
          
    #region ThrowIf Methods

    protected void ThrowIfSetIncorrectValueToSignature(
        long value, 
        params int[] indices)
    {
        ThrowIfLargeValue(
            value,
            CalculateBitLengthFromIndices(indices));
        
        for(var i = 0; i < indices.Length; ++i)
        {
            if (indices[i] != VertexCount - 1)
            {
                ++indices[i];
                ThrowIfIncorrectSignatureNextValue(
                    value,
                    Convert.ToInt64(Value.GetValue(indices)),
                    i,
                    indices);
                --indices[i];
            }

            if (indices[i] != 0)
            {
                --indices[i];
                ThrowIfIncorrectSignatureNextValue(
                    Convert.ToInt64(Value.GetValue(indices)),
                    value,
                    i,
                    indices);
                ++indices[i];
            }
        }
    }

    protected void ThrowIfIncorrectSignature(IMatrix value)
    {
        if (value.ElementType != typeof(long))
            throw new ArgumentException($"{nameof(value)} elements must be of type long");
        
        this.Traverse(state =>
        {
            var currValue = 
                Convert.ToInt64(value.GetValue(state.SignatureIndices));
            
            var currBitLength =
                CalculateBitLengthFromIndices(state.SignatureIndices);
            
            ThrowIfLargeValue(currValue, currBitLength);
                    
            for (var k = 0; k < state.SignatureIndices.Length - 1; ++k)
            {
                if (state.SignatureIndices[k] > state.SignatureIndices[k + 1])
                    return;
            }
           
            for (var i = 0; i < state.SignatureIndices.Length; ++i)
            {
                if (state.SignatureIndices[i] == state.SignatureArraySize - 1) 
                    continue;
                
                ++state.SignatureIndices[i];
                ThrowIfIncorrectSignatureNextValue(
                    currValue, 
                    Convert.ToInt64(value.GetValue(state.SignatureIndices)),
                    i,
                    state.SignatureIndices);
                --state.SignatureIndices[i];
            }
        });
    }

    protected void ThrowIfLargeValue(
        long value, 
        int bitLength)
    {
        if (value >= (1L << bitLength))
            throw new ArgumentException(
                $"Such a signature cannot exist. The number {value} is too large for this position in the array.", nameof(value));
    }

    protected void ThrowIfIncorrectSignatureNextValue(
        long lastValue,
        long currValue,
        int changedIndex,
        params int[] indices)
    {
        if(1 != SignatureValueCompare(
               lastValue, 
               currValue, 
               CalculateBitLengthFromIndices(indices)))
            return;
        
        var signatureIndicesStrTo = string.Join(", ", indices);
        var adjacencyIndices = new int[indices.Length];
        for (var i = 0; i < indices.Length; ++i)
        {
            adjacencyIndices[i] = i + indices[i];
        }
        var adjacencyIndicesStrTo = string.Join(", ", adjacencyIndices);

        --indices[changedIndex];
        var signatureIndicesStrFrom = string.Join(", ", indices);
        adjacencyIndices = new int[indices.Length];
        for (var i = 0; i < indices.Length; ++i)
        {
            adjacencyIndices[i] = i + indices[i];
        }
        var adjacencyIndicesStrFrom = string.Join(", ", adjacencyIndices);

        
        throw new ArgumentException(
            $"Invalid input. Domain partitioning rule violated by values:\nAdjacency section at:\t\t[{adjacencyIndicesStrFrom}] and [{adjacencyIndicesStrTo}]\nSignature at:\t\t[{signatureIndicesStrFrom}] and [{signatureIndicesStrTo}]\n");
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
        
        this.Traverse(state =>
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
        
        this.Traverse(state =>
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
        //отбрасывать старшие разряды? 
        //у каждого числа?
        //прохрдить по краю и уменьшать на один разряд
        
        /*
        if (n <= 0) 
            return this;
        
        Value &= (1L << n) - 1;
        */
        return this;
    }

    public Signature Add(Signature other, AddType type)
    {
        //получить вектор степеней вершин. Сложить все числа там. Получится ровно колво единиц
        //это оно и есть
        //тогда ассоциативность выполнится
        
        var toAdd = 0;
        var counter = Signature.Empty(VertexCount, UniformityDegree);
        while (counter < other)
        {
            ++toAdd;
            counter.Add(1, type);   
        }
        
        this.Add(toAdd, type);
        return this;
    }

    public Signature Add(long constant, AddType type)
    {
        this.Traverse(state =>
        {
            var bitLen = CalculateBitLengthFromIndices(state.SignatureIndices);
            var maxValue = (1L << bitLen) - 1;
            var currentValue = System.Convert.ToInt64(
                Value.GetValue(state.SignatureIndices));
            
            while (constant != 0 &&
                   currentValue != maxValue)
            {
                --constant;
                currentValue = type switch
                {
                    AddType.Vertical => 
                        GetNextLongValueFromTopToBottom(currentValue, bitLen),
                    
                    AddType.Horizontal =>
                        GetNextLongValueFromLeftToRight(currentValue, bitLen),

                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
            }
            
            Value.SetValue(currentValue, state.SignatureIndices);
        });
        
        if (constant != 0)
            throw new ArgumentException("The value passed is too large. The signature is full.");
        
        return this;
    }

    #endregion
    
    
    #region Static Operator Methods
    
    public static Signature Intersect(Signature a, Signature b) => 
        a.Clone().Intersect(b);

    public static Signature Union(Signature a, Signature b) => 
        a.Clone().Union(b);
    
    public static Signature Add(Signature a, Signature b, AddType type) =>
        a.Clone().Add(b, type);

    public static Signature Add(Signature a, long constant, AddType type) =>
        a.Clone().Add(constant, type);

    public static Signature Mod2N(Signature a, int n) =>
        a.Clone().Mod2N(n);
    
    #endregion
    
    
    #region Operators
    
    public static Signature operator &(Signature a, Signature b) => 
        Signature.Intersect(a, b);

    public static Signature operator |(Signature a, Signature b) => 
        Signature.Union(a, b);
    
    public static Signature operator +(Signature a, Signature b) =>
        Signature.Add(a, b, AddType.Vertical);

    public static Signature operator +(Signature a, long constant) =>
        Signature.Add(a, constant, AddType.Vertical);
    
    public static bool operator ==(Signature? a, Signature? b) =>
        a?.Equals(b) ?? b is null;

    public static bool operator !=(Signature? a, Signature? b) =>
        !(a == b);

    public static bool operator <(Signature a, Signature b) =>
        a.CompareTo(b) < 0;

    public static bool operator >(Signature a, Signature b) =>
        a.CompareTo(b) > 0;

    public static bool operator <=(Signature a, Signature b) =>
        a.CompareTo(b) <= 0;

    public static bool operator >=(Signature a, Signature b) =>
        a.CompareTo(b) >= 0;
    
    #endregion


    #region Nested

    public enum AddType
    {
        Vertical = 0,
        Horizontal = 1
    }
    
    public enum OperationsTypes
    {
        Union = 0,
        Intersection = 1,
        AdditionVertical = 2,
        AdditionHorizontal = 3,
        AdditionVerticalConst = 4,
        AdditionHorizontalConst = 5
    }

    #endregion
    
    
    #region IGraphRepresentation Implementation
    
    object? IGraphRepresentation.GetValue(params int[] indices)
        => GetValue(indices);

    void IGraphRepresentation.SetValue(object? value, params int[] indices)
        => SetValue(
            (long)(value ?? throw new ArgumentNullException(nameof(value))), 
            indices);

    int IGraphRepresentation.Rank =>
        Value.Rank;

    int IGraphRepresentation.Size =>
        Value.Size;

    Type IGraphRepresentation.ElementType => 
        Value.ElementType;
    
    #endregion
    

    #region ICloneable Implementation

    public Signature Clone()
    {
        return new Signature(
            (Matrix<long>) Value.Clone(),
            VertexCount,
            UniformityDegree);
    }

    object ICloneable.Clone() => 
        Clone();

    #endregion

    
    #region IEquatable<Signature> Implementation

    public bool Equals(Signature? other)
    {
        if (other is null) 
            return false;
        
        if (ReferenceEquals(this, other)) 
            return true;
        
        return 
            _vertexCount == other._vertexCount &&
            _uniformityDegree == other._uniformityDegree &&
            _value.Equals(other._value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        
        if (ReferenceEquals(this, obj)) 
            return true;
        
        if (obj.GetType() != GetType())
            return false;
        
        return Equals((Signature)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _vertexCount, 
            _uniformityDegree, 
            _value);
    }
    
    #endregion


    #region IComparable Implementation

    public int CompareTo(Signature? other)
    {
        if (ReferenceEquals(this, other))
            return 0;
        
        if (other is null) 
            return 1;
        
        var uniformityDegreeComparison = 
            _uniformityDegree.CompareTo(other._uniformityDegree);
        if (uniformityDegreeComparison != 0) 
            return uniformityDegreeComparison;
        
        var vertexCountComparison = 
            _vertexCount.CompareTo(other._vertexCount);
        if (vertexCountComparison != 0) 
            return vertexCountComparison;

        var compareResult = 0;
        this.Traverse(state =>
        {
            var ownValue =
                Convert.ToInt64(Value.GetValue(state.SignatureIndices));
            var otherValue =
                Convert.ToInt64(other.Value.GetValue(state.SignatureIndices));

            var result = ownValue.CompareTo(otherValue);
            if (result != 0 && compareResult == 0)
                compareResult = result;
        });

        return compareResult;
    }

    #endregion
}