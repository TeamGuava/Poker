namespace Poker.Contracts
{
    using System.Collections.Generic;

    public interface IHandRanking
    {
        List<Type> Win { get; }

        int[] Reserve { get; }

        IType WinningHand { get; }

        double Type { get; set; }

        void TryFindingStraightFlush(IGameParticipant currentGameParticipant, int[] st1, int[] st2, int[] st3, int[] st4);

        void TryFindingFourOfAKind(IGameParticipant currentGameParticipant, int[] straight);

        void TryFindingFullHouse(IGameParticipant currentGameParticipant, int[] straight);

        void TryFindingFlush(IGameParticipant currentGameParticipant, int[] straight);

        void TryFindingStraight(IGameParticipant currentGameParticipant, int[] straight);

        void TryFindingThreeOfAKind(IGameParticipant currentGameParticipant, int[] straight);

        void TryFindingTwoPair(IGameParticipant currentGameParticipant);

        void TryFindingPairTwoPair(IGameParticipant currentGameParticipant);

        void TryFindingPairFromHand(IGameParticipant currentGameParticipant);

        void TryFindingHighCard(IGameParticipant currentGameParticipant);
    }
}
