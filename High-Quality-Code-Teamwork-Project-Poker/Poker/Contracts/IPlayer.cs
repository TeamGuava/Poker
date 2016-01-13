namespace Poker.Contracts
{
    public interface IPlayer : ICaller, IRaiser, IFold, IChecker
    {
        // TO DO: public properties
        int Chips { get; }

    }
}
