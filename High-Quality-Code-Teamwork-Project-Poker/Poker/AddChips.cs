namespace Poker
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        private int chips = 0;

        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            this.InitializeComponent();
            this.ControlBox = false;
            this.labelMainMessage.BorderStyle = BorderStyle.FixedSingle;
        }

        public int NewChips
        {
            get
            {
                return this.chips;
            }

            set
            {
                this.chips = value;
            }
        }

        public void ButtonAddChips_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(this.textBoxAddNewChips.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");

                return;
            }
            else if (int.Parse(this.textBoxAddNewChips.Text) > 100000000)
            {
                MessageBox.Show("The maximium chips you can add is 100000000");

                return;
            }
            else
            {
                this.NewChips = int.Parse(this.textBoxAddNewChips.Text);
                this.Close();
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(
                                message,
                                title,
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
