namespace Poker.Contracts
{
    using System.Drawing;

    public interface ICard
    {
        string Name { get; set; }

        Image Image { get; set; }
    }
}

