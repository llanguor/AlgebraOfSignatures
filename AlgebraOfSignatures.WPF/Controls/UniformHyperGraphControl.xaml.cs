using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.WPF.Converters.DataGridAppearance;
using DistributedSystems.LaboratoryWork.Nuget.Command;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class UniformHyperGraphControl : 
    UserControl
{
    
    #region Fields
    
    private readonly Lazy<ICommand> _saveToFileCommand;
    
    private readonly Lazy<ICommand> _loadFromFileCommand;

    #endregion
    
    
    #region Properties
    
    public ICommand SaveToFileCommand =>
        _saveToFileCommand.Value;
    
    public ICommand LoadFromFileCommand =>
        _loadFromFileCommand.Value;

    
    #endregion
    
    
    #region Constructors
    
    public UniformHyperGraphControl()
    {
        InitializeComponent();
                
        _saveToFileCommand = new Lazy<ICommand>(() =>
            new RelayCommand(_ => SaveToFileCommandExecute()));
        
        _loadFromFileCommand = new Lazy<ICommand>(() =>
            new RelayCommand(_ => LoadFromFileCommandExecute()));
    }
    
    #endregion
    
    
    #region Methods

    private void SaveToFileCommandExecute()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter     = "JSON файлы (*.json)|*.json",
            DefaultExt = ".json",
            FileName = "matrix.json"
        };

        if (dialog.ShowDialog() != true)
            return;

        UniformHyperGraph.SaveToFile(dialog.FileName);
    }

    private void LoadFromFileCommandExecute()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter     = "JSON файлы (*.json)|*.json",
            DefaultExt = ".json",
            FileName = "matrix.json"
        };

        if (dialog.ShowDialog() != true)
            return;
        
        UniformHyperGraph = UniformHyperGraph.FromFile(dialog.FileName);
        UpdateGraphCommand.Execute(UniformHyperGraph);
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
            typeof(UniformHyperGraphControl),
            new PropertyMetadata(null));

    public Core.UniformHyperGraph UniformHyperGraph
    {
        get => (Core.UniformHyperGraph)GetValue(UniformHyperGraphProperty);
        set => SetValue(UniformHyperGraphProperty, value);
    }

    public static readonly DependencyProperty UniformHyperGraphProperty =
        DependencyProperty.Register(
            nameof(UniformHyperGraph),
            typeof(Core.UniformHyperGraph),
            typeof(UniformHyperGraphControl));
    
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
            typeof(UniformHyperGraphControl));
    
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
            typeof(UniformHyperGraphControl));
    
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(UniformHyperGraphControl),
            new PropertyMetadata(false));
    
    public UniformHyperGraph.RepresentationTypes RepresentationToVisualize
    {
        get => (UniformHyperGraph.RepresentationTypes)GetValue(RepresentationToVisualizeProperty);
        set => SetValue(RepresentationToVisualizeProperty, value);
    }

    public static readonly DependencyProperty RepresentationToVisualizeProperty =
        DependencyProperty.Register(
            nameof(RepresentationToVisualize),
            typeof(UniformHyperGraph.RepresentationTypes),
            typeof(UniformHyperGraphControl));
    
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
            typeof(UniformHyperGraphControl),
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
            typeof(UniformHyperGraphControl),
            new PropertyMetadata(true));
    
    public bool RequireInputConfirmation
    {
        get => (bool)GetValue(RequireInputConfirmationProperty);
        set => SetValue(RequireInputConfirmationProperty, value);
    }

    public static readonly DependencyProperty RequireInputConfirmationProperty =
        DependencyProperty.Register(
            nameof(RequireInputConfirmation),
            typeof(bool),
            typeof(UniformHyperGraphControl),
            new PropertyMetadata(true));
    
    public bool IsHeadersVisible
    {
        get => (bool)GetValue(IsHeadersVisibleProperty);
        set => SetValue(IsHeadersVisibleProperty, value);
    }

    public static readonly DependencyProperty IsHeadersVisibleProperty =
        DependencyProperty.Register(
            nameof(IsHeadersVisible),
            typeof(bool),
            typeof(UniformHyperGraphControl),
            new PropertyMetadata(true));

    #endregion
}