using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Dénimeur
{
    /// <summary>
    /// Logique d'interaction pour ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        bool inited = false;
        public ConfigWindow()
        {
            
            InitializeComponent();
            Loaded += ConfigWindow_Loaded;
        }

        private void ConfigWindow_Loaded(object sender, RoutedEventArgs e)
        {
            textBox.Text = Grille.CotéMap.ToString();
            textBox1.Text = Grille.UnitésVisibles.ToString();
            textBox2.Text = Grille.iov.ToString();
            textBox3.Text = Grille.jov.ToString();
            slEcartUnité.Value = Grille.EcartUnités;
            slFormeLosange.Value = Grille.Rapportiso;
            slHauteurGrille.Value = Grille.HauteurMap;
            
        KeyDown += ConfigWindow_KeyDown;
            inited = true;
        }

        private void ConfigWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System)
            {

                Close();

            }
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button2_Click(null, null);
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int result;           
           if (int.TryParse(textBox.Text, out result))
                Grille.ChangeCotéMap(result);
                
            if (int.TryParse(textBox1.Text, out result))

                Grille.ChangeUnitésVisibles(result);

            if (int.TryParse(textBox2.Text, out result))

                Grille.Changeiov(result);

            if (int.TryParse(textBox3.Text, out result))

                Grille.Changejov(result);

        }

        private void slEcartUnité_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Grille.EcartUnités =(int) slEcartUnité.Value;
            MainWindow.thisOne.grille.InvalidateVisual();
               
        }

        private void slHauteurGrille_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (inited == false)
                return;
            
            Grille.HauteurMap = (int) slHauteurGrille.Value;
            MainWindow.thisOne.grille.InitialisationMatrice();
            MainWindow.thisOne.grille.InvalidateVisual();
        }
    }
}
