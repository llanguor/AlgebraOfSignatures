using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DistributedSystems.LaboratoryWork.Nuget.Command;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class SignatureMatrixControl : 
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
    
    public SignatureMatrixControl()
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
            typeof(SignatureMatrixControl));
    
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(SignatureMatrixControl),
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
            typeof(SignatureMatrixControl),
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
            typeof(SignatureMatrixControl));
    
    #endregion
    
    
    #region Event Handlers
    
    private static void OnInputArrayChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not SignatureMatrixControl control)
            return;
        
        if (e.NewValue is not Array inputArray) 
            return;

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
                    throw new ArgumentException(
                        $"Invalid value at [{i},{j}]. Expected a long, but got '{value}'.");
                }

                InputArray.SetValue(parsedValue, indices);
            }
        }
    }

    private void FillDataGrid()
    {
        if (Indices.Count != InputArray.Rank-2)
            return;
        
        var indices = new int[Indices.Count+2];
        for (var i = 0; i < Indices.Count; ++i)
        {
            indices[i] = Indices[i].Value;
        }
        
        var table = new DataTable();
        
        for (var i = 0; i < RowsCount; ++i)
        {
            table.Columns.Add($"V{i}");
        }
        
        for (var i = 0; i < RowsCount; ++i)
        {
            var row = new object?[RowsCount]; 
            for (var j = 0; j < RowsCount; ++j)
            {
                indices[^2] = i;
                indices[^1] = j;
                row[j] = InputArray.GetValue(indices);
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
}