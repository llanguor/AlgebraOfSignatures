using System.Text.Unicode;
using System.Windows;
using System.Windows.Input;
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;
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
    #region Fields 
    
    private readonly IDialogAware _dialogAware; 
    
    private readonly Lazy<ICommand> _showLeftGraphCommand; 
    
    private readonly Lazy<ICommand> _updateLeftGraphCommand; 
    
    private readonly Lazy<ICommand> _showRightGraphCommand; 
    
    private readonly Lazy<ICommand> _showResultGraphCommand; 
    
    private readonly Lazy<ICommand> _updateRightGraphCommand; 
    
    private readonly Lazy<ICommand> _operationPerformCommand; 
    
    #endregion
    
    
    #region Properties
    
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
        Enum.GetValues(typeof(Signature.OperationsTypes));
    
    #endregion

    
    #region Constructors

    public OperationsPageViewModel(NavigationManager navigationManager) :
        base(navigationManager)
    {
        _dialogAware = App.Container.Resolve<IDialogAware>();
        UniformityDegree = 2; 
        VertexCount = 6;
        SelectedCellValueLeftOperand = 0;
        _selectedCellValueValueRightOperand = 0;
        
        
        
        VertexCount = 6; 
        UniformityDegree = 3;
        var array = new Matrix<long>( 
            VertexCount - UniformityDegree + 1,
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
            new RelayCommand(
                _ =>
                {
                    try
                    {
                        OperationPerformCommandExecute(null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            ));
    }
    
    #endregion
    
    
    #region Command methods
    
    private void OperationPerformCommandExecute(object? _)
    {
        Result = SelectedOperationType switch
        {
            Signature.OperationsTypes.Union =>
                LeftOperand | RightOperand,

            Signature.OperationsTypes.Intersection =>
                LeftOperand & RightOperand,

            Signature.OperationsTypes.AdditionHorizontal =>
                UniformHyperGraph.Add(
                    LeftOperand,
                    RightOperand,
                    Signature.AddType.Horizontal),
            
            Signature.OperationsTypes.AdditionVertical =>
                UniformHyperGraph.Add(
                    LeftOperand,
                    RightOperand,
                    Signature.AddType.Vertical),
            
            Signature.OperationsTypes.AdditionHorizontalConst =>
                UniformHyperGraph.Add(
                    LeftOperand,
                    RightOperand.Signature.GetValue(),
                    Signature.AddType.Horizontal),
            
            Signature.OperationsTypes.AdditionVerticalConst =>
                UniformHyperGraph.Add(
                    LeftOperand, 
                    RightOperand.Signature.GetValue(),
                    Signature.AddType.Vertical),

            _ => throw new ArgumentOutOfRangeException()
        };
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

    private void UpdateRightGraphCommandExecute(object? input)
    {
        if (input is not Core.UniformHyperGraph prev)
            prev = RightOperand;

        var uh = GetNewUniformHyperGraph(prev);

        if (SelectedOperationType != Signature.OperationsTypes.AdditionHorizontalConst &&
            SelectedOperationType != Signature.OperationsTypes.AdditionVerticalConst)
        {
            if (uh.UniformityDegree != UniformityDegree)
                UniformityDegree = uh.UniformityDegree;
            if (uh.VertexCount != VertexCount)
                VertexCount = uh.VertexCount;
        }

        RightOperand = null!;
        RightOperand = uh;

    }
    
    private void UpdateLeftGraphCommandExecute(object? input)
    {
        if (input is not Core.UniformHyperGraph prev)
            prev = LeftOperand;
        
        var uh = GetNewUniformHyperGraph(prev);
        
        if (uh.UniformityDegree != UniformityDegree)
            UniformityDegree = uh.UniformityDegree;
        if (uh.VertexCount != VertexCount)
            VertexCount = uh.VertexCount;
        
        LeftOperand = null!;
        LeftOperand = uh;
    }
    
    #endregion

    
    #region Methods

    private void ResetRightUniformHyperGraph()
    {
        if (SelectedOperationType == Signature.OperationsTypes.AdditionHorizontalConst ||
            SelectedOperationType == Signature.OperationsTypes.AdditionVerticalConst)
        {
            RightOperand =
                UniformHyperGraph.Empty(20, 2);
        }
        else
        {
            RightOperand =
                UniformHyperGraph.Empty(VertexCount, UniformityDegree);
        }
    }
    
    private void ResetUniformHyperGraphs()
    {
        LeftOperand =
            UniformHyperGraph.Empty(VertexCount, UniformityDegree);
        Result  =
            UniformHyperGraph.Empty(VertexCount, UniformityDegree);

        ResetRightUniformHyperGraph();
    }

    private UniformHyperGraph GetNewUniformHyperGraph(UniformHyperGraph operand)
    {
        return SelectedRepresentationType switch
        {
            UniformHyperGraph.RepresentationTypes.Signature => 
                Core.UniformHyperGraph.FromSignature( 
                    operand.Signature, 
                    operand.VertexCount, 
                    operand.UniformityDegree),
            
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
    
    #endregion
    
    
    #region Dependency Properties
    
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
            ResetUniformHyperGraphs(); 
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
                    nameof(VertexCount)); 
            
            _vertexCount = value; 
            RaisePropertyChanged(nameof(VertexCount)); 
            ResetUniformHyperGraphs();
        }
    } 
    
    private object _selectedCellValueLeftOperand;

    public object SelectedCellValueLeftOperand
    {
        get => _selectedCellValueLeftOperand; 
        set 
        {
            _selectedCellValueLeftOperand = value; 
            RaisePropertyChanged(nameof(SelectedCellValueLeftOperand));
        }
    } 
    
    private object _selectedCellValueValueRightOperand;

    public object SelectedCellValueRightOperand
    {
        get => _selectedCellValueValueRightOperand;
        set
        {
            _selectedCellValueValueRightOperand = value; 
            RaisePropertyChanged(nameof(SelectedCellValueRightOperand));
        }
    }
    
    private Signature.OperationsTypes _selectedOperationType = 
        Core.Signature.OperationsTypes.Union; 
    
    public Signature.OperationsTypes SelectedOperationType
    {
        get => _selectedOperationType;
        set
        {
            var lastIsConst =
                _selectedOperationType == Signature.OperationsTypes.AdditionHorizontalConst ||
                _selectedOperationType == Signature.OperationsTypes.AdditionVerticalConst;
            var currentIsConst =
                value == Signature.OperationsTypes.AdditionHorizontalConst ||
                value == Signature.OperationsTypes.AdditionVerticalConst;
            
            _selectedOperationType = value; 
            RaisePropertyChanged(nameof(SelectedOperationType));
            
            if (currentIsConst != lastIsConst)
                ResetRightUniformHyperGraph();
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
    
    #endregion
}