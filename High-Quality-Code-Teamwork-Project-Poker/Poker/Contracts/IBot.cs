namespace Poker.Contracts
{
    public interface IBot : IPlayer
    {
        void MakeDecision();
    }
}
