using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serial_Port
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string str;

        private void Form1_Load(object sender, EventArgs e)
        {
            Show_State();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.RemoveAt(0);
            comboBox1.Items.Insert(0, "搜索中。。。");
            comboBox1.SelectedIndex = 0;
            comboBox1.Refresh();
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            for (int i = 0; i < 20;i++ )
            {  
                serialPort.PortName = "COM" + i;
                try
                {
                    serialPort.Open();
                }
                catch
                {
                    ;
                }
                if (serialPort.IsOpen)
                {
                    comboBox1.Items.Add(serialPort.PortName);
                    serialPort.Close();
                }
            }
            comboBox1.Items.RemoveAt(0);
            comboBox1.Items.Insert(0, "请选择端口");
            comboBox1.SelectedIndex = 0;
            comboBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.Text.Equals("连接端口"))
            {
                try
                {
                    serialPort.PortName = comboBox1.SelectedItem.ToString();
                    serialPort.BaudRate = 38400;
                    serialPort.Parity = System.IO.Ports.Parity.None;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = System.IO.Ports.StopBits.One;
                    serialPort.Open();
                }
                catch
                {
                    return;
                }
                timer.Start();
                button2.Text = "断开端口";
            }
            else if(button2.Text.Equals("断开端口"))
            {
                serialPort.Close();
                timer.Stop();
                button2.Text = "连接端口";
            }
        }

        private void serialPort_Receive(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            int bytes = serialPort.BytesToRead;
            byte[] buffer = new byte[bytes];
            if (bytes == 0)
                return;
            try
            {
                serialPort.Read(buffer, 0, bytes);
            }
            catch
            {

            }
                int[] num = new int[bytes];
            for (int i = 0; i < bytes ; i++ )
            {
                byte temp = buffer[i];
                str += temp.ToString("X2") + "/";
                //MessageBox.Show(str);
            }
        }

        private delegate void Reveive_data();

        private void onTimerEvent(object sender, EventArgs e)
        {
            Reveive_data Display_data = new Reveive_data(data_judge);
            Display_data();
        }

        void data_judge()
       {
            //System.Threading.Thread.Sleep(10);
            try
            {
                string[] str_splited = str.Split('/');
                int index = 0;
                foreach (string str_temp in str_splited)
                {
                    if (str_temp.Equals("00"))
                    {
                        switch (str_splited[index + 1])
                        {
                            case "01": 
                                textBox1.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[0] = 2;
                                else link_state[0] = 1;
                                break;
                            case "02": textBox2.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[1] = 2;
                                else link_state[1] = 1;
                                break;
                            case "03": textBox3.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[2] = 2;
                                else link_state[2] = 1;
                                break;
                            case "04": textBox4.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[3] = 2;
                                else link_state[3] = 1;
                                break;
                            case "05": textBox5.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[4] = 2;
                                else link_state[4] = 1;
                                break;
                            case "06": textBox6.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[5] = 2;
                                else link_state[5] = 1;
                                break;
                            case "07": textBox7.Text = "" + Convert.ToInt32(str_splited[index + 2], 16); 
                                if(Convert.ToInt32(str_splited[index + 2],16) > 30) link_state[6] = 2;
                                else link_state[6] = 1;
                                break;
                            default: MessageBox.Show(str_splited[index + 1] + "\\ERROR");
                                break;
                        }
                        //MessageBox.Show(str_splited[index] + str_splited[index + 1] + str_splited[index + 2]);
                        break;
                    }
                    index++;
                }
            }
            catch
            {

            }
            str = "";
            Show_State();
        }


        private void Show_State()
        {
            this.Refresh();
            Graphics dc = this.CreateGraphics();
            Brush GreenBrush = Brushes.Green;
            Brush GrayBrush = Brushes.Gray;
            Brush RedBrush = Brushes.Red;
            int index = 0;
            foreach (int link_state_member in link_state)
            {
                if (link_state_member == 0) dc.FillRectangle(GrayBrush, 300, 105 + 30 * index, 20, 20);
                else if (link_state_member == 1) dc.FillRectangle(GreenBrush, 300, 105 + 30 * index, 20, 20);
                else dc.FillRectangle(RedBrush, 300, 105 + 30 * index, 20, 20);
                index++;
            }
        }
    }
}
