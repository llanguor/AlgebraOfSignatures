using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AlgebraOfSignatures.Core;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class UniformHyperGraphControl : 
    UserControl
{
    #region Constructors
    
    public UniformHyperGraphControl()
    {
        InitializeComponent();
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
    
    public Array InputArray
    {
        get => (Array)GetValue(InputArrayProperty);
        set => SetValue(InputArrayProperty, value);
    }

    public static readonly DependencyProperty InputArrayProperty =
        DependencyProperty.Register(
            nameof(InputArray),
            typeof(Array),
            typeof(UniformHyperGraphControl));

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

    #endregion
    
    #region Nested


    
    #endregion
}