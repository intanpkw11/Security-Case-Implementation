using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    class Program
    {
        //Memeriksa apakah ada pesan yang masuk
        public class CheckMessage
        {

            public void Check(TcpClient tcpClient)
            {
                StreamReader streamReader = new StreamReader(tcpClient.GetStream());

                //Pemeriksaan dilakukan secara - terus menerus
                //Ketika program dijalankan
                while (true)
                {
                    try
                    {
                        string message = streamReader.ReadLine();
                        Console.WriteLine(message);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        break;
                    }
                }
            }
        }

        //Mencatat pesan yang masuk pada fungsi main
        static void Main(string[] args)
        {
            CheckMessage read = new CheckMessage();
            Console.WriteLine("Masukkan Nama : ");
            string name = Console.ReadLine();

            //Ketika ada data yang masuk
            //Maka data dibaca, dan ditampilkan
            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 1000);
                Console.WriteLine("Terhubung ke Server.");

                Thread thread = new Thread(() => read.Check(tcpClient));
                thread.Start();

                StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream());

                while (true)
                {
                    if (tcpClient.Connected)
                    {
                        string input = Console.ReadLine();
                        streamWriter.WriteLine(name + " : " + input);
                        streamWriter.Flush();
                    }
                }

            }

            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.ReadKey();

        }
    }
}
