namespace Poker.Contracts
{
    public interface ICaller
    {
        int Call { get; set; }

        //// TODO: Should be fixed without paramether.
        //void ChooseToCall(IGameParticipant gameParticipant);
    }
}
