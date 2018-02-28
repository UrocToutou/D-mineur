using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dénimeur
{
    class MenuPrincipal : Canvas
    {//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 
        internal MenuPrincipal()
        {

            var Couleurdefond = new ImageBrush();
            Couleurdefond.ImageSource =
                new System.Windows.Media.Imaging.BitmapImage(new Uri("C:\\DonnéesdeCyril\\Mes Images\\Divers\\Tank.png", UriKind.Relative));

            Background = Couleurdefond;

            SizeChanged += MenuPrincipal_SizeChanged;

        }

        private void MenuPrincipal_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
           
            Width = ActualWidth;
            Height = ActualHeight;
        }
    }
}
