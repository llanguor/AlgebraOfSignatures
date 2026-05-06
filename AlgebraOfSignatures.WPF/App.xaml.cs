using System.Windows;
using AlgebraOfSignatures.WPF.View.Pages;
using AlgebraOfSignatures.WPF.View.Windows;
using AlgebraOfSignatures.WPF.ViewModel.Pages;
using AlgebraOfSignatures.WPF.ViewModel.Windows;
using DistributedSystems.LaboratoryWork.Nuget.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Navigation;
using DryIoc;

namespace AlgebraOfSignatures.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Fields

   private static readonly Lazy<IContainer> _container;
   
   #endregion

   #region Constructors

   static App()
   {
       _container = new Lazy<IContainer>(() => new Container());
   }

   #endregion

   #region Properties
   public static IContainer Container =>
       _container.Value;

   #endregion

   #region System.Windows.Application overrides

   protected override void OnStartup(
       StartupEventArgs e)
   {
       this.RegisterLogging()
           .RegisterConfiguration()
           .RegisterViews()
           .RegisterViewModels()
           .RegisterNavigationDialogAware()
           .RegisterNavigation();

       Container.Resolve<MainWindow>().Show();
   }

   #endregion

   #region Methods

   private App RegisterLogging()
   {
       // using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
       // ILogger logger = factory.CreateLogger("Program");

       return this;
   }

   private App RegisterConfiguration()
   {
       return this;
   }

   #endregion



   #region Navigations Methods

   private App RegisterNavigation()
   {
       var navigationManager = new NavigationManager(Container);
       Container.RegisterInstance(navigationManager);
       
       return this;
   }

    private App RegisterNavigationDialogAware()
    {
        var navigationManager = new NavigationManagerDialogAware(Container);
        Container.RegisterInstance(navigationManager);
        Container.RegisterMapping<IDialogAware, NavigationManagerDialogAware>();

        return this;
    }

    #endregion
    
   #region Views Methods

    private App RegisterViews()
    {
        return RegisterWindowsViews()
            .RegisterPagesViews()
            .RegisterDialogsViews();
    }
    private App RegisterViewModels()
    {
        return RegisterWindowsViewModels()
            .RegisterPagesViewModels()
            .RegisterDialogsViewModels();
    }

    #endregion

   #region Pages Methods

    private App RegisterPagesViews()
    {
        Container.Register<ConversionPage>(Reuse.Singleton);
        Container.Register<OperationsPage>(Reuse.Singleton);
        return this;
    }

    private App RegisterPagesViewModels()
    {
        Container.Register<ConversionPageViewModel>(Reuse.Singleton);
        Container.Register<OperationsPageViewModel>(Reuse.Singleton);
        return this;
    }

    #endregion

   #region Dialogs Methods

    private App RegisterDialogsViews()
    {
        return this;
    }

    private App RegisterDialogsViewModels()
    {
        return this;
    }

    #endregion

   #region Windows Methods

    private App RegisterWindowsViews()
    {
        Container.Register<MainWindow>(Reuse.Singleton);

        return this;
    }

    private App RegisterWindowsViewModels()
    {
        Container.Register<MainWindowViewModel>(Reuse.Singleton);

        return this;
    }

    #endregion
}