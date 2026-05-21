using System.Globalization;
using AlgebraOfSignatures.Core;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;
using Signature = System.Security.Cryptography.Xml.Signature;

namespace AlgebraOfSignatures.WPF.Converters.DataGridDescription;

public class RepresentationTypeToHeaderTextConverter :
    MultiValueConverterBase<RepresentationTypeToHeaderTextConverter>
{
    public enum HeaderType
    {
        RowHeader = 0,
        ColumnHeader = 1
    }
    
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        //RepresentationType, HeaderType => text 
        if (values.Length != 2)
            throw new ArgumentException("Invalid count of values!");
        
        if (values[0] is not UniformHyperGraph.RepresentationTypes representationType)
            throw new ArgumentException($"First argument must be {typeof(UniformHyperGraph.RepresentationTypes)}");
        
        if (values[1] is not HeaderType headerType)
            throw new ArgumentException($"Second argument must be {typeof(HeaderType)}");

        return representationType switch
        {
            UniformHyperGraph.RepresentationTypes.Signature => 
                "",
            
            UniformHyperGraph.RepresentationTypes.AdjacencyMatrix => 
                "V",
            
            UniformHyperGraph.RepresentationTypes.VertexDegreeVector => 
                headerType == HeaderType.RowHeader ? "" : "V",
            
            _ => throw new ArgumentOutOfRangeException()
        };

    }
}