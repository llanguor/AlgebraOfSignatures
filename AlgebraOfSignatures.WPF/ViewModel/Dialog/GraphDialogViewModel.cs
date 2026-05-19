using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;
using DistributedSystems.LaboratoryWork.Nuget.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.ViewModel.Dialog;

public class GraphDialogViewModel :
    DialogViewModelBase
{
    #region Fields

    private Matrix<bool> _adjacencyMatrix = null!;
    
    #endregion
    
    #region Properties

    public Matrix<bool> AdjacencyMatrix
    {
        get => _adjacencyMatrix;
        set
        {
            _adjacencyMatrix = value;
            RaisePropertyChanged(nameof(AdjacencyMatrix));
        }
    }
    
    #endregion
    
    #region Methods
    
    protected override void HandleParameters(
        DialogAwareParameters parameters)
    {
        AdjacencyMatrix = (parameters[Parameters.AdjacencyMatrix] as Matrix<bool>)!;
    }
    
    #endregion

    #region Nested

    public static class Parameters
    {
        public const string AdjacencyMatrix = nameof(AdjacencyMatrix);
    }

    #endregion
}