using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace AlgebraOfSignatures.WPF.Converters.DataGridAppearance;

public class BackgroundToChangedValueConverter :
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.MultiValueConverterBase<BackgroundToChangedValueConverter>
{
    public override object? Convert(
        object[] values,
        Type targetType, 
        object? parameter,
        CultureInfo culture)
    {
        if (values[0] is not Brush brush ||
            brush == Application.Current.Resources["InsignificantCellBackground"] as SolidColorBrush)
            return Application.Current.Resources["InsignificantCellForeground"];

        return Application.Current.Resources["DefaultCellForeground"];
    }
}