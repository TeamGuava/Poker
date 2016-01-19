namespace Poker.Models
{
    public class Player : GameParticipant
    {
        public Player(int chips)
            : base()
        {
            this.Chips = chips;
            this.Call = 0;
            this.Raise = 0;
            this.IsFolded = false;
            this.ParticipantPanel = new GameParticipantPanel();
        }
    }
}
