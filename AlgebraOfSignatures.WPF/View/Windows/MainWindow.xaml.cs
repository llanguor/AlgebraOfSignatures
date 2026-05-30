using System.Windows;
using System.Windows.Controls;
using AlgebraOfSignatures.WPF.View.Pages;
using AlgebraOfSignatures.WPF.ViewModel.Windows;
using DryIoc;

namespace AlgebraOfSignatures.WPF.View.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Container.Resolve<MainWindowViewModel>();
        Tabs.SelectionChanged += Tabs_SelectionChanged;
    }

    private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Tabs.SelectedItem is not TabItem selectedTab)
            return;
        
        if (Equals(selectedTab.Content, ConversionFrame) && ConversionFrame.Content == null)
            ConversionFrame.Navigate(App.Container.Resolve<ConversionPage>());

        if (Equals(selectedTab.Content, OperationsFrame) && OperationsFrame.Content == null)
            OperationsFrame.Navigate(App.Container.Resolve<OperationsPage>());
        
        if (Equals(selectedTab.Content, CatalogFrame) && CatalogFrame.Content == null)
            CatalogFrame.Navigate(App.Container.Resolve<CatalogPage>());
    }
}