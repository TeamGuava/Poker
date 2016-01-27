namespace Poker.Contracts
{
    using Poker.Models;

    public interface IGameParticipant : ICaller, IRaiser, IFolder, IChecker
    {
        bool Turn { get; set; }

        int Chips { get; set; }

        int Power { get; set; }

        double Type { get; set; }

        GameParticipantPanel ParticipantPanel { get; set; }
    }
}
