using System.Formats.Tar;
using System.Numerics;
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.Extensions;
using DryIoc;

namespace AlgebraOfSignatures.Console;
using System;

class Program
{
    static void Main(string[] args)
    {
        var vertexCount = 6; 
        var uniformityDegree = 3;
        var array = new Matrix<long>(
            vertexCount - uniformityDegree + 1,
            uniformityDegree - 2);
        
        array.SetValue(11, 0);
        array.SetValue(3, 1);
        array.SetValue(1, 2);
        array.SetValue(0, 3); 
        
        var signature = new Core.Signature(
            array, 
            vertexCount, 
            uniformityDegree);
        
        var uniformHyperGraph =
            Core.UniformHyperGraph.FromSignature(
                signature,
                vertexCount,
                uniformityDegree);
        
        var array2 = new Matrix<long>(
            vertexCount - uniformityDegree + 1,
            uniformityDegree - 2);
        
        array2.SetValue(11, 0);
        array2.SetValue(1, 1);
        array2.SetValue(0, 2);
        array2.SetValue(0, 3); 
        
        var signature2 = new Core.Signature(
            array2, 
            vertexCount, 
            uniformityDegree);
        
        var uniformHyperGraph2 =
            Core.UniformHyperGraph.FromSignature(
                signature2,
                vertexCount,
                uniformityDegree);

        uniformHyperGraph.SaveToFile("asd.json");
        Console.ReadKey();
    }

    protected static long GetNextLongValueFromTopToBottom(
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
    
    protected static long GetNextLongValueFromLeftToRight(
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


}