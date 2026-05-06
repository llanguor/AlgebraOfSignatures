using System.Data;
using System.Globalization;
using DistributedSystems.LaboratoryWork.Nuget.Converters.Base;

namespace AlgebraOfSignatures.WPF.Converters;

public class ArrayToDataTableConverter :
    MultiValueConverterBase<ArrayToDataTableConverter>
{
    public override object? Convert(
        object[] values,
        Type targetType, 
        object? parameter,
        CultureInfo culture)
    {
        if (values.Length < 1 || values[0] is not Array array)
        {
            return null; // или Binding.DoNothing
        }
        DataTable data = new DataTable();
        //todo: change
        // var indices = new int [array.Rank];

        var columns = array.GetLength(0);

        for (var i = 0; i < columns; i++)
        {
            data.Columns.Add($"V{i}");
        }
        
        if (array.Rank == 1)
        {
            var row = new object?[columns]; 
            for (var i = 0; i < columns; i++)
            {
                row[i] = array.GetValue(i);
            }
            data.Rows.Add(row);
        }
        
        if (array.Rank == 2)
        {
            for (var i = 0; i < columns; i++)
            {
                var row = new object?[columns];
                for (var j = 0; j < columns; j++)
                {
                    row[j] = array.GetValue(i, j);
                }
                
                data.Rows.Add(row);
            }
        }

        return data.DefaultView;
    }
    
    //в convertback надо проставлять значения вручную с помощью SetValue для конкретно этой таблицы
    //да и непонятно как обратно сделать это частью массива
    //тогда надо создавать Dependency и по индексам туда парсить просто.
    //При изменении значений или индексов обновлять обратно. 
    //лучше не после каждого индекса а после всех изменений. Мб галочку поставить справа
    //будет появляться при каком то незаписанном измененииы
    //не в конвертере а в элементе
}