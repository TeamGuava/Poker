namespace Poker.UI
{
    using System.Windows.Forms;
    using Poker.Contracts;

    public class ApplicationWriter : IWriter
    {
        public void Print(string message)
        {
            MessageBox.Show(message);
        }
    }
}
