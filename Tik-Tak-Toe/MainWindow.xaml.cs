using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tik_Tak_Toe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool player = true;
        public bool Player
        {
            get { return player; }
            set { player = value;
                if (player)
                    MenuStPlayer.Icon = 'X';
                else
                    MenuStPlayer.Icon = 'O';
            }
        }
        private bool winMessage = true;
        public bool WinMessage
        {
            get { return winMessage; }
            set { winMessage = value;
                if (winMessage)
                    MenuWinMessage.Icon = '✓';
                else
                    MenuWinMessage.Icon = ' ';
            }
        }
        public int[] playerXsum = new int[9];
        public int[] playerOsum = new int[9];
        public int x = 0;
        public double width;
        public double height;
        public MainWindow()
        {
            InitializeComponent();
            //Menu "Settings"
            MenuWinMessage.Click += (s, e) => WinMessage = !WinMessage;
            MenuStPlayer.Click += (s, e) => Player = !Player;

            //Menu "File"
            MenuNew.Click += (s, e) => Clear();
            MenuExit.Click += (s, e) => Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int total = 0;
            MenuStPlayer.IsEnabled = false;
            if (e.GetPosition(field).Y < field.Height / 3)
            {
                height = field.Height / 3;
                total += 1;
            }
            //File.WriteAllText(@"Position.txt", File.ReadAllText(@"Position.txt") + "Erste Reihe\n");
            else if (e.GetPosition(field).Y < (field.Height / 3) * 2)
            {
                height = field.Height / 3 * 2;
                total += 10;
            }
            //File.WriteAllText(@"Position.txt", File.ReadAllText(@"Position.txt") + "Zweite Reihe\n");
            else if (e.GetPosition(field).Y < field.Height) 
            {
                height = field.Height;
                total += 100;
            }
            //File.WriteAllText(@"Position.txt", File.ReadAllText(@"Position.txt") + "Dritte Reihe\n");

            if (e.GetPosition(field).X < field.Width / 3)
            {
                width = field.Width / 3;
                total *= 1;
            }
            //File.WriteAllText(@"Position.txt", File.ReadAllText(@"Position.txt") + "Erste Linie\n");
            else if (e.GetPosition(field).X < (field.Width / 3) * 2)
            {
                width = field.Width / 3 * 2;
                total *= 2;
            }
            //File.WriteAllText(@"Position.txt", File.ReadAllText(@"Position.txt") + "Zweite Linie\n");
            else if (e.GetPosition(field).X < field.Width)
            {
                width = field.Width;
                total *= 3;
            }
            //File.WriteAllText(@"Position.txt", File.ReadAllText(@"Position.txt") + "Dritte Linie\n");
            if (!(playerOsum.Contains(total) || playerXsum.Contains(total)))
            {
                if (Player)
                    playerXsum[x] = total;
                else
                    playerOsum[x] = total;
                if (playerOsum[x] != 0 && playerXsum[x] != 0)
                    x++;
                DrawPlayer(width, height);
            }
        }
        public void DrawPlayer(double width, double height)
        {
            if (Player)
            {
                myCanva.Children.Add(DrawX(width, width - field.Width / 3, height, height - field.Height / 3));
                myCanva.Children.Add(DrawX(width - field.Width / 3, width, height, height - field.Height / 3));
            }
            else 
                myCanva.Children.Add(DrawO(width, height));
            
            if (WinConditions(Player ? playerXsum : playerOsum))
            {
                if(WinMessage)
                    MessageBox.Show(Player ? "PlayerX hat Gewonnen!" : "PlayerO hat Gewonnen!",
                    "Gewonnen!");
                Clear();
            }
            Player = !Player;
        }
        private void Clear()
        {
            playerXsum = new int[9];
            playerOsum = new int[9];
            x = 0;
            MenuStPlayer.IsEnabled = true;
            myCanva.Children.Clear();
        }
        private bool WinConditions(int[] sum)
        {
            if ((sum.Contains(1) && sum.Contains(10) && sum.Contains(100)) || (sum.Contains(2) && sum.Contains(20) && sum.Contains(200)) || (sum.Contains(3) && sum.Contains(30) && sum.Contains(300)) ||
                (sum.Contains(1) && sum.Contains(2) && sum.Contains(3)) || (sum.Contains(10) && sum.Contains(20) && sum.Contains(30)) || (sum.Contains(100) && sum.Contains(200) && sum.Contains(300)) ||
                (sum.Contains(1) && sum.Contains(20) && sum.Contains(300)) || (sum.Contains(100) && sum.Contains(20) && sum.Contains(3)))
                return true;
            return false;
        }
        public Line DrawX(double X1, double X2, double Y1, double Y2)
        {
            Line playerX = new Line
            {
                X1 = X1,
                X2 = X2,
                Y1 = Y1,
                Y2 = Y2,
                StrokeThickness = 5,
                Stroke = Brushes.Gray
            };
            return playerX;
        }
        public Ellipse DrawO(double width, double height)
        {
            Ellipse playerO = new Ellipse
            {
                Width = field.Width / 3,
                Height = field.Height / 3,
                StrokeThickness = 5,
                Stroke = Brushes.Gray
            };
            Canvas.SetLeft(playerO, width - field.Width / 3);
            Canvas.SetTop(playerO, height - field.Height / 3);
            return playerO;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((Height < 800 || Width < 800))
            {
                if (Height > Width)
                {
                    field.Height = Width - 100;
                    field.Width = Width - 100;
                }
                else
                {
                    field.Height = Height - 100;
                    field.Width = Height - 100;
                }
            }
        }
    }
}
