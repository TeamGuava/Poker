namespace Poker.UI
{
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Poker.Contracts;

    public class ApplicationDrawer : IApplicationDrawer
    {
        private const int AllCardsOnTheTable = 17;

        private Image[] deck;
        private IDeckOfCards deckOfCards;
        private Image backImage;
        private IGameRules rules;
        private IHandRanking handRanking;
        private IBot[] gameBots;

        public ApplicationDrawer(
            IBot[] gameBots, 
            IHandRanking handRanking, 
            IGameRules rule, 
            IDeckOfCards deckOfCards, 
            Image[] deck, Image backImage)
        {
            this.gameBots = this.gameBots;
            this.handRanking = handRanking;
            this.rules = rule;
            this.deckOfCards = deckOfCards;
            this.deck = deck;
            this.backImage = backImage;
        }

        public void DrawCards(Point[] cardLocations, Control.ControlCollection controls)
        {
            for (int currentCard = 0; currentCard < AllCardsOnTheTable; currentCard++)
            {
                // Next card from the deck. // Aleksandar
                ICard drawnCard = this.deckOfCards.DrawOneCard();

                this.deck[currentCard] = drawnCard.Image;

                // TODO: The name of the card file should not be used like this. // Aleksandar
                this.handRanking.Reserve[currentCard] = int.Parse(drawnCard.Name) - 1;

                this.rules.CardImages[currentCard] = new PictureBox();
                this.rules.CardImages[currentCard].SizeMode = PictureBoxSizeMode.StretchImage;
                this.rules.CardImages[currentCard].Height = 130;
                this.rules.CardImages[currentCard].Width = 80;
                this.rules.CardImages[currentCard].Image = this.backImage;
                this.rules.CardImages[currentCard].Location = cardLocations[currentCard];
                controls.Add(this.rules.CardImages[currentCard]);
                this.rules.CardImages[currentCard].Name = "pb" + currentCard; 

                Task.Delay(200);

                // Throwing Cards
                if (currentCard < 2)
                {
                    if (this.rules.CardImages[0].Tag != null)
                    {
                        this.rules.CardImages[1].Tag = this.handRanking.Reserve[1];
                    }

                    this.rules.CardImages[0].Tag = this.handRanking.Reserve[0];
                    this.rules.CardImages[currentCard].Image = this.deck[currentCard];
                    this.rules.CardImages[currentCard].Anchor = AnchorStyles.Bottom;
                }

                if (this.gameBots[0].Chips > 0)
                {
                    if (currentCard >= 2 && currentCard < 4)
                    {
                        this.SetBotCards(
                            this.rules.CardImages,
                            this.handRanking.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[1].Chips > 0)
                {
                    if (currentCard >= 4 && currentCard < 6)
                    {
                        this.SetBotCards(
                            this.rules.CardImages,
                            this.handRanking.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[2].Chips > 0)
                {
                    if (currentCard >= 6 && currentCard < 8)
                    {
                        this.SetBotCards(
                            this.rules.CardImages,
                            this.handRanking.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[3].Chips > 0)
                {
                    if (currentCard >= 8 && currentCard < 10)
                    {
                        this.SetBotCards(
                            this.rules.CardImages,
                            this.handRanking.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[4].Chips > 0)
                {
                    if (currentCard >= 10 && currentCard < 12)
                    {
                        this.SetBotCards(
                            this.rules.CardImages,
                            this.handRanking.Reserve,
                            currentCard);
                    }
                }

                // Printing the five river cards
                if (currentCard >= 12)
                {
                    this.rules.CardImages[currentCard].Tag = 
                        this.handRanking.Reserve[currentCard];
                    if (this.rules.CardImages[currentCard] != null)
                    {
                        this.rules.CardImages[currentCard].Anchor = AnchorStyles.None;
                    }
                }
            }
        }

        private void SetBotCards(PictureBox[] cardImages,
            int[] reserve,
            int currentCard)
        {
            cardImages[currentCard].Tag = reserve[currentCard];
            cardImages[currentCard].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cardImages[currentCard].Visible = true;
        }
    }
}