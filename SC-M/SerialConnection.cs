using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M
{
    public partial class SerialConnection : Form
    {
        Form1 form1 = new Form1();
        public SerialConnection(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        public string[] baudList = { "9600", "19200", "38400", "57600", "115200" };
        public string[] serialPortList;  // List of serial port
        private void SerialConnection_Load(object sender, EventArgs e)
        {
            serialPortList = SerialPort.GetPortNames();
            comboBoxPort.Items.AddRange(serialPortList);
            comboBoxBaud.Items.AddRange(baudList);

            if (form1.serialPort1.IsOpen)
            {
                comboBoxPort.Text = form1.serialPort1.PortName;
                comboBoxBaud.Text = form1.serialPort1.BaudRate.ToString();
            }
        }

        private void btConnection_Click(object sender, EventArgs e)
        {
            try
            {
                if(comboBoxPort.SelectedIndex == -1 || comboBoxBaud.SelectedIndex == -1)
                {
                    throw new Exception("Please select port or baud rate");
                }
                bool connected = this.form1.ConnectionSerial(comboBoxPort.SelectedItem.ToString(), comboBoxBaud.SelectedItem.ToString());
                if (!connected)
                {
                    throw new Exception("Can not connect to serial port");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }
    }
}
