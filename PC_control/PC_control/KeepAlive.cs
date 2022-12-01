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
    public delegate void Status_update(float axis_x, float axis_y, float to_north);

    class KeepAlive
    {        
        private int m_port;
        private string m_server_name;        
        private Thread m_thread;
        public static bool  m_is_alive { get; private set; }
        private static UdpClient m_udp_client = null;
        private static Mutex m_mutex = new Mutex();


        public KeepAlive(string server_name, int port, Keep_alive_method alive_callback, Keep_alive_method not_alive_callback, 
            Status_update status_update_callback)
        {
            m_is_alive = false;
            m_port = port;
            m_server_name = server_name;            
            m_thread = new Thread(() => Check_server_alive(server_name, port, alive_callback, not_alive_callback, status_update_callback));
            m_thread.Start();
        }


        public static void get_status(Status_update status_update_callback)
        {                        
            try
            {
                //m_mutex.WaitOne();
                Byte[] send_bytes = { 3 };
                m_udp_client.Send(send_bytes, send_bytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint remote_ip = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receive_bytes = m_udp_client.Receive(ref remote_ip);

                //m_mutex.ReleaseMutex();

                if ( receive_bytes.Length == 6 )
                {
                    UInt16 axis_x = BitConverter.ToUInt16(receive_bytes, 0);
                    UInt16 axis_y = BitConverter.ToUInt16(receive_bytes, 2);
                    UInt16 heading_north = BitConverter.ToUInt16(receive_bytes, 4);
                    float float_axis_x = (float)axis_x / (float)100;
                    float float_axis_y = (float)axis_y / (float)100;
                    float heading_north_float = (float)heading_north / (float)100;
                    Console.WriteLine(@"status: x = " + axis_x + ", y = " + axis_y + ", heading_north = " + heading_north_float);
                    status_update_callback(float_axis_x, float_axis_y, heading_north_float);
                }
                else 
                {
                    Console.WriteLine("Keepalive therad: Get unknown answer");
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("Keepalive therad: " + e1.Message);
                //m_mutex.ReleaseMutex();                
            }            
        }

        private static void Check_server_alive(string server_name, int port, Keep_alive_method alive_event, Keep_alive_method not_alive_event,
            Status_update status_update_callback)
        {
            int num_connect_failed = 0;
            m_udp_client = new UdpClient();
            m_udp_client.Client.ReceiveTimeout = 200;       
            
            while (true)
            {
                try
                {
                    while (true)
                    {
                        try
                        {
                            m_mutex.WaitOne();

                            if ( !m_is_alive )
                                m_udp_client.Connect(server_name, port);

                            // Sends a message to the host to which you have connected.
                            Byte[] send_bytes = { 1 };
                            m_udp_client.Send(send_bytes, send_bytes.Length);

                            //IPEndPoint object will allow us to read datagrams sent from any source.
                            IPEndPoint remote_ip = new IPEndPoint(IPAddress.Any, 0);

                            // Blocks until a message returns on this socket from a remote host.
                            Byte[] receive_bytes = m_udp_client.Receive(ref remote_ip);

                            m_mutex.ReleaseMutex();

                            if (receive_bytes.Length == 0 && m_is_alive)
                            {
                                not_alive_event.Invoke();
                                m_is_alive = false;
                            }
                            else if (receive_bytes.Length != 0 && receive_bytes[0] == 1 && !m_is_alive)
                            {
                                alive_event.Invoke();
                                m_is_alive = true;
                                num_connect_failed = 0;
                            }

                            if (m_is_alive)
                                get_status(status_update_callback);
                        }
                        catch (Exception e1)
                        {
                            if (m_is_alive)
                            {
                                num_connect_failed++;

                                if (num_connect_failed == 3)
                                {
                                    not_alive_event.Invoke();
                                    m_is_alive = false;
                                }
                            }

                            Console.WriteLine("Keepalive therad: " + e1.Message);
                            m_mutex.ReleaseMutex();
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
