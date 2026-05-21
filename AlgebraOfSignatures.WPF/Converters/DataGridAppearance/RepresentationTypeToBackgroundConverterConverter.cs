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
            ])
            throw new ArgumentException($"value is not a {nameof(UniformHyperGraph.RepresentationTypes)}");

        return type switch
        {
            UniformHyperGraph.RepresentationTypes.Signature =>
                first,

            UniformHyperGraph.RepresentationTypes.AdjacencyMatrix =>
                second,
            
            UniformHyperGraph.RepresentationTypes.VertexDegreeVector =>
                third,

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}