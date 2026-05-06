using System.Formats.Tar;
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
        // 3 1 0
        // - 1 0
        // - - 0
        var arr1 = ArrayExtensions.CreateRankedArray<long>(3, 2);
        arr1.SetValue(6, 0, 0);
        arr1.SetValue(2, 0, 1);
        arr1.SetValue(0, 0, 2);
        arr1.SetValue(1, 1, 1);
        arr1.SetValue(0, 1, 2);
        arr1.SetValue(0, 2, 2);
            
        var arr2 = ArrayExtensions.CreateRankedArray<long>(3, 2);
        arr2.SetValue(5, 0, 0);
        arr2.SetValue(1, 0, 1);
        arr2.SetValue(0, 0, 2);
        arr2.SetValue(0, 1, 1);
        arr2.SetValue(0, 1, 2);
        arr2.SetValue(0, 2, 2);
        
        var signature1 = new Signature(
            arr1,
            6,
            4);
        var signature2 = new Signature(
            arr2,
            6,
            4);
        signature1.Union(signature2);
        
        Console.WriteLine("value");

    }
}