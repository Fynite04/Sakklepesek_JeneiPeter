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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sakklepesek_JeneiPeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle[,] sakkTabla = new Rectangle[8,8];
        bool[,] mezoUres = new bool[8,8];
        List<int> figurakSzama = new List<int>();
        BrushConverter bc = new BrushConverter();
        List<Rectangle> szinezettMezok = new List<Rectangle>();

        public MainWindow()
        {
            InitializeComponent();
            TablaLetrehozas();
            figurakSzama.Add(1); // 0: kiráy
            figurakSzama.Add(1); // 1: kiráynő
            figurakSzama.Add(2); // 2: bástya           
            figurakSzama.Add(2); // 3: futó
            figurakSzama.Add(2); // 4: huszár
            figurakSzama.Add(8); // 5: fehér gyalog
            figurakSzama.Add(8); // 6: fekete gyalog
        }

        private void TablaLetrehozas()
        {
            for (int i = 0; i < 8; i++)
            {
                tablaGrid.RowDefinitions.Add(new RowDefinition());
                tablaGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Rectangle mezo = new Rectangle();
                    if (y % 2 == 0)
                    {
                        if (x % 2 == 0)
                            mezo.Fill = (Brush)(bc.ConvertFrom("#ebecd0"));
                        else
                            mezo.Fill = (Brush)(bc.ConvertFrom("#779556"));
                    }
                    else
                    {
                        if (x % 2 == 0)
                            mezo.Fill = (Brush)(bc.ConvertFrom("#779556"));
                        else
                            mezo.Fill = (Brush)(bc.ConvertFrom("#ebecd0"));
                    }

                    tablaGrid.Children.Add(mezo);
                    sakkTabla[x, y] = mezo;
                    Grid.SetRow(mezo, x);
                    Grid.SetColumn(mezo, y);
                    mezo.MouseUp += MezoKlikk;
                    mezoUres[x, y] = true;
                }
            }
        }

        private void MezoKlikk(object sender, MouseButtonEventArgs e)
        {
            int kijeloltFigIndex = figura_CBx.SelectedIndex;
            if (kijeloltFigIndex == -1)
                return;

            Rectangle mezo = (Rectangle)sender;
            int posX = 0;
            int posY = 0;

            // Pozíció címke
            string[] pozBetuk = { "A", "B", "C", "D", "E", "F", "G", "H"};
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (sakkTabla[y, x].Equals(mezo))
                    {
                        posX = x;
                        posY = y;
                    }
                }
            }
            pozicio_Lbl.Content = $"Pozíció: {pozBetuk[posX]}{8 - posY}";

            // LÉPÉS

            // Király
            if (kijeloltFigIndex == 0)
            {
                FiguraElhelyezo(posX, posY, "kiraly");

                LepesMezoSzinezo(posX + 1, posY + 1);
                LepesMezoSzinezo(posX + 1, posY);
                LepesMezoSzinezo(posX + 1, posY - 1);
                LepesMezoSzinezo(posX, posY + 1);
                LepesMezoSzinezo(posX, posY - 1);
                LepesMezoSzinezo(posX - 1, posY + 1);
                LepesMezoSzinezo(posX - 1, posY);
                LepesMezoSzinezo(posX - 1, posY - 1);
            }

            // Királynő
            if (kijeloltFigIndex == 1)
            {
                FiguraElhelyezo(posX, posY, "kiralyno");
            }

            // Bástya
            if (kijeloltFigIndex == 2)
            {
                FiguraElhelyezo(posX, posY, "bastya");

                for (int y = 0; y < 8; y++)
                {
                    LepesMezoSzinezo(posX, y);
                }
                for (int x = 0; x < 8; x++)
                {
                    LepesMezoSzinezo(x, posY);
                }
            }

            FiguraSzamKorlatozo(kijeloltFigIndex);
            figura_CBx.SelectedIndex = -1;
        }

        private void FiguraElhelyezo(int x, int y, string kepFajl)
        {
            if (!mezoUres[x, y])
                return;

            Rectangle figura = new Rectangle();
            figura.Height = 50;
            figura.Width = 50;
            var posLeft = 35 + x * 50;
            var posTop = 40 + y * 50;
            foCanvas.Children.Add(figura);
            Canvas.SetLeft(figura, posLeft);
            Canvas.SetTop(figura, posTop);

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri($"Img/{kepFajl}.png", UriKind.Relative));
            figura.Fill = imgBrush;

            mezoUres[x, y] = false;
        }

        private void FiguraSzamKorlatozo(int index)
        {
            figurakSzama[index]--;

            if (figurakSzama[index] == 0)
            {
                ComboBoxItem kijelolt = (ComboBoxItem)figura_CBx.SelectedItem;
                kijelolt.IsEnabled = false;
            }
        }
        private void LepesMezoSzinezo(int x, int y)
        {
            if (!mezoUres[x, y])
                return;

            Rectangle lephetoMezo = new Rectangle();
            lephetoMezo.Width = 50;
            lephetoMezo.Height = 50;
            var posLeft = 35 + x * 50;
            var posTop = 40 + y * 50;
            foCanvas.Children.Add(lephetoMezo);
            Canvas.SetLeft(lephetoMezo, posLeft);
            Canvas.SetTop(lephetoMezo, posTop);

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri("Img/lepes.png", UriKind.Relative));
            lephetoMezo.Fill = imgBrush;

            szinezettMezok.Add(lephetoMezo);
        }

        private void figura_CBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (figura_CBx.SelectedIndex == -1)
                return;

            foreach (Rectangle mezo in szinezettMezok)
            {
                foCanvas.Children.Remove(mezo);
            }
            szinezettMezok.Clear();
        }
    }
}
