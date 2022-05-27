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

        // Sakktábla inicializálása
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

        // Ha rákattint egy mezőre
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

            // LÉPÉSEK

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

                // Fel
                for (int y = posY - 1; y >= 0; y--)
                {
                    if (!mezoUres[posX, y])
                        break;
                    LepesMezoSzinezo(posX, y);
                }
                // Le
                for (int y = posY + 1; y < 8; y++)
                {
                    if (!mezoUres[posX, y])
                        break;
                    LepesMezoSzinezo(posX, y);
                }
                // Bal
                for (int x = posX - 1; x >= 0; x--)
                {
                    if (!mezoUres[x, posY])
                        break;
                    LepesMezoSzinezo(x, posY);
                }
                // Jobb
                for (int x = posX + 1; x < 8; x++)
                {
                    if (!mezoUres[x, posY])
                        break;
                    LepesMezoSzinezo(x, posY);
                }
                // Bal-fel
                int tempX = posX;
                for (int y = posY - 1; y >= 0; y--)
                {
                    tempX--;
                    if (tempX < 0)
                        break;
                    if (!mezoUres[tempX, y])
                        break;
                    LepesMezoSzinezo(tempX, y);
                }
                // Jobb-le
                tempX = posX;
                for (int y = posY + 1; y < 8; y++)
                {
                    tempX++;
                    if (tempX >= 8)
                        break;
                    if (!mezoUres[tempX, y])
                        break;
                    LepesMezoSzinezo(tempX, y);
                }
                // Bal-le
                int tempY = posY;
                for (int x = posX - 1; x >= 0; x--)
                {
                    tempY++;
                    if (tempY >= 8)
                        break;
                    if (!mezoUres[x, tempY])
                        break;
                    LepesMezoSzinezo(x, tempY);
                }
                // Jobb-fel
                tempY = posY;
                for (int x = posX + 1; x < 8; x++)
                {
                    tempY--;
                    if (tempY < 0)
                        break;
                    if (!mezoUres[x, tempY])
                        break;
                    LepesMezoSzinezo(x, tempY);
                }
            }

            // Bástya
            if (kijeloltFigIndex == 2)
            {
                FiguraElhelyezo(posX, posY, "bastya");

                // Fel
                for (int y = posY - 1; y >= 0; y--)
                {
                    if (!mezoUres[posX, y])
                        break;
                    LepesMezoSzinezo(posX, y);
                }
                // Le
                for (int y = posY + 1; y < 8; y++)
                {
                    if (!mezoUres[posX, y])
                        break;
                    LepesMezoSzinezo(posX, y);
                }
                // Bal
                for (int x = posX - 1; x >= 0; x--)
                {
                    if (!mezoUres[x, posY])
                        break;
                    LepesMezoSzinezo(x, posY);
                }
                // Jobb
                for (int x = posX + 1; x < 8; x++)
                {
                    if (!mezoUres[x, posY])
                        break;
                    LepesMezoSzinezo(x, posY);
                }
            }

            // Futó
            if (kijeloltFigIndex == 3)
            {
                FiguraElhelyezo(posX, posY, "futo");

                // Bal-fel
                int tempX = posX;
                for (int y = posY - 1; y >= 0; y--)
                {
                    tempX--;
                    if (tempX < 0)
                        break;
                    if (!mezoUres[tempX, y])
                        break;
                    LepesMezoSzinezo(tempX, y);
                }
                // Jobb-le
                tempX = posX;
                for (int y = posY + 1; y < 8; y++)
                {
                    tempX++;
                    if (tempX >= 8)
                        break;
                    if (!mezoUres[tempX, y])
                        break;
                    LepesMezoSzinezo(tempX, y);
                }
                // Bal-le
                int tempY = posY;
                for (int x = posX - 1; x >= 0; x--)
                {
                    tempY++;
                    if (tempY >= 8)
                        break;
                    if (!mezoUres[x, tempY])
                        break;
                    LepesMezoSzinezo(x, tempY);
                }
                // Jobb-fel
                tempY = posY;
                for (int x = posX + 1; x < 8; x++)
                {
                    tempY--;
                    if (tempY < 0)
                        break;
                    if (!mezoUres[x, tempY])
                        break;
                    LepesMezoSzinezo(x, tempY);
                }
            }

            // Huszár
            if (kijeloltFigIndex == 4)
            {
                FiguraElhelyezo(posX, posY, "huszar");

                LepesMezoSzinezo(posX + 1, posY - 2);
                LepesMezoSzinezo(posX + 1, posY + 2);
                LepesMezoSzinezo(posX - 1, posY - 2);
                LepesMezoSzinezo(posX - 1, posY + 2);
                LepesMezoSzinezo(posX + 2, posY - 1);
                LepesMezoSzinezo(posX + 2, posY + 1);
                LepesMezoSzinezo(posX - 2, posY - 1);
                LepesMezoSzinezo(posX - 2, posY + 1);
            }

            // Fehér gyalog
            if (kijeloltFigIndex == 5)
            {
                FiguraElhelyezo(posX, posY, "gyalog_feher");

                // Kezdő dupla lépés
                if (posY == 6)
                {
                    for (int y = posY - 1; y >= posY - 2; y--)
                    {
                        if (!mezoUres[posX, y])
                            break;
                        LepesMezoSzinezo(posX, y);
                    }
                }
                else
                {
                    LepesMezoSzinezo(posX, posY - 1);
                }
            }

            // Fekete gyalog
            if (kijeloltFigIndex == 6)
            {
                FiguraElhelyezo(posX, posY, "gyalog_fekete");

                // Kezdő dupla lépés
                if (posY == 1)
                {
                    for (int y = posY + 1; y <= posY + 2; y++)
                    {
                        if (!mezoUres[posX, y])
                            break;
                        LepesMezoSzinezo(posX, y);
                    }
                }
                else
                {
                    LepesMezoSzinezo(posX, posY + 1);
                }
            }

            FiguraSzamKorlatozo(kijeloltFigIndex);
            figura_CBx.SelectedIndex = -1;
        }

        // Kiválasztott figura elhelyezése a rákattintott mezőre, amennyiben üres a mező
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

        // Figurák számának korlátozása adott érték szerint
        private void FiguraSzamKorlatozo(int index)
        {
            figurakSzama[index]--;
            ComboBoxItem kijelolt = (ComboBoxItem)figura_CBx.SelectedItem;

            if (figurakSzama[index] == 0)
            {
                kijelolt.IsEnabled = false;
            }

            string nev = (string)kijelolt.Content;
            nev = nev.Substring(0, nev.Length - 2) + $"{figurakSzama[index]})";
            kijelolt.Content = nev;
        }

        // Léphető mezők mutatása
        private void LepesMezoSzinezo(int x, int y)
        {
            if (x >= 8 || x < 0 || y >= 8 || y < 0 || !mezoUres[x, y])
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

        // Ha változik a kiválasztott figura, akkor törli a léphető mezőkön lévő jelzéseket
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
