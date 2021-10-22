using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    //Memproses permintaan client
    class ProcessingData
    {
        SaveData save = new SaveData();

        public List<TcpClient> clientList = new List<TcpClient>();

        public void addClients(TcpClient client)
        {
            clientList.Add(client);
        }

        //Mengambil pesan dari client
        public void GetMessage(TcpClient tcpClient)
        {
            Console.WriteLine("Client Terhubung.");
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            //Setiap pesan yang masuk akan dibaca dan direkam
            while (true)
            {
                string message = reader.ReadLine();
                Broadcast(message, tcpClient);

                string chat = message;
                Console.WriteLine(chat);

                save.writeMessage(chat);
            }
        }

        //Menyimpan data ke dalam file .txt
        class SaveData
        {
            private List<string> saveMessages = new List<string>();
            public void writeMessage(string chat)
            {
                saveMessages.Add(chat);
                File.WriteAllLines("C:/Users/TUF/Documents/Multiclient/ChatHistory.txt", saveMessages);
            }
        }

        //Mengambil pesan dari pengirim & menyebarkan ke penerima 
        public void Broadcast(string message, TcpClient excludeClient)
        {
            foreach (TcpClient client in clientList)
            {

                StreamWriter sWriter = new StreamWriter(client.GetStream());

                if (client != excludeClient)
                {
                    sWriter.WriteLine(message);
                }

                sWriter.Flush();
            }
        }
    }

    //Mencatat semua data client yang masuk pada fungsi main
    class Program
    {
        public static TcpListener tcpListener;

        static void Main(string[] args)
        {
            ProcessingData dataClient = new ProcessingData();

            tcpListener = new TcpListener(IPAddress.Any, 1000);
            tcpListener.Start();
            Console.WriteLine("Server Dibuat.");

            //Untuk memeriksa client yang terhubung ke server.
            while (true)
            {

                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                dataClient.addClients(tcpClient);

                Thread startListen = new Thread(() => dataClient.GetMessage(tcpClient));
                startListen.Start();
            }
        }
    }
}