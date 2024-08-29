using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShootingGame
{
    public class DrawCanvas : Canvas
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Pen pen = new Pen(Brushes.Black, 10);
            Rect rect = new Rect(10, 10, 400, 200);
            drawingContext.DrawRectangle(Brushes.Red, pen, rect);
        }
    }
}
