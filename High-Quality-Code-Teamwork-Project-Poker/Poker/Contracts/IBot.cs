namespace Poker.Contracts
{
    using Poker.Models;

    public interface IBot : ICaller, IRaiser, IFolder, IChecker, IGameParticipant
    {
        //void MakeDecision();
    }
}
