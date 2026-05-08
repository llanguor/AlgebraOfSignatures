using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AlgebraOfSignatures.Core.Extensions;
using AlgebraOfSignatures.WPF.ViewModel.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Command;
using DistributedSystems.LaboratoryWork.Nuget.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Navigation;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;
using DryIoc;

namespace AlgebraOfSignatures.WPF.ViewModel.Pages;

public class ConversionPageViewModel : 
    PageViewModelBase
{
    private readonly IDialogAware _dialogAware;
     
    private readonly Lazy<ICommand> _showGraphCommand;
    
    public ICommand ShowGraphCommand =>
        _showGraphCommand.Value;

    
    public ConversionPageViewModel(
        NavigationManager navigationManager) : 
        base(navigationManager)
    {
        _dialogAware = App.Container.Resolve<IDialogAware>();
        
        _showGraphCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                param => ShowGraphCommandExecute()));
        
        var vertexCount = 6;
        var uniformityDegree = 5;


        var array = ArrayExtensions.CreateRankedArray<long>(
            vertexCount - uniformityDegree + 1,
            uniformityDegree - 2);
        array.SetValue(11, 0, 0, 0);
        array.SetValue(3, 0, 0, 1);
        array.SetValue(1, 0,1, 1);
        array.SetValue(1, 1,1, 1);

        Signature = new Core.Signature(array, vertexCount, uniformityDegree);
        UniformHyperGraph =
            Core.UniformHyperGraph.FromSignature(
                Signature,
                vertexCount,
                uniformityDegree);
        
         /*
         var vertexCount = 6;
         var uniformityDegree = 3;


         var array = ArrayExtensions.CreateRankedArray<long>(
             vertexCount - uniformityDegree + 1,
             uniformityDegree - 2);
         array.SetValue(11, 0);
         array.SetValue(3, 1);
         array.SetValue(1, 2);
         array.SetValue(0, 3);

         Signature = new Core.Signature(array, vertexCount, uniformityDegree);
         UniformHyperGraph =
             Core.UniformHyperGraph.FromSignature(
                 Signature,
                 vertexCount,
                 uniformityDegree);
                 */
                 
    }

    private void ShowGraphCommandExecute()
    {
        var parameters = DialogAwareParameters.Builder.Create()
            .ForDialogType<GraphDialogViewModel>()
            .AddParameter(
                GraphDialogViewModel.Parameters.InputArray, 
                UniformHyperGraph.Signature)
            .Build();

        _dialogAware.Show(parameters);
    }
    
    private Core.UniformHyperGraph _uniformHyperGraph;

    public Core.UniformHyperGraph UniformHyperGraph
    {
        get => _uniformHyperGraph;
        set
        {
            _uniformHyperGraph = value;
            RaisePropertyChanged(nameof(UniformHyperGraph));
        }
    }
    
    private Core.Signature _signature;

    public Core.Signature Signature
    {
        get => _signature;
        set
        {
            _signature = value;
            RaisePropertyChanged(nameof(Signature));
        }
    }
}