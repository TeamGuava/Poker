using System;
using System.Linq;
using System.Windows.Forms;

namespace Poker.PokerMechanics
{
    using Poker.Contracts;

    public class GameRules : IGameRules
    {
        private IGameParticipant player;

        public GameRules(HandRanking handRanking, IGameParticipant player)
        {
            this.HandRanking = handRanking;
            this.player = player;
            this.CardImages = new PictureBox[52];
        }

        public HandRanking HandRanking { get; private set; }

        public PictureBox[] CardImages { get; set; }

        public void ExecuteGameRules(
            int firstCard,
            int secondCard,
            IGameParticipant currentGameParticipant)
        {
            //if (firstCard == 0 &&
            //    secondCard == 1)
            //{
            //}

            if (!currentGameParticipant.FoldTurn ||
                firstCard == 0 &&
                secondCard == 1 &&
                this.player.ParticipantPanel.StatusButton.Text.Contains("Fold") == false)
            {
                //int[] straight1 = new int[5];
                int[] straight = new int[7];

                straight[0] = this.HandRanking.Reserve[firstCard];
                straight[1] = this.HandRanking.Reserve[secondCard];
                straight[2] = this.HandRanking.Reserve[12];
                straight[3] = this.HandRanking.Reserve[13];
                straight[4] = this.HandRanking.Reserve[14];
                straight[5] = this.HandRanking.Reserve[15];
                straight[6] = this.HandRanking.Reserve[16];

                var a = straight.Where(o => o % 4 == 0).ToArray();
                var b = straight.Where(o => o % 4 == 1).ToArray();
                var c = straight.Where(o => o % 4 == 2).ToArray();
                var d = straight.Where(o => o % 4 == 3).ToArray();

                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();

                Array.Sort(straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);

                for (int cardIndex = 0; cardIndex < 17; cardIndex++)
                {
                    // bool  
                    if (this.HandRanking.Reserve[cardIndex] == int.Parse(
                        this.CardImages[firstCard].Tag.ToString()) &&
                        this.HandRanking.Reserve[cardIndex + 1] == int.Parse(
                            this.CardImages[secondCard].Tag.ToString()))
                    {
                        //Pair from Hand current = 1
                        this.HandRanking.rPairFromHand(currentGameParticipant);

                        // Pair or Two Pair from Table current = 2 || 0
                        this.HandRanking.rPairTwoPair(currentGameParticipant);

                        // Two Pair current = 2
                        this.HandRanking.rTwoPair(currentGameParticipant);

                        // Three of a kind current = 3
                        this.HandRanking.rThreeOfAKind(currentGameParticipant, straight);

                        // straight current = 4
                        this.HandRanking.rStraight(currentGameParticipant, straight);

                        // Flush current = 5 || 5.5
                        this.HandRanking.rFlush(currentGameParticipant, straight);

                        // Full House current = 6
                        this.HandRanking.rFullHouse(currentGameParticipant, straight);

                        // Four of a Kind current = 7
                        this.HandRanking.rFourOfAKind(currentGameParticipant, straight);

                        // straight Flush current = 8 || 9
                        this.HandRanking.rStraightFlush(currentGameParticipant, st1, st2, st3, st4);

                        // High Card current = -1
                        this.HandRanking.rHighCard(currentGameParticipant);
                    }
                }
            }
        }

    }
}
