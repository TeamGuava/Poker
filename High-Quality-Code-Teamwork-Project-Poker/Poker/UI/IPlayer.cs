namespace Poker.UI
{
    public interface IPlayer : ICaller, IRaiser, IFold, IChecker
    {
        // TO DO: public properties

        void Call();

        void Raise();

        void Check();

        void Fold(); 
    }
}
