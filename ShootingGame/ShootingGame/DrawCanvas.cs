using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    public class DrawCanvas : Canvas
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Uri uri = new Uri("../../../img/Player.png", UriKind.RelativeOrAbsolute);
            BitmapImage image = new BitmapImage(uri);
            Rect rect = new Rect(10, 10, image.Width, image.Height);
            drawingContext.DrawImage(image, rect);
        }
    }
}
