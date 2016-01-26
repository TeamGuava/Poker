namespace Poker.Contracts
{
    public interface ICardDealer
    {
        void ShuffleDeck();

        ICard DrawOneCard();
    }
}

