using System.Globalization;
using System.Text;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;

namespace AlgebraOfSignatures.WPF.Converters;

public class LongToBitsConverter :
    ValueConverterBase<LongToBitsConverter>
{
    public override object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (!long.TryParse(value.ToString(), out var number))
            return null;

        if (number == 0)
            return "0";

        var result = new StringBuilder();
        var started = false;

        for (var i = 63; i >= 0; i--)
        {
            var bit = (number & (1L << i)) != 0;

            if (bit)
                started = true;

            if (started)
                result.Append(bit ? '1' : '0');
        }

        return result.ToString();
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string s)
            return 0L;

        long result = 0;

        foreach (var c in s)
        {
            result <<= 1;
            if (c == '1')
                result |= 1;
        }

        return result;
    }
}