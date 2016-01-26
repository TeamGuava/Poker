namespace Poker.Contracts
{
    using System.Drawing;

    public interface ICard
    {
        string Name { get; }

        Image Image { get; }
    }
}
