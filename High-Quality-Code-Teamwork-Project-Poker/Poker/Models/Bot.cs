namespace Poker.Models
{
    using System.Collections.Generic;

    // This class is a model. It should be upgraded!
    public class Bot : GameParticipant
    {
        public Bot()
            : base()
        {
                this.ParticipantPanel = new GameParticipantPanel();
        }
    }
}
