using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
namespace Dénimeur
{
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    enum Motifs
    {
        
        dalle3d,
        dalle,
        bombe,
        bomberouge,
        zero,
        un,
        un1,
        deux,
        trois,
        quatre,
        drapeau,
        compt0,
        compt1,
        compt2,
        compt3,
        compt4,
        compt5,
        compt6,
        compt7,
        compt8,
        compt9,
        comptmoins,

    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    class Grille : Canvas
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        static Grille thisOne;
        Unité2D[,,] matrice = new Unité2D[CotéMap, CotéMap, HauteurMap];
        internal bool gameover = false;
        internal static int CotéMap = 10;
        internal static int HauteurMap = 1;
        internal static int nbdebombes = 25;
        internal static int UnitésVisibles = 22;
        internal static int iov = -6;
        internal static int jov = 6;
        internal static int AttributDeHauteur = 10;//44
        internal static int EcartUnités = 0;
        internal static int nbdedrapeaux=0;
        internal static double Rapportiso = 2;
        internal static Dictionary<Motifs, BitmapImage> DicoTuiles = new Dictionary<Motifs, BitmapImage>();
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        static void addImage(Motifs motif, string path)
        {
            var imageFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            var image = new BitmapImage(new Uri(imageFullPath));
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            DicoTuiles.Add(motif, image);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        static Grille()
        {

            addImage(Motifs.dalle3d, @"Images\dalle3d.png");
            addImage(Motifs.dalle, @"Images\dale.png");
            addImage(Motifs.bombe, @"Images\Bombe.png");
            addImage(Motifs.bomberouge, @"Images\Bomberouge.png");
            addImage(Motifs.zero, @"Images\0.png");
            addImage(Motifs.un, @"Images\1.png");
            addImage(Motifs.un1, @"Images\11.png");
            addImage(Motifs.trois, @"Images\3.png");
            addImage(Motifs.deux, @"Images\2.png");
            addImage(Motifs.drapeau, @"Images\drapeau.png");
            addImage(Motifs.quatre, @"Images\4.png");
            addImage(Motifs.compt0, @"Images\c0.png");
            addImage(Motifs.compt1, @"Images\c1.png");
            addImage(Motifs.compt2, @"Images\c2.png");
            addImage(Motifs.compt3, @"Images\c3.png");
            addImage(Motifs.compt4, @"Images\c4.png");
            addImage(Motifs.compt5, @"Images\c5.png");
            addImage(Motifs.compt6, @"Images\c6.png");
            addImage(Motifs.compt7, @"Images\c7.png");
            addImage(Motifs.compt8, @"Images\c8.png");
            addImage(Motifs.compt9, @"Images\c9.png");
            addImage(Motifs.comptmoins, @"Images\cmoins.png");
    

        }

        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal static void ChangeCotéMap(int NouveauCotéMap)
        {
            CotéMap = NouveauCotéMap;
            thisOne.InitialisationMatrice();
            thisOne.InvalidateVisual();
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal static void ChangeUnitésVisibles(int result)
        {
            UnitésVisibles = result;
            thisOne.InitialisationMatrice();
            thisOne.InvalidateVisual();
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal static void Changejov(int result)
        {
            jov = result;
            thisOne.InitialisationMatrice();
            thisOne.InvalidateVisual();
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal static void Changeiov(int result)
        {
            iov = result;
            thisOne.InitialisationMatrice();
            thisOne.InvalidateVisual();
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal void creation_matrice()
        {
            matrice = new Unité2D[CotéMap, CotéMap, HauteurMap];
            for (int k = 0; k < HauteurMap; k++)
            {
                for (int i = 0; i < CotéMap; i++)
                {
                    for (int j = 0; j < CotéMap; j++)
                    {
                        var u = new Unité2D();
                        u.I = i;
                        u.J = j;
                        u.color = Colors.Red;
                        u.image = DicoTuiles[Motifs.dalle3d];
                        matrice[i, j, k] = u;
                    }
                }


            }


        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal void placement_des_bombes()
        {
            // nbdebombes random unités2d


            var r = new Random((int)DateTime.Now.ToBinary());
            int bombesrestantes = nbdebombes;
            while (bombesrestantes > 0)
            {
                int xybombes = r.Next(0, CotéMap * CotéMap - 1);
                int x = xybombes % CotéMap;
                int y = xybombes / CotéMap;

                if (matrice[x, y, 0].bombe == false)
                {
                    matrice[x, y, 0].bombe = true;

                    bombesrestantes--;
                }

            }

            bool moins;
            int dizaine;
            int unité;
            unitédizaine(bombesrestantes, out unité, out dizaine, out moins);
            if (moins)
            {
                
   

                MainWindow.thisOne.bomberestante__.Source = DicoTuiles[Motifs.comptmoins];
               
            }
            else
            {
                MainWindow.thisOne.bomberestante__.Source = DicoTuiles[Motifs.compt0];
            }
        }

        void unitédizaine(int bomberestante, out int unité, out int dizaine, out bool moins)
        {
            if (bomberestante >= 0)

            {
                moins = false;
            }
            else
            {
                moins = true;
                bomberestante = bomberestante * -1;
            }

            dizaine = bomberestante / 10;
            unité = bomberestante % 10;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal void InitialisationMatrice()
        {
            creation_matrice();
            placement_des_bombes();
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int[] placeCentrale = new int[] { 3, 4, 5 };
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal Grille()
        {
            thisOne = this;
            InitialisationMatrice();
            MouseRightButtonDown += Grille_MouseClickRight;
            MouseLeftButtonDown += Grille_MouseClickLeft;

            MainWindow.thisOne.bomberestante__.Source = DicoTuiles[Motifs.comptmoins];
            MainWindow.thisOne.bomberestantedizaine.Source = DicoTuiles[Motifs.compt2];
            MainWindow.thisOne.bomberestanteunité.Source = DicoTuiles[Motifs.compt7];
        }

        Unité2D detectiondelasouris(Point souris)
        {
            
            double hup = ActualHeight / UnitésVisibles;
            double wup = hup * Rapportiso;

            double Ox = -(jov + iov) * wup; // abscisse en pixel
            double Oy = (jov - iov) * hup;
            int CotéMapDeuxFois = CotéMap * 2;
            int k = 0;
            double EcartEffectif = k * ((AttributDeHauteur + EcartUnités) * hup / 49);
            double x = souris.X;
            double y = souris.Y;
            double j = (x * hup + y * wup - Ox * hup - Oy * wup) / (2 * hup * wup);
            double i = (x * hup - y * wup - Ox * hup + Oy * wup) / (2 * hup * wup);

            var msg = string.Format("i={0}, j={1}", i, j);
            Debug.WriteLine(msg);

            int I = (int)Math.Truncate(i);
            int J = (int)Math.Truncate(j);

          //  if ((I == current_i) && (J == current_j))
                //return null;

            current_i = I;
            current_j = J;
            //CODE CYRIL
            I = I + AttributDeHauteur;// / 2;
            J = J + AttributDeHauteur;// / 2;
            //CODE CYRIL
            var u = matrice[current_i, current_j, k];
            Debug.WriteLine(string.Format("", I, J));
            return u;
        }


        private void Grille_MouseClickRight(object sender, MouseButtonEventArgs e)
        {


            if (gameover) return;
            var souris = e.GetPosition(this);
            var u = detectiondelasouris(souris);
            if (u == null) return;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
              
                if(u.découvert==true)
                {
                  //  u.image = DicoTuiles[Motifs.dalle];
                    découverteauatique2(u);
                    InvalidateVisual();
                   
                    //si drapeau limitrophe >= a bombe liitrophe découvrir dalles sans drapeau
                    //si une case découverte a 0 zero 
                }
            }
            if (u.image==DicoTuiles[Motifs.dalle3d])
            {
                u.image= DicoTuiles[Motifs.drapeau];
                nbdedrapeaux++;
                InvalidateVisual();
            }
            else if (u.image==DicoTuiles[Motifs.drapeau])
            {
                u.image = DicoTuiles[Motifs.dalle3d];
                nbdedrapeaux--;
                InvalidateVisual();
            }
        }


        void découverteauatique(Unité2D u)
        {
            var list = new List<Unité2D>();
            int I = u.I;
            int J = u.J;

            if (I < CotéMap - 1)
                list.Add(matrice[I + 1, J, 0]);
            if ((I < CotéMap - 1) && (J < CotéMap - 1))
                list.Add(matrice[I + 1, J + 1, 0]);
            if (J < CotéMap - 1)
                list.Add(matrice[I, J + 1, 0]);
            if (I > 0)
                list.Add(matrice[I - 1, J, 0]);
            if (J > 0)
                list.Add(matrice[I, J - 1, 0]);
            if ((I > 0) && (J > 0))
                list.Add(matrice[I - 1, J - 1, 0]);
            if ((I > 0) && (J < CotéMap - 1))
                list.Add(matrice[I - 1, J + 1, 0]);
            if ((I < CotéMap - 1) && (J > 0))
                list.Add(matrice[I + 1, J - 1, 0]);

            int bombeslimitrohes = 0;
            foreach (var unité2d in list)
            {
                if (unité2d.bombe == true)
                {
                    bombeslimitrohes++;
                }
                
                if (bombeslimitrohes==0)
                {
                    
                }

            }
            if (bombeslimitrohes == 0)
            {
                u.image = DicoTuiles[Motifs.zero];
                u.découvert = true;
                foreach (var unité2d in list)
                {
                    if (unité2d.bombe == true)
                    {
                        bombeslimitrohes++;
                    }
                }

            }
            if (bombeslimitrohes == 1)
            {
                u.image = DicoTuiles[Motifs.un];
                u.découvert = true;
            }
            if (bombeslimitrohes == 2)
            {
                u.image = DicoTuiles[Motifs.deux];
                u.découvert = true;
            }
            if (bombeslimitrohes == 3)
            {
                u.image = DicoTuiles[Motifs.trois];
                u.découvert = true;
            }
            if (bombeslimitrohes >= 4)
            {
                u.image = DicoTuiles[Motifs.quatre];
                u.découvert = true;
            }

            if (bombeslimitrohes!=0) return;
            foreach(var unit in list)
            {
                if ((unit.bombe == false) && (unit.découvert==false))
                {
                    découverteauatique(unit);
                }
            }
        }



        void découverteauatique2(Unité2D u)
        {
            var list = new List<Unité2D>();
            int I = u.I;
            int J = u.J;

            if (I < CotéMap - 1)
                list.Add(matrice[I + 1, J, 0]);
            if ((I < CotéMap - 1) && (J < CotéMap - 1))
                list.Add(matrice[I + 1, J + 1, 0]);
            if (J < CotéMap - 1)
                list.Add(matrice[I, J + 1, 0]);
            if (I > 0)
                list.Add(matrice[I - 1, J, 0]);
            if (J > 0)
                list.Add(matrice[I, J - 1, 0]);
            if ((I > 0) && (J > 0))
                list.Add(matrice[I - 1, J - 1, 0]);
            if ((I > 0) && (J < CotéMap - 1))
                list.Add(matrice[I - 1, J + 1, 0]);
            if ((I < CotéMap - 1) && (J > 0))
                list.Add(matrice[I + 1, J - 1, 0]);
            foreach (var unité2d in list)
            {
                if (unité2d.bombe == true)
                {
                    if (unité2d.image != DicoTuiles[Motifs.drapeau])
               { 
                    gameover = true;
             

                    découvrelesbombes(u);
                    unité2d.image = DicoTuiles[Motifs.bomberouge];
                    SonPerdu();
                    return;
                }


                }
                else                     //   unité2d.image = DicoTuiles[Motifs.un];
                découverteauatique(unité2d);

            }

           
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double hup = ActualHeight / UnitésVisibles;
            double wup = hup * Rapportiso;

            double Ox = -(jov + iov) * wup; // abscisse en pixel
            double Oy = (jov - iov) * hup;

            int CotéMapDeuxFois = CotéMap * 2;
            for (int k = 0; k < HauteurMap; k++)
            {
                double EcartEffectif = k * ((AttributDeHauteur + EcartUnités) * hup / 49);

                for (int i = CotéMap - 1; i >= 0; i--)
                    for (int j = 0; j < CotéMap; j++)
                    {
                        var u = matrice[i, j, k];
                        double x = Ox + (i + j) * wup;
                        double y = Oy + (j - i) * hup;
                        y -= EcartEffectif;

                        if (x > ActualWidth)
                            continue;
                        if (y > ActualHeight + hup)
                            continue;
                        if (x + 2 * wup < 0)
                            continue;
                        if (y + 2 * hup < 0)
                            continue;

                        u.draw(dc, x, y, wup, hup);

                    }
            }
            // ..................................
            //Fabriquation du texte
            //  var tf = new Typeface("Verdanna");
            //  var ft = new FormattedText("F12/F10 Ouvrir/Fermer la fenêtre de configuration", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, 24, Brushes.White);
            //  dc.DrawText(ft, new Point(40, 40));
              var tf = new Typeface("Verdanna");
              //bombesrestantes.ToString();
              int bombesrestantes=nbdebombes-nbdedrapeaux;
              var ft = new FormattedText("Bombes restantes "+bombesrestantes, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, 24, Brushes.Black);
              dc.DrawText(ft, new Point(40, 40));
              
            

        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int current_i = -1;
        int current_j = -1;

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private void Grille_MouseClickLeft(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (gameover) return;
            var souris = e.GetPosition(this);
            var u = detectiondelasouris(souris);
            if (u == null) return;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                if (u.découvert == true)
                {
                    //  u.image = DicoTuiles[Motifs.dalle];
                    découverteauatique2(u);
                    InvalidateVisual();

                    //si drapeau limitrophe >= a bombe liitrophe découvrir dalles sans drapeau
                    //si une case découverte a 0 zero 
                }
            }
                if (u.bombe == false)
                {
                    découverteauatique(u);

                }
                else
                {
                    foreach (var u2d in matrice)
                    {
                        if (u2d.bombe == true)
                        {
                            
                                u2d.image = DicoTuiles[Motifs.bombe];
                                u.image = DicoTuiles[Motifs.bomberouge];
                                // u.image = DicoTuiles[Motifs.bomberouge];

                                //u.image = DicoTuiles[Motifs.bomberouge];


                            }
                        }
                    SonPerdu();
                    gameover = true;
                }
                    InvalidateVisual();
                }
            
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void SonPerdu()
        {
            var error = MessageBeep(beepType.SimpleBeep);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void SonGagné()
        {
            Beep(notes.si, tempo.blanche);
            //Thread.Sleep(100);
            Beep(notes.la, tempo.noire);
            Beep(notes.sol, tempo.blanche);
            Beep(notes.sol, tempo.noire);
            Beep(notes.la, tempo.noire);
            Beep(notes.sol, tempo.noire);
            Beep(notes.la, tempo.noire);
            Beep(notes.si, tempo.blanche);
            Beep(notes.sol, tempo.noire);
            Beep(notes.si, tempo.blanche);
            Beep(notes.la, tempo.noire);
            Beep(notes.sol, tempo.blanche);
            Beep(notes.sol, tempo.noire);
            Beep(notes.la, tempo.noire);
            Beep(notes.si, tempo.blanche);
            Beep(notes.la, tempo.noire);
            Beep(notes.sol, tempo.blanche);
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void découvrelesbombes(Unité2D u)
        {
            
                foreach (var u2d in matrice)
            {
                if (u2d.bombe == true)
                {
                    u2d.image = DicoTuiles[Motifs.bombe];
            
                    
                        u.image = DicoTuiles[Motifs.bomberouge];
                    
                }

                }
            }
            
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool MessageBeep(beepType uType);
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Beep(notes dwFreq, tempo dwDuration);
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    public enum notes : uint
    {
        si=494,
        la=440,
        sol=392
    }
    public enum tempo : uint
    {
        noire = 200,
        croche = 100,
        blanche = 400
    }
    /// <summary>
    /// Enum type that enables intellisense on the private <see cref="Beep"/> method.
    /// </summary>
    /// <remarks>
    /// Used by the public Beep <see cref="Beep"/></remarks>
    public enum beepType : uint
    {
        /// <summary>
        /// A simple windows beep
        /// </summary>            
        SimpleBeep = 0xFFFFFFFF,
        /// <summary>
        /// A standard windows OK beep
        /// </summary>
        OK = 0x00,
        /// <summary>
        /// A standard windows Question beep
        /// </summary>
        Question = 0x20,
        /// <summary>
        /// A standard windows Exclamation beep
        /// </summary>
        Exclamation = 0x30,
        /// <summary>
        /// A standard windows Asterisk beep
        /// </summary>
        Asterisk = 0x40,
    }
}
// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
