namespace Poker.Models.Cards
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Poker.Contracts;

    public class DeckOfCards : IDeckOfCards, ICardDealer
    {
        private IList<Card> listOfCards;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckOfCards" /> class.
        /// </summary>
        public DeckOfCards(string directory, string extention)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("Cannot provide an empty path to the cards directory.");
            }

            if (extention == null)
            {
                throw new ArgumentNullException(
                    "Cannot provide an empty extension of the card file images.");
            }

            this.ListOfCards = this.LoadCardsFromDirectory(directory, extention);
        }

        public IList<Card> ListOfCards
        {
            get
            {
                return this.listOfCards;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(
                        "No list of cards was provided.");
                }

                if (value.Count == 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "A deck of cards cannot have 0 cards.");
                }

                this.listOfCards = value;
            }
        }
      
        /// <summary>
        /// Randomly shuffles the cards in the deck.
        /// </summary>
        public void ShuffleDeck()
        {
            Random random = new Random();

            for (int i = this.ListOfCards.Count; i > 0; i--)
            {
                int generatedRandomNumber = random.Next(i);
                var k = this.ListOfCards[generatedRandomNumber];
                this.ListOfCards[generatedRandomNumber] = this.ListOfCards[i - 1];
                this.ListOfCards[i - 1] = k;
            }
        }

        /// <summary>
        /// Draw one card from the deck. This process permanently removes the card from the deck.
        /// </summary>
        /// <returns></returns>
        public ICard DrawOneCard()
        {
            if (this.listOfCards.Count == 0)
            {
                throw new ArgumentOutOfRangeException("No cards are left in the deck!");
            }

            Card drawnCard = this.listOfCards[0];
            this.listOfCards.RemoveAt(0);

            return drawnCard;
        }

        /// <summary>
        /// Extracts the card name from the location where it can be found.
        /// </summary>
        /// <param name="location">Location of the card.</param>
        /// <returns>The name of the card from a given location.</returns>
        private string ExtractCardNameFromLocation(string location)
        {
            string name = location;

            if (name.Contains("\\"))
            {
                name = name.Substring(name.LastIndexOf("\\") + 1);
            }

            if (name.Contains("."))
            {
                name = name.Substring(0, name.LastIndexOf("."));
            }

            return name;
        }

        /// <summary>
        /// Loads a list of cards from a given location.
        /// </summary>
        /// <param name="directory">Directory in which the cards are located.</param>
        /// <param name="extention">Extension of the files, which represent the cards.</param>
        /// <returns>List of cards.</returns>
        private IList<Card> LoadCardsFromDirectory(
            string directory, 
            string extention)
        {
            string[] cardsInDirectory = Directory.GetFiles(
                directory,
                extention,
                SearchOption.TopDirectoryOnly);

            IList<Card> tempListOfCards = new List<Card>();

            foreach (var cardLocation in cardsInDirectory)
            {
                string cardName = this.ExtractCardNameFromLocation(cardLocation);

                tempListOfCards.Add(new Card(cardName, cardLocation));
            }

            return tempListOfCards;
        }
    }
}
