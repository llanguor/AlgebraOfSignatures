using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using AlgebraOfSignatures.Core.Extensions;
using DistributedSystems.LaboratoryWork.Nuget.Navigation;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.ViewModel.Pages;

public class ConversionPageViewModel : PageViewModelBase
{
    public ConversionPageViewModel(
        NavigationManager navigationManager) : 
        base(navigationManager)
    {
        var vertexCount = 6;
        var uniformityDegree = 4;


        var array = ArrayExtensions.CreateRankedArray<long>(
            vertexCount - uniformityDegree + 1,
            uniformityDegree - 2);
        array.SetValue(11, 0, 0);
        array.SetValue(3, 0, 1);
        array.SetValue(1, 0, 2);
        array.SetValue(3, 1, 1);
        array.SetValue(1, 1, 2);
        array.SetValue(0, 2, 2);

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