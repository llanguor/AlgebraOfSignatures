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
        UniformityDegree = 2;
        VertexCount = 6;
        SelectedCellValue = 0;
        
        /*
        VertexCount = 6;
        UniformityDegree = 5;
        
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
                */
        
        /*
        VertexCount = 6;
        UniformityDegree = 4;
        
        var array = ArrayExtensions.CreateRankedArray<long>(
            vertexCount - uniformityDegree + 1,
            uniformityDegree - 2);
        array.SetValue(11, 0, 0);
        array.SetValue(3, 0, 1);
        array.SetValue(1, 0,2);

        Signature = new Core.Signature(array, vertexCount, uniformityDegree);
        UniformHyperGraph =
            Core.UniformHyperGraph.FromSignature(
                Signature,
                vertexCount,
                uniformityDegree);
        */
        
        
        VertexCount = 6;
        UniformityDegree = 3;
        
        var array = ArrayExtensions.CreateRankedArray<long>(
            VertexCount - UniformityDegree + 1,
            UniformityDegree - 2);
        array.SetValue(11, 0);
        array.SetValue(3, 1);
        array.SetValue(1, 2);
        array.SetValue(0, 3);
        
        var signature = new Core.Signature(array, VertexCount, UniformityDegree);
        UniformHyperGraph =
            Core.UniformHyperGraph.FromSignature(
                signature,
                VertexCount,
                UniformityDegree);
        
        /*
        VertexCount = 6;
        UniformityDegree = 2;
        Signature = new Core.Signature(11, vertexCount);
        UniformHyperGraph =
            Core.UniformHyperGraph.FromSignature(
                Signature,
                vertexCount,
                uniformityDegree);
        
            */     
        _showGraphCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                param => ShowGraphCommandExecute(),
                _ => UniformHyperGraph.UniformityDegree == 2));
    }

    private void ShowGraphCommandExecute()
    {
        var parameters = DialogAwareParameters.Builder.Create()
            .ForDialogType<GraphDialogViewModel>()
            .AddParameter(
                GraphDialogViewModel.Parameters.AdjacencyMatrix, 
                UniformHyperGraph.AdjacencyMatrix)
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
    
    private int _uniformityDegree;

    public int UniformityDegree
    {
        get => _uniformityDegree;
        set
        {
            if (value < 2)
                throw new ArgumentException("Uniformity Degree must be at least 2", nameof(UniformityDegree));
            
            _uniformityDegree = value;
            RaisePropertyChanged(nameof(UniformityDegree));
        }
    }
    
    private int _vertexCount;

    public int VertexCount
    {
        get => _vertexCount;
        set
        {
            if (value < 1)
                throw new ArgumentException("Vertex count must be at least 2", nameof(UniformityDegree));
            
            _vertexCount = value;
            RaisePropertyChanged(nameof(VertexCount));
        }
    }
    
    private object _selectedCellValue;

    public object SelectedCellValue
    {
        get => _selectedCellValue;
        set
        {
            _selectedCellValue = value;
            RaisePropertyChanged(nameof(SelectedCellValue));
        }
    }
}