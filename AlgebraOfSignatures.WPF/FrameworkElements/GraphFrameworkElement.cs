using System;
using System.Windows;
using System.Windows.Media;
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;
using AlgebraOfSignatures.WPF.FrameworkElements.Base;

namespace AlgebraOfSignatures.WPF.FrameworkElements
{
    internal class GraphFrameworkElement : CustomFrameworkElementBase
    {
        #region Constants

        private const double NodeRadius     = 26.0;
        private const double SelfLoopRadius = 16.0;

        #endregion

        #region Visual Style

        private static readonly Color BackgroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
        private static readonly Color NodeFillColor   = Color.FromRgb(0x64, 0x82, 0xA8);
        private static readonly Color NodeStrokeColor = Color.FromRgb(0x47, 0x60, 0x7A);
        private static readonly Color EdgeColor       = Color.FromRgb(0x94, 0xA3, 0xB8);
        private static readonly Color LabelColor      = Color.FromRgb(0xFF, 0xFF, 0xFF);
        private static readonly Color ShadowColor     = Color.FromArgb(0x30, 0x47, 0x60, 0x7A);

        private static readonly Typeface LabelTypeface =
            new Typeface(new FontFamily("Consolas, Courier New, monospace"),
                         FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

        #endregion

        #region DependencyProperty

        public Matrix<bool> InputArray
        {
            get => (Matrix<bool>)GetValue(InputArrayProperty);
            set => SetValue(InputArrayProperty, value);
        }

        public static readonly DependencyProperty InputArrayProperty =
            DependencyProperty.Register(
                nameof(InputArray),
                typeof(Matrix<bool>),
                typeof(GraphFrameworkElement),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnInputArrayChanged));

        private static void OnInputArrayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphFrameworkElement self)
                self.InvalidateVisual();
        }

        #endregion

        #region OnRenderSizeChanged

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (ActualWidth > 1 && ActualHeight > 1)
                Dispatcher.BeginInvoke(new Action(DrawWithAxis));
        }

        #endregion

        #region Draw

        protected override void Draw(DrawingContext dc)
        {
            dc.DrawRectangle(new SolidColorBrush(BackgroundColor), null,
                             new Rect(0, 0, Width, Height));

            var matrix = InputArray;
            if (matrix is null) return;

            var n = matrix.Size;
            if (n == 0) return;

            var positions = ComputeLayout(n);

            DrawEdges(dc, n, matrix, positions);
            DrawNodes(dc, n, positions);
        }

        #endregion

        #region Layout

        private Point[] ComputeLayout(int n)
        {
            var positions = new Point[n];

            if (n == 1)
            {
                positions[0] = new Point(Width / 2, Height / 2);
                return positions;
            }

            var padding = NodeRadius * 2.0;
            var cx      = Width  / 2;
            var cy      = Height / 2;
            var rx      = Width  / 2 - padding;
            var ry      = Height / 2 - padding;

            for (var i = 0; i < n; i++)
            {
                var angle   = 2 * Math.PI * i / n - Math.PI / 2;
                positions[i] = new Point(cx + rx * Math.Cos(angle),
                                         cy + ry * Math.Sin(angle));
            }

            return positions;
        }

        #endregion

        #region Edge Drawing

        private void DrawEdges(DrawingContext dc, int n, Matrix<bool> matrix, Point[] positions)
        {
            var edgePen = new Pen(new SolidColorBrush(EdgeColor), 2.5)
            {
                LineJoin     = PenLineJoin.Round,
                StartLineCap = PenLineCap.Round,
                EndLineCap   = PenLineCap.Round
            };

            for (var i = 0; i < n; i++)
            for (var j = 0; j < n; j++)
            {
                if (i == j)
                {
                    if (matrix.GetValue(i, j))
                        DrawSelfLoop(dc, positions[i], edgePen);
                    continue;
                }

                if (i < j && (matrix.GetValue(i, j) || matrix.GetValue(j, i)))
                    DrawEdge(dc, positions[i], positions[j], edgePen);
            }
        }

        private void DrawEdge(DrawingContext dc, Point from, Point to, Pen pen)
        {
            var dir    = to - from;
            var length = dir.Length;
            if (length < 1) return;
            var unit  = dir / length;
            var start = from + unit * NodeRadius;
            var end   = to   - unit * NodeRadius;
            dc.DrawLine(pen, start, end);
        }

        private void DrawSelfLoop(DrawingContext dc, Point node, Pen pen)
        {
            var centre = new Point(node.X, node.Y - NodeRadius - SelfLoopRadius);
            dc.DrawEllipse(null, pen, centre, SelfLoopRadius, SelfLoopRadius);
        }

        #endregion

        #region Node Drawing

        private void DrawNodes(DrawingContext dc, int n, Point[] positions)
        {
            var fillBrush  = new SolidColorBrush(NodeFillColor);
            var strokePen  = new Pen(new SolidColorBrush(NodeStrokeColor), 2.0);
            var shadowPen  = new Pen(new SolidColorBrush(ShadowColor), 6.0);
            var labelBrush = new SolidColorBrush(LabelColor);
            var emSize     = 20.0;

            for (var i = 0; i < n; i++)
            {
                var p = positions[i];

                dc.DrawEllipse(null, shadowPen, p, NodeRadius + 3, NodeRadius + 3);
                dc.DrawEllipse(fillBrush, strokePen, p, NodeRadius, NodeRadius);

                var text = new FormattedText(
                    (i + 1).ToString(),
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    LabelTypeface,
                    emSize,
                    labelBrush,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                dc.DrawText(text, new Point(p.X - text.Width  / 2,
                                            p.Y - text.Height / 2));
            }
        }

        #endregion
    }
}