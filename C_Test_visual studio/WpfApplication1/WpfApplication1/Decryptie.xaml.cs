using Microsoft.Win32;
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
        string File_1;
        string File_2;
        string File_3;
        string Public_A;
        string Private_B;

        int hash_decrypted;

        public byte[] desKey;
        public byte[] desIV;


        public Decryptie()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // save name of file in filename var
                string filename = dlg.FileName;
                File1.Content = "File 1 ingeladen";
                File_1 = filename;
            }

        }

        private void File2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // save name of file in filename var
                string filename = dlg.FileName;
                File2.Content = "File 2 ingeladen";
                File_2 = filename;
            }
        }

        private void File3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // save name of file in filename var
                string filename = dlg.FileName;
                File3.Content = "File 3 ingeladen";
                File_3 = filename;
            }
        }

        private void PublicA_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // save name of file in filename var
                string filename = dlg.FileName;
                PublicA.Content = "Public key A ingeladen";
                Public_A = filename;
            }
        }

        private void PrivateB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // save name of file in filename var
                string filename = dlg.FileName;
                PrivateB.Content = "Private key B ingeladen";
                Private_B = filename;
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (File_1 != "" && File_2 != "" && File_3 != "" && Private_B != "" && Public_A != "")
            {
                Byte[] decData;
                DecryptDesKey(File_2);

                decData = File.ReadAllBytes(File_1);

                DecryptFile(decData, @"DecryptedTekst.txt", desKey, desIV);
                hash_decrypted = File.ReadAllText("DecryptedTekst.txt").GetHashCode();

                Decrypted decryptWindow = new Decrypted();
                decryptWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Gelieve eerst de nodige files in te laden.");
            }
        }



        private void DecryptDesKey(string file)
        {
            byte[] encKey;
            byte[] encIV;
            string encKeyIV = "";

            byte[] DecKey;
            byte[] DecIV;

            using (StreamReader sr = File.OpenText("File_2.txt"))
            {
                encKeyIV = sr.ReadToEnd();
            }

            char[] seperator = { ':' };
            string[] split = encKeyIV.Split(seperator);
            encKey = Convert.FromBase64String(split[0]);
            encIV = Convert.FromBase64String(split[1]);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                string Private_Bxml = File.ReadAllText(Private_B);
                rsa.FromXmlString(Private_Bxml);
                DecKey = rsa.Decrypt(encKey, false);
                DecIV = rsa.Decrypt(encIV, false);
            }

            desKey = DecKey;
            desIV = DecIV;
        }


        private void DecryptFile(Byte[] Data, String FileName, byte[] Key, byte[] IV)
        {


            FileStream fStream = File.Open(FileName, FileMode.Create);
            DES DESalg = DES.Create();
            CryptoStream cStream = new CryptoStream(fStream, DESalg.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
            cStream.Write(Data, 0, Data.Length);
            cStream.Close();
            fStream.Close();
        }

        

    }


       
}


