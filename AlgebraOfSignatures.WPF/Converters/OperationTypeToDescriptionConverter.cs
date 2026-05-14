using System.Globalization;
using AlgebraOfSignatures.Core;

namespace AlgebraOfSignatures.WPF.Converters;

public class OperationTypeToDescriptionConverter : 
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.ValueConverterBase<OperationTypeToDescriptionConverter>
{
    public override object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not UniformHyperGraph.OperationsTypes type)
            return string.Empty;

        return type switch
        {
            UniformHyperGraph.OperationsTypes.Union 
                => "Объединение",
                
            UniformHyperGraph.OperationsTypes.Intersection 
                => "Пересечение",
            
            UniformHyperGraph.OperationsTypes.Addition 
                => "Сложение",
            
            UniformHyperGraph.OperationsTypes.AdditionConst 
                => "Сложение с константой",
            
            UniformHyperGraph.OperationsTypes.Multiply 
                => "Умножение",
            
            UniformHyperGraph.OperationsTypes.MultiplyConst
                => "Умножение на константу",
                
            _ => string.Empty
        };
    }
}