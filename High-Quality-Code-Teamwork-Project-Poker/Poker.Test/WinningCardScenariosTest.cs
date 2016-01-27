using System.Linq;
using System.Windows.Forms;
using Poker.Models.Cards;

namespace Poker.Test
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Poker.Contracts;
    using Poker.Models;
    using Poker.PokerMechanics;

    [TestClass]
    public class WinningCardScenariosTest
    {
        private static DeckOfCards deckOfCards;
        private static List<int> allCards;
        private static HandRanking ranking;
        private IGameParticipant participant;
        private GameRules rules;

        [ClassInitialize]
        public static void ClassInitialization(TestContext context)
        {
            deckOfCards = new DeckOfCards("Assets\\Cards", "*.png");
            ranking = new HandRanking();
            allCards = InitializeParticipantsCards();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {         
        }

        [TestInitialize]
        public void TestInitialize()
        {
            participant = new GameParticipant();
            participant.ParticipantPanel = new GameParticipantPanel();
            rules = new GameRules(ranking, participant);
        }

        [TestCleanup]
        public void TestCleanup()
        {          
        }

        [TestMethod]
        public void StraightFlush_HaveIt()
        {
            int[] potCards = this.AddPotCards(52, 46, 40, 5, 7);
            ranking.Reserve = potCards;

            rules.ExecuteGameRules(6, 7, participant);

            Assert.AreEqual(participant.Type, 8, "If participant hand type is 8, he has straight flush");
        }

        /// <summary>
        /// After distrubiting cards to the players this methods adds cards which will be on the desk
        /// </summary>
        /// <param name="cardOne">First showed card on the table</param>
        /// <param name="cardTwo">Second showed card on the table</param>
        /// <param name="cardThree">Third showed card on the table</param>
        /// <param name="cardFour">Fourth showed card on the table</param>
        /// <param name="cardFive">Fifth showed card on the table</param>
        /// <returns></returns>
        private int[] AddPotCards(int cardOne, int cardTwo, int cardThree, int cardFour, int cardFive)
        {
            allCards.Add(cardOne);
            allCards.Add(cardTwo);
            allCards.Add(cardThree);
            allCards.Add(cardFour);
            allCards.Add(cardFive);

            return allCards.ToArray();
        }

        /// <summary>
        /// This method initialize "random" cards to the participants, not to the table
        /// </summary>
        /// <returns>Returns participants' cards</returns>
        private static List<int> InitializeParticipantsCards()
        {
            // player cards
            int Diamond_7 = 26;
            int Spade_J = 44;

            // bot 1 cards
            int Club_4 = 13;
            int Diamond_2 = 6;

            // bot 2 cards
            int Spade_6 = 24;
            int Club_5 = 17;

            // bot 3 cards
            int Hearth_A = 3;
            int Club_10 = 37;

            // bot 4 cards
            int Club_6 = 21;
            int Diamond_9 = 34;

            // bot 5 cards
            int Spade_9 = 36;
            int Club_J = 41;

            int[] participantsCards = new int[]
            {
                Diamond_7, Spade_J,
                Club_4, Diamond_2,
                Spade_6, Club_5,
                Hearth_A, Club_10,
                Club_6, Diamond_9,
                Spade_9, Club_J
            };

            return participantsCards.ToList();
        }
    }
}
