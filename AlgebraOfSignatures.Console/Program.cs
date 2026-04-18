using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.RepresentationConverters;
using DryIoc;

namespace AlgebraOfSignatures.Console;
using System;

class Program
{
    private static Container? _container;
    
    static void Main(string[] args)
    {
        
        var uh1 = UniformHyperGraph.FromSignature(
            new int[] { 57 },
            8,
            2);
        
        var uh2 = UniformHyperGraph.FromSignature(
            new int[] { 101 },
            8,
            2);

        var result = uh1 & uh2;
        
        Console.WriteLine(result);
        
        /*
        _container = new Container();

        _container.RegisterInstance<IRepresentationConverter>(
            new RepresentationConverterUniform2());

        _container.Register<UniformHyperGraph>(
            Reuse.Singleton,
            Made.Of(() => UniformHyperGraph.FromSignature(
                Arg.Of<IRepresentationConverter>(), 459, 12)));

        var uh = _container.Resolve<UniformHyperGraph>();
        var uh2 = _container.Resolve<UniformHyperGraph>();
        var array = uh.AdjacencyMatrix;

        _container?.Dispose();
        */

    }
}