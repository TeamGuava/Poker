namespace Poker.Contracts
{
    using System.Collections.Generic;

    using Poker.Models.Cards;

    public interface IDeckOfCards
    {
        IList<Card> ListOfCards { get; }

        void ShuffleDeck();

        ICard DrawOneCard();
    }
}
