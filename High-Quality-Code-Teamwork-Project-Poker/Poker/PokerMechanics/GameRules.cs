namespace Poker.PokerMechanics
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Poker.Contracts;

    public class GameRules : IGameRules
    {
        private IGameParticipant player;

        public GameRules(IHandRanking handRanking, IGameParticipant player)
        {
            this.HandRanking = handRanking;
            this.player = player;
            this.CardImages = new PictureBox[52];
        }

        public IHandRanking HandRanking { get; private set; }

        public PictureBox[] CardImages { get; set; }

        /// <summary>
        /// Executes the cheking of the game participant type and power.
        /// </summary>
        /// <param name="firstCard">First card of the participant.</param>
        /// <param name="secondCard">Second card of the participant.</param>
        /// <param name="currentGameParticipant">Participant.</param>
        public void ExecuteGameRules(
            int firstCard,
            int secondCard,
            IGameParticipant currentGameParticipant)
        {
            if (!currentGameParticipant.FoldTurn ||
                firstCard == 0 &&
                secondCard == 1 &&
                this.player.ParticipantPanel.StatusButton.Text.Contains("Fold") == false)
            {
                // The array represents the cards in the hand and table together
                int[] straight = new int[7];

                straight[0] = this.HandRanking.Reserve[firstCard];
                straight[1] = this.HandRanking.Reserve[secondCard];
                straight[2] = this.HandRanking.Reserve[12];
                straight[3] = this.HandRanking.Reserve[13];
                straight[4] = this.HandRanking.Reserve[14];
                straight[5] = this.HandRanking.Reserve[15];
                straight[6] = this.HandRanking.Reserve[16];

                // Array only of Clubs
                var clubs = straight.Where(o => o % 4 == 0).ToArray();

                // Array only of Diamonds
                var diamonds = straight.Where(o => o % 4 == 1).ToArray();

                // Array only of Hearths
                var hearths = straight.Where(o => o % 4 == 2).ToArray();

                // Array only of Spades
                var spades = straight.Where(o => o % 4 == 3).ToArray();

                var st1 = clubs.Select(o => o / 4).Distinct().ToArray();
                var st2 = diamonds.Select(o => o / 4).Distinct().ToArray();
                var st3 = hearths.Select(o => o / 4).Distinct().ToArray();
                var st4 = spades.Select(o => o / 4).Distinct().ToArray();

                Array.Sort(straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);

                for (int cardIndex = 0; cardIndex < 17; cardIndex++)
                {
                    bool haveCards = 
                        this.HandRanking.Reserve[cardIndex] == int.Parse(
                            this.CardImages[firstCard].Tag.ToString()) &&
                        this.HandRanking.Reserve[cardIndex + 1] == int.Parse(
                            this.CardImages[secondCard].Tag.ToString());
                    if (haveCards)
                    {
                        //Pair from Hand current = 1
                        this.HandRanking.TryFindingPairFromHand(currentGameParticipant);

                        // Pair or Two Pair from Table current = 2 || 0
                        this.HandRanking.TryFindingPairTwoPair(currentGameParticipant);

                        // Two Pair current = 2
                        this.HandRanking.TryFindingTwoPair(currentGameParticipant);

                        // Three of a kind current = 3
                        this.HandRanking.TryFindingThreeOfAKind(currentGameParticipant, straight);

                        // straight current = 4
                        this.HandRanking.TryFindingStraight(currentGameParticipant, straight);

                        // Flush current = 5 || 5.5
                        this.HandRanking.TryFindingFlush(currentGameParticipant, straight);

                        // Full House current = 6
                        this.HandRanking.TryFindingFullHouse(currentGameParticipant, straight);

                        // Four of a Kind current = 7
                        this.HandRanking.TryFindingFourOfAKind(currentGameParticipant, straight);

                        // straight Flush current = 8 || 9
                        this.HandRanking.TryFindingStraightFlush(currentGameParticipant, st1, st2, st3, st4);

                        // High Card current = -1
                        this.HandRanking.TryFindingHighCard(currentGameParticipant);
                    }
                }
            }
        }
    }
}
