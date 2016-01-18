namespace Poker.Models
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    // This class is a model. It should be upgraded! Interface is needed!
    public class Bot : Panel
    {
        public Bot()
        {
                
        }
        
        public List<bool?> Bools { get; set; }

        public int Chips { get; set; }

        public bool FoldTurn { get; set; }

        public bool Turn { get; set; }

        public int Raise { get; set; }
        
        public double Type { get; set; }

        public int Power { get; set; }

        public bool Folded { get; set; }

        public Label StatusButton { get; set; }

        public int Call { get; set; }

        public TextBox ChipsTextBox { get; set; }
    }
}
