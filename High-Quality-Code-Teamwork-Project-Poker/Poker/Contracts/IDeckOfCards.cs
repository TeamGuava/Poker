namespace Poker.Contracts
{
    using System.Collections.Generic;

    using Poker.Models.Cards;

    public interface IDeckOfCards
    {
        IList<Card> ListOfUsedCards { get; }

        void ShuffleDeck();

        ICard DrawOneCard();

        void RenewUsedDeck();
    }
}
