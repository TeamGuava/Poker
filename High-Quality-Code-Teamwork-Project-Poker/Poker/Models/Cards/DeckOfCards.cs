namespace Poker.Models.Cards
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class DeckOfCards
    {
        private List<Card> listOfCards;
        private string locationOfTheCards;
        private string extentionOfTheImages;

        public DeckOfCards(string directory, string extention)
        {
            this.ListOfCards = LoadCardsFromDirectory(directory, extention);
        }

        public List<Card> ListOfCards
        {
            get { return this.listOfCards; }
            private set
            {
                if (value.Count == 0)
                {
                    throw new ArgumentOutOfRangeException("No cards were found to be loaded.");
                }
                this.listOfCards = value;
            }
        }

        private string LocationOfTheCards
        {
            get { return this.locationOfTheCards; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Cannot provide an empty path to the cards directory.");
                }

                this.locationOfTheCards = value;
            }
        }

        private string ExtentionOfTheImages
        {
            get { return this.extentionOfTheImages; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Cannot provide an empty extension of the card file images.");
                }

                this.extentionOfTheImages = value;
            }
        }

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

        private List<Card> LoadCardsFromDirectory(string directory, string extention)
        {
            string[] cardsInDirectory = Directory.GetFiles(directory, extention, SearchOption.TopDirectoryOnly);

            List<Card> tempListOfCards = new List<Card>();

            foreach (var cardLocation in cardsInDirectory)
            {
                string cardName = ExtractCardNameFromLocation(cardLocation);

                tempListOfCards.Add(new Card(cardName, cardLocation));
            }

            return tempListOfCards;
        }

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

        public Card DrawOneCard()
        {
            if (listOfCards.Count == 0)
            {
                // TODO: Create exception OutOfCardsException.
                throw new Exception("OutOfCards");
            }

            Card drawnCard = listOfCards[0];
            listOfCards.RemoveAt(0);

            return drawnCard;
        }

    }
}
