using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PC_control
{
    class Stellarium
    {
        private Thread m_thread;
        private static TcpClient m_tcp_client = null;
        private static readonly int m_port = 10001;


        private Stellarium()
        {
            m_thread = new Thread(() => Main_loop());
            m_thread.Start();
        }

        private static void Main_loop()
        {
            bool is_alive = false;
            NetworkStream nwStream = null;
            Socket socket = null;
            m_tcp_client = new TcpClient();
            byte[] bufferReceive = new byte[4096];
            TcpListener listener = new TcpListener(IPAddress.Any, m_port);
            
            while (true)
            {                
                try
                {
                    if (!is_alive)
                    {
                        listener.Start();
                        socket = listener.AcceptSocket();
                        if (socket.Connected)
                        {
                            Console.WriteLine("Stellarium is alive");
                            is_alive = true;
                        }

                        //m_tcp_client.Connect("127.0.0.1", m_port);
                        //nwStream = m_tcp_client.GetStream();
                    }

                    

                    int length = socket.Receive(bufferReceive);
                    if (length > 0)
                    {
                        Console.WriteLine("Stellarium received ; " + Encoding.Unicode.GetString(bufferReceive));
                    }
                    //Console.WriteLine("Stellarium is alive");


                    //byte[] bytesToRead = new byte[m_tcp_client.ReceiveBufferSize];
                    //int bytesRead = nwStream.Read(bytesToRead, 0, m_tcp_client.ReceiveBufferSize);
                    //Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));

                }
                catch (Exception e1)
                {
                    Console.WriteLine("Stellarum therad: " + e1.Message);
                    is_alive = false;
                }
            }
        }



        private static volatile Stellarium m_instance = null;
        private static object m_sync_root = new object();
        public static Stellarium Instance()
        {
             if (m_instance == null)
             {
                lock (m_sync_root)
                {
                    if (m_instance == null)
                        m_instance = new Stellarium();
                }
             }

            return m_instance;            
        }
    }
}
