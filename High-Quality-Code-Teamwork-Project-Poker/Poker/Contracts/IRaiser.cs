﻿namespace Poker.Contracts
{
    public interface IRaiser
    {
        int Raise { get; set; }

        //// TODO: Should be fixed without paramether.
        //void ChooseToRaise(IGameParticipant gameParticipant);
    }
}
