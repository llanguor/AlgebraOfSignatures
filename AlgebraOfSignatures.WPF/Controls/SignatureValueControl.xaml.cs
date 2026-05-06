using System.Windows;
using System.Windows.Controls;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class SignatureValueControl : 
    UserControl
{
    #region Constructors
    
    public SignatureValueControl()
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
            typeof(SignatureValueControl),
            new PropertyMetadata(null, OnUniformHyperGraphChanged));

    public long SignatureFirstValue
    {
        get => (long)GetValue(SignatureFirstValueProperty);
        set => SetValue(SignatureFirstValueProperty, value);
    }

    public static readonly DependencyProperty SignatureFirstValueProperty =
        DependencyProperty.Register(
            nameof(SignatureFirstValue),
            typeof(long),
            typeof(SignatureValueControl),
            new PropertyMetadata(0L, OnSignatureFirstValueChanged));

    #endregion
    
    
    #region Event Handlers
    
    private static void OnSignatureFirstValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (SignatureValueControl)d;
        control.UniformHyperGraph.Signature.Value.SetValue(e.NewValue, 0); 
        
        var uh = control.UniformHyperGraph;
        control.UniformHyperGraph = null!;
        control.UniformHyperGraph = uh;
    }

    private static void OnUniformHyperGraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (SignatureValueControl)d;
        if (e.NewValue is Core.UniformHyperGraph uh)
            control.SignatureFirstValue = (long)uh.Signature.Value.GetValue(0)!;
    }
    
    #endregion
}