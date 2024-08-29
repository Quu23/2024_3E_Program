using System;
using System.Collections.Generic;
using System.Globalization;
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
        DrawingGroup backingStore = new DrawingGroup();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            drawingContext.DrawImage(App.window.player.img, App.window.player.Rect);

            foreach(var bl in App.window.bullets)
            {
                drawingContext.DrawImage(bl.img, bl.Rect);
            }

            //hack:FormattedTextを使うのは非推奨？調べたほうがいい（とりあえず動く）
            drawingContext.DrawText(
                new FormattedText($"bullets Length is #{App.window.bullets.Count}"
                ,CultureInfo.GetCultureInfo("en")
                ,FlowDirection.LeftToRight
                ,new Typeface("Verdana")
                ,36
                , Brushes.Black)
                ,new Point(10, 10));
        }
    }
}
