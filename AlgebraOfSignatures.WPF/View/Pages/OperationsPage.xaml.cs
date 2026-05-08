using System.Windows.Controls;
using AlgebraOfSignatures.WPF.ViewModel.Windows;
using DryIoc;

namespace AlgebraOfSignatures.WPF.View.Pages;

public partial class OperationsPage :
    Page
{
    public OperationsPage()
    {
        InitializeComponent();
        DataContext = App.Container.Resolve<OperationsPageViewModel>();
    }
}