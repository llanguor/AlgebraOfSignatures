using DistributedSystems.LaboratoryWork.Nuget.Dialog;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.ViewModel.Dialog;

public class GraphDialogViewModel :
    DialogViewModelBase
{
    #region Fields

    private Array _inputArray = null!;
    
    #endregion
    
    #region Properties

    public Array InputArray
    {
        get => _inputArray;
        set
        {
            _inputArray = value;
            RaisePropertyChanged(nameof(InputArray));
        }
    }
    
    #endregion
    
    #region Methods
    
    protected override void HandleParameters(
        DialogAwareParameters parameters)
    {
        InputArray = (parameters[Parameters.InputArray] as Array)!;
    }
    
    #endregion

    #region Nested

    public static class Parameters
    {
        public const string InputArray = nameof(InputArray);
    }

    #endregion
}