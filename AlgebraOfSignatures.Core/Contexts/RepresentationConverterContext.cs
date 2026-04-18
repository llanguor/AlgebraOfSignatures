using AlgebraOfSignatures.Core.Base.Interfaces;
using AlgebraOfSignatures.Core.RepresentationConverters;
namespace AlgebraOfSignatures.Core.Contexts;

internal sealed class RepresentationConverterContext :
    IRepresentationConverter
{
    #region Fields
    
    private readonly IRepresentationConverter _converter;
    
    #endregion
    
    
    #region Constructors

    public RepresentationConverterContext(
        int uniformityDegree)
    {
        if(uniformityDegree < 2)
            throw new ArgumentException("Uniformity Degree must be at least 2.");

        _converter = uniformityDegree switch
        {
            2 =>
                new RepresentationConverterUniform2(),
            3 =>
                new RepresentationConverterUniform3(),
            _ =>
                new RepresentationConverterUniformN(),
        };
    }
    
    #endregion
    
    
    #region Methods
    
    public Array ComputeSignatureFromIncidence(Array incidenceMatrix, int uniformityDegree) => 
        _converter!.ComputeSignatureFromIncidence(incidenceMatrix, uniformityDegree);

    public Array ComputeSignatureFromAdjacency(Array adjacencyMatrix) =>
        _converter!.ComputeSignatureFromAdjacency(adjacencyMatrix);

    public Array ComputeIncidenceFromSignature(Array signature, int vertexCount, int uniformityDegree) =>
        _converter!.ComputeIncidenceFromSignature(signature, vertexCount, uniformityDegree);

    public Array ComputeIncidenceFromAdjacency(Array adjacencyMatrix) =>
        _converter!.ComputeIncidenceFromAdjacency(adjacencyMatrix);

    public Array ComputeAdjacencyFromSignature(Array signature, int vertexCount, int uniformityDegree) =>
        _converter!.ComputeAdjacencyFromSignature(signature, vertexCount, uniformityDegree);

    public Array ComputeAdjacencyFromIncidence(Array incidenceMatrix, int uniformityDegree) => 
        _converter!.ComputeAdjacencyFromIncidence(incidenceMatrix, uniformityDegree);
    
    #endregion
}