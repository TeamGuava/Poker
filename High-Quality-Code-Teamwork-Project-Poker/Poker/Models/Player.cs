namespace Poker.Models
{
    public class Player : GameParticipant
    {
        private const int StartChips = 10000;

        public Player()
            : base()
        {
            this.Chips = StartChips;
            this.Call = 0;
            this.Raise = 0;
            this.Turn = true;
            this.IsFolded = false;
        }
    }
}
