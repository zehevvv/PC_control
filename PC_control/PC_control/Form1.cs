using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_control
{
    public partial class Form1 : Form
    {
        private UdpClient m_udpClient = new UdpClient();
        private int m_Port = 9001;
        private string m_server_name = "orangepizero2";
        private KeepAlive m_keepalive;

        public Form1()
        {
            InitializeComponent();            
            m_udpClient.Connect(m_server_name, m_Port);
            m_keepalive = new KeepAlive(m_server_name, m_Port, Server_alive, Server_dead);
        }

        ~Form1()
        {
            m_udpClient.Close();
        }

        private void Server_alive()
        {
            Console.WriteLine("Server alive");
        }

        private void Server_dead()
        {
            Console.WriteLine("Server dead");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            // This constructor arbitrarily assigns the local port number.
            stopwatch.Start();

            try
            {
                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
                m_udpClient.Send(sendBytes, sendBytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = m_udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +returnData.ToString());
                Console.WriteLine("This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            stopwatch.Stop();

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

            //    try
            //    {
            //        String server = "orangepizero2";
            //        String message = "hello";


            //        // Create a TcpClient.
            //        // Note, for this client to work you need to have a TcpServer
            //        // connected to the same address as specified by the server, port
            //        // combination.
            //        Int32 port = 9001;

            //        // Prefer using declaration to ensure the instance is Disposed later.
            //        TcpClient client = new TcpClient(server, port);

            //        // Translate the passed message into ASCII and store it as a Byte array.
            //        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            //        // Get a client stream for reading and writing.
            //        NetworkStream stream = client.GetStream();

            //        // Send the message to the connected TcpServer.
            //        stream.Write(data, 0, data.Length);

            //        Console.WriteLine("Sent: {0}", message);

            //        // Receive the server response.

            //        // Buffer to store the response bytes.
            //        data = new Byte[256];

            //        // String to store the response ASCII representation.
            //        String responseData = String.Empty;

            //        // Read the first batch of the TcpServer response bytes.
            //        Int32 bytes = stream.Read(data, 0, data.Length);
            //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //        Console.WriteLine("Received: {0}", responseData);

            //        // Explicit close is not necessary since TcpClient.Dispose() will be
            //        // called automatically.
            //        // stream.Close();
            //        // client.Close();
            //    }
            //    catch (ArgumentNullException e1)
            //    {
            //        Console.WriteLine("ArgumentNullException: {0}", e1);
            //    }
            //    catch (SocketException e2)
            //    {
            //        Console.WriteLine("SocketException: {0}", e2);
            //    }

            //    Console.WriteLine("\n Press Enter to continue...");
            //    Console.Read();
        }
        }
}
