namespace Poker.Contracts
{
    using System.Drawing;

    using Poker.Enums;

    public interface ICard
    {
        Image Picture { get; }

        //CardValue CardValue { get; }

        //SuitSign CardSign { get; }
    }
}
