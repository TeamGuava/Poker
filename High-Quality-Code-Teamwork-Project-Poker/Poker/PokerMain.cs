namespace Poker
{
    using System;
    using System.Windows.Forms;

    using Poker.Models;
    using Poker.PokerMechanics;

    public static class PokerMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var gameParticipant = new GameParticipant();
            var handRanking = new HandRanking();
            var gameRule = new GameRules(handRanking, gameParticipant);

            Application.Run(new GameEngine(gameRule, handRanking));
        }
    }
}
