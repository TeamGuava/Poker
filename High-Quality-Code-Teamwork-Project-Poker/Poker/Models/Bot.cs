namespace Poker.Models
{
    using System.Collections.Generic;

    // This class is a model. It should be upgraded!
    public class Bot : GameParticipant
    {
        private const int StartChips = 10000;

        public Bot()
            : base()
        {
            this.Chips = StartChips;
            this.ParticipantPanel = new GameParticipantPanel();
        }
    }
}
