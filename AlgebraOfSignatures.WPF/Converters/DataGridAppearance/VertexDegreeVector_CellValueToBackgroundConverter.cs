using System.Globalization;
using System.Windows;
using System.Windows.Media;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;

namespace AlgebraOfSignatures.WPF.Converters.DataGridAppearance;

public class VertexDegreeVector_CellValueToBackgroundConverter:
    MultiValueConverterBase<VertexDegreeVector_CellValueToBackgroundConverter>
{
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        return Application.Current.Resources["DefaultCellBackground"] as SolidColorBrush;
    }
}