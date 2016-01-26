namespace Poker.Contracts
{
    public interface IGameRules
    {
        void ExecuteGameRules(
            int firstCard, 
            int secondCard, 
            IGameParticipant currentGameParticipant);
    }
}
