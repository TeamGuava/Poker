using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker
{
    public partial class AddChips : Form
    {
        public int a=0;
        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            InitializeComponent();
            ControlBox = false;
            label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (int.Parse(textBox1.Text) > 100000000)
            {
                MessageBox.Show("The maximium chips you can add is 100000000");
                return;
            }
            if (!int.TryParse(textBox1.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                return;

            }
            else if (int.TryParse(textBox1.Text, out parsedValue) && int.Parse(textBox1.Text) <= 100000000)
            {
                a = int.Parse(textBox1.Text);
                this.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(
            message,title,
            MessageBoxButtons.YesNo, 
            MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
        }
    }
}
