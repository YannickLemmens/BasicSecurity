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
        String label;
        String labelDecrypted;
        String hash;
        String hashDecryptie;

        public Decryptie()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File_1 = LaadIn();
            StreamReader sr = new StreamReader(File_1);
            File_1 = sr.ReadToEnd();
            MessageBox.Show(File_1);
            
        }

        private void File2_Click(object sender, RoutedEventArgs e)
        {
            //File_2 = LaadIn();
            //MessageBox.Show(File_2);
            File_2 = LaadIn();
            StreamReader sr = new StreamReader(File_2);
            File_2 = sr.ReadToEnd();
            MessageBox.Show(File_2);
        }

        private void File3_Click(object sender, RoutedEventArgs e)
        {
            //File_3 = LaadIn();
            //MessageBox.Show(File_3);
            File_3 = LaadIn();
            StreamReader sr = new StreamReader(File_3);
            File_3 = sr.ReadToEnd();
            MessageBox.Show(File_3);
        }

        private void PublicA_Click(object sender, RoutedEventArgs e)
        {
            //Public_A = LaadIn();
            //MessageBox.Show(Public_A);

            Public_A = LaadIn();
            StreamReader sr = new StreamReader(Public_A);
            Public_A = sr.ReadToEnd();
            MessageBox.Show(Public_A);
        }

        private void PrivateB_Click(object sender, RoutedEventArgs e)
        {
            //Private_B = LaadIn();
            //MessageBox.Show(Private_B);

            Private_B = LaadIn();
            StreamReader sr = new StreamReader(Private_B);
            Private_B = sr.ReadToEnd();
            MessageBox.Show(Private_B);
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            DecryptFile(File_2, label, Private_B);
            DecryptFile(File_1,labelDecrypted,label);
            labeltekst.Content = labelDecrypted;
            CreateHash(labelDecrypted);
            DecryptFile(File_3, hash, Public_A);
            CreateHash(hash);
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

        public static void CreateHash(string boodschap)
        {
            string source = boodschap ;
            using (MD5 md5Hash = MD5.Create())
            {
                //maken hash
                GetMd5Hash(md5Hash, source);
                
            }
        }

              public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }


}
