using System;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Client
{
    class Program
    {

        private static string EncyrptRSA(string message)
        {
            string publicKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(publicKey);

                    byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(message), true);

                    string base64Encrypted = Convert.ToBase64String(encryptedData);
                    Console.WriteLine("\nPesan Terenkripsi : " + base64Encrypted + "\n");

                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

         static void Main(string[] args)
        {

            string ipAdress = "";
            bool ipCorrect = false;

            NetworkStream networkStream = null;

            do
            {
                try
                {
                    Console.Write("IP Server : ");
                    ipAdress = Console.ReadLine();
                    TcpClient client = new TcpClient(ipAdress, 1000);
                    networkStream = client.GetStream();
                    ipCorrect = true;
                }
                catch
                {
                    Console.WriteLine("Tidak dapat terhubung ke alamat IP yang diberikan...");
                }
            } while (!ipCorrect);

            Console.WriteLine("Koneksi dibuat ke : " + ipAdress + " \n");

            while (true)
            {
                Console.Write("Pesan untuk dikirim : ");
                byte[] bytesToSend = Convert.FromBase64String(EncyrptRSA(Console.ReadLine()));
                networkStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
        }
    }
}