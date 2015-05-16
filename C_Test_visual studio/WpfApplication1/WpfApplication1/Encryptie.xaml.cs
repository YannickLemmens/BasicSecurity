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
    /// Interaction logic for Encryptie.xaml
    /// </summary>
    public partial class Encryptie : Window
    {
        string boodschap;
        string encryptieFilePath;
        int hashBoodschap;

        public Encryptie()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    StreamReader sr = new StreamReader(file);
                    boodschap = sr.ReadToEnd();
                    encryptieTextbox.Text = boodschap;
                    encryptieFilePath = fileDialog.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    encryptieTextbox.Text = "Kies een bestand en klik op OK";
                    break;
            }
        }

        private void Start_Encryptie(object sender, RoutedEventArgs e)
        {
            byte[] encData;

            DES des = DES.Create();

            encData = File.ReadAllBytes(encryptieFilePath);
            EncryptFile(encData, @"File_1.txt", des.Key, des.IV);

            string symmetricKey = des.Key+ "/" + des.IV;


            GenerateRSAKeyPair();
            string publicA = File.ReadAllText("Public_A.txt");
            string privateA = File.ReadAllText("Private_A.txt");
            string publicB = File.ReadAllText("Public_B.txt");
            string privateB = File.ReadAllText("Private_B.txt");

            EncryptDesKey(publicB, des.Key, des.IV);
            hashBoodschap = boodschap.GetHashCode();
            
            byte[] hashedBoodschap = BitConverter.GetBytes(hashBoodschap);

            EncryptFile(hashedBoodschap, @"File_3.txt", des.Key, des.IV);

            MessageBox.Show("Encryptie is gelukt. Alle bestanden zijn te vinden in de debug folder van dit programma");
            
        }

      
      

        private static void GenerateRSAKeyPair()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            string publicA = rsa.ToXmlString(false);
            File.WriteAllText(@"Public_A.txt", publicA);
            string privateA = rsa.ToXmlString(true);
            File.WriteAllText(@"Private_A.txt", privateA);

            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(2048);
            string publicB = rsa2.ToXmlString(false);
            File.WriteAllText(@"Public_B.txt", publicB);
            string privateB = rsa2.ToXmlString(true);
            File.WriteAllText(@"Private_B.txt", privateB);
        }

        private void EncryptFile(Byte[] Data, String FileName, byte[] Key, byte[] IV)
        {
           
            
                FileStream fStream = File.Open(FileName, FileMode.Create);
                DES DESalg = DES.Create();
                CryptoStream cStream = new CryptoStream(fStream, DESalg.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(Data, 0, Data.Length);
                cStream.Close();
                fStream.Close();

        
        }

        private void EncryptDesKey (string keyName, byte[] Key, byte[] IV)
        {
            byte[] encKey;
            byte[] encIV;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(keyName);
                encKey = rsa.Encrypt(Key, false);
                encIV = rsa.Encrypt(IV, false);
            }

            string keyIV = Convert.ToBase64String(encKey) + ":" + Convert.ToBase64String(encIV);
            File.WriteAllText(@"File_2.txt", keyIV);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }

       




       

       
        


    }
        
}

