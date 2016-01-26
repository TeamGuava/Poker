namespace Poker.Contracts
{
    using System.Windows.Forms;

    public interface IGameRules
    {
        IHandRanking HandRanking { get; }

        PictureBox[] CardImages { get; }

        void ExecuteGameRules(
            int firstCard, 
            int secondCard, 
            IGameParticipant currentGameParticipant);
    }
}
