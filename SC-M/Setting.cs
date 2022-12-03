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

        private void Setting_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (tbSoftwareECU.Text != "" && tbSoftwareLabel.Text != "")
            {
                MasterData md = new MasterData();
                md.softwareLabel = tbSoftwareLabel.Text;
                md.softwareECU = tbSoftwareECU.Text;
                md.Save();
                MessageBox.Show("Added");
            }
            else
            {
                MessageBox.Show("Please fill all fields");
            }
            LoadData();
        }

        private void LoadData()
        {
            var list = MasterData.GetAll();
            // dataGridView1
            dataGridView1.DataSource = list;
        }
    }
}
