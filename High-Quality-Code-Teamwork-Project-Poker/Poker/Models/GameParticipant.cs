namespace Poker.Models
{
    using Poker.Contracts;

    public abstract class GameParticipant : IGameParticipant
    {
        protected GameParticipant()
        {
            
        }

        // TODO: VALIDATION
        public int Call { get; set; }

        public int Raise { get; set; }

        public bool FoldTurn { get; set; }

        public bool IsFolded { get; set; }

        public bool Turn { get; set; }

        public int Chips { get; set; }

        public int Power { get; set; }

        public double Type { get; set; }

        public GameParticipantPanel ParticipantPanel { get; set; }
    }
}
