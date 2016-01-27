namespace Poker.Models
{
    using Poker.Contracts;

    public class GameParticipant : IGameParticipant
    {
        // TODO:
        // This field should be in other class, extracted from GameEngine later.
        // It is used temporarily here as public (!) in order to extract game participant's 
        // method ChooseToCheck(..) from GameEngine (to use an appropriate interface)
        //public static bool raising;

        private const int StartChips = 10000;

        public GameParticipant()
        {
            this.ParticipantPanel = new GameParticipantPanel();
            this.Chips = StartChips;
        }

        // TODO: VALIDATION
        public int Call { get; set; }

        public int Raise { get; set; }

        public bool FoldTurn { get; set; }

        public bool RaiseTurn { get; set; }

        public bool Turn { get; set; }

        public int Chips { get; set; }

        public int Power { get; set; }

        public double Type { get; set; }

        public GameParticipantPanel ParticipantPanel { get; set; }

        public void ChooseToCall()
        {
            this.RaiseTurn = false;
            this.Turn = false;
            this.Chips -= this.Call;
            this.ParticipantPanel.Text = "Call " + this.Call;
           // this.potTextBox.Text = (int.Parse(this.potTextBox.Text) + this.call).ToString();
        }

        public void ChooseToRaise()
        {
            this.Chips -= this.Raise;
            this.ParticipantPanel.StatusButton.Text = "Raise " + this.Raise;
            // this.potTextBox.Text = (int.Parse(this.potTextBox.Text) + currentGameParticipant.Raise).ToString();
            this.RaiseTurn = true;
            this.Turn = false;
        }

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
