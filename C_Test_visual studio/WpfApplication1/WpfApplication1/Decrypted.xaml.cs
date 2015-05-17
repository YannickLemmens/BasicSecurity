using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Decrypted : Window
    {
        string decryptedBoodschap;


        public Decrypted()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            decryptedBoodschap = File.ReadAllText("DecryptedTekst.txt");
            decryptedText.Text = decryptedBoodschap;

            if (Decryptie.validHash)
            {
                decryptedText.AppendText("\n De Hashes komen overeen");
            }
            else
            {
                decryptedText.AppendText("\n De hashes komen niet overeen");
            }
            

        }
    }
}
