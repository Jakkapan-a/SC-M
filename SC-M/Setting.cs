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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }
        int dataGridViewSelectedId = 0;
        MasterData master = new MasterData(); 

        private void Setting_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (tbSoftwareECU.Text != "" && tbSoftwareLabel.Text != "" && (tbSoftwareLabel.Text != master.softwareLabel || tbSoftwareECU.Text != master.softwareECU))

            {
                MasterData md = new MasterData();
                md.softwareLabel = tbSoftwareLabel.Text;
                md.softwareECU = tbSoftwareECU.Text;
                md.Save();
                MessageBox.Show("Added", "Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Duplicate information.", "Information",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            LoadData();
        }

        private void LoadData()
        {
            var list = MasterData.GetAll();
            int num = 1;
            var list2 = (from x in list
                         select new
                         {
                             ID= x.id,
                             No = num++,
                             Software_On_Labe =x.softwareLabel,
                             Software_On_ECU= x.softwareECU,
                             Update = x.updated_at,
                         }).ToList();
            // dataGridView1
            dataGridView1.DataSource = list2;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = (int)(dataGridView1.Width * 0.1);
            dataGridView1.Columns[dataGridView1.ColumnCount-1].Width= (int)(dataGridView1.Width * 0.3);
            dataGridView1.Update();
        }
        
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dynamic row = dataGridView1.SelectedRows[0].DataBoundItem;
                master.id = row.ID;
                master.softwareLabel = row.Software_On_Labe;
                master.softwareECU = row.Software_On_ECU;
                toolStripStatusLabel1.Text = "Selected ID: " + master.id;
                
                tbSoftwareLabel.Text = master.softwareLabel;
                tbSoftwareECU.Text = master.softwareECU;
            }
        }

        private void btChange_Click(object sender, EventArgs e)
        {
            try
            {
                MasterData md = new MasterData();
                md.id = master.id;
                md.softwareLabel = tbSoftwareLabel.Text;
                md.softwareECU = tbSoftwareECU.Text;
                md.Update();
                MessageBox.Show("Updated","Information",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                LoadData();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error " + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                MasterData md = new MasterData();
                md.id = master.id;
                md.Delete();
                MessageBox.Show("Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
