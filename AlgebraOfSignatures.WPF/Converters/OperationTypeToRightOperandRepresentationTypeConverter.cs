using System.Globalization;
using System.Text;
using System.Text.Unicode;
using System.Windows.Data;
using AlgebraOfSignatures.Core;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;

namespace AlgebraOfSignatures.WPF.Converters;

public class OperationTypeToRightOperandRepresentationTypeConverter : 
    MultiValueConverterBase<OperationTypeToRightOperandRepresentationTypeConverter>
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
        
        if (values[0] is not Signature.OperationsTypes operationType)
            throw new ArgumentException($"First argument must be {typeof(Signature.OperationsTypes)}");

        if (values[1] is not UniformHyperGraph.RepresentationTypes defaultRepresentationType)
            throw new ArgumentException($"Second argument must be {typeof(UniformHyperGraph.RepresentationTypes)}");

        return operationType == Signature.OperationsTypes.AdditionVerticalConst ||
               operationType == Signature.OperationsTypes.AdditionHorizontalConst ? 
                UniformHyperGraph.RepresentationTypes.Signature :
                defaultRepresentationType;
    }
}