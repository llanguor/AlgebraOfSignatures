using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base.Interfaces;
using DryIoc;

namespace AlgebraOfSignatures.Console;
using System;

class Program
{
    private static Container? _container;
    
    static void Main(string[] args)
    {
        
        var value = 16;
        var n = Math.Floor(Math.Log2(value)) + 1;
        Console.WriteLine(n);
        for (var k = 0; k < n; k++)
        {
            var currentBit = (value >> k) & 1;
            Console.WriteLine(currentBit);
        }
        
        /*
        _container = new Container();
        
        _container.RegisterInstance<IHyperGraphRepresentationConverter>(
            new HyperGraphRepresentationConverter());
        
        _container.Register<IHyperGraph>(
            Reuse.Singleton,
            Made.Of(() => HyperGraph.FromSignature(
                Arg.Of<IHyperGraphRepresentationConverter>(), 42)));
        
       
        var uh = _container.Resolve<IHyperGraph>();
        
        Console.WriteLine("Hello, World!");
        _container?.Dispose();
        */
    }
}