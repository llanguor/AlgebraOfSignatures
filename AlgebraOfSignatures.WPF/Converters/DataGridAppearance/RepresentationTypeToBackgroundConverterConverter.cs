using System.Globalization;
using System.Windows.Data;
using AlgebraOfSignatures.Core;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;

namespace AlgebraOfSignatures.WPF.Converters.DataGridAppearance;

public class RepresentationTypeToBackgroundConverterConverter :
    MultiValueConverterBase<RepresentationTypeToBackgroundConverterConverter>
{
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not 
            [
                UniformHyperGraph.RepresentationTypes type, 
                IMultiValueConverter first, 
                IMultiValueConverter second, 
                IMultiValueConverter third,
                IMultiValueConverter fourth
            ])
            throw new ArgumentException($"value is not a {nameof(UniformHyperGraph.RepresentationTypes)}");

        return type switch
        {
            UniformHyperGraph.RepresentationTypes.Signature =>
                first,

            UniformHyperGraph.RepresentationTypes.AdjacencyMatrix =>
                second,

            UniformHyperGraph.RepresentationTypes.IncidenceMatrix =>
                third,
            
            UniformHyperGraph.RepresentationTypes.VertexDegreeVector =>
                fourth,

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}