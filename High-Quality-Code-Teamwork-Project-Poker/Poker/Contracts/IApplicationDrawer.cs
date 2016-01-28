namespace Poker.Contracts
{
    using System.Drawing;
    using System.Windows.Forms;

    public interface IApplicationDrawer
    {
        void DrawCards(
            Point[] cardLocations, 
            Control.ControlCollection controls);
    }
}
