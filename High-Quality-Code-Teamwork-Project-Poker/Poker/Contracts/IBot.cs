namespace Poker.Contracts
{
    public interface IBot : IGameParticipant
    {
        void MakeDecision();
    }
}
