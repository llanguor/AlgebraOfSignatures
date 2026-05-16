using System.Linq.Expressions;
using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;

namespace AlgebraOfSignatures.Core;

public class Signature :
    ISignature,
    ICloneable
{
    #region Fields

    private int _vertexCount;

    private int _uniformityDegree;

    private Array _value = null!;

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
        private set
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
        private set
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

    public static Signature FromArray(
        Array array,
        int vertexCount,
        int uniformityDegree)
    {
        return new Signature(
            array,
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
            ArrayExtensions.CreateRankedArray<long>(arraySize, arrayRank),
            vertexCount,
            uniformityDegree);
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
        : this(value < 0 ? 
                throw new ArgumentException("Value cannot be negative.", nameof(value)) :
                new[] { value }, 
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

    protected void ThrowIfSetIncorrectValueToSignature(
        long value, 
        params int[] indices)
    {
        for(var i = 0; i < indices.Length - 1; ++i)
        {
            if (indices[i] != VertexCount - 1)
            {
                ++indices[i];
                ThrowIfIncorrectSignatureNextValue(
                    value,
                    Convert.ToInt64(Value.GetValue(indices)));
                --indices[i];
            }

            if (indices[i] != 0)
            {
                --indices[i];
                ThrowIfIncorrectSignatureNextValue(
                    Convert.ToInt64(Value.GetValue(indices)),
                    value);
                ++indices[i];
            }
        }
    }

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

    public Signature Add(Signature other)
    {
        //сигнатура с сигнатурой: поразрядное добавления одной суммы к другой по сложению КОЛВА ячеек к другому КОЛВУ ячеек
        //на каждом слое определить какое колво ячеек закрашено и добавить их к другой сигнатуре (КОЛВО (ЧИСЛО ЦЕЛОЕ))
        //делегировать к след
        
        
        //ЕСЛИ СЛОЖЕНИЕ обычное то переводим остаток на след
        //если по модулю на следующий слой не переносим?
        //сложение двух слоев можно отдельно операцию определить
        
        //послойное сложение и отдельное
        //сверху вниз и слева направо
        //разные варианты
        
        //проверить коммутативность, ассоциативность
        //определить выполняются ли свойства при совершении операций всех
        //есть ли единичный элемент?
        
        //есть единичный но все не получим. (состояния могут переходить только в конкретные)
        
        
        //операции: 
        //1. единичек: слева направо и сверху вниз
        //2. послойную (==сложения по модулю)
        //3. сложения двух сигнатур (все слои)
        
        //в сигнатуре есть свойство длины. использовать
        
        
        //!!!!!!!!!!
        //все это побитово
        //колво единиц при прибавлении увеличивается. Не может быть сначала 2 елиницы потом 3
        //смотреть фото
        //сдвиг бита просто во втором варианте единичного. Просто ползет влево
        
        //также техническип роверить что все состояния достижимы при нашем сложении (из состояния в состояния)
        
        //11, 3, 1, 0
        // 1100, 100, 10, 1
        //????
        /*
        Value.TraverseSignature(VertexCount, UniformityDegree, state =>
        { 
            this.Value.SetValue(
                this.GetValue(state.SignatureIndices) + this.GetValue(state.SignatureIndices), 
                state.SignatureIndices);
        });
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

    public static Signature Mod2N(Signature a, int n) =>
        a.Clone().Mod2N(n);
    
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