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
using SC_M.Modules;

namespace SC_M
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Focus on the tbName
            tbName.Select();
            this.ActiveControl = tbName;
            tbName.Focus();
            toolStripStatusConnection.Text = "";
            toolStripStatusData.Text = "";
            toolStripStatusReceive.Text = "";
            LoadHistoryData();
            timer1.Start();
        }

        private void _KeyDown(object sender, KeyEventArgs e)
        {
            // Get Name
            var s = sender as TextBox;
            string name = s.Name;
            switch (name)
            {
                case "tbName":
                    if (e.KeyCode == Keys.Enter)
                    {
                        tbLabel.Select();
                        this.ActiveControl = tbLabel;
                        tbLabel.Focus();
                    }
                    break;
                case "tbLabel":
                    if (e.KeyCode == Keys.Enter)
                    {
                        tbECU.Select();
                        this.ActiveControl = tbECU;
                        tbECU.Focus();
                    }
                    break;
                case "tbECU":
                    if (e.KeyCode == Keys.Enter)
                    {
                        Comparedata();

                    }
                    break;
            }
        }

        private void Comparedata()
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    throw new Exception("Please connect to the controller");
                }

                string judge = "";
                if (tbLabel.Text == tbECU.Text || CheckInMasterData())
                {
                    judge = "OK";
                }
                else
                {
                    judge = "NG";
                }
                // Save to database
                HistoryData hd = new HistoryData();
                hd.name = tbName.Text.Trim();
                hd.softwareLabel = tbLabel.Text.Trim();
                hd.softwareECU = tbECU.Text.Trim();
                hd.judgement = judge;
                hd.Save();
                sendSerialData(judge.ToUpper());

                JudgeMentOutoput(judge);
                LoadHistoryData();
                // Clear data
                ClearTextBox();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool CheckInMasterData()
        {
            string sql = "select * from master_data where softwareLabel = '" + tbLabel.Text.Trim() + "' and softwareECU = '" + tbECU.Text + "'";
            var row = SQliteDataAccess.GetRow<MasterData>(sql);
            if (row.Count() > 0)
            {
                return true;
            }
            return false;
        }
        private async void JudgeMentOutoput(string data)
        {
            data.ToUpper();

            lbResult.Text = "Loading";
            lbResult.BackColor = Color.Transparent;
            await Task.Delay(500);
            switch (data)
            {
                case "OK":
                    lbResult.Text = "OK";
                    lbResult.BackColor = Color.Green;
                    break;

                case "NG":
                    lbResult.Text = "NG";
                    lbResult.BackColor = Color.Red;
                    break;

                case "WAIT":
                    lbResult.Text = "Waiting..";
                    lbResult.BackColor = Color.Transparent;
                    break;
            }

            // Save to db

        }
        private void ClearTextBox()
        {
            tbLabel.Text = "";
            tbECU.Text = "";

            tbLabel.Select();
            this.ActiveControl = tbLabel;
            tbLabel.Focus();
        }

        private void LoadHistoryData()
        {
            string sql = "select * from history_data order by id desc limit 30";
            var rows = SQliteDataAccess.GetRow<HistoryData>(sql);
            int num = 1;
            var list2 = (from r in rows
                         select new
                         {
                             r.id,
                             No = num++,
                             Name = r.name,
                             Software_On_Label = r.softwareLabel,
                             Software_On_ECU = r.softwareECU,
                             Judgement = r.judgement,
                             Created = r.created_at
                         }).ToList();

            dataGridView1.DataSource = list2;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = (int)(dataGridView1.Width * 0.05);
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].Width = (int)(dataGridView1.Width * 0.2);
            dataGridView1.Update();
        }
        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting settingPage = new Setting();
            settingPage.ShowDialog();
        }
        public string PortName = "";
        public string BaudRate = "";

        public bool ConnectionSerial(string serialPortName, string baud)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            PortName = serialPortName;
            BaudRate = baud;
            serialPort1.PortName = serialPortName;
            serialPort1.BaudRate = int.Parse(baud);
            serialPort1.Open();
            toolStripStatusConnection.Text = "Connection: " + PortName + " " + BaudRate;
            toolStripStatusConnection.ForeColor = Color.Green;
            sendSerialData("Conn");
            return serialPort1.IsOpen;
        }
        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialConnection serial = new SerialConnection(this);
            serial.ShowDialog();
        }
        string ReadDataSerial,dataSerialReceived;

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ReadDataSerial = serialPort1.ReadExisting();
                this.Invoke(new EventHandler(dataReceived));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataReceived(object sender, EventArgs e)
        {
            try
            {
                dataSerialReceived += ReadDataSerial;
                toolStripStatusReceive.Text = dataSerialReceived.Replace("\r\n", string.Empty);
                if (dataSerialReceived.Contains(">") && dataSerialReceived.Contains("<"))
                {
                    string data = dataSerialReceived.Replace("\r\n", string.Empty);
                    dataSerialReceived = string.Empty;
                    
                }
                else if (!dataSerialReceived.Contains(">"))
                {
                    dataSerialReceived = string.Empty;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                toolStripStatusConnection.Text = "Connection: " + PortName + " " + BaudRate;
            }
            else
            {
                toolStripStatusConnection.Text = "Connection: Disconnected";
                toolStripStatusConnection.ForeColor = Color.Red;
                // reconnecttion
                
            }
        }

        public void sendSerialData(string data)
        {
            try
            {
                toolStripStatusData.Text = String.Empty;
                toolStripStatusData.Text = "Data: " + data;
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(">" + data + "<#");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    } 
}
