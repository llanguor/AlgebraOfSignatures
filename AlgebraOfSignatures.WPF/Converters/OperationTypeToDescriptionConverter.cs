using System.Globalization;
using AlgebraOfSignatures.Core;

namespace AlgebraOfSignatures.WPF.Converters;

public class OperationTypeToDescriptionConverter : 
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.ValueConverterBase<OperationTypeToDescriptionConverter>
{
    public override object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Signature.OperationsTypes type)
            return string.Empty;

        return type switch
        {
            Signature.OperationsTypes.Union 
                => "Объединение",
                
            Signature.OperationsTypes.Intersection 
                => "Пересечение",
            
            Signature.OperationsTypes.AdditionVertical 
                => "Сложение (вертикальное)",
            
            Signature.OperationsTypes.AdditionVerticalConst
                => "Сложение с константой (вертикальное)",
            
            Signature.OperationsTypes.AdditionHorizontal
                => "Сложение (горизонтальное)",
            
            Signature.OperationsTypes.AdditionHorizontalConst
                => "Сложение с константой (горизонтальное)",
                
            _ => string.Empty
        };
    }
}