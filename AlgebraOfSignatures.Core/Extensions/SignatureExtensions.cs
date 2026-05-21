using AlgebraOfSignatures.Core.Base.Interfaces;

namespace AlgebraOfSignatures.Core.Extensions;

public static class SignatureExtensions
{
    #region Extensions

    //note: can be replaced with iterator
    public static void Traverse(
        this Signature signature,
        Action<MultiDimensionalArrayExtensions.TraverseState> delegateAction)
    {
        signature.Value.Traverse(
            signature.VertexCount,
            signature.UniformityDegree,
            delegateAction);
    }
    
    #endregion
}