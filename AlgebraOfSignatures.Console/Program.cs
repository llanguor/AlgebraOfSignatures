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
        
        var uh1 = HyperGraph.FromSignature(
            new HyperGraphRepresentationConverter(),
            new int[] { 57 },
            8,
            2);
        
        var uh2 = HyperGraph.FromSignature(
            new HyperGraphRepresentationConverter(),
            new int[] { 101 },
            8,
            2);

        var result = uh1 & uh2;
        
        Console.WriteLine(result);
        
        /*
        _container = new Container();

        _container.RegisterInstance<IHyperGraphRepresentationConverter>(
            new HyperGraphRepresentationConverter());

        _container.Register<HyperGraph>(
            Reuse.Singleton,
            Made.Of(() => HyperGraph.FromSignature(
                Arg.Of<IHyperGraphRepresentationConverter>(), 459, 12)));

        var uh = _container.Resolve<HyperGraph>();
        var uh2 = _container.Resolve<HyperGraph>();
        var array = uh.AdjacencyMatrix;

        _container?.Dispose();
        */

    }
}