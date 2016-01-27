namespace Poker.Models.Cards
{
    using System;
    using System.Drawing;

    using Poker.Contracts;

    /// <summary>
    /// Represents the object Card.
    /// </summary>
    public class Card : ICard
    {
        private string name;
        private Image image;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card" /> class.
        /// </summary>
        public Card(string name, string imageLocation)
        {
            this.Name = name;
            this.Image = Image.FromFile(imageLocation); 
        }

        /// <summary>
        /// Represents the name of the object.
        /// </summary>
        /// <value>The Name property gets/sets the value of the field name.</value>
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

        /// <summary>
        /// Represents the image of the object.
        /// </summary>
        /// <value>The Image property gets/sets the value of the field image.</value>
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