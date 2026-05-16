using System.Globalization;
using System.Reflection.Metadata;
using AlgebraOfSignatures.Core;
using DistributedSystems.LaboratoryWork.Nuget.Converters;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;

namespace AlgebraOfSignatures.WPF.Converters;

public class RepresentationTypeToArrayConverter :
    MultiValueConverterBase<RepresentationTypeToArrayConverter>
{
    public override object? Convert(
        object[] values,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (values.Length != 2)
        {
            throw new ArgumentException("Invalid count of values!");
        }
        
        if (values[0] is not UniformHyperGraph uh)
            return null;

        if (values[1] is not UniformHyperGraph.RepresentationTypes type)
            return null;

        return type switch
        {
            UniformHyperGraph.RepresentationTypes.Signature =>
                uh.Signature.Value,

            UniformHyperGraph.RepresentationTypes.AdjacencyMatrix =>
                uh.AdjacencyMatrix,

            UniformHyperGraph.RepresentationTypes.IncidenceMatrix =>
                uh.IncidenceMatrix,

            _ => null
        };

    }
}