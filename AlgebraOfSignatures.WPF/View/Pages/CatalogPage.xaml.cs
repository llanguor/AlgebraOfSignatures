using System.Windows.Controls;
using AlgebraOfSignatures.WPF.ViewModel.Pages;
using DryIoc;

namespace AlgebraOfSignatures.WPF.View.Pages;

public partial class CatalogPage : Page
{
    public CatalogPage()
    {
        InitializeComponent();
        DataContext = App.Container.Resolve<CatalogPageViewModel>();
    }
}