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
using AForge.Video;

//Stopwatch stopwatch = new Stopwatch();
//stopwatch.Start();      
//stopwatch.Stop();
//Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);


namespace PC_control
{
    public partial class Form1 : Form
    {
        delegate void NewFrameDelegeta(NewFrameEventArgs args);
        private int m_Port = 9001;
        private int m_Port_camera = 10001;
        private string m_server_name = "192.168.0.188";
        private KeepAlive m_keepalive;
        private Motor_manager m_motor_Manager;
        private MJPEGStream m_stream;


        public Form1()
        {
            InitializeComponent();            
            m_keepalive = new KeepAlive(m_server_name, m_Port, Server_alive, Server_dead, Status_update);

            m_motor_Manager = Motor_manager.Start(m_server_name, m_Port);

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);

            m_motor_Manager.m_motor_action_time = int.Parse(m_tbxTime.Text);
            Stellarium.Instance();
        }

        private void Server_alive()
        {                  
            this.Invoke(new MethodInvoker(delegate () {
                Console.WriteLine("Server alive");
                m_lbServer_status.BackColor = Color.Green;                
                m_lbServer_status.Text = "Online";

                m_stream = new MJPEGStream("http://" + m_server_name + ':' + m_Port_camera);
                m_stream.NewFrame += NewFrameEvent;
                m_stream.Start();
            }));
        }

        private void Server_dead()
        {
            this.Invoke(new MethodInvoker(delegate () {
                Console.WriteLine("Server dead");
                m_lbServer_status.BackColor = Color.Red;
                m_lbServer_status.Text = "Offline";

                m_stream.Stop();                
            }));
        }

        private void NewFrameEvent(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                if (!this.Disposing && !this.IsDisposed)
                    this.Invoke(new NewFrameDelegeta(NewFrame), new object[] { eventArgs });
            }
            catch (Exception) { }
        }

        private void NewFrame(NewFrameEventArgs eventArgs)
        {
            try
            {
                m_pbxScreen.Image = (Image)eventArgs.Frame.Clone();
                m_pbxScreen.Update();
                //m_fps_counter++;
                //lbHeight.Text = "Frame Height : " + eventArgs.Frame.Height;
                //lbWidth.Text = "Frame Width : " + eventArgs.Frame.Width;
            }
            catch (Exception) { }
        }

        private void Status_update(float azimuth, float altitude)
        {
            this.Invoke(new MethodInvoker(delegate () {
                m_lblAzimuth.Text = azimuth.ToString();
                m_lblAltitude.Text = altitude.ToString();                
            }));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                m_motor_Manager.m_arrow_right = true;
            if (e.KeyCode == Keys.Left)
                m_motor_Manager.m_arrow_left = true;
            if (e.KeyCode == Keys.Up)
                m_motor_Manager.m_arrow_up = true;
            if (e.KeyCode == Keys.Down)
                m_motor_Manager.m_arrow_down = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                m_motor_Manager.m_arrow_right = false;
            if (e.KeyCode == Keys.Left)
                m_motor_Manager.m_arrow_left = false;
            if (e.KeyCode == Keys.Up)
                m_motor_Manager.m_arrow_up = false;
            if (e.KeyCode == Keys.Down)
                m_motor_Manager.m_arrow_down = false;
        }

        private void m_tbxTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void m_tbxTime_TextChanged(object sender, EventArgs e)
        {
            if (m_tbxTime.Text.Length > 0)
            {
                int num = int.Parse(m_tbxTime.Text);
                if (num > 100)
                    m_motor_Manager.m_motor_action_time = 100;
                else if (num < 5)
                    m_motor_Manager.m_motor_action_time = 5;
                else
                    m_motor_Manager.m_motor_action_time = num;
            }           
        }
    }
}
