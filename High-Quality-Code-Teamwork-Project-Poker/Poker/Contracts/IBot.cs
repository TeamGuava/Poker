namespace Poker.Contracts
{
    using Poker.Models;

    public interface IBot : ICaller, IRaiser, IFold, IChecker, IGameParticipant
    {
        //void MakeDecision();
    }
}
