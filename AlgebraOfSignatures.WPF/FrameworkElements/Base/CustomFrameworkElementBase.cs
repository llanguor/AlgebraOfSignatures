using System.Windows;
using System.Windows.Media;

namespace AlgebraOfSignatures.WPF.FrameworkElements.Base;

public abstract class CustomFrameworkElementBase : FrameworkElement
{
    #region Fields
    
    private readonly VisualCollection _children;
    private readonly DrawingVisual _drawingVisual;
    protected Random _random;
    private readonly System.Windows.Threading.DispatcherTimer _timer;
    
    #endregion
    
    #region Properties
    
    protected override int VisualChildrenCount => _children.Count;
    protected override Visual GetVisualChild(int index) => _children[index];
    protected double Width => ActualWidth;
    protected double Height => ActualHeight;
    protected double CenterX => Width / 2;
    protected double CenterY => Height / 2;
    protected Point CenterPoint => new Point(CenterX, CenterY);

    protected double MaxSquareSize
    {
        get
        {
           var res =  Math.Min(ActualWidth, ActualHeight);
           return res < 1 ? 1 : res;
        }
    }

    #endregion
    
    #region Constructors
    
    public CustomFrameworkElementBase()
    {
        _random = new Random(Guid.NewGuid().GetHashCode());
        _children = new VisualCollection(this);
        _drawingVisual = new DrawingVisual();
        _children.Add(_drawingVisual);

        _timer = new System.Windows.Threading.DispatcherTimer();
        _timer.Tick += (s, e) => DrawWithAxis();
        
        this.Loaded += (s, e) => DrawWithAxis();
    }
    
    #endregion
    
    #region Methods Draw
    
    protected void DrawWithAxis()
    {
        using var dc = _drawingVisual.RenderOpen();
        
        /*
        Pen axisPen = new Pen(Brushes.Gray, 1);
        dc.DrawLine(axisPen, new Point(0, CenterY), new Point(Width, CenterY));
        dc.DrawLine(axisPen, new Point(CenterX, 0), new Point(CenterX, Height));
        */

        Draw(dc);
    }

    protected abstract void Draw(
        DrawingContext dc);
    
    #endregion
    
    #region Methods Animate

    protected void AnimateStart(TimeSpan interval)
    {
        _timer.Interval = interval;
        _timer.Start();
    }

    protected void AnimateStop()
    {
        _timer.Stop();
    }
    
    #endregion
    
}