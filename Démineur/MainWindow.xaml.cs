using Dénimeur.Properties;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
namespace Dénimeur
{
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow thisOne;
        internal Grille grille;
        ConfigWindow w;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public MainWindow()
        {
            InitializeComponent();
            thisOne = this;
            grille = new Grille();
            mainGrid.Children.Insert(0,grille);

            //menu = new MenuPrincipal();
            //Grid1.Children.Add(menu);
            //this.Content = mainGrid;
            Loaded += MainWindow_Loaded;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            MouseWheel += MainWindow_MouseWheel;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var point = Mouse.GetPosition(this);

            var valeur = Grille.UnitésVisibles;                      
            Grille.UnitésVisibles -= e.Delta / 120;

            if (Grille.UnitésVisibles < 10)
                Grille.UnitésVisibles = 10;

            if (Grille.UnitésVisibles > 80)
                Grille.UnitésVisibles = 80;

            if (valeur != Grille.UnitésVisibles)
            {
                Double Coefficient = (double)(Grille.UnitésVisibles - valeur) / valeur;
                Double dx = point.X * Coefficient;
                Double dy = point.Y * Coefficient;

                double hup = ActualHeight / Grille.UnitésVisibles;
                double wup = hup * Math.Tan(Math.PI / 3);

                int duy =(int)Math.Round(dy / hup);
                int dux = (int)Math.Round(dx / wup);

                Grille.jov += duy;
                Grille.jov -= dux;

                Grille.iov -= duy;
                Grille.iov -= dux;

                grille.InvalidateVisual();
                
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool droiteDown = false;
        bool gaucheDown = false;
        bool basDown = false;
        bool hautDown = false;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    hautDown = false;
                    break;
                case Key.Down:
                    basDown = false;
                    break;
                case Key.Left:
                    gaucheDown = false;
                    break;
                case Key.Right:
                    droiteDown = false;
                    break;
            }

        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {

                w = new ConfigWindow();
                w.Show();
            }
            /*      if (e.Key == Key.Down)
                  {
                      Grille.iov = Grille.iov + 1;
                      Grille.jov = Grille.jov - 1;

                      grille.InvalidateVisual();
                  }
                  if(e.Key == Key.Up)
                  {
                      Grille.iov = Grille.iov - 1;
                      Grille.jov = Grille.jov + 1;
                      grille.InvalidateVisual();
                  }
                  if (e.Key == Key.Left)
                  {
                      Grille.iov = Grille.iov - 1;
                      Grille.jov = Grille.jov - 1;
                      grille.InvalidateVisual();
                  }
                  if (e.Key == Key.Right)
                  {
                      Grille.iov = Grille.iov + 1;
                      Grille.jov = Grille.jov + 1;
                      grille.InvalidateVisual();

                  }*/
            switch (e.Key)
            {
                case Key.Up:
                    hautDown = true;
                    break;
                case Key.Down:
                    basDown = true;
                    break;
                case Key.Left:
                    gaucheDown = true;
                    break;
                case Key.Right:
                    droiteDown = true;
                    break;
            }
            if (basDown)
            {
                Grille.iov = Grille.iov + 1;
                Grille.jov = Grille.jov - 1;

                grille.InvalidateVisual();
            }
            if (hautDown)
            {
                Grille.iov = Grille.iov - 1;
                Grille.jov = Grille.jov + 1;
                grille.InvalidateVisual();
            }
            if (gaucheDown)
            {
                Grille.iov = Grille.iov - 1;
                Grille.jov = Grille.jov - 1;
                grille.InvalidateVisual();
            }
            if (droiteDown)
            {
                Grille.iov = Grille.iov + 1;
                Grille.jov = Grille.jov + 1;
                grille.InvalidateVisual();

            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (this.Content == grille) 
            {
                grille.InvalidateVisual();
            }
        }

       
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    class Unité2D
    {
        internal bool bombe = false;
        internal int I;
        internal int J;
        internal bool découvert = false;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        static double ddw;
        static double ddh;
       
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        static Unité2D()
        {
            const double dd = 0;
                
            ddh = dd;
            ddw= ddh*Math.Tan(Math.PI / 3);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal System.Windows.Media.Color color;
        internal BitmapImage image;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal void draw(DrawingContext dc, double x, double y, double dw, double dh)
        {
           dw += ddw;
           dh += ddh;
            x -= ddw;
            y += ddw;


            // image
            if (image != null)
            {
                double h = (dw * 2) * image.Height / image.Width;
                var r = new Rect(x, y - h + dh, dw * 2, h);
                dc.DrawImage(image, r);
               

            }
            else
            {
                // couleur de fond du losange
                var f = new PathFigure(
                    new System.Windows.Point(x, y),
                    new LineSegment[] {
                            new LineSegment(new Point(x+dw, y-dh),false),
                            new LineSegment(new Point(x+2*dw,y),false),
                            new LineSegment(new Point(x+dw,y+dh), false)
                    }, true);
                var g = new PathGeometry(new PathFigure[] { f }, FillRule.Nonzero, null);
                var b = new SolidColorBrush(color);
                dc.DrawGeometry(b, null, g);
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
}
// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
