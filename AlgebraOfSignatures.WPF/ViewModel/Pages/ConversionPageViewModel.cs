using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.Core.Extensions;
using AlgebraOfSignatures.WPF.ViewModel.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Command;
using DistributedSystems.LaboratoryWork.Nuget.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Navigation;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;
using DryIoc;
using DryIoc.ImTools;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException; 
namespace AlgebraOfSignatures.WPF.ViewModel.Pages;

public class ConversionPageViewModel :
    PageViewModelBase
{
    private readonly IDialogAware _dialogAware; 
    
    private readonly Lazy<ICommand> _showGraphCommand; 
    
    private readonly Lazy<ICommand> _loadFromFileCommand; 
    
    private readonly Lazy<ICommand> _updateGraphCommand; 
    
    public ICommand ShowGraphCommand => 
        _showGraphCommand.Value; 
    
    public ICommand UpdateGraphCommand => 
        _updateGraphCommand.Value;
    
    public ICommand LoadFromFileCommand => 
        _loadFromFileCommand.Value;
    
    public static Array RepresentationTypesValues => 
        Enum.GetValues(typeof(UniformHyperGraph.RepresentationTypes));
    
    public ConversionPageViewModel(NavigationManager navigationManager) :
        base(navigationManager)
    {
        _dialogAware = App.Container.Resolve<IDialogAware>();
        UniformityDegree = 2; 
        VertexCount = 6;
        SelectedCellValueFrom = 0;
        SelectedCellValueTo = 0;
        
        _showGraphCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                ShowGraphCommandExecute, 
                _ => UniformHyperGraph.UniformityDegree == 2));
        
        _updateGraphCommand = new Lazy<ICommand>(() => 
            new RelayCommand(UpdateGraphCommandExecute));
        
        _loadFromFileCommand = new Lazy<ICommand>(() => 
            new RelayCommand(LoadFromFileCommandExecute));
    }

    private void ResetUniformHyperGraph()
    {
        UniformHyperGraph =
            UniformHyperGraph.Empty( VertexCount, UniformityDegree);
    }

    private void ShowGraphCommandExecute(object? _)
    {
        var parameters =
            DialogAwareParameters.Builder.Create() 
                .ForDialogType<GraphDialogViewModel>()
                .AddParameter(
                    GraphDialogViewModel.Parameters.AdjacencyMatrix, 
                    UniformHyperGraph.AdjacencyMatrix) 
                .Build();
        
        _dialogAware.Show(parameters);
    }

    private void LoadFromFileCommandExecute(object? _)
    {
        MessageBox.Show("LoadFromFile");
        
        var uh = Core.UniformHyperGraph.FromFile("");
        
        UniformHyperGraph = null!;
        UniformityDegree = uh.UniformityDegree;
        VertexCount = uh.VertexCount;
        UniformHyperGraph = uh;
    }

    private void UpdateGraphCommandExecute(object? input)
    {
        if (input is not Core.UniformHyperGraph prev)
            prev = UniformHyperGraph;
        
        try
        {
            var uh = SelectedRepresentationTypeFrom switch
            {
                UniformHyperGraph.RepresentationTypes.Signature =>
                    Core.UniformHyperGraph.FromSignature(
                        prev.Signature.Value,
                        prev.VertexCount,
                        prev.UniformityDegree),

                UniformHyperGraph.RepresentationTypes.AdjacencyMatrix =>
                    Core.UniformHyperGraph.FromAdjacencyMatrix(
                        prev.AdjacencyMatrix),

                UniformHyperGraph.RepresentationTypes.VertexDegreeVector =>
                    Core.UniformHyperGraph.FromVertexDegreeVector(
                        prev.VertexDegreeVector),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(SelectedRepresentationTypeFrom))
            };
            
            UniformHyperGraph = null!;
            UniformHyperGraph = uh;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
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
    
    private int _uniformityDegree = 2;

    public int UniformityDegree
    {
        get => _uniformityDegree;
        set 
        { 
            if (value < 2)
                throw new ArgumentException(
                    "Uniformity Degree must be at least 2", 
                    nameof(UniformityDegree)); 
            
            _uniformityDegree = value; 
            RaisePropertyChanged(nameof(UniformityDegree)); 
            ResetUniformHyperGraph(); 
        }
    } 
    
    private int _vertexCount = 2;

    public int VertexCount
    {
        get => _vertexCount;
        set
        {
            if (value < 1) 
                throw new ArgumentException(
                    "Vertex count must be at least 2",
                    nameof(UniformityDegree)); 
            
            _vertexCount = value; 
            RaisePropertyChanged(nameof(VertexCount)); 
            ResetUniformHyperGraph();
        }
    } 
    
    private object _selectedCellValueFrom = 0;

    public object SelectedCellValueFrom
    {
        get => _selectedCellValueFrom; 
        set 
        {
            _selectedCellValueFrom = value; 
            RaisePropertyChanged(nameof(SelectedCellValueFrom));
        }
    } 
    
    private object _selectedCellValueTo = 0;

    public object SelectedCellValueTo
    {
        get => _selectedCellValueTo;
        set
        {
            _selectedCellValueTo = value; 
            RaisePropertyChanged(nameof(SelectedCellValueTo));
        }
    }
    
    private UniformHyperGraph.RepresentationTypes _selectedRepresentationTypeTo = 
        Core.UniformHyperGraph.RepresentationTypes.AdjacencyMatrix; 
    
    public UniformHyperGraph.RepresentationTypes SelectedRepresentationTypeTo 
    {
        get => _selectedRepresentationTypeTo;
        set
        {
            _selectedRepresentationTypeTo = value; 
            RaisePropertyChanged(nameof(SelectedRepresentationTypeTo));
            
            if (SelectedRepresentationTypeFrom == SelectedRepresentationTypeTo)
            {
                SelectedRepresentationTypeFrom = 
                    SelectedRepresentationTypeTo == UniformHyperGraph.RepresentationTypes.Signature ?
                        UniformHyperGraph.RepresentationTypes.AdjacencyMatrix : 
                        UniformHyperGraph.RepresentationTypes.Signature;
            }
        } 
    }
    
    private UniformHyperGraph.RepresentationTypes _selectedRepresentationTypeFrom =
        Core.UniformHyperGraph.RepresentationTypes.Signature;

    public UniformHyperGraph.RepresentationTypes SelectedRepresentationTypeFrom
    {
        get => _selectedRepresentationTypeFrom;
        set
        {
            _selectedRepresentationTypeFrom = value;
            RaisePropertyChanged(nameof(SelectedRepresentationTypeFrom));
            
            if (SelectedRepresentationTypeFrom == SelectedRepresentationTypeTo)
            {
                SelectedRepresentationTypeTo = 
                    SelectedRepresentationTypeFrom == UniformHyperGraph.RepresentationTypes.Signature ?
                        UniformHyperGraph.RepresentationTypes.AdjacencyMatrix : 
                        UniformHyperGraph.RepresentationTypes.Signature;
            }
        }
    }
}