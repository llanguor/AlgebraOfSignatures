using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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

    public Core.UniformHyperGraph UniformHyperGraph
    {
        get => (Core.UniformHyperGraph)GetValue(UniformHyperGraphProperty);
        set => SetValue(UniformHyperGraphProperty, value);
    }

    public static readonly DependencyProperty UniformHyperGraphProperty =
        DependencyProperty.Register(
            nameof(UniformHyperGraph),
            typeof(Core.UniformHyperGraph),
            typeof(UniformHyperGraphControl),
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
            typeof(UniformHyperGraphControl),
            new PropertyMetadata(0L, OnSignatureFirstValueChanged));

    #endregion
    
    
    #region Event Handlers
    private static void OnSignatureFirstValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (UniformHyperGraphControl)d;
        control.UniformHyperGraph.Signature.Value.SetValue(e.NewValue, 0); 
    }

    private static void OnUniformHyperGraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (UniformHyperGraphControl)d;
        if (e.NewValue is Core.UniformHyperGraph uh)
            control.SignatureFirstValue = (long)uh.Signature.Value.GetValue(0)!;
    }
    
    #endregion
}