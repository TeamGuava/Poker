namespace Poker.Models
{
    using System.Windows.Forms;

    public class GameParticipantPanel : Panel
    {
        public GameParticipantPanel()
        {
            this.StatusButton = new Label();
            this.ChipsTextBox = new TextBox();
        }

        public Label StatusButton { get; set; }

        public TextBox ChipsTextBox { get; set; }
    }
}
