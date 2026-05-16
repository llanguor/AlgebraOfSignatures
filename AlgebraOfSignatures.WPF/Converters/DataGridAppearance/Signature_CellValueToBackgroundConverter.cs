using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using AlgebraOfSignatures.WPF.Controls;

namespace AlgebraOfSignatures.WPF.Converters.DataGridAppearance;

public class Signature_CellValueToBackgroundConverter :
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.MultiValueConverterBase<Signature_CellValueToBackgroundConverter>
{
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [ObservableCollection<MatrixControl.IntValue> externalIndices, int i, int j])
            return Application.Current.Resources["DefaultCellBackground"] as SolidColorBrush;
        
        if (i > j ||
            externalIndices.Count != 0 && externalIndices[^1].Value > i)
            return Application.Current.Resources["InsignificantCellBackground"] as SolidColorBrush;
        
        for (var k = 0; k < externalIndices.Count - 1; ++k)
        {
            if (externalIndices[k].Value > externalIndices[k + 1].Value)
                return Application.Current.Resources["InsignificantCellBackground"] as SolidColorBrush;
        }
        
        return Application.Current.Resources["DefaultCellBackground"] as SolidColorBrush;
    }
}