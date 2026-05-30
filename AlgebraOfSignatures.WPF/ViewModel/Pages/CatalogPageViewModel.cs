using System.Collections.ObjectModel;
using AlgebraOfSignatures.Core;
using DistributedSystems.LaboratoryWork.Nuget.Navigation;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.ViewModel.Pages;

public class CatalogPageViewModel :
    PageViewModelBase
{
    public static Array RepresentationTypesValues => 
        Enum.GetValues(typeof(UniformHyperGraph.RepresentationTypes));
    
    public CatalogPageViewModel(NavigationManager navigationManager) : 
        base(navigationManager)
    {
        LoadUniformHyperGraphsArray();
    }

    private void LoadUniformHyperGraphsArray()
    {
        UniformHyperGraphsList.Clear();
        
        UniformHyperGraph =
            Core.UniformHyperGraph.Empty(
                VertexCount,
                UniformityDegree);
        
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        UniformHyperGraphsList.Add(UniformHyperGraph.Clone());
        
        
    }   
    
    public ObservableCollection<Core.UniformHyperGraph> UniformHyperGraphsList { get; }
        = new();
    
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
            LoadUniformHyperGraphsArray(); 
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
            LoadUniformHyperGraphsArray();
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
}