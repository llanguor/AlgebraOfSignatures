using System.Windows.Controls;
using AlgebraOfSignatures.WPF.ViewModel.Pages;
using AlgebraOfSignatures.WPF.ViewModel.Windows;
using DryIoc;

namespace AlgebraOfSignatures.WPF.View.Pages;

public partial class ConversionPage : Page
{
    public ConversionPage()
    {
        InitializeComponent();
        DataContext = App.Container.Resolve<ConversionPageViewModel>();
    }
}