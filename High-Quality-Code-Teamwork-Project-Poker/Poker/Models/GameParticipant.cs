namespace Poker.Models
{
    using System.Windows.Forms;
    using Poker.Contracts;

    public class GameParticipant : IGameParticipant
    {
        private const int StartChips = 10000;

        public GameParticipant()
        {
            this.ParticipantPanel = new GameParticipantPanel();
            this.Chips = StartChips;
        }

        public int Call { get; set; }

        public int Raise { get; set; }

        public bool FoldTurn { get; set; }

        public bool RaiseTurn { get; set; }

        public bool Turn { get; set; }

        public int Chips { get; set; }

        public int Power { get; set; }

        public double Type { get; set; }

        public GameParticipantPanel ParticipantPanel { get; set; }

        //public void ChooseToCall(TextBox potTextBox)
        //{
        //    this.RaiseTurn = false;
        //    this.Turn = false;
        //    this.Chips -= this.Call;
        //    this.ParticipantPanel.Text = "Call " + this.Call;
        //    potTextBox.Text = (int.Parse(potTextBox.Text) + this.Call).ToString();
        //}

        //public void ChooseToRaise(TextBox potTextBox)
        //{
        //    this.Chips -= this.Raise;
        //    this.ParticipantPanel.StatusButton.Text = "Raise " + this.Raise;
        //    potTextBox.Text = (int.Parse(potTextBox.Text) + this.Raise).ToString();
        //    this.RaiseTurn = true;
        //    this.Turn = false;
        //}

        public void ChooseToFold()
        {
            this.RaiseTurn = false;
            this.ParticipantPanel.StatusButton.Text = "Is Folded";
            this.Turn = false;
            this.FoldTurn = true;
        }

        public void ChooseToCheck()
        {
            this.ParticipantPanel.StatusButton.Text = "Check";
            this.Turn = false;
            this.RaiseTurn = false;
        }
    }
}
