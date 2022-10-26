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
    public delegate void Keep_alive_method();    

    class KeepAlive
    {        
        private int m_port;
        private string m_server_name;        
        private Thread m_thread;


        public KeepAlive(string server_name, int port, Keep_alive_method alive_callback, Keep_alive_method not_alive_callback)
        {            
            m_port = port;
            m_server_name = server_name;            
            m_thread = new Thread(() => Check_server_alive(server_name, port, alive_callback, not_alive_callback));
            m_thread.Start();
        }

        private static void Check_server_alive(string server_name, int port, Keep_alive_method alive_event, Keep_alive_method not_alive_event)
        {
            bool is_alive = false;
            int num_connect_failed = 0;
            UdpClient udp_client = new UdpClient();
            udp_client.Client.ReceiveTimeout = 200;       
            
            while (true)
            {
                try
                {
                    udp_client.Connect(server_name, port);

                    while (true)
                    {
                        try
                        {
                            // Sends a message to the host to which you have connected.
                            Byte[] send_bytes = { 1 };
                            udp_client.Send(send_bytes, send_bytes.Length);

                            //IPEndPoint object will allow us to read datagrams sent from any source.
                            IPEndPoint remote_ip = new IPEndPoint(IPAddress.Any, 0);

                            // Blocks until a message returns on this socket from a remote host.
                            Byte[] receive_bytes = udp_client.Receive(ref remote_ip);

                            if (receive_bytes.Length == 0 && is_alive)
                            {
                                not_alive_event.Invoke();
                                is_alive = false;
                            }
                            else if (receive_bytes.Length != 0 && receive_bytes[0] == 1 && !is_alive)
                            {
                                alive_event.Invoke();
                                is_alive = true;
                                num_connect_failed = 0;
                            }
                        }
                        catch (Exception e1)
                        {
                            if (is_alive)
                            {
                                num_connect_failed++;

                                if (num_connect_failed == 3)
                                {
                                    not_alive_event.Invoke();
                                    is_alive = false;
                                }
                            }

                            Console.WriteLine("Keepalive therad: " + e1.Message);
                        }

                        Thread.Sleep(300);
                    }
                }
                catch (Exception e1)
                {
                    Console.WriteLine("Keepalive therad: " + e1.Message);
                }

                //if (receive_bytes.Length == 0)
                //    Console.WriteLine("alive");
                //else
                //    Console.WriteLine("dead");                    

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                //Console.WriteLine("This is the message you received " + returnData.ToString());
                //Console.WriteLine("This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());                
         
                Thread.Sleep(100);
            }
        }
    }
}
