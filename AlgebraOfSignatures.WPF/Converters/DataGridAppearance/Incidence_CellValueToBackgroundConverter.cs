using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace AlgebraOfSignatures.WPF.Converters.DataGridAppearance;

public class Incidence_CellValueToBackgroundConverter :
    DistributedSystems.LaboratoryWork.Nuget.Converters.Base.MultiValueConverterBase<Adjacency_CellValueToBackgroundConverter>
{
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        return Application.Current.Resources["DefaultCellBackground"] as SolidColorBrush;
    }
}