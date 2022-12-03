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
                    if(e.KeyCode == Keys.Enter)
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
            if (tbLabel.Text == tbECU.Text)
            {
                JudgeMentOutoput("OK");
            }
            else
            {



                JudgeMentOutoput("NG");
            }
            // Save to database

            // Clear data
            ClearTextBox();
        }
        private void JudgeMentOutoput(string data)
        {
            data.ToUpper();
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

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting settingPage = new Setting();
            settingPage.ShowDialog();
        }
    }
}
