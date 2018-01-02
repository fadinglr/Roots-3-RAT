using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            textBox6.ReadOnly = true;
            textBox6.ScrollBars = ScrollBars.Vertical;

            string text = File.ReadAllText(@"C:\Users\" + Environment.UserName + @"\rootsLOG.txt");
            textBox6.AppendText(text);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
