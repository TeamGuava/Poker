namespace Poker.Models
{
    public class Player : GameParticipant
    {
        public Player()
            : base()
        {
            this.Call = 0;
            this.Raise = 0;
            this.Turn = true;
        }
    }
}
