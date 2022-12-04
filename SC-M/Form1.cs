using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            LoadHistoryData();
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
            JudgeMentOutoput(judge);
            LoadHistoryData();
            // Clear data
            ClearTextBox();
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
    }
}
