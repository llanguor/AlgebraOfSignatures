using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DistributedSystems.LaboratoryWork.Nuget.Command;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class MatrixControl : 
    UserControl
{
    #region Fields
    
    private readonly Lazy<ICommand> _saveCommand;
    
    private readonly Lazy<ICommand> _loadCommand;
    
    private readonly Lazy<int> _rowsCount;
    
    #endregion
    
    
    #region Properties
    
    public ICommand SaveCommand =>
        _saveCommand.Value;
    
    public ICommand LoadCommand =>
        _loadCommand.Value;
    
    public int RowsCount =>
        _rowsCount.Value;
    
    public ObservableCollection<IntValue> Indices { get; set; } =
        [];
    
    #endregion
    
    
    #region Constructors
    
    public MatrixControl()
    {
        InitializeComponent();
        
        _loadCommand = new Lazy<ICommand>(() =>
            new RelayCommand(param => 
                LoadCommandExecute(Convert.ToBoolean(param))));
        
        _saveCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                param => SaveCommandExecute(Convert.ToBoolean(param)),
                _ => !IsReadOnly));

        _rowsCount = new Lazy<int>(
            () => InputArray.GetLength(0));
    }
    
    #endregion
    
    
    #region Dependency Properties
    
    public object? SelectedCellValue
    {
        get => GetValue(SelectedCellValueProperty);
        set => SetValue(SelectedCellValueProperty, value);
    }

    public static readonly DependencyProperty SelectedCellValueProperty =
        DependencyProperty.Register(
            nameof(SelectedCellValue),
            typeof(object),
            typeof(MatrixControl),
            new PropertyMetadata(null));
    
    public ICommand ShowGraphCommand
    {
        get =>
            (ICommand)GetValue(ShowGraphCommandProperty);

        set =>
            SetValue(ShowGraphCommandProperty, value);
    }

    public static readonly DependencyProperty ShowGraphCommandProperty
        = DependencyProperty.Register(
            nameof(ShowGraphCommand),
            typeof(ICommand),
            typeof(MatrixControl),
            new PropertyMetadata(
                null,
                OnShowGraphCommandChanged));

    public bool IsDrawGraphButtonVisible
    {
        get =>
            (bool)GetValue(IsDrawGraphButtonVisibleProperty);

        set =>
            SetValue(IsDrawGraphButtonVisibleProperty, value);
    }

    public static readonly DependencyProperty IsDrawGraphButtonVisibleProperty
        = DependencyProperty.Register(
            nameof(IsDrawGraphButtonVisible),
            typeof(bool),
            typeof(MatrixControl));

    
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(MatrixControl),
            new PropertyMetadata(false));
    
    public Array InputArray
    {
        get => (Array)GetValue(InputArrayProperty);
        set => SetValue(InputArrayProperty, value);
    }

    public static readonly DependencyProperty InputArrayProperty =
        DependencyProperty.Register(
            nameof(InputArray),
            typeof(Array),
            typeof(MatrixControl),
            new PropertyMetadata(null, OnInputArrayChanged));

    
    public DataTable MatrixDataTable
    {
        get => (DataTable)GetValue(MatrixDataTableProperty);
        set => SetValue(MatrixDataTableProperty, value);
    }

    public static readonly DependencyProperty MatrixDataTableProperty =
        DependencyProperty.Register(
            nameof(MatrixDataTable),
            typeof(DataTable),
            typeof(MatrixControl));
    
    
    public Type? MatrixElementType
    {
        get => (Type)GetValue(MatrixElementTypeProperty);
        set => SetValue(MatrixElementTypeProperty, value);
    }

    public static readonly DependencyProperty MatrixElementTypeProperty =
        DependencyProperty.Register(
            nameof(MatrixElementType),
            typeof(Type),
            typeof(MatrixControl));
    
    #endregion
    
    
    #region Callbacks
    
    
    private static void OnShowGraphCommandChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not MatrixControl control)
            return;
        
        if (e.NewValue is not ICommand drawGraphCommand) 
            return;
        
        control.IsDrawGraphButtonVisible = 
            drawGraphCommand.CanExecute(null) == true;
    }
    
    private static void OnInputArrayChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not MatrixControl control)
            return;
        
        if (e.NewValue is not Array inputArray) 
            return;

        control.MatrixElementType = 
            inputArray.GetType().GetElementType();
        
        control.Indices.Clear();
        for (var i = 0; i < inputArray.Rank-2; ++i)
        {
            control.Indices.Add(
                new IntValue(
                    control.SaveCommand,
                    control.LoadCommand) { Value = 0 });
        }

        control.FillDataGrid();
    }
    
    #endregion
    
    
    #region Methods
    
    private void LoadCommandExecute(bool isMessageBoxShow)
    {
        try
        {
            FillDataGrid();
        }
        catch (ArgumentException e)
        {
            if (isMessageBoxShow)
                MessageBox.Show(e.Message, "Error");
        }
    }

    private void SaveCommandExecute(bool isMessageBoxShow)
    {
        try
        {
            SaveDataGrid();
            if (isMessageBoxShow) 
                MessageBox.Show("Данные сохранены");
        }
        catch (ArgumentException e)
        {
            if (isMessageBoxShow) 
                MessageBox.Show(e.Message, "Error");
        }
    }
    
    private void SaveDataGrid()
    {
        if (Indices.Count != InputArray.Rank-2)
            return;

        var indices = new int[Indices.Count + 2];
        for (var i = 0; i < Indices.Count; ++i)
        {
            indices[i] = Indices[i].Value;
        }
        
        for (var i = 0; i < RowsCount; ++i)
        {
            for (var j = 0; j < RowsCount; ++j)
            {
                //todo: throw if incorrect input
                
                indices[^2] = i;
                indices[^1] = j;
                
                var value = MatrixDataTable.Rows[i][j].ToString();
                
                if (!long.TryParse(value, out var parsedValue))
                {
                    if (MatrixElementType==typeof(bool))
                        throw new ArgumentException(
                            $"Invalid value at [{i},{j}]. Expected a 1 or 0, but got '{value}'.");
                    else
                        throw new ArgumentException(
                            $"Invalid value at [{i},{j}]. Expected long value, but got '{value}'.");
                }
                
                InputArray.SetValue(
                    MatrixElementType == typeof(bool) ? 
                        parsedValue == 1 : 
                        parsedValue, 
                    indices);
            }
        }
    }

    private void FillDataGrid()
    {
        var axisCount = InputArray.Rank == 1 ? 1 : 2;
        if (Indices.Count != InputArray.Rank - axisCount)
            return;
        
        var indices = new int[Indices.Count+axisCount];
        for (var i = 0; i < Indices.Count; ++i)
        {
            indices[i] = Indices[i].Value;
        }
        
        var table = new DataTable();
        
        for (var i = 0; i < RowsCount; ++i)
        {
            table.Columns.Add(
                $"V{i}",
                typeof(long));
        }
        
        for (var i = 0; i < (axisCount == 1 ? 1 : RowsCount); ++i)
        {
            var row = new object?[RowsCount]; 
            for (var j = 0; j < RowsCount; ++j)
            {
                if (axisCount != 1)
                    indices[^2] = i;
                
                indices[^1] = j;
                
                var value = InputArray.GetValue(indices);
                row[j] = value is bool b ? (b ? 1 : 0) : value;
            }
            
            table.Rows.Add(row);
        }
        
        MatrixDataTable = table;
    }
    
    #endregion
    
    
    #region Nested
    
    public class IntValue : 
        INotifyPropertyChanged
    {
        private int _value;
        private readonly ICommand _saveCommand;
        private readonly ICommand _loadCommand;

        public IntValue(ICommand saveCommand, ICommand loadCommand)
        {
            _saveCommand = saveCommand;
            _loadCommand = loadCommand;
        }
        
        public int Value
        {
            get => _value;
            set
            {
                try
                {
                    if (_saveCommand.CanExecute(false))
                        _saveCommand.Execute(false);

                    _value = value;
                    OnPropertyChanged(nameof(Value));
                
                    if (_loadCommand.CanExecute(false))
                        _loadCommand.Execute(false);
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show(e.Message, "Error");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    #endregion

    
    //todo: replace with trigger
    private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        var grid = (DataGrid)sender;

        var cell = grid.SelectedCells.FirstOrDefault();
        if (cell == default)
            return;

        var content = cell.Column.GetCellContent(cell.Item);

        string? value = null;

        if (content is TextBlock tb)
            value = tb.Text;

        SelectedCellValue = value;
    }
}