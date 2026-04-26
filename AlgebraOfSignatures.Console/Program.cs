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
        var converter = new RepresentationConverterUniform3();
        var matrix = converter.CreateRankedArray<bool>(6, 3);
        
        matrix.SetValue(true, 0,1,2);
        matrix.SetValue(true, 0,1,3);
        matrix.SetValue(true, 0,1,4);
        matrix.SetValue(true, 0,1,5);
        matrix.SetValue(true, 0,2,3);
        matrix.SetValue(true, 0,2,4);
        matrix.SetValue(true, 0,3,4);
        
        matrix.SetValue(true, 1,1,2);
        matrix.SetValue(true, 1,1,3);
        matrix.SetValue(true, 1,1,4);
        matrix.SetValue(true, 1,1,5);
        matrix.SetValue(true, 1,2,3);
        matrix.SetValue(true, 1,2,4);
        matrix.SetValue(true, 1,3,4);
        
        matrix.SetValue(true, 2,1,2);
        matrix.SetValue(true, 2,1,3);
        matrix.SetValue(true, 2,1,4);
        matrix.SetValue(true, 2,1,5);
        matrix.SetValue(true, 2,2,3);
        matrix.SetValue(true, 2,2,4);
        matrix.SetValue(true, 2,3,4);
        
        matrix.SetValue(true, 3,1,2);
        matrix.SetValue(true, 3,1,3);
        matrix.SetValue(true, 3,1,4);
        matrix.SetValue(true, 3,1,5);
        matrix.SetValue(true, 3,2,3);
        matrix.SetValue(true, 3,2,4);
        matrix.SetValue(true, 3,3,4);

        var result = converter.ComputeSignatureFromAdjacency(matrix);
        /*var uh1 = UniformHyperGraph.FromSignature(
            new int[] { 57 },
            8,
            2);
        
        var uh2 = UniformHyperGraph.FromSignature(
            new int[] { 101 },
            8,
            2);

        var result = uh1 & uh2;
        */
        
        Console.WriteLine("result");
        
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