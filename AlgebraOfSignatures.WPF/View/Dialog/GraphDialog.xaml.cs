using System.Windows;
using AlgebraOfSignatures.WPF.ViewModel.Dialog;
using DryIoc;

namespace AlgebraOfSignatures.WPF.View.Dialog;

public partial class GraphDialog : Window
{
    public GraphDialog()
    {
        InitializeComponent();
        DataContext = App.Container.Resolve<GraphDialogViewModel>();
    }
}