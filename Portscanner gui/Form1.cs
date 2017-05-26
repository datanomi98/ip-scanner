using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portscanner_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i = 0;
            while (i < 4)
            {
                i++;
                string testi = i.ToString();
                //DataGridView ip = new DataGridView();
                ip.ColumnCount = 1;
                ip.Columns[0].Name = "IP";
                string firstvalue = "test";
                DataGridViewRow row = (DataGridViewRow)ip.Rows[0].Clone();
                string[] rows = new string[] { firstvalue };
                ip.Rows.Add(testi);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
