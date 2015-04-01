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
        string Private_A;
        string Public_A;
        string Private_B;
        string Public_B;
        string boodschap;
        
        public Encryptie()
        {
            InitializeComponent();
            encryptieTextbox.IsReadOnly = true;
            
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
                    
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    encryptieTextbox.Text = "Kies een bestand en klik op OK";
                    break;
            }

        }

        private void Start_Encryptie(object sender, RoutedEventArgs e)
        {
            String encString = encryptieTextbox.Text;
            this.Close();
            EncryptedPage encPage = new EncryptedPage();
            encPage.Show();
            using (Aes myAes = Aes.Create())
            {

                 //Encrypt the string to an array of bytes. 
                byte[] encrypted = EncryptStringToBytes_Aes(encString,myAes.Key, myAes.IV);
                string encKey = System.Text.Encoding.UTF8.GetString(myAes.Key);
                encPage.encryptedlabel.Content = encKey;
                string encryptedMessage = System.Text.Encoding.UTF8.GetString(encrypted);
                encPage.testlabel.Content = encryptedMessage;



                File.WriteAllText(@"./File_1.txt", encryptedMessage);
                MessageBox.Show("Encrypted messagestaat in /bin/debug");
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            Public_A = rsa.ToXmlString(false);
            Private_A = rsa.ToXmlString(true);
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(2048);
            Public_B = rsa2.ToXmlString(false);
            Private_B = rsa2.ToXmlString(true);

            File.WriteAllText(@".\Public_A.txt", Public_A);
            File.WriteAllText(@".\Private_A.txt", Private_A);
            File.WriteAllText(@".\Public_B.txt", Public_B);
            File.WriteAllText(@".\Private_B.txt", Private_B);


            using (Aes Aes2 = Aes.Create())
            {
                byte[] waytoFile2 = EncryptStringToBytes_Aes(Public_B, Aes2.Key, Aes2.IV);
                string encryptedMessageBobKey = System.Text.Encoding.UTF8.GetString(waytoFile2);
                File.WriteAllText(@"./File_2.txt", encryptedMessageBobKey);
            }
        

        }



        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key,byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                            
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                                        plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static void GenerateRSAKeyPair(out string publicKey, out string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            publicKey = rsa.ToXmlString(false);
            privateKey = rsa.ToXmlString(true);
        }
        
        //Hashing

        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

    }     
}
