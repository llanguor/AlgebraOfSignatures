using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Mime;
using System.Windows;
using System.Windows.Media;
using AlgebraOfSignatures.WPF.Controls;
using Color = System.Drawing.Color;

namespace AlgebraOfSignatures.WPF.Converters.DataGridAppearance;

public class Adjacency_CellValueToBackgroundConverter :
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.MultiValueConverterBase<Adjacency_CellValueToBackgroundConverter>
{
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [ObservableCollection<MatrixControl.IntValue> externalIndices, int i, int j])
            return Application.Current.Resources["DefaultCellBackground"];

        if (i == j)
            return Application.Current.Resources["InsignificantCellBackground"];

        if (externalIndices.Count == 0)
            return j < i ? 
                    Application.Current.Resources["RepeatCellBackgroundForSquareMatrix"] : 
                    Application.Current.Resources["DefaultCellBackground"];
                
        
        var set = new HashSet<int> { i, j };
        if (externalIndices.Any(
                value => !set.Add(value.Value)))
            return Application.Current.Resources["InsignificantCellBackground"];

        if (i < externalIndices[^1].Value ||
            j < externalIndices[^1].Value) 
            return Application.Current.Resources["RepeatCellBackground"];

        for (var k = 0; k < externalIndices.Count - 1; ++k)
        {
            if (externalIndices[k].Value > externalIndices[k + 1].Value)
                return Application.Current.Resources["RepeatCellBackground"];
        }
        
        if (j < i)
            return Application.Current.Resources["RepeatCellBackgroundForSquareMatrix"];
        
        return Application.Current.Resources["DefaultCellBackground"];
    }
}