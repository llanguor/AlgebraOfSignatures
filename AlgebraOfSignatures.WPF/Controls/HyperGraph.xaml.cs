using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DistributedSystems.LaboratoryWork.Nuget.ViewModel;

namespace AlgebraOfSignatures.WPF.Controls;

public partial class HyperGraph : 
    UserControl
{
    #region Properties


    #endregion
    
    
    #region Constructors
    
    public HyperGraph()
    {
        InitializeComponent();
        DataContext = this;
    }
    
    #endregion
}