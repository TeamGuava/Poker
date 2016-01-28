namespace Poker.Models
{
    using Poker.Contracts;

    public abstract class GameParticipant : IGameParticipant
    {
        private const int StartChips = 10000;

        protected GameParticipant()
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

        //public void ChooseToCall(IGameParticipant currentChooseToCall);

        //public void ChooseToRaise(IGameParticipant currentChooseToCall);

        public void ChooseToFold(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.RaiseTurn = false;
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Is Folded";
            currentGameParticipant.Turn = false;
            currentGameParticipant.FoldTurn = true;
        }

        public void ChooseToCheck(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Check";
            currentGameParticipant.Turn = false;
            currentGameParticipant.RaiseTurn = false;
        }
    }
}
