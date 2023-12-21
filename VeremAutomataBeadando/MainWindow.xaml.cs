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
using System.IO;
using System.Data;

namespace VeremAutomataBeadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[,] matrix = new string[12,7];
        public MainWindow()
        {
            InitializeComponent();
            string path = "C:\\Users\\ocbto\\source\\repos\\VeremAutomataBeadando\\VeremAutomataBeadando\\bin\\Debug\\rules.txt";
            StreamReader sr = new StreamReader(path,Encoding.UTF8);
            int n = 0;
            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(';');
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[n, j] = sor[j];
                }
                n++;
            }
            sr.Close();
            DataTable dt = ConvertMatrixToDataTable(matrix);

            matrixGrid.ItemsSource = dt.DefaultView;
        }
        private DataTable ConvertMatrixToDataTable(string[,] matrix)
        {
            DataTable dt = new DataTable();


            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                dt.Columns.Add();
            }


            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row[j] = matrix[i, j];
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
        private void Elemez(object sender, RoutedEventArgs e)
        {
            string input = inputString.Text;
            Automata a = new Automata(input,matrix);
            string output = a.Elemzes();
            kiir.Text = output;
        }
    }
}
