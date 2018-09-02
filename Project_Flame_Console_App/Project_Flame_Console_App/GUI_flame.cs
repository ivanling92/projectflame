using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Flame_Console_App
{
    public partial class GUI_flame : Form
    {

        public GUI_flame()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.user_started = true;
            TwitterData.Twitterhandle = textBox1.Text;
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.user_started = false;
            TwitterData.Twitterhandle = "Projectflame_MY";
            textBox1.Text = "";
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}
