namespace Poker.Contracts
{
    using System.Collections.Generic;

    public interface IHandRanking
    {
        List<Type> Win { get; }

        int[] Reserve { get; }

        IType WinningHand { get; }

        double Type { get; set; }

        void rStraightFlush(IGameParticipant currentGameParticipant, int[] st1, int[] st2, int[] st3, int[] st4);

        void rFourOfAKind(IGameParticipant currentGameParticipant, int[] straight);

        void rFullHouse(IGameParticipant currentGameParticipant, int[] straight);

        void rFlush(IGameParticipant currentGameParticipant, int[] straight);

        void rStraight(IGameParticipant currentGameParticipant, int[] straight);

        void rThreeOfAKind(IGameParticipant currentGameParticipant, int[] straight);

        void rTwoPair(IGameParticipant currentGameParticipant);

        void rPairTwoPair(IGameParticipant currentGameParticipant);

        void rPairFromHand(IGameParticipant currentGameParticipant);

        void rHighCard(IGameParticipant currentGameParticipant);
    }
}
