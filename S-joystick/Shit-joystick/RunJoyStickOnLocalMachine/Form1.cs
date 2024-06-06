using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Management;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using Sharer;
using System.IO.Ports;

namespace RunJoyStickOnLocalMachine
{
    public partial class Form1 : Form
    {
        private Joystick joystick;
        readonly SharerConnection connection = new SharerConnection("COM8", 115200);
        int CoarseGear = 0;
        int FineGear = 0;
        public Form1()
        {
            InitializeComponent();
            joystick = new Joystick(this.Handle);
            connectToJoystick(joystick);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.Connect();
        }

        //---------------------------------------------------------------------
        private void connectToJoystick(Joystick joystick)
        {
            while (true)
            {
                string sticks = joystick.FindJoysticks();
                if (sticks != null)
                {
                    if (joystick.AcquireJoystick(sticks))
                    {
                        enableTimer();
                        break;
                    }
                }
            }
        }

        private void enableTimer()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ThreadStart(delegate ()
                {
                    joystickTimer.Enabled = true;
                }));
            }
            else
                joystickTimer.Enabled = true;
        }

        private void joystickTimer_Tick_1(object sender, EventArgs e)
        {
            try
            {
                
                joystick.UpdateStatus();

                if (joystick.buttons[4] == true) { 
                    if((1000 + (CoarseGear * 100) + (FineGear * 10)) > 999) { CoarseGear--; }
                }
                if (joystick.buttons[5] == true)
                {
                    if ((1000 + (CoarseGear * 100) + (FineGear * 10)) < 2001) { CoarseGear++; }
                }
                if (joystick.buttons[6] == true) { 
                    if((1000 + (CoarseGear * 100) + (FineGear * 10)) > 999) { FineGear--; }
                }
                if (joystick.buttons[7] == true)
                {
                    if ((1000 + (CoarseGear * 100) + (FineGear * 10)) < 2001) { FineGear++; }
                }
                
                progressBar1.Value = map(joystick.Lx, 0, 65536, progressBar1.Minimum, progressBar1.Maximum);
                progressBar3.Value = map(joystick.Ly, 0, 65536, progressBar1.Minimum, progressBar1.Maximum);
                progressBar2.Value = map(joystick.Rx, 0, 65536, progressBar1.Minimum, progressBar1.Maximum);
                progressBar4.Value = map(joystick.Ry, 0, 65536, progressBar1.Minimum, progressBar1.Maximum);
                label3.Text =  "Sending PWM = " + (1000 + (CoarseGear * 100)+(FineGear * 10));
                for (int i = 0; i < 12; i++)
                {
                    if (joystick.buttons[i] == true)
                        richTextBox1.Text += "Button " + i + " pressed\n";
                }
                //richTextBox1.Text += "Pressed\n";
                //for (int i = 0;i < 12; i++)
                //{
                //    if (joystick.buttons[i] == false)
                //        richTextBox1.Text += "Button " + i + ", ";
                //}
                //richTextBox1.Text += "NOT Pressed\n";
                //richTextBox1.Clear();
            }
            catch
            {
                joystickTimer.Enabled = false;
                connectToJoystick(joystick);
            }
        }

        private static int map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            connection.WriteVariable("sent_data.ch1", map(joystick.Rx, 0, 65536, 0, 255));
            connection.WriteVariable("sent_data.ch2", map(joystick.Ry, 0, 65536, 0, 255));
            //connection.WriteVariable("sent_data.ch3", mapAF(joystick.Ly, 0, 65536, 0, 255));
            connection.WriteVariable("sent_data.ch4", map(joystick.Lx, 0, 65536, 0, 255));
            try
            {
                connection.WriteVariable("sent_data.ch3", map((1000 + (CoarseGear * 100) + (FineGear * 10)), 2000, 1000, 0, 255));
            }
            catch
            {
               
            }


        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
