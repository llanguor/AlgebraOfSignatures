using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AlgebraOfSignatures.WPF.Packages.MultiValueConverterProxy;

public class MultiValueConverterProxy : Freezable, IMultiValueConverter
{
    public static readonly DependencyProperty ConverterProperty =
        DependencyProperty.Register(
            nameof(Converter),
            typeof(IMultiValueConverter),
            typeof(MultiValueConverterProxy),
            new PropertyMetadata(null));

    public IMultiValueConverter? Converter
    {
        get => (IMultiValueConverter?)GetValue(ConverterProperty);
        set => SetValue(ConverterProperty, value);
    }

    protected override Freezable CreateInstanceCore() => new MultiValueConverterProxy();

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => Converter?.Convert(values, targetType, parameter, culture) ?? Binding.DoNothing;

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => Converter?.ConvertBack(value, targetTypes, parameter, culture) ?? [];
}