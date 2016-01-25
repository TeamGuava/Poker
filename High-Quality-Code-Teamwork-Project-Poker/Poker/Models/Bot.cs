namespace Poker.Models
{
    using Poker.Contracts;

    public class Bot : GameParticipant, IBot
    {
        public Bot()
            : base()
        {
            this.Type = -1;
        }
    }
}
