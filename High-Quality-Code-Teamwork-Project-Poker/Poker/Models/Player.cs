namespace Poker.Models
{
    using System;
    using System.Windows.Forms;

    public class Player
    {
        private int chips;
        private int call;

        public Player(int chips)
        {
            this.Chips = chips;
            this.Call = 0;
            this.Raise = 0;
            this.Folded = false;
            this.PlayerPanel = new Panel();
        }

        // TODO: Validations!
        public int Chips { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public bool Folded { get; set; }

        public Panel PlayerPanel { get; set; }

    }
}
