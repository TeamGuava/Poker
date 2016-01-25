namespace Poker.Contracts
{
    public interface ICaller
    {
        //void Call();

        int Call { get; set; }

        //// TODO: Should be fixed without paramether.
        //void ChooseToCall(IGameParticipant gameParticipant);
    }
}
