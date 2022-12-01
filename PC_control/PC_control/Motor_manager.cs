using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PC_control
{
    enum DIRECTION
    {
        UP = 0,
        DOWN = 1,
        RIGHT = 2,
        LEFT = 3
    };


    class Motor_manager
    {
        static private Motor_manager m_instance = null;
        static private Thread m_thread;

        private UdpClient m_udp_client = null;        
        private int m_port;
        private string m_server_name;        

        public bool m_arrow_right = false;
        public bool m_arrow_left = false;
        public bool m_arrow_up = false;
        public bool m_arrow_down = false;
        public int m_motor_action_time = 5;

        public static Motor_manager Start(string server_name, int port)
        {
            if (m_instance == null)
            {
                m_instance =  new Motor_manager(port, server_name);
                m_thread = new Thread(() => Monitor_motor());
                m_thread.Start();
            }
                
            return m_instance;
        }        

        private Motor_manager(int port, string server_name)
        {
            m_port = port;
            m_server_name = server_name;
        }

        private static void Monitor_motor()
        {
            m_instance.Start_client();
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (m_instance.m_arrow_down)
                    m_instance.Send_message(DIRECTION.DOWN);
                if (m_instance.m_arrow_up)
                    m_instance.Send_message(DIRECTION.UP);
                if (m_instance.m_arrow_right)
                    m_instance.Send_message(DIRECTION.RIGHT);
                if (m_instance.m_arrow_left)
                    m_instance.Send_message(DIRECTION.LEFT);

                while (stopwatch.ElapsedMilliseconds < 100);
                Thread.Sleep(100);
            }    
        }

        private void Start_client()
        {            
            while (m_udp_client == null)
            {
                try
                {
                    m_udp_client = new UdpClient();
                    m_udp_client.Connect(m_server_name, m_port);
                }
                catch (Exception e1)
                {
                    Console.WriteLine("Monitor therad: " + e1.Message);
                    m_udp_client = null;
                    Thread.Sleep(100);
                }                
            }

            m_udp_client.Client.ReceiveTimeout = 100;
        }

        private void Send_message(DIRECTION direction)
        {
            Byte[] msg = { 2, (byte)direction, (byte)m_motor_action_time };
            try
            {
                // Sends a message to the host to which you have connected.                
                m_udp_client.Send(msg, msg.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint remote_ip = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receive_bytes = m_udp_client.Receive(ref remote_ip);
                string msg_recv = Encoding.ASCII.GetString(receive_bytes);

                if (msg_recv.Equals(msg))
                    Console.WriteLine("recv ack:" + msg_recv);
                else
                    Console.WriteLine("Error: not recv ack");
            }
            catch (Exception e1)
            {
                Console.WriteLine("Monitor therad: " + e1.Message);
            }
        }
    }
}
