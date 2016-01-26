namespace Poker
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// This class represents the logic of the AddChips form, which is used to add more chips to all participants in the game (player and bots).
    /// </summary>
    public partial class AddChips : Form
    {
        /// <summary>
        /// Maximum number of chips that can be added to the game at once.
        /// </summary>
        private const int MaxNumberOfChipsToAdd = 100 * 1000 * 1000;

        /// <summary>
        /// New chips that are to be added to the game.
        /// </summary>
        private int newChips;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddChips" /> class.
        /// </summary>
        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            this.InitializeComponent();
            this.ControlBox = false;
            this.labelMainMessage.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// The NewChips property represents the new chips that are to be added to the game.
        /// </summary>
        /// <value>The Name property gets/sets the value of the field newChips.</value>
        public int NewChips
        {
            get
            {
                return this.newChips;
            }

            set
            {
                // The chips value cannot be negative.
                if (value < 0)
                {
                    value = 0;
                }

                // Checking if the given number in textBoxAddNewChips exceeds the maximum.
                if (value > MaxNumberOfChipsToAdd)
                {
                    MessageBox.Show(
                        "Too many chips were given! The maximium chips you can add is 100 000 000.");
                }

                this.newChips = value;
            }
        }

        /// <summary>
        /// Takes the given number of chips in TextBox "AddNewChips".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClickButtonAddChips(object sender, EventArgs e)
        {
            int parsedValue;

            // Checking, if the text in textBoxAddNewChips is a number.
            if (!int.TryParse(this.textBoxAddNewChips.Text, out parsedValue))
            {
                MessageBox.Show(
                    "This is a number only field! " + "The entered text " + this.textBoxAddNewChips.Text + "is not a number.");
            }
            else
            {
                this.NewChips = int.Parse(this.textBoxAddNewChips.Text);
                this.Close();
            }
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickButtonExit(object sender, EventArgs e)
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
