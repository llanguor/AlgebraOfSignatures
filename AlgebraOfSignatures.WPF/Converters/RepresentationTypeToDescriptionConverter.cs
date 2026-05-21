using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using AlgebraOfSignatures.Core;

namespace AlgebraOfSignatures.WPF.Converters;

public class RepresentationTypeToDescriptionConverter :
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.ValueConverterBase<RepresentationTypeToDescriptionConverter>
{
    public override object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not UniformHyperGraph.RepresentationTypes type)
            return string.Empty;

        return type switch
        {
            UniformHyperGraph.RepresentationTypes.Signature 
                => "Сигнатура",
            
            UniformHyperGraph.RepresentationTypes.AdjacencyMatrix 
                => "Матрица смежности",
            
            UniformHyperGraph.RepresentationTypes.VertexDegreeVector
                => "Вектор степеней вершин",
            
            _ => string.Empty
        };
    }
}