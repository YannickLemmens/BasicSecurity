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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Encryptie encScherm = new Encryptie();
            encScherm.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("test");
        }

        private void Decrypt(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("decryptie");
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Steganography.Steganography stega = new Steganography.Steganography();
            
            stega.Show();
            
        }

       
    }
}
