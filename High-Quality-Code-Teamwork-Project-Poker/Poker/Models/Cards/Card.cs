namespace Poker.Models.Cards
{
    using System;
    using System.Drawing;

    using Poker.Contracts;

    public class Card : ICard
    {
        private string name;
        private Image image;

        public Card(string name, string imageLocation)
        {
            this.Name = name;
            this.Image = Image.FromFile(imageLocation);
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(
                        "The name of the card cannot be empty.");
                }

                this.name = value;
            }
        }

        public Image Image
        {
            get
            {
                return this.image;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(
                        "The Image of the card cannot be null.");
                }

                this.image = value;
            }
        }
    }
}