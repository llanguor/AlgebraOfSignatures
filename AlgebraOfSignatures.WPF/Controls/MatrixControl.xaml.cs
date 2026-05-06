using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class MatrixControl : UserControl
{
    #region Constructors
    
    public MatrixControl()
    {
        InitializeComponent();
    }
    
    #endregion

    
    #region Dependency Properties
    
    public Core.UniformHyperGraph UniformHyperGraph
    {
        get => (Core.UniformHyperGraph)GetValue(UniformHyperGraphProperty);
        set => SetValue(UniformHyperGraphProperty, value);
    }

    public static readonly DependencyProperty UniformHyperGraphProperty =
        DependencyProperty.Register(
            nameof(UniformHyperGraph),
            typeof(Core.UniformHyperGraph),
            typeof(MatrixControl),
            new PropertyMetadata(null, OnUniformHyperGraphChanged));

    
    #endregion
    
    
    #region Event Handlers
    
    private static void OnUniformHyperGraphChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {

    }
    
    #endregion
}