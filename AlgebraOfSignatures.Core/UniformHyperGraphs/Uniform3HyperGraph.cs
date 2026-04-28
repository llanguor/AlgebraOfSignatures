using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Base.Interfaces;
namespace AlgebraOfSignatures.Core.UniformHyperGraphs;

internal sealed class Uniform3HyperGraph(
    IRepresentationConverter converter,
    Signature signature,
    int vertexCount,
    int uniformityDegree) :
    UniformHyperGraph(
        converter,
        signature, 
        vertexCount,
        uniformityDegree)
{
    public override IUniformHyperGraph Intersect(IUniformHyperGraph other)
    {
        throw new NotImplementedException();
    }

    public override IUniformHyperGraph Union(IUniformHyperGraph other)
    {
        throw new NotImplementedException();
    }

    public override IUniformHyperGraph Mod2N(int n)
    {
        throw new NotImplementedException();
    }

    public override IUniformHyperGraph Add(IUniformHyperGraph other)
    {
        throw new NotImplementedException();
    }

    public override IUniformHyperGraph Add(int constant)
    {
        throw new NotImplementedException();
    }

    public override IUniformHyperGraph Multiply(IUniformHyperGraph other)
    {
        throw new NotImplementedException();
    }

    public override IUniformHyperGraph Multiply(int constant)
    {
        throw new NotImplementedException();
    }
}