using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using AlgebraOfSignatures.Core.Base.Interfaces;
using DistributedSystems.LaboratoryWork.Nuget.Command;
using ArgumentException = System.ArgumentException;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class MatrixControl : 
    UserControl
{
    #region Fields
    
    private readonly Lazy<ICommand> _saveGridCommand;
    
    private readonly Lazy<ICommand> _loadGridCommand;
    
    private readonly Lazy<ICommand> _savePageCommand;
    
    private readonly Lazy<ICommand> _loadPageCommand;
    
    #endregion
    
    
    #region Properties
    
    public ICommand SaveGridCommand =>
        _saveGridCommand.Value;
    
    public ICommand LoadGridCommand =>
        _loadGridCommand.Value;
    
    public ICommand SavePageCommand =>
        _savePageCommand.Value;
    
    public ICommand LoadPageCommand =>
        _loadPageCommand.Value;
    
    public ObservableCollection<IntValue> Indices { get; set; } =
        [];
    
    #endregion
    
    
    #region Constructors
    
    public MatrixControl()
    {
        InitializeComponent();
        
        _saveGridCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                SaveCommandExecute,
                _ => !IsReadOnly));
        
        _loadGridCommand = new Lazy<ICommand>(() =>
            new RelayCommand(LoadCommandExecute));
        
        _savePageCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                _ => SaveDataGrid(),
                _ => !IsReadOnly));
        
        _loadPageCommand = new Lazy<ICommand>(() =>
            new RelayCommand(_ => FillDataGrid()));
    }
    
    #endregion
    
    
    #region Dependency Properties
    
    public ICommand LoadFromFileCommand
    {
        get =>
            (ICommand)GetValue(LoadFromFileCommandProperty);

        set =>
            SetValue(LoadFromFileCommandProperty, value);
    }

    public static readonly DependencyProperty LoadFromFileCommandProperty
        = DependencyProperty.Register(
            nameof(LoadFromFileCommand),
            typeof(ICommand),
            typeof(MatrixControl));
    
    public ICommand SaveToFileCommand
    {
        get => (ICommand)GetValue(SaveToFileCommandProperty);
        set => SetValue(SaveToFileCommandProperty, value);
    }
 
    public static readonly DependencyProperty SaveToFileCommandProperty =
        DependencyProperty.Register(
            nameof(SaveToFileCommand),
            typeof(ICommand),
            typeof(MatrixControl));
    
    public bool IsHeadersVisible
    {
        get => (bool)GetValue(IsHeadersVisibleProperty);
        set => SetValue(IsHeadersVisibleProperty, value);
    }

    public static readonly DependencyProperty IsHeadersVisibleProperty =
        DependencyProperty.Register(
            nameof(IsHeadersVisible),
            typeof(bool),
            typeof(MatrixControl),
            new PropertyMetadata(true));
    
    public string RowHeaderText
    {
        get => (string)GetValue(RowHeaderTextProperty);
        set => SetValue(RowHeaderTextProperty, value);
    }

    public static readonly DependencyProperty RowHeaderTextProperty =
        DependencyProperty.Register(
            nameof(RowHeaderText),
            typeof(string),
            typeof(MatrixControl),
            new PropertyMetadata(""));
    
    public string ColumnHeaderText
    {
        get => (string)GetValue(ColumnHeaderTextProperty);
        set => SetValue(ColumnHeaderTextProperty, value);
    }

    public static readonly DependencyProperty ColumnHeaderTextProperty =
        DependencyProperty.Register(
            nameof(ColumnHeaderText),
            typeof(string),
            typeof(MatrixControl),
            new PropertyMetadata("", OnRowHeaderTextChanged));

    private static void OnRowHeaderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not MatrixControl control)
            return;
    
        var oldTable = control.MatrixDataTable;
        if (oldTable == null || oldTable.Columns.Count == 0)
            return;

        var newTable = new DataTable();

        for (var i = 0; i < oldTable.Columns.Count; i++)
            newTable.Columns.Add($"{e.NewValue}{i}", typeof(string));

        foreach (DataRow oldRow in oldTable.Rows)
        {
            var newRow = newTable.NewRow();
            for (var i = 0; i < oldTable.Columns.Count; i++)
                newRow[i] = oldRow[i];
            newTable.Rows.Add(newRow);
        }

        control.MatrixDataTable = newTable;
    }

    public int RowsCount
    {
        get => (int) GetValue(RowsCountProperty);
        set => SetValue(RowsCountProperty, value);
    }

    public static readonly DependencyProperty RowsCountProperty =
        DependencyProperty.Register(
            nameof(RowsCount),
            typeof(int),
            typeof(MatrixControl));
    
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
    
    public ICommand UpdateGraphCommand
    {
        get =>
            (ICommand)GetValue(UpdateGraphCommandProperty);

        set =>
            SetValue(UpdateGraphCommandProperty, value);
    }

    public static readonly DependencyProperty UpdateGraphCommandProperty
        = DependencyProperty.Register(
            nameof(UpdateGraphCommand),
            typeof(ICommand),
            typeof(MatrixControl));
    
    public bool IsDrawGraphCommandCanExecute
    {
        get =>
            (bool)GetValue(IsDrawGraphCommandCanExecuteProperty);

        set =>
            SetValue(IsDrawGraphCommandCanExecuteProperty, value);
    }

    public static readonly DependencyProperty IsDrawGraphCommandCanExecuteProperty
        = DependencyProperty.Register(
            nameof(IsDrawGraphCommandCanExecute),
            typeof(bool),
            typeof(MatrixControl),
            new PropertyMetadata(true));
    
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
            typeof(MatrixControl),
            new PropertyMetadata(true));

    public bool IsLoadGraphButtonVisible
    {
        get =>
            (bool)GetValue(IsLoadGraphButtonVisibleProperty);

        set =>
            SetValue(IsLoadGraphButtonVisibleProperty, value);
    }

    public static readonly DependencyProperty IsLoadGraphButtonVisibleProperty
        = DependencyProperty.Register(
            nameof(IsLoadGraphButtonVisible),
            typeof(bool),
            typeof(MatrixControl),
            new PropertyMetadata(true));
    
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
    
    public IMatrix InputArray
    {
        get => (IMatrix)GetValue(InputArrayProperty);
        set => SetValue(InputArrayProperty, value);
    }

    public static readonly DependencyProperty InputArrayProperty =
        DependencyProperty.Register(
            nameof(InputArray),
            typeof(IMatrix),
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
    
    
    public IMultiValueConverter BackgroundMultiValueConverter
    {
        get => (IMultiValueConverter)GetValue(BackgroundMultiValueConverterProperty);
        set => SetValue(BackgroundMultiValueConverterProperty, value);
    }

    public static readonly DependencyProperty BackgroundMultiValueConverterProperty =
        DependencyProperty.Register(
            nameof(BackgroundMultiValueConverter),
            typeof(IMultiValueConverter),
            typeof(MatrixControl));
        
    public bool RequireInputConfirmation
    {
        get => (bool)GetValue(RequireInputConfirmationProperty);
        set => SetValue(RequireInputConfirmationProperty, value);
    }

    public static readonly DependencyProperty RequireInputConfirmationProperty =
        DependencyProperty.Register(
            nameof(RequireInputConfirmation),
            typeof(bool),
            typeof(MatrixControl),
            new PropertyMetadata(true));
    
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
        
        control.IsDrawGraphCommandCanExecute = 
            drawGraphCommand.CanExecute(null) == true;
    }
    
    private static void OnInputArrayChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not MatrixControl control)
            return;
        
        if (e.NewValue is not IMatrix inputArray) 
            return;
        
        control.RowsCount =
            inputArray.Size;
        
        control.MatrixElementType = 
            inputArray.ElementType;
        
        if (control.Indices.Count != inputArray.Rank - 2)
        {
            control.Indices.Clear();
            for (var i = 0; i < inputArray.Rank - 2; ++i)
            {
                control.Indices.Add(
                    new IntValue(
                        control.SavePageCommand,
                        control.LoadPageCommand,
                        0));
            }
        }

        control.FillDataGrid();
        
        control.IsDrawGraphCommandCanExecute  = 
            control.ShowGraphCommand.CanExecute(null) == true;
    }
    
    #endregion
    
    
    #region Methods
    
    private void LoadCommandExecute(object? _)
    {
        try
        {
            FillDataGrid();
        }
        catch (ArgumentException e)
        { 
            MessageBox.Show(e.Message, "Error");
        }
    }

    private void SaveCommandExecute(object? _)
    {
        try
        {
            SaveDataGrid();
            UpdateGraphCommand.Execute(null);
        }
        catch (ArgumentException e)
        {
            MessageBox.Show(e.Message, "Error");
        }
    }
    
    private void SaveDataGrid()
    {
        var fullIndices = new int[InputArray.Rank];
        for (var i = 0; i < Indices.Count; ++i)
        {
            fullIndices[i] = Indices[i].Value;
        }
        
        for (var i = 0; i < (fullIndices.Length == 1 ? 1 : RowsCount); ++i)
        {
            for (var j = 0; j < RowsCount; ++j)
            {
                if (fullIndices.Length > 1)
                    fullIndices[^2] = i;
                fullIndices[^1] = j;
                
                var value = MatrixDataTable.Rows[i][j].ToString();

                if (MatrixElementType == typeof(long) &&
                    long.TryParse(value, out var longParsedValue))
                { 
                    InputArray.SetValue(longParsedValue, fullIndices);
                    continue;
                }
                
                if (MatrixElementType == typeof(int) &&
                    int.TryParse(value, out var intParsedValue))
                { 
                    InputArray.SetValue(intParsedValue, fullIndices);
                    continue;
                }
                
                if (MatrixElementType == typeof(bool) &&
                    int.TryParse(value, out var bitParsedValue))
                { 
                    InputArray.SetValue(bitParsedValue == 1, fullIndices);
                    continue;
                }

                if (MatrixElementType != typeof(long) &&
                    MatrixElementType != typeof(int) &&
                    MatrixElementType != typeof(bool))
                    throw new ArgumentException("Invalid matrix element type.");
                else
                    throw new ArgumentException(
                            $"Invalid value at [{i},{j}]. Expected {MatrixElementType} value, but got '{value}'.");
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
                $"{ColumnHeaderText}{i}",
                typeof(string));
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
        SelectedCellValue = 
            MatrixDataTable?.Rows.Count > 0 && 
            MatrixDataTable.Columns.Count > 0
            ? MatrixDataTable.Rows[0][0]
            : 0;
    }
    
    #endregion
    
    
    #region Nested
    
    public class IntValue : 
        INotifyPropertyChanged
    {
        private int _value;
        private readonly ICommand _savePageCommand;
        private readonly ICommand _loadPageCommand;

        public IntValue(
            ICommand savePageCommand, 
            ICommand loadPageCommand,
            int value)
        {
            _savePageCommand = savePageCommand;
            _loadPageCommand = loadPageCommand;
            _value = value;
        }
        
        public int Value
        {
            get => _value;
            set
            {
                try
                {
                    if (_savePageCommand.CanExecute(null))
                        _savePageCommand.Execute(null);

                    _value = value;
                    OnPropertyChanged(nameof(Value));
                
                    if (_loadPageCommand.CanExecute(null))
                        _loadPageCommand.Execute(null);
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

    
    #region Events
    
    //note: can be replaced with trigger
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

    //note: can be replaced with trigger
    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.EditingElement is TextBox tb)
        {
            SelectedCellValue = tb.Text;
        }
        
        if (RequireInputConfirmation)
            return;
        
        if (e.EditAction != DataGridEditAction.Commit)
            return;

        Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            () => SaveGridCommand.Execute(null));
    }
    
    private void FolderButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            btn.ContextMenu!.PlacementTarget = btn;
            btn.ContextMenu.IsOpen = true;
        }
    }
    
    #endregion
    
}