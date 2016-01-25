namespace Poker.Models
{
    using Poker.Contracts;

    public class Player : GameParticipant, IPlayer
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
