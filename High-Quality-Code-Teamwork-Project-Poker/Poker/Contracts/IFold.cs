namespace Poker.Contracts
{
    public interface IFold
    {
        bool FoldTurn { get; set; }

        //bool IsFolded { get; set; }

        void ChooseToFold();
    }
}
