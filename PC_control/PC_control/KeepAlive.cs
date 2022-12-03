using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


//class ExceptionLogger
//{
//    Exception _ex;

//    public ExceptionLogger(Exception ex)
//    {
//        _ex = ex;
//    }

//    public void DoLog()
//    {
//        Console.WriteLine(_ex.ToString()); //Will display en-US message
//    }
//}

//ExceptionLogger el = new ExceptionLogger(ex);
//System.Threading.Thread t = new System.Threading.Thread(el.DoLog);
//t.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
//        t.Start();


namespace PC_control
{
    public delegate void Keep_alive_method();
    public delegate void Status_update(float azimuth, float altitude);

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
                string cmd = "{\"cmd_id\":3}";
                Byte[] send_bytes = Encoding.ASCII.GetBytes(cmd);
                m_udp_client.Send(send_bytes, send_bytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint remote_ip = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receive_bytes = m_udp_client.Receive(ref remote_ip);
                string cmd_string = Encoding.ASCII.GetString(receive_bytes);

                try
                {
                    dynamic cmd_status = JsonConvert.DeserializeObject(cmd_string);
                    Console.WriteLine(@"status: azimuth = " + cmd_status.azimuth + ", altitude = " + cmd_status.altitude);
                    status_update_callback(Convert.ToSingle(cmd_status.azimuth), Convert.ToSingle(cmd_status.altitude));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Keepalive therad: Get unknown answer : " + cmd_string);
                }
                
            }
            catch (Exception e1)
            {
                Console.WriteLine("Keepalive therad get status: " + e1.Message);                
            }            
        }        

        private static bool Is_cmd_alive(Byte[] receive_bytes)
        {
            string cmd_string = Encoding.ASCII.GetString(receive_bytes);            

            try
            {
                dynamic stuff = JsonConvert.DeserializeObject(cmd_string);
                
                if (stuff.alive == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
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
                            
                            string cmd = "{\"cmd_id\":1}";
                            Byte[] send_bytes = Encoding.ASCII.GetBytes(cmd);
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
                            else if (receive_bytes.Length != 0 && Is_cmd_alive(receive_bytes) && !m_is_alive)
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
