namespace Poker.Contracts
{
    public interface IFolder
    {
        bool FoldTurn { get; set; }

        void ChooseToFold();
    }
}
