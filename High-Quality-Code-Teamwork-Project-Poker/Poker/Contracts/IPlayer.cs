namespace Poker.Contracts
{
    public interface IPlayer : ICaller, IRaiser, IFold, IChecker, IGameParticipant
    {
    }
}
