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

        public MainWindow()
        {
            InitializeComponent();
            TablaLetrehozas();
        }

        void TablaLetrehozas()
        {
            for (int i = 0; i < 8; i++)
            {
                tablaGrid.RowDefinitions.Add(new RowDefinition());
                tablaGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            BrushConverter bc = new BrushConverter();
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
                }
            }
        }

        private void figura_CBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MezoKlikk(object sender, MouseButtonEventArgs e)
        {
            Rectangle mezo = (Rectangle)sender;

            // Pozíció
            string[] pozBetuk = { "A", "B", "C", "D", "E", "F", "G", "H"};
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (sakkTabla[y, x].Equals(mezo))
                        pozicio_Lbl.Content = $"Pozíció: {pozBetuk[x]}{8 - y}";
                }
            }
        }
    }
}
