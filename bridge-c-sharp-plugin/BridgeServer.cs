﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace bridge_c_sharp_plugin
{
    public class BridgeServer
    {
        private TcpListener tcpListener;
        private Thread tcpListenerThread;
        private TcpClient connectedTcpClient;

        public List<string> receivedMessage = new List<string>();
        //Port number to listen to. Please make sure it is the same in Bridge as well.
        public int MessageReceivingPort = 24981;

        public void StartServer()
        {
            // Start TcpServer background thread 		
            tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();
        }

        public void EndServer()
        {
            tcpListener.Stop();
        }

        private void ListenForIncommingRequests()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), MessageReceivingPort);
                tcpListener.Start();
                Console.WriteLine("Server is listening");
                Byte[] bytes = new Byte[512];
                while (true)
                {
                    using (connectedTcpClient = tcpListener.AcceptTcpClient())
                    {
                        using (NetworkStream stream = connectedTcpClient.GetStream())
                        {
                            int length;
                            string clientMessage = "";
                            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                byte[] incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);
                                clientMessage += Encoding.ASCII.GetString(incommingData);
                            }
                            receivedMessage.Add(clientMessage);

                            if (receivedMessage.Count > 0)
                            {
                                BridgeImporter.AssetImporter(receivedMessage[0]);
                                receivedMessage.RemoveAt(0);
                            }
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("SocketException " + socketException.ToString());
            }
        }
    }
}
