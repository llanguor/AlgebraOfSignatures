using System.Windows;
using System.Windows.Input;
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Extensions;
using AlgebraOfSignatures.WPF.ViewModel.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Command;
using DistributedSystems.LaboratoryWork.Nuget.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.Navigation;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;
using DryIoc;

namespace AlgebraOfSignatures.WPF.ViewModel.Pages;

public class OperationsPageViewModel : PageViewModelBase
{
    private readonly IDialogAware _dialogAware; 
    
    private readonly Lazy<ICommand> _showLeftGraphCommand; 
    
    private readonly Lazy<ICommand> _updateLeftGraphCommand; 
    
    private readonly Lazy<ICommand> _showRightGraphCommand; 
    
    private readonly Lazy<ICommand> _showResultGraphCommand; 
    
    private readonly Lazy<ICommand> _updateRightGraphCommand; 
    
    private readonly Lazy<ICommand> _operationPerformCommand; 
    
    public ICommand ShowLeftGraphCommand => 
        _showLeftGraphCommand.Value; 
            
    public ICommand ShowRightGraphCommand => 
        _showRightGraphCommand.Value; 
    
    public ICommand ShowResultGraphCommand => 
        _showResultGraphCommand.Value; 
    
    public ICommand UpdateLeftGraphCommand => 
        _updateLeftGraphCommand.Value;
    
    public ICommand UpdateRightGraphCommand => 
        _updateRightGraphCommand.Value;
    
    public ICommand OperationPerformCommand => 
        _operationPerformCommand.Value;
    
    public static Array RepresentationTypesValues => 
        Enum.GetValues(typeof(UniformHyperGraph.RepresentationTypes));
    
    public static Array OperationTypesValues => 
        Enum.GetValues(typeof(UniformHyperGraph.OperationsTypes));

    public OperationsPageViewModel(NavigationManager navigationManager) :
        base(navigationManager)
    {
        _dialogAware = App.Container.Resolve<IDialogAware>();
        UniformityDegree = 2; 
        VertexCount = 6;
        SelectedCellValueFrom = 0;
        SelectedCellValueTo = 0;
        
        /* VertexCount = 6;
         UniformityDegree = 5;
          var array = ArrayExtensions.CreateRankedArray<long>( vertexCount - uniformityDegree + 1, uniformityDegree - 2); array.SetValue(11, 0, 0, 0); array.SetValue(3, 0, 0, 1); array.SetValue(1, 0,1, 1); array.SetValue(1, 1,1, 1); Signature = new Core.Signature(array, vertexCount, uniformityDegree); UniformHyperGraph = Core.UniformHyperGraph.FromSignature( Signature, vertexCount, uniformityDegree); */ /* VertexCount = 6; UniformityDegree = 4; var array = ArrayExtensions.CreateRankedArray<long>( vertexCount - uniformityDegree + 1, uniformityDegree - 2); array.SetValue(11, 0, 0); array.SetValue(3, 0, 1); array.SetValue(1, 0,2); Signature = new Core.Signature(array, vertexCount, uniformityDegree); UniformHyperGraph = Core.UniformHyperGraph.FromSignature( Signature, vertexCount, uniformityDegree); */ 
            
        /* VertexCount = 6; UniformityDegree = 2; Signature = new Core.Signature(11, vertexCount); UniformHyperGraph = Core.UniformHyperGraph.FromSignature( Signature, vertexCount, uniformityDegree); */

        
        
        
        
        VertexCount = 6; 
        UniformityDegree = 3;
        var array = ArrayExtensions.CreateRankedArray<long>
            ( VertexCount - UniformityDegree + 1,
                UniformityDegree - 2);
        
        array.SetValue(11, 0);
        array.SetValue(3, 1);
        array.SetValue(1, 2);
        array.SetValue(0, 3); 
        
        var leftSignature = new Core.Signature(
            array, 
            VertexCount, 
            UniformityDegree);
        
        LeftOperand =
            Core.UniformHyperGraph.FromSignature(
                leftSignature,
                VertexCount,
                UniformityDegree);
        
        RightOperand = LeftOperand.Clone();

        
        
        
        
        
        _showLeftGraphCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                _ => ShowGraphCommandExecute(LeftOperand), 
                _ => LeftOperand.UniformityDegree == 2));
        
        _showRightGraphCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                _ => ShowGraphCommandExecute(RightOperand), 
                _ => RightOperand.UniformityDegree == 2));
        
        _showResultGraphCommand = new Lazy<ICommand>(() =>
            new RelayCommand(
                _ => ShowGraphCommandExecute(Result), 
                _ => Result.UniformityDegree == 2));
        
        _updateLeftGraphCommand = new Lazy<ICommand>(() => 
            new RelayCommand(UpdateLeftGraphCommandExecute));
        
        _updateRightGraphCommand = new Lazy<ICommand>(() => 
            new RelayCommand(UpdateRightGraphCommandExecute));
            
        _operationPerformCommand= new Lazy<ICommand>(() => 
            new RelayCommand(OperationPerformCommandExecute));
    }

    private void UpdateRightGraphCommandExecute(object? _)
    {
        var uh = GetNewUniformHyperGraph(RightOperand);
        RightOperand = null!;
        RightOperand = uh;
    }
    
    private void UpdateLeftGraphCommandExecute(object? _)
    {
        var uh = GetNewUniformHyperGraph(LeftOperand);
        LeftOperand = null!;
        LeftOperand = uh;
    }

    private void ResetUniformHyperGraph()
    {
        LeftOperand =
            UniformHyperGraph.Empty( VertexCount, UniformityDegree);
        RightOperand =
            UniformHyperGraph.Empty( VertexCount, UniformityDegree);
        Result  =
            UniformHyperGraph.Empty( VertexCount, UniformityDegree);
    }

    private void ShowGraphCommandExecute(UniformHyperGraph uh)
    {
        var parameters =
            DialogAwareParameters.Builder.Create() 
                .ForDialogType<GraphDialogViewModel>()
                .AddParameter(
                    GraphDialogViewModel.Parameters.AdjacencyMatrix, 
                    uh.AdjacencyMatrix) 
                .Build();
        
        _dialogAware.Show(parameters);
    }

    private UniformHyperGraph GetNewUniformHyperGraph(UniformHyperGraph operand)
    {
        return SelectedRepresentationType switch
        {
            UniformHyperGraph.RepresentationTypes.Signature => 
                Core.UniformHyperGraph.FromSignature( 
                    operand.Signature.Value, 
                    VertexCount, 
                    UniformityDegree),
            
            UniformHyperGraph.RepresentationTypes.AdjacencyMatrix =>
                Core.UniformHyperGraph.FromAdjacencyMatrix( 
                    operand.AdjacencyMatrix),
            
            UniformHyperGraph.RepresentationTypes.IncidenceMatrix => 
                Core.UniformHyperGraph.FromAdjacencyMatrix(
                    operand.IncidenceMatrix), 
            
            _ => throw new ArgumentOutOfRangeException( 
                nameof(SelectedRepresentationType))
        };
    }
    
    private void OperationPerformCommandExecute(object? _)
    {
        Result = SelectedOperationType switch
        {
            UniformHyperGraph.OperationsTypes.Union =>
                LeftOperand | RightOperand,

            UniformHyperGraph.OperationsTypes.Intersection =>
                LeftOperand & RightOperand,

            UniformHyperGraph.OperationsTypes.Addition =>
                LeftOperand + RightOperand,

            //todo: const
            UniformHyperGraph.OperationsTypes.AdditionConst =>
                LeftOperand + RightOperand,

            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Core.UniformHyperGraph _leftOperand;

    public Core.UniformHyperGraph LeftOperand
    {
        get => _leftOperand;
        set 
        { 
            _leftOperand = value;
            RaisePropertyChanged(nameof(LeftOperand));
        }
    } 
    
    private Core.UniformHyperGraph _rightOperand;

    public Core.UniformHyperGraph RightOperand
    {
        get => _rightOperand;
        set 
        { 
            _rightOperand = value;
            RaisePropertyChanged(nameof(RightOperand));
        }
    } 
    
    private Core.UniformHyperGraph _result;

    public Core.UniformHyperGraph Result
    {
        get => _result;
        set 
        { 
            _result = value;
            RaisePropertyChanged(nameof(Result));
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
    
    private object _selectedCellValueFrom;

    public object SelectedCellValueFrom
    {
        get => _selectedCellValueFrom; 
        set 
        {
            _selectedCellValueFrom = value; 
            RaisePropertyChanged(nameof(SelectedCellValueFrom));
        }
    } 
    
    private object _selectedCellValueTo;

    public object SelectedCellValueTo
    {
        get => _selectedCellValueTo;
        set
        {
            _selectedCellValueTo = value; 
            RaisePropertyChanged(nameof(SelectedCellValueTo));
        }
    }
    
    private UniformHyperGraph.OperationsTypes _selectedOperationType = 
        Core.UniformHyperGraph.OperationsTypes.Union; 
    
    public UniformHyperGraph.OperationsTypes SelectedOperationType
    {
        get => _selectedOperationType;
        set
        {
            _selectedOperationType = value; 
            RaisePropertyChanged(nameof(SelectedOperationType));
        } 
    }
    
    private UniformHyperGraph.RepresentationTypes _selectedRepresentationType =
        Core.UniformHyperGraph.RepresentationTypes.Signature;

    public UniformHyperGraph.RepresentationTypes SelectedRepresentationType
    {
        get => _selectedRepresentationType;
        set
        {
            _selectedRepresentationType = value; 
            RaisePropertyChanged(nameof(SelectedRepresentationType));
        }
    }
}