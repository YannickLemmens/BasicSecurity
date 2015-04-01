using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Decryptie.xaml
    /// </summary>
    public partial class Decryptie : Window
    {
        String File_1;
        String File_2;
        String File_3;
        String Public_A;
        String Private_B;
        String outputtest = "C:\\Users\\11301151\\Documents\\CryptoProgramOutput\\File.txt";
        public Decryptie()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File_1 = LaadIn();
            MessageBox.Show(File_1);
            
        }

        private void File2_Click(object sender, RoutedEventArgs e)
        {
            File_2 = LaadIn();
            MessageBox.Show(File_2);
        }

        private void File3_Click(object sender, RoutedEventArgs e)
        {
            File_3 = LaadIn();
            MessageBox.Show(File_3);
        }

        private void PublicA_Click(object sender, RoutedEventArgs e)
        {
            Public_A = LaadIn();
            MessageBox.Show(Public_A);
        }

        private void PrivateB_Click(object sender, RoutedEventArgs e)
        {
            Private_B = LaadIn();
            MessageBox.Show(Private_B);
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(File_1);
            MessageBox.Show(outputtest);
            DecryptFile(File_1, outputtest, "1234512345678976");
           
        }
        private static void DecryptFile(string inputFile, string outputFile, string skey)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.CBC;
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);
 
                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                        
                                    }
                                }
                            }
                        }
                    }
                    MessageBox.Show("Decryptie Gelukt");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
        public String LaadIn() {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            var result = fileDialog.ShowDialog();
            String k = "";
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    k = fileDialog.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    k = "";
                    break;
            }
            return k;
        }
    }
}
