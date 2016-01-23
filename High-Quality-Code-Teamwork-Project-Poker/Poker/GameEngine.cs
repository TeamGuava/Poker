namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Poker.Contracts;
    using Poker.Models;
    using Poker.UI;

    public partial class GameEngine : Form
    {
        #region Constants
        private const int NumberOfBots = 5;
        private const int AllCardsOnTheTable = 17;
        #endregion

        #region Readonly
        private ProgressBar progressBar = new ProgressBar();
        private readonly ApplicationWriter writer = new ApplicationWriter();
        private readonly Bot[] gameBots = new Bot[5] 
        { new Bot(), new Bot(), new Bot(), new Bot(), new Bot() };
        private readonly Player player = new Player();
        private readonly List<Type> win = new List<Type>();
        private readonly List<string> winnersChecker = new List<string>();
        private readonly List<int> ints = new List<int>();
        private readonly int[] reserve = new int[17];
        private readonly Image[] deck = new Image[52];
        private readonly PictureBox[] holder = new PictureBox[52];
        #endregion

        #region Variables
        private int call = 500;
       // private int foldedPlayers = 5;
        private double type;
        private double rounds;
        private double raise;
        private bool intsadded;
        bool changed;
        // TODO: to make enumaration
        int winners, Flop = 1, Turn = 2, River = 3, End = 4, maxLeft = 6;
        private int last = 123;
        int raisedTurn = 1;
        //List<bool?> bools = new List<bool?>();
        //bool playerFoldTurn = false, playerTurn = true;
        private bool restart;
        private bool raising;
        private Type winningHand;
        private string[] imageLocation = Directory.GetFiles(
            "Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        /*string[] imageLocation ={card
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/

        private Timer timer = new Timer();
        private Timer update = new Timer();
        private int t = 60;
        private int i;
        private int bigBlind = 500;
        private int smallBlind = 250;
       // private int maxUp = 10000000;
        private int turnCount;
        #endregion
        public GameEngine()
        {
            //bools.Add(PFturn); bools.Add(B1Fturn); bools.Add(B2Fturn); bools.Add(B3Fturn); bools.Add(B4Fturn); bools.Add(B5Fturn);
            this.call = this.bigBlind;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.update.Start();
            this.InitializeComponent();
            //int width = this.Width;
            //int height = this.Height;
            this.Shuffle();
            this.potTextBox.Enabled = false;

            this.player.ParticipantPanel.ChipsTextBox.Enabled = false;
            this.player.ParticipantPanel.ChipsTextBox.Text =
                string.Format("Chips : {0}", this.player.Chips);

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                this.gameBots[bot].ParticipantPanel.ChipsTextBox.Enabled = false;
                this.gameBots[bot].ParticipantPanel.ChipsTextBox.Text =
                    string.Format("Chips: {0}", this.gameBots[bot].Chips);
            }

            this.timer.Interval = 1 * 1 * 1000;
            this.timer.Tick += this.timerTick;
            this.update.Interval = 1 * 1 * 100;
            this.update.Tick += this.UpdateTick;

            this.bigBlindTextBox.Visible = true;
            this.smallBlindTextBox.Visible = true;

            this.raiseTextBox.Text = (this.bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            //bools.Add(player.FoldTurn);
            //for (int bot = 0; bot < NumberOfBots; bot++)
            //{
            //    bools.Add(gameBots[bot].FoldTurn);
            //}

            this.callButton.Enabled = false;
            this.raiseButton.Enabled = false;
            this.foldButton.Enabled = false;
            this.checkButton.Enabled = false;

            this.MaximizeBox = false;
            bool check = false;

            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580;
            int vertical = 480;
            Random random = new Random();
            for (int i = this.imageLocation.Length; i > 0; i--)
            {
                int generatedNumber = random.Next(i);
                var k = this.imageLocation[generatedNumber];
                this.imageLocation[generatedNumber] = this.imageLocation[i - 1];
                this.imageLocation[i - 1] = k;
            }
            // Not sure
            for (int currentCard = 0; currentCard < AllCardsOnTheTable; currentCard++)
            {
                this.deck[currentCard] = Image.FromFile(
                    this.imageLocation[currentCard]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    this.imageLocation[currentCard] = 
                        this.imageLocation[currentCard].Replace(c, string.Empty);
                }

                this.reserve[currentCard] = int.Parse(this.imageLocation[currentCard]) - 1;
                this.holder[currentCard] = new PictureBox();
                this.holder[currentCard].SizeMode = PictureBoxSizeMode.StretchImage;
                this.holder[currentCard].Height = 130;
                this.holder[currentCard].Width = 80;
                this.Controls.Add(this.holder[currentCard]);
                this.holder[currentCard].Name = "pb" + currentCard;
                await Task.Delay(200);

                // Throwing Cards
                if (currentCard < 2)
                {
                    if (this.holder[0].Tag != null)
                    {
                        this.holder[1].Tag = this.reserve[1];
                    }

                    this.holder[0].Tag = this.reserve[0];
                    this.holder[currentCard].Image = this.deck[currentCard];
                    this.holder[currentCard].Anchor = AnchorStyles.Bottom;
                    //holder[i].Dock = DockStyle.Top;
                    this.holder[currentCard].Location = new Point(horizontal, vertical);
                    horizontal += this.holder[currentCard].Width;
                    this.Controls.Add(this.player.ParticipantPanel);
                    this.player.ParticipantPanel.Location = new Point(
                        this.holder[0].Left - 10, this.holder[0].Top - 10);
                    this.player.ParticipantPanel.BackColor = Color.DarkBlue;
                    this.player.ParticipantPanel.Height = 150;
                    this.player.ParticipantPanel.Width = 180;
                    this.player.ParticipantPanel.Visible = false;
                }

                //for (int bot = 0; bot < NumberOfBots; bot++)
                //{
                //    if (gameBots[bot].Chips > 0)
                //    {
                        
                //    }
                //}

                if (this.gameBots[0].Chips > 0)
                {
                    //foldedPlayers--;
                    if (currentCard >= 2 && currentCard < 4)
                    {
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;

                        if (currentCard % 2 == 1)
                        {
                            horizontal += this.holder[currentCard].Width;
                        }

                        this.SetBotCards(
                            this.gameBots[0], 
                            this.holder, 
                            backImage, 
                            this.reserve, 
                            horizontal, 
                            vertical, 
                            currentCard);

                        if (currentCard == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (this.gameBots[1].Chips > 0)
                {
                    //foldedPlayers--;
                    if (currentCard >= 4 && currentCard < 6)
                    {
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        if (currentCard % 2 == 1)
                        {
                            horizontal += this.holder[currentCard].Width;
                        }

                        this.SetBotCards(
                            this.gameBots[1],
                            this.holder, backImage, 
                            this.reserve, 
                            horizontal, 
                            vertical, 
                            currentCard);

                        if (currentCard == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (this.gameBots[2].Chips > 0)
                {
                    //foldedPlayers--;
                    if (currentCard >= 6 && currentCard < 8)
                    {
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;

                        if (currentCard % 2 == 1)
                        {
                            horizontal += this.holder[currentCard].Width;
                        }

                        this.SetBotCards(this.gameBots[2],
                            this.holder, backImage,
                            this.reserve, 
                            horizontal, 
                            vertical, 
                            currentCard);

                        if (currentCard == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (this.gameBots[3].Chips > 0)
                {
                    //foldedPlayers--;
                    if (currentCard >= 8 && currentCard < 10)
                    {
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;

                        if (currentCard % 2 == 1)
                        {
                            horizontal += this.holder[currentCard].Width;
                        }

                        this.SetBotCards(this.gameBots[3],
                            this.holder, 
                            backImage, 
                            this.reserve, 
                            horizontal, 
                            vertical, 
                            currentCard);

                        if (currentCard == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (this.gameBots[4].Chips > 0)
                {
                    //foldedPlayers--;
                    if (currentCard >= 10 && currentCard < 12)
                    {
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;

                        if (currentCard % 2 == 1)
                        {
                            horizontal += this.holder[currentCard].Width;
                        }

                        this.SetBotCards(this.gameBots[4],
                            this.holder,
                            backImage, 
                            this.reserve,
                            horizontal, 
                            vertical, 
                            currentCard);

                        if (currentCard == 11)
                        {
                            check = false;
                        }
                    }
                }

                // Printing five cards on the desk
                if (currentCard >= 12)
                {
                    this.holder[currentCard].Tag = this.reserve[currentCard];

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (this.holder[currentCard] != null)
                    {
                        this.holder[currentCard].Anchor = AnchorStyles.None;
                        this.holder[currentCard].Image = backImage;
                        //holder[i].Image = deck[i];
                        this.holder[currentCard].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }

                // Commented the code below as a fix for the bugged printing of cards after first game.
                // TODO: Delete the commented code below if proved redundant.
                //for (int bot = 0; bot < NumberOfBots; bot++)
                //{
                //    // This loop skips the first two cards (player's cards)
                //    for (int card = 2; card < 12; card += 2)
                //    {
                //        if (this.gameBots[bot].Chips <= 0)
                //        {
                //            this.gameBots[bot].FoldTurn = true;
                //            this.holder[card].Visible = false;
                //            this.holder[card + 1].Visible = false;
                //        }
                //        else
                //        {
                //            this.gameBots[0].FoldTurn = false;
                //            if (currentCard == card)
                //            {
                //                if (this.holder[card + 1] != null)
                //                {
                //                    this.holder[card].Visible = true;
                //                    this.holder[card + 1].Visible = true;
                //                }
                //            }
                //        }
                //    }                   
                //}

                if (currentCard == 16)
                {
                    if (!this.restart)
                    {
                        this.MaximizeBox = true;
                    }

                    this.EnableButtons();
                    this.timer.Start();
                }
            }

            //if (foldedPlayers == 5)
            //{
            //    DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        Application.Restart();
            //    }
            //    else if (dialogResult == DialogResult.No)
            //    {
            //        Application.Exit();
            //    }
            //}
            //else
            //{
            //    foldedPlayers = 5;
            //}
        }

        async Task Turns()
        {
            // Rotating
            if (!this.player.FoldTurn)
            {
                if (this.player.Turn)
                {
                    this.FixCallPlayer(1);
                    this.writer.Print("Player's Turn");

                    this.timerProgressBar.Visible = true;
                    this.timerProgressBar.Value = 1000;
                    this.t = 60;
                    //this.maxUp = 10000000;
                    this.timer.Start();
 
                    this.raiseButton.Enabled = true;
                    this.callButton.Enabled = true;
                    this.foldButton.Enabled = true;
                    this.turnCount++;
                    this.FixCallPlayer(2);
                }
            }

            if (this.player.FoldTurn || !this.player.Turn)
            {
                await this.AllIn();
                if (this.player.FoldTurn && !this.player.IsFolded)
                {
                    if (this.callButton.Text.Contains("All in") == false || 
                        this.raiseButton.Text.Contains("All in") == false)
                    {
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        this.maxLeft--;
                        this.player.IsFolded = true;
                    }
                }

                await this.CheckRaise(0, 0);
                this.timerProgressBar.Visible = false;
                this.raiseButton.Enabled = false;
                this.callButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.timer.Stop();
                // TODO: Maybe there is a bug here
                this.gameBots[0].Turn = true;
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = this.gameBots[bot];
                    int botIndex = bot + 1;
                    if (!currentBot.FoldTurn)
                    {
                        if (currentBot.Turn)
                        {
                            // These are just magic values, but it's better than no info for them
                            int firstCard = botIndex * 2;
                            int secondCard = botIndex * 2 + 1;

                            this.FixCallBot(currentBot, 1);
                            this.FixCallBot(currentBot, 2);
                            this.Rules(firstCard, secondCard, currentBot);

                            // TODO: Maybe we can remove this
                            this.writer.Print($"Bot {botIndex}'s Turn");

                            this.AI(firstCard, secondCard, currentBot);
                            this.turnCount++;
                            this.last = botIndex;
                            currentBot.Turn = false;
                            if (botIndex != NumberOfBots)
                            {
                                this.gameBots[bot + 1].Turn = true;
                            }
                        }

                        if (currentBot.FoldTurn &&
                            !currentBot.IsFolded)
                        {
                            //bools.RemoveAt(botIndex);
                            //bools.Insert(botIndex, null);
                            this.maxLeft--;
                            currentBot.IsFolded = true;
                        }

                        if (currentBot.FoldTurn ||
                            !currentBot.Turn)
                        {
                            await this.CheckRaise(botIndex, botIndex);
                            if(botIndex != NumberOfBots)
                            {
                                this.gameBots[bot + 1].Turn = true;
                            }
                        }
                    }
                }

                this.player.Turn = true;
                if (this.player.FoldTurn && !this.player.IsFolded)
                {
                    if (!this.callButton.Text.Contains("All in") ||
                        !this.raiseButton.Text.Contains("All in"))
                    {
                        // TODO: Create PlayerClass and work with its bools
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        this.maxLeft--;
                        this.player.IsFolded = true;
                    }
                }

                await this.AllIn();
                if (!this.restart)
                {
                    await this.Turns();
                }

                this.restart = false;
            }
        }

        void Rules(
            int firstCard, 
            int secondCard, 
            IGameParticipant currentGameParticipant)
        {
            if (firstCard == 0 &&
                secondCard == 1)
            {
            }

            if (!currentGameParticipant.FoldTurn ||
                firstCard == 0 &&
                secondCard == 1 && 
                this.player.ParticipantPanel.StatusButton.Text.Contains("Fold") == false)
            {
                // Variables
                bool done = false;
                //bool vf = false;

                int[] straight1 = new int[5];
                int[] straight = new int[7];

                straight[0] = this.reserve[firstCard];
                straight[1] = this.reserve[secondCard];
                straight1[0] = straight[2] = this.reserve[12];
                straight1[1] = straight[3] = this.reserve[13];
                straight1[2] = straight[4] = this.reserve[14];
                straight1[3] = straight[5] = this.reserve[15];
                straight1[4] = straight[6] = this.reserve[16];

                var a = straight.Where(o => o % 4 == 0).ToArray();
                var b = straight.Where(o => o % 4 == 1).ToArray();
                var c = straight.Where(o => o % 4 == 2).ToArray();
                var d = straight.Where(o => o % 4 == 3).ToArray();

                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();

                Array.Sort(straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);

                for (int cardIndex = 0; cardIndex < AllCardsOnTheTable; cardIndex++)
                {
                    if (this.reserve[cardIndex] == int.Parse(
                        this.holder[firstCard].Tag.ToString()) &&
                        this.reserve[cardIndex + 1] == int.Parse(
                            this.holder[secondCard].Tag.ToString()))
                    {
                        //Pair from Hand current = 1
                        this.rPairFromHand(currentGameParticipant);

                        // Pair or Two Pair from Table current = 2 || 0
                        this.rPairTwoPair(currentGameParticipant);

                        // Two Pair current = 2
                        this.rTwoPair(currentGameParticipant);

                        // Three of a kind current = 3
                        this.rThreeOfAKind(currentGameParticipant, straight);

                        // straight current = 4
                        this.rStraight(currentGameParticipant, straight);

                        // Flush current = 5 || 5.5

                        this.rFlush(currentGameParticipant, straight1);

                        rFlush(currentGameParticipant, straight1);

                        // Full House current = 6
                        this.rFullHouse(currentGameParticipant, ref done, straight);

                        // Four of a Kind current = 7
                        this.rFourOfAKind(currentGameParticipant, straight);

                        // straight Flush current = 8 || 9
                        this.rStraightFlush(currentGameParticipant, st1, st2, st3, st4);

                        // High Card current = -1
                        this.rHighCard(currentGameParticipant);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a winning hand to the collection of <see cref="Type"/>.
        /// </summary>
        /// <param botIndex="power"></param>
        /// <param botIndex="current"></param>
        private void AddWin(double power, double current)
        {
            Type typeToAdd = new Type(power, current);
            this.win.Add(typeToAdd);
            // Returns the best hand so far by sorting the win collection.
            this.winningHand = this.win
                .OrderByDescending(op1 => op1.Current)
                .ThenByDescending(op1 => op1.Power)
                .First();
        }

        private void rStraightFlush(
            IGameParticipant currentGameParticipant, 
            int[] st1, 
            int[] st2,
            int[] st3,
            int[] st4)
        {
            if (currentGameParticipant.Type >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power = 
                            (int)(st1.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }

                    if (st1[0] == 0 && 
                        st1[1] == 9 && 
                        st1[2] == 10 && 
                        st1[3] == 11 && 
                        st1[0] + 12 == st1[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power = 
                            (int)(st1.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power =
                            (int)(st2.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }

                    if (st2[0] == 0 && st2[1] == 9 &&
                        st2[2] == 10 && st2[3] == 11 &&
                        st2[0] + 12 == st2[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power =
                            (int)(st2.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power = 
                            (int)(st3.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }

                    if (st3[0] == 0 && 
                        st3[1] == 9 && 
                        st3[2] == 10 && 
                        st3[3] == 11 && 
                        st3[0] + 12 == st3[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power =
                            (int)(st3.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power =
                            (int)(st4.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }

                    if (st4[0] == 0 &&
                        st4[1] == 9 && 
                        st4[2] == 10 && 
                        st4[3] == 11 && 
                        st4[0] + 12 == st4[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power = 
                            (int)(st4.Max() / 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rFourOfAKind(IGameParticipant currentGameParticipant, int[] straight)
        {
            if (currentGameParticipant.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 &&
                        straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        currentGameParticipant.Type = 7;
                        currentGameParticipant.Power = 
                            (int)(straight[j] / 4 * 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }

                    if (straight[j] / 4 == 0 &&
                        straight[j + 1] / 4 == 0 &&
                        straight[j + 2] / 4 == 0 &&
                        straight[j + 3] / 4 == 0)
                    {
                        currentGameParticipant.Type = 7;
                        currentGameParticipant.Power = 
                            (int)(13 * 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                } 
            }
        }

        // TODO: Extract method
        private void rFullHouse(
            IGameParticipant currentGameParticipant, 
            ref bool done, 
            int[] straight)
        {
            if (currentGameParticipant.Type >= -1)
            {
                this.type = currentGameParticipant.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentGameParticipant.Type = 6;
                                currentGameParticipant.Power =
                                    (int)(13 * 2 + currentGameParticipant.Type * 100);
                                this.AddWin(
                                    currentGameParticipant.Power, 
                                    currentGameParticipant.Type);
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                currentGameParticipant.Type = 6;
                                currentGameParticipant.Power = 
                                    (int)(fh.Max() / 4 * 2 + currentGameParticipant.Type * 100);
                                this.AddWin(
                                    currentGameParticipant.Power, 
                                    currentGameParticipant.Type);
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentGameParticipant.Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                currentGameParticipant.Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (currentGameParticipant.Type != 6)
                {
                    currentGameParticipant.Power = (int) this.type;
                }
            }
        }

        private void rFlush(IGameParticipant currentGameParticipant, int[] straight1)
        {
            if (currentGameParticipant.Type >= -1)
            {
                var f1 = straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = straight1.Where(o => o % 4 == 3).ToArray();

                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 &&
                        this.reserve[i] % 4 == f1[0] % 4)
                    {
                        currentGameParticipant.Type = 5;

                        if (this.reserve[i] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                        }

                        if (this.reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                        }
                        else if (this.reserve[i] / 4 < f1.Max() / 4 &&
                            this.reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            currentGameParticipant.Power = 
                                (int)(f1.Max() + currentGameParticipant.Type * 100);
                        }

                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                }

                if (f1.Length == 4) // different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 && 
                        this.reserve[i] % 4 == f1[0] % 4)
                    {
                        currentGameParticipant.Type = 5;

                        if (this.reserve[i] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                        }
                        else
                        {
                            currentGameParticipant.Power = 
                                (int)(f1.Max() + currentGameParticipant.Type * 100);
                        }

                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }

                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 && 
                        this.reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        currentGameParticipant.Type = 5;
                        if (this.reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f1.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type); 
                            //vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (this.reserve[i] % 4 == f1[0] % 4 && 
                        this.reserve[i] / 4 > f1.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                        //vf = true;
                    }

                    if (this.reserve[i + 1] % 4 == f1[0] % 4 && 
                        this.reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                    else if (this.reserve[i] / 4 < f1.Min() / 4 &&
                        this.reserve[i + 1] / 4 < f1.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f1.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 && 
                        this.reserve[i] % 4 == f2[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }

                        if (this.reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else if (this.reserve[i] / 4 < f2.Max() / 4 &&
                            this.reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f2.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }
                }

                if (f2.Length == 4) // different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 &&
                        this.reserve[i] % 4 == f2[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f2.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }

                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 && this.reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f2.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (this.reserve[i] % 4 == f2[0] % 4 && 
                        this.reserve[i] / 4 > f2.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }

                    if (this.reserve[i + 1] % 4 == f2[0] % 4 &&
                        this.reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                    else if (this.reserve[i] / 4 < f2.Min() / 4 && this.reserve[i + 1] / 4 < f2.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(f2.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 &&
                        this.reserve[i] % 4 == f3[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                            this.AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            //vf = true;
                        }

                        if (this.reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power =
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else if (this.reserve[i] / 4 < f3.Max() / 4 && this.reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f3.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 &&
                        this.reserve[i] % 4 == f3[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f3.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }

                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 &&
                        this.reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power =
                                (int)(f3.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (this.reserve[i] % 4 == f3[0] % 4 && 
                        this.reserve[i] / 4 > f3.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                        this.AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        //vf = true;
                    }

                    if (this.reserve[i + 1] % 4 == f3[0] % 4 &&
                        this.reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                    else if (this.reserve[i] / 4 < f3.Min() / 4 &&
                        this.reserve[i + 1] / 4 < f3.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(f3.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 &&
                        this.reserve[i] % 4 == f4[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }

                        if (this.reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else if (this.reserve[i] / 4 < f4.Max() / 4 &&
                            this.reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f4.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 &&
                        this.reserve[i] % 4 == f4[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power =
                                (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f4.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }

                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 &&
                        this.reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power =
                                (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = 
                                (int)(f4.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                            //vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (this.reserve[i] % 4 == f4[0] % 4 && 
                        this.reserve[i] / 4 > f4.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }

                    if (this.reserve[i + 1] % 4 == f4[0] % 4 && 
                        this.reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(this.reserve[i + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                    else if (this.reserve[i] / 4 < f4.Min() / 4 &&
                        this.reserve[i + 1] / 4 < f4.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = 
                            (int)(f4.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                        //vf = true;
                    }
                }

                // ace
                if (f1.Length > 0)
                {
                    // vf is removed from the if-statements
                    if (this.reserve[i] / 4 == 0 && 
                        this.reserve[i] % 4 == f1[0] % 4 &&  
                        f1.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power =
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }

                    if (this.reserve[i + 1] / 4 == 0 && 
                        this.reserve[i + 1] % 4 == f1[0] % 4 
                        && f1.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = 
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }

                if (f2.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 &&
                        this.reserve[i] % 4 == f2[0] % 4 &&
                        f2.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = 
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                    
                    if (this.reserve[i + 1] / 4 == 0 && 
                        this.reserve[i + 1] % 4 == f2[0] % 4 &&
                        f2.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power =
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }

                if (f3.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 &&
                        this.reserve[i] % 4 == f3[0] % 4 &&
                        f3.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = 
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }

                    if (this.reserve[i + 1] / 4 == 0 && 
                        this.reserve[i + 1] % 4 == f3[0] % 4 &&
                        f3.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = 
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }
                }

                if (f4.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 && 
                        this.reserve[i] % 4 == f4[0] % 4 &&       
                        f4.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = 
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, 
                            currentGameParticipant.Type);
                    }

                    if (this.reserve[i + 1] / 4 == 0 &&
                        this.reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power =
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rStraight(IGameParticipant currentGameParticipant, int[] straight)
        {
            // TODO: Etract method
            if (currentGameParticipant.Type >= -1)
            {
                var op = straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            currentGameParticipant.Type = 4;
                            currentGameParticipant.Power = 
                                (int)(op.Max() + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 4;
                            currentGameParticipant.Power =
                                (int)(op[j + 4] + currentGameParticipant.Type * 100);
                            this.AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        }
                    }

                    if (op[j] == 0 &&
                        op[j + 1] == 9 &&
                        op[j + 2] == 10 &&
                        op[j + 3] == 11 && 
                        op[j + 4] == 12)
                    {
                        currentGameParticipant.Type = 4;
                        currentGameParticipant.Power = 
                            (int)(13 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rThreeOfAKind(IGameParticipant currentGameParticipant, int[] straight)
        {
            // TODO: Etract method
            if (currentGameParticipant.Type >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            currentGameParticipant.Type = 3;
                            currentGameParticipant.Power = 
                                (int)(13 * 3 + currentGameParticipant.Type * 100);
                           this.AddWin(
                               currentGameParticipant.Power, currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 3;
                            currentGameParticipant.Power = 
                                (int)(fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                        }
                    }
                }
            }
        }

        private void rTwoPair(IGameParticipant currentGameParticipant)
        {
            if (currentGameParticipant.Type >= -1)
            {
                bool msgbox = false;
                // TODO: Extract method
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (this.reserve[i] / 4 != this.reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            else
                            {
                                if (this.reserve[i] / 4 == this.reserve[tc] / 4 &&
                                    this.reserve[i + 1] / 4 == this.reserve[tc - k] / 4 ||
                                    this.reserve[i + 1] / 4 == this.reserve[tc] / 4 && 
                                    this.reserve[i] / 4 == this.reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reserve[i] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)(13 * 4 + (this.reserve[i + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                            this.AddWin(
                                                currentGameParticipant.Power,
                                                currentGameParticipant.Type);
                                        }

                                        if (this.reserve[i + 1] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)(13 * 4 + (this.reserve[i] / 4) * 2 + currentGameParticipant.Type * 100);
                                            this.AddWin
                                                (currentGameParticipant.Power, 
                                                currentGameParticipant.Type);
                                        }

                                        if (this.reserve[i + 1] / 4 != 0 &&
                                            this.reserve[i] / 4 != 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)((this.reserve[i] / 4) * 2 + (this.reserve[i + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                            this.AddWin(
                                                currentGameParticipant.Power, 
                                                currentGameParticipant.Type);
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                       }
                    }
                }
            }
        }

        private void rPairTwoPair(IGameParticipant currentGameParticipant)
        {
            if (currentGameParticipant.Type >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;

                // TODO: Extract method
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (this.reserve[tc] / 4 == this.reserve[tc - k] / 4)
                            {
                                if (this.reserve[tc] / 4 != this.reserve[i] / 4 &&
                                    this.reserve[tc] / 4 != this.reserve[i + 1] / 4 &&
                                    currentGameParticipant.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reserve[i + 1] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)((this.reserve[i] / 4) * 2 + 13 * 4 + currentGameParticipant.Type * 100);
                                            this.AddWin(
                                                currentGameParticipant.Power, 
                                                currentGameParticipant.Type);
                                        }

                                        if (this.reserve[i] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)((this.reserve[i + 1] / 4) * 2 + 13 * 4 + currentGameParticipant.Type * 100);
                                            this.AddWin(
                                                currentGameParticipant.Power, 
                                                currentGameParticipant.Type);
                                        }

                                        if (this.reserve[i + 1] / 4 != 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)((this.reserve[tc] / 4) * 2 + (this.reserve[i + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                            this.AddWin(
                                                currentGameParticipant.Power, 
                                                currentGameParticipant.Type);
                                        }

                                        if (this.reserve[i] / 4 != 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = 
                                                (int)((this.reserve[tc] / 4) * 2 + (this.reserve[i] / 4) * 2 + currentGameParticipant.Type * 100);
                                            this.AddWin(
                                                currentGameParticipant.Power, 
                                                currentGameParticipant.Type);
                                        }
                                    }

                                    msgbox = true;
                                }
                                if (currentGameParticipant.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (this.reserve[i] / 4 > this.reserve[i + 1] / 4)
                                        {
                                            if (this.reserve[tc] / 4 == 0)
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = 
                                                    (int)(13 + this.reserve[i] / 4 + currentGameParticipant.Type * 100);
                                                // using AddWin method with current value = 1 was intended
                                                // to match with default game logic 
                                                this.AddWin(currentGameParticipant.Power, 1);
                                            }
                                            else
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = 
                                                    (int)(this.reserve[tc] / 4 + this.reserve[i] / 4 + currentGameParticipant.Type * 100);
                                                this.AddWin(currentGameParticipant.Power, 1);
                                            }
                                        }
                                        else
                                        {
                                            if (this.reserve[tc] / 4 == 0)
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power =
                                                    (int)(13 + this.reserve[i + 1] + currentGameParticipant.Type * 100);
                                                this.AddWin(currentGameParticipant.Power, 1);
                                            }
                                            else
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = 
                                                    (int)(this.reserve[tc] / 4 + this.reserve[i + 1] / 4 + currentGameParticipant.Type * 100);
                                                this.AddWin(currentGameParticipant.Power, 1);
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairFromHand(IGameParticipant currentGameParticipant)
        {
            if (currentGameParticipant.Type >= -1)
            {
                bool msgbox = false;
                if (this.reserve[i] / 4 == this.reserve[i + 1] / 4)
                {
                    // This expression is always true
                    if (!msgbox)
                    {
                        if (this.reserve[i] / 4 == 0)
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power = 
                                (int) (13 * 4 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power =
                                (int) ((this.reserve[i + 1] / 4) * 4 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power, 
                                currentGameParticipant.Type);
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (this.reserve[i + 1] / 4 == this.reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reserve[i + 1] / 4 == 0)
                            {
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = 
                                    (int)(13 * 4 + this.reserve[i] / 4 + currentGameParticipant.Type * 100);
                                this.AddWin(
                                    currentGameParticipant.Power, 
                                    currentGameParticipant.Type);
                            }
                            else
                            {
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = 
                                    (int)((this.reserve[i + 1] / 4) * 4 + this.reserve[i] / 4 + currentGameParticipant.Type * 100);
                                this.AddWin(
                                    currentGameParticipant.Power, 
                                    currentGameParticipant.Type);
                            }
                        }

                        msgbox = true;
                    }

                    if (this.reserve[i] / 4 == this.reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reserve[i] / 4 == 0)
                            {
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = (int)(13 * 4 + this.reserve[i + 1] / 4 + currentGameParticipant.Type * 100);
                                this.AddWin(
                                    currentGameParticipant.Power, 
                                    currentGameParticipant.Type);
                            }
                            else
                            {
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = 
                                    (int)((this.reserve[tc] / 4) * 4 + this.reserve[i + 1] / 4 + currentGameParticipant.Type * 100);
                                this.AddWin(
                                    currentGameParticipant.Power, 
                                    currentGameParticipant.Type);
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }

        private void rHighCard(IGameParticipant currentGameParticipant)
        {
            if (currentGameParticipant.Type == -1)
            {
                if (this.reserve[i] / 4 > this.reserve[i + 1] / 4)
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = this.reserve[i] / 4;
                    this.AddWin(
                        currentGameParticipant.Power, 
                        currentGameParticipant.Type);
                }
                else
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = 
                        this.reserve[i + 1] / 4;
                    this.AddWin(
                        currentGameParticipant.Power, 
                        currentGameParticipant.Type);
                }

                if (this.reserve[i] / 4 == 0 || 
                    this.reserve[i + 1] / 4 == 0)
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = 13;
                    this.AddWin(
                        currentGameParticipant.Power, 
                        currentGameParticipant.Type);
                }
            }
        }

        void ValidateWinner(
            IGameParticipant currentGameParticipant,
            string currentText, 
            string lastly)
        {
            //TODO: needs some upgrading
            double currentGameParticipantType = currentGameParticipant.Type;
            int currentGameParticipantPower = currentGameParticipant.Power;

            if (lastly == " ")
            {
                lastly = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.holder[j].Visible)
                    this.holder[j].Image = this.deck[j];
            }

            if (currentGameParticipantType == this.winningHand.Current)
            {
                if (currentGameParticipantPower == this.winningHand.Power)
                {
                    this.winners++;
                    this.winnersChecker.Add(currentText);
                    if (currentGameParticipantType == -1)
                    {
                        this.writer.Print(currentText + " High Card ");
                    }
                    else if (currentGameParticipantType == 1 || 
                        currentGameParticipantType == 0)
                    {
                        this.writer.Print(currentText + " Pair ");
                    }
                    else if (currentGameParticipantType == 2)
                    {
                        this.writer.Print(currentText + " Two Pair ");
                    }
                    else if (currentGameParticipantType == 3)
                    {
                        this.writer.Print(currentText + " Three of a Kind ");
                    }
                    else if (currentGameParticipantType == 4)
                    {
                        this.writer.Print(currentText + " straight ");
                    }
                    else if (currentGameParticipantType == 5 || 
                        currentGameParticipantType == 5.5)
                    {
                        this.writer.Print(currentText + " Flush ");
                    }
                    else if (currentGameParticipantType == 6)
                    {
                        this.writer.Print(currentText + " Full House ");
                    }
                    else if (currentGameParticipantType == 7)
                    {
                        this.writer.Print(currentText + " Four of a Kind ");
                    }
                    else if (currentGameParticipantType == 8)
                    {
                        this.writer.Print(currentText + " straight Flush ");
                    }
                    else if (currentGameParticipantType == 9)
                    {
                        this.writer.Print(currentText + " Royal Flush ! ");
                    }
                }
            }

            if (currentText == lastly) //lastfixed
            {
                if (this.winners > 1)
                {
                    if (this.winnersChecker.Contains("Player"))
                    {
                        this.player.Chips += int.Parse(this.potTextBox.Text) / this.winners;
                        this.player.ParticipantPanel.ChipsTextBox.Text = this.player.Chips.ToString();
                        //pPanel.Visible = true;

                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (this.winnersChecker.Contains($"Bot {botIndex}"))
                        {
                            this.gameBots[bot].Chips += int.Parse(this.potTextBox.Text)/this.winners;
                            this.gameBots[bot].ParticipantPanel.ChipsTextBox.Text = 
                                this.gameBots[bot].Chips.ToString();
                            //gameBots[bot].Visible = true;
                        }
                    }

                    //await Finish(1);
                }

                if (this.winners == 1)
                {
                    if (this.winnersChecker.Contains("Player"))
                    {
                        this.player.Chips += int.Parse(this.potTextBox.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (this.winnersChecker.Contains($"Bot {botIndex}"))
                        {
                            this.gameBots[bot].Chips += int.Parse(this.potTextBox.Text) / this.winners;
                            //await Finish(1)
                            //gameBots[bot].Visible = true;
                        }
                    }                   
                }
            }
        }

        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (this.raising)
            {
                this.turnCount = 0;
                this.raising = false;
                this.raisedTurn = currentTurn;
                this.changed = true;
            }
            else
            {
                if (this.turnCount >= this.maxLeft - 1 || 
                    !this.changed && 
                    this.turnCount == this.maxLeft)
                {
                    if (currentTurn == this.raisedTurn - 1 || 
                        !this.changed && this.turnCount == this.maxLeft || 
                        this.raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        this.raise = 0;
                        this.call = 0;
                        this.raisedTurn = 123;
                        this.rounds++;

                        if (!this.player.FoldTurn)
                        {
                            this.player.ParticipantPanel.StatusButton.Text = string.Empty;
                        }

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            if (!this.gameBots[bot].FoldTurn)
                            {
                                this.gameBots[bot].ParticipantPanel.StatusButton.Text = string.Empty;
                            }
                        }
                    }
                }
            }

            if (this.rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.holder[j].Image != this.deck[j])
                    {
                        this.holder[j].Image = this.deck[j];

                        this.player.Call = 0;  
                        this.player.Raise = 0;

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            this.gameBots[bot].Call = 0;
                            this.gameBots[bot].Raise = 0;
                        }
                    }
                }
            }

            if (this.rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.holder[j].Image != this.deck[j])
                    {
                        this.holder[j].Image = this.deck[j];
    
                        this.player.Call = 0;
                        this.player.Raise = 0;

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            this.gameBots[bot].Call = 0;
                            this.gameBots[bot].Raise = 0;
                        }
                    }
                }
            }

            if (this.rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.holder[j].Image != this.deck[j])
                    {
                        this.holder[j].Image = this.deck[j];

                        this.player.Call = 0;
                        this.player.Raise = 0;

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            this.gameBots[bot].Call = 0;
                            this.gameBots[bot].Raise = 0;
                        }
                    }
                }
            }

            if (this.rounds == End && this.maxLeft == 6)
            {
                string fixedLast = string.Empty;
                if (!this.player.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    this.Rules(0, 1, this.player);
                }

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = this.gameBots[bot];
                    int botIndex = bot + 1;
                    int firstCard = botIndex * 2;
                    int seconCard = botIndex * 2 + 1;

                    if (!currentBot.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                    {
                        fixedLast = $"Bot {botIndex}";
                        this.Rules(firstCard, seconCard, currentBot);
                    } 
                }
                
                this.ValidateWinner(this.player, "Player", fixedLast);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = this.gameBots[bot];
                    this.ValidateWinner(currentBot, $"Bot {bot + 1}", fixedLast);
                    currentBot.FoldTurn = false;
                }
                
                this.restart = true;
                this.player.Turn = true;
                this.player.FoldTurn = false;

                if (this.player.Chips <= 0)
                {
                    AddChips addMoreChipsForm = new AddChips();
                    addMoreChipsForm.ShowDialog();
                    if (addMoreChipsForm.NewChips != 0)
                    {
                        this.player.Chips = addMoreChipsForm.NewChips;
                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            this.gameBots[bot].Chips = addMoreChipsForm.NewChips;
                        }

                        this.player.FoldTurn = false;
                        this.player.Turn = true;
                        this.raiseButton.Enabled = true;
                        this.foldButton.Enabled = true;
                        this.checkButton.Enabled = true;

                        this.raiseButton.Text = "raise";
                    }
                }

                this.player.ParticipantPanel.Visible = false;
                this.player.Call = 0;
                this.player.Raise = 0;
                this.player.Power = 0;
                this.player.Type = -1;

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = this.gameBots[bot];

                    currentBot.ParticipantPanel.Visible = false;
                    currentBot.Call = 0;
                    currentBot.Raise = 0;

                    // Moved up (they were below the variable type)
                    currentBot.Power = 0;
                    currentBot.Type = -1;
                }

                this.last = 0;
                this.call = this.bigBlind;
                this.raise = 0;
                this.imageLocation = 
                    Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                //bools.Clear();
                this.rounds = 0;
                this.type = 0;                

                this.ints.Clear();
                this.winnersChecker.Clear();
                this.winners = 0;
                this.win.Clear();
                this.winningHand.Current = 0;
                this.winningHand.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    this.holder[os].Image = null;
                    this.holder[os].Invalidate();
                    this.holder[os].Visible = false;
                }

                this.potTextBox.Text = "0";
                this.player.ParticipantPanel.StatusButton.Text = string.Empty;

                await this.Shuffle();
                await this.Turns();
            }
        }

        //void FixCall(Label status, ref int currentCall, ref int currentRaise, int options)
        //{
        //    if (rounds != 4)
        //    {
        //        if (options == 1)
        //        {
        //            if (status.Text.Contains("raise"))
        //            {
        //                var changeRaise = status.Text.Substring(6);
        //                currentRaise = int.Parse(changeRaise);
        //            }
        //            else if (status.Text.Contains("Call"))
        //            {
        //                var changeCall = status.Text.Substring(5);
        //                currentCall = int.Parse(changeCall);
        //            }
        //            else if (status.Text.Contains("Check"))
        //            {
        //                currentRaise = 0;
        //                currentCall = 0;
        //            }
        //        }
        //        if (options == 2)
        //        {
        //            if (currentRaise != raise && currentRaise <= raise)
        //            {
        //                call = Convert.ToInt32(raise) - currentRaise;
        //            }

        //            if (currentCall != call || currentCall <= call)
        //            {
        //                call = call - currentCall;
        //            }

        //            if (currentRaise == raise && raise > 0)
        //            {
        //                call = 0;
        //                callButton.Enabled = false;
        //                callButton.Text = "Call button is unable.";
        //            }
        //        }
        //    }
        //}

        // Temporary FixCall workaround
        // TODO: Consolidate FixCall for player and for bot. (Process through shared interface)
        void FixCallPlayer(int options)
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (this.player.ParticipantPanel.StatusButton.Text.Contains("raise"))
                    {
                        var changeRaise = this.player.ParticipantPanel.StatusButton.Text.Substring(6);
                        this.player.Raise = int.Parse(changeRaise);
                    }
                    else if (this.player.ParticipantPanel.StatusButton.Text.Contains("Call"))
                    {
                        var changeCall = this.player.ParticipantPanel.StatusButton.Text.Substring(5);
                        this.player.Call = int.Parse(changeCall);
                    }
                    else if (this.player.ParticipantPanel.StatusButton.Text.Contains("Check"))
                    {
                        this.player.Raise = 0;
                        this.player.Call = 0;
                    }
                }
                if (options == 2)
                {
                    if (this.player.Raise != this.raise && 
                        this.player.Raise <= this.raise)
                    {
                        this.call = Convert.ToInt32(this.raise) - this.player.Raise;
                    }

                    if (this.player.Call != call || this.player.Call <= call)
                    {
                        this.call = this.call - this.player.Call;
                    }

                    if (this.player.Raise == this.raise && 
                        this.raise > 0)
                    {
                        this.call = 0;
                        this.callButton.Enabled = false;
                        this.callButton.Text = "Call button is unable.";
                    }
                }
            }
        }

        void FixCallBot(Bot currentBot, int options)
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (currentBot.ParticipantPanel.StatusButton.Text.Contains("raise"))
                    {
                        var changeRaise = currentBot.ParticipantPanel.StatusButton.Text.Substring(6);
                        currentBot.Raise = int.Parse(changeRaise);
                    }
                    else if (currentBot.ParticipantPanel.StatusButton.Text.Contains("Call"))
                    {
                        var changeCall = currentBot.ParticipantPanel.StatusButton.Text.Substring(5);
                        currentBot.Call = int.Parse(changeCall);
                    }
                    else if (currentBot.ParticipantPanel.StatusButton.Text.Contains("Check"))
                    {
                        currentBot.Raise = 0;
                        currentBot.Call = 0;
                    }
                }

                if (options == 2)
                {
                    if (currentBot.Raise != this.raise &&
                        currentBot.Raise <= this.raise)
                    {
                        call = Convert.ToInt32(this.raise) - currentBot.Raise;
                    }

                    if (currentBot.Call != this.call ||
                        currentBot.Call <= this.call)
                    {
                        this.call = this.call - currentBot.Call;
                    }

                    if (currentBot.Raise == this.raise && 
                        this.raise > 0)
                    {
                        this.call = 0;
                        this.callButton.Enabled = false;
                        this.callButton.Text = "Call button is unable.";
                    }
                }
            }
        }
        
        async Task AllIn()
        {
            // All in
            if (this.player.Chips <= 0 && !this.intsadded)
            {
                if (this.player.ParticipantPanel.StatusButton.Text.Contains("raise"))
                {
                    this.ints.Add(this.player.Chips);
                    this.intsadded = true;
                }
                else if (this.player.ParticipantPanel.StatusButton.Text.Contains("Call"))
                {
                    this.ints.Add(this.player.Chips);
                    this.intsadded = true;
                }
            }

            this.intsadded = false;
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = this.gameBots[bot];

                if (currentBot.Chips <= 0 && 
                    !currentBot.FoldTurn)
                {
                    if (!this.intsadded)
                    {
                        this.ints.Add(currentBot.Chips);
                        this.intsadded = true;
                    }

                    this.intsadded = false;
                }
            }
            
            if (this.ints.ToArray().Length == this.maxLeft)
            {
                await this.Finish(2);
            }
            else
            {
                this.ints.Clear();
            }

            int leftOneNotFoldedPlayer = this.gameBots.Count(x => x.FoldTurn == false);
            if (!this.player.FoldTurn)
            {
                leftOneNotFoldedPlayer++;
            }

            // LastManStanding
            if (leftOneNotFoldedPlayer == 1)
            {
                if (!this.player.FoldTurn)
                {
                    this.player.Chips += int.Parse(this.potTextBox.Text);
                    this.player.ParticipantPanel.ChipsTextBox.Text = this.player.Chips.ToString();
                    this.player.ParticipantPanel.Visible = true;
                    this.writer.Print("Player Wins");
                }
                else
                {
                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (!this.gameBots[bot].FoldTurn ||
                            !this.gameBots[bot].IsFolded)
                        {
                            this.gameBots[bot].Chips += int.Parse(this.potTextBox.Text);

                            // why is this here?!
                            this.player.ParticipantPanel.ChipsTextBox.Text = this.gameBots[bot].Chips.ToString();

                            this.gameBots[bot].ParticipantPanel.Visible = true;
                            this.writer.Print($"Bot {botIndex} wins");
                        }
                    }
                }

                for (int j = 0; j <= 16; j++)
                {
                    this.holder[j].Visible = false;
                }

                await this.Finish(1);
            }

            this.intsadded = false;

            // FiveOrLessLeft
            if (leftOneNotFoldedPlayer < 6 && leftOneNotFoldedPlayer > 1 &&
                this.rounds >= End)
            {
                await this.Finish(2);
            }
        }

        async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            this.player.ParticipantPanel.Visible = false;
            this.call = this.bigBlind;
            this.raise = 0;
            //this.foldedPlayers = 5;
            this.type = 0;
            this.rounds = 0;

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = this.gameBots[bot];

                currentBot.ParticipantPanel.Visible = false;
                currentBot.Power = 0;

                // Moved up (they were below variable raise)
                currentBot.Type = -1;
                currentBot.Turn = false;
                currentBot.FoldTurn = false;
                currentBot.IsFolded = false;
                currentBot.Call = 0;
                currentBot.Raise = 0;
            }

            this.player.Power = 0;
            this.player.Type = -1;
            this.raise = 0;
            this.player.IsFolded = false;
            this.player.FoldTurn = false;
            this.player.Turn = true;

            this.restart = false;
            this.raising = false;

            this.player.Call = 0;
            this.player.Raise = 0;

            //height = 0;
            //width = 0;
            this.winners = 0;
            Flop = 1;
            Turn = 2;
            River = 3;
            End = 4;
            maxLeft = 6;
            last = 123;
            this.raisedTurn = 1;

            //bools.Clear();
            this.winnersChecker.Clear();
            this.ints.Clear();
            this.win.Clear();

            this.winningHand.Current = 0;
            this.winningHand.Power = 0;
            this.potTextBox.Text = "0";
            this.t = 60;
            //this.maxUp = 10000000;
            this.turnCount = 0;
            this.player.ParticipantPanel.StatusButton.Text = string.Empty;
 
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                this.gameBots[bot].ParticipantPanel.StatusButton.Text = string.Empty;
            }

            if (this.player.Chips <= 0)
            {
                AddChips chipsAdder = new AddChips();
                chipsAdder.ShowDialog();
                if (chipsAdder.NewChips != 0)
                {
                    this.player.Chips = chipsAdder.NewChips;
                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        this.gameBots[bot].Chips = chipsAdder.NewChips;
                    }

                    this.player.FoldTurn = false;
                    this.player.Turn = true;
                    this.raiseButton.Enabled = true;
                    this.foldButton.Enabled = true;
                    this.checkButton.Enabled = true;
                    this.raiseButton.Text = "raise";
                }
            }

            this.imageLocation = 
                Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                this.holder[os].Image = null;
                this.holder[os].Invalidate();
                this.holder[os].Visible = false;
            }

            await this.Shuffle();
            //await Turns();
        }

        void FixWinners()
        {
            this.win.Clear();
            this.winningHand.Current = 0;
            this.winningHand.Power = 0;
            string fixedLast = string.Empty;

            if (!this.player.ParticipantPanel.StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rules(0, 1, this.player);
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = this.gameBots[bot];
                int botIndex = bot + 1;
                int firstCard = botIndex*2;
                int secondCard = botIndex*2 + 1;

                if (!currentBot.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = $"Bot {botIndex}";
                    this.Rules(firstCard, secondCard, currentBot);
                }
            }
           
            this.ValidateWinner(this.player, "Player", fixedLast);
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                this.ValidateWinner(this.gameBots[bot], $"Bot {bot + 1}", fixedLast);
            }
        }

        void AI(int firstCard, int secondCard, IGameParticipant currentGameParticipant)
        {
            int botType = (int)currentGameParticipant.Type;

            if (!currentGameParticipant.FoldTurn)
            {
                if (botType == -1)
                {
                    this.HighCard(currentGameParticipant);
                }
                else if (botType == 0)
                {
                    this.PairTable(currentGameParticipant);
                }
                else if (botType == 1)
                {
                    this.PairHand(currentGameParticipant);
                }
                else if (botType == 2)
                {
                    this.TwoPair(currentGameParticipant);
                }
                else if (botType == 3)
                {
                    this.ThreeOfAKind(currentGameParticipant);
                }
                else if (botType == 4)
                {
                    this.Straight(currentGameParticipant);
                }
                else if (botType == 5 || 
                    botType == 5.5)
                {
                    this.Flush(currentGameParticipant);
                }
                else if (botType == 6)
                {
                    this.FullHouse(currentGameParticipant);
                }
                else if (botType == 7)
                {
                    this.FourOfAKind(currentGameParticipant);
                }
                else if (botType == 8 || 
                    botType == 9)
                {
                   this. StraightFlush(currentGameParticipant);
                }
            }

            if (currentGameParticipant.FoldTurn)
            {
                this.holder[firstCard].Visible = false;
                this.holder[secondCard].Visible = false;
            }
        }

        private void HighCard(IGameParticipant currentGameParticipant)
        {
            this.HP(currentGameParticipant, 20, 25);
        }

        private void PairTable(IGameParticipant currentGameParticipant)
        {
            this.HP(currentGameParticipant, 16, 25);
        }

        private void PairHand(IGameParticipant currentGameParticipant)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);

            if (currentGameParticipant.Power <= 199 && 
                currentGameParticipant.Power >= 140)
            {
                this.PH(currentGameParticipant, rCall, 6, rRaise);
            }
            else if (currentGameParticipant.Power <= 139 && 
                currentGameParticipant.Power >= 128)
            {
                this.PH(currentGameParticipant, rCall, 7, rRaise);
            }
            else if (currentGameParticipant.Power < 128 && 
                currentGameParticipant.Power >= 101)
            {
                this.PH(currentGameParticipant, rCall, 9, rRaise);
            }
        }

        private void TwoPair(IGameParticipant currentGameParticipant)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);

            if (currentGameParticipant.Power <= 290 &&
                currentGameParticipant.Power >= 246)
            {
                this.PH(currentGameParticipant, rCall, 3, rRaise);
            }
            else if (currentGameParticipant.Power <= 244 && 
                currentGameParticipant.Power >= 234)
            {
                this.PH(currentGameParticipant, rCall, 4, rRaise);
            }
            else if (currentGameParticipant.Power < 234 && 
                currentGameParticipant.Power >= 201)
            {
                this.PH(currentGameParticipant, rCall, 4, rRaise);
            }
        }

        private void ThreeOfAKind(IGameParticipant currentGameParticipant)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);

            if (currentGameParticipant.Power <= 390 && 
                currentGameParticipant.Power >= 330)
            {
                this.Smooth(currentGameParticipant, tCall, tRaise);
            }

            if (currentGameParticipant.Power <= 327 &&
                currentGameParticipant.Power >= 321)//10  8
            {
                this.Smooth(currentGameParticipant, tCall, tRaise);
            }

            if (currentGameParticipant.Power < 321 && 
                currentGameParticipant.Power >= 303)//7 2
            {
                this.Smooth(currentGameParticipant, tCall, tRaise);
            }
        }

        private void Straight(IGameParticipant currentGameParticipant)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);

            if (currentGameParticipant.Power <= 480 &&
                currentGameParticipant.Power >= 410)
            {
                this.Smooth(currentGameParticipant, sCall, sRaise);
            }
            else if (currentGameParticipant.Power <= 409 &&
                currentGameParticipant.Power >= 407)//10  8
            {
                this.Smooth(currentGameParticipant, sCall, sRaise);
            }
            else if (currentGameParticipant.Power < 407 &&
                currentGameParticipant.Power >= 404)
            {
                this.Smooth(currentGameParticipant, sCall, sRaise);
            }
        }

        private void Flush(IGameParticipant currentGameParticipant)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);

            this.Smooth(currentGameParticipant, fCall, fRaise);
        }

        private void FullHouse(IGameParticipant currentGameParticipant)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);

            if (currentGameParticipant.Power <= 626 && 
                currentGameParticipant.Power >= 620)
            {
                this.Smooth(currentGameParticipant, fhCall, fhRaise);
            }

            if (currentGameParticipant.Power < 620 &&
                currentGameParticipant.Power >= 602)
            {
                this.Smooth(currentGameParticipant, fhCall, fhRaise);
            }
        }

        private void FourOfAKind(IGameParticipant currentGameParticipant)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (currentGameParticipant.Power <= 752 &&
                currentGameParticipant.Power >= 704)
            {
                this.Smooth(currentGameParticipant, fkCall, fkRaise);
            }
        }

        private void StraightFlush(IGameParticipant currentGameParticipant)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);

            if (currentGameParticipant.Power <= 913 &&
                currentGameParticipant.Power >= 804)
            {
                this.Smooth(currentGameParticipant, sfCall, sfRaise);
            }
        }

        private void Fold(IGameParticipant currentGameParticipant)
        {
            this.raising = false;
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Is Folded";
            currentGameParticipant.Turn = false;
            currentGameParticipant.FoldTurn = true;
        }

        private void Check(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Check";
            currentGameParticipant.Turn = false;
            this.raising = false;
        }

        private void Call(IGameParticipant currentGameParticipant)
        {
            this.raising = false;
            currentGameParticipant.Turn = false;
            currentGameParticipant.Chips -= this.call;
            currentGameParticipant.ParticipantPanel.Text = "Call " + this.call;
            this.potTextBox.Text = (int.Parse(this.potTextBox.Text) + this.call).ToString();
        }

        private void Raised(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.Chips -= Convert.ToInt32(this.raise);
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "raise " + this.raise;
            this.potTextBox.Text =
                (int.Parse(this.potTextBox.Text) + Convert.ToInt32(this.raise)).ToString();
            this.call = Convert.ToInt32(this.raise);
            this.raising = true;
            currentGameParticipant.Turn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        // private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        private void HP(
            IGameParticipant currrentGameParticipant, 
            int n, 
            int n1)
        {
            Random random = new Random();
            int rnd = random.Next(1, 4);

            if (this.call <= 0)
            {
                this.Check(currrentGameParticipant);
            }

            if (this.call > 0)
            {
                if (rnd == 1)
                {
                    if (this.call <= RoundN(currrentGameParticipant.Chips, n))
                    {
                        this.Call(currrentGameParticipant);
                    }
                    else
                    {
                        this.Fold(currrentGameParticipant);
                    }
                }

                if (rnd == 2)
                {
                    if (this.call <= RoundN(currrentGameParticipant.Chips, n1))
                    {
                        this.Call(currrentGameParticipant);
                    }
                    else
                    {
                        this.Fold(currrentGameParticipant);
                    }
                }
            }

            if (rnd == 3)
            {
                if (this.raise == 0)
                {
                    this.raise = this.call * 2;
                    this.Raised(currrentGameParticipant);
                }
                else
                {
                    if (this.raise <= RoundN(currrentGameParticipant.Chips, n))
                    {
                        this.raise = this.call * 2;
                        this.Raised(currrentGameParticipant);
                    }
                    else
                    {
                        this.Fold(currrentGameParticipant);
                    }
                }
            }

            if (currrentGameParticipant.Chips <= 0)
            {
                currrentGameParticipant.FoldTurn = true;
            }
        }

        private void PH(
            IGameParticipant currentGameParticipant,
            int n,
            int n1,
            int r)
        {
            Random random = new Random();
            int rnd = random.Next(1, 3);

            if (this.rounds < 2)
            {
                if (this.call <= 0)
                {
                    this.Check(currentGameParticipant);
                }

                if (this.call > 0)
                {
                    if (this.call >= RoundN(currentGameParticipant.Chips, n1))
                    {
                        this.Fold(currentGameParticipant);
                    }

                    if (this.raise > RoundN(currentGameParticipant.Chips, n))
                    {
                        this.Fold(currentGameParticipant);
                    }

                    if (!currentGameParticipant.FoldTurn)
                    {
                        if (this.call >= RoundN(currentGameParticipant.Chips, n) &&
                            this.call <= RoundN(currentGameParticipant.Chips, n1))
                        {
                            this.Call(currentGameParticipant);
                        }

                        if (this.raise <= RoundN(currentGameParticipant.Chips, n) && 
                            this.raise >= RoundN(currentGameParticipant.Chips, n) / 2)
                        {
                            this.Call(currentGameParticipant);
                        }

                        if (this.raise <= RoundN(currentGameParticipant.Chips, n) / 2)
                        {
                            if (this.raise > 0)
                            {
                                this.raise = RoundN(currentGameParticipant.Chips, n);
                                this.Raised(currentGameParticipant);
                            }
                            else
                            {
                                this.raise = this.call * 2;
                                this.Raised(currentGameParticipant);
                            }
                        }
                    }
                }
            }

            if (this.rounds >= 2)
            {
                if (this.call > 0)
                {
                    if (this.call >= RoundN(currentGameParticipant.Chips, n1 - rnd))
                    {
                        this.Fold(currentGameParticipant);
                    }

                    if (this.raise > RoundN(currentGameParticipant.Chips, n - rnd))
                    {
                        this.Fold(currentGameParticipant);
                    }

                    if (!currentGameParticipant.FoldTurn)
                    {
                        if (this.call >= RoundN(currentGameParticipant.Chips, n - rnd) && 
                            this.call <= RoundN(currentGameParticipant.Chips, n1 - rnd))
                        {
                            this.Call(currentGameParticipant);
                        }

                        if (this.raise <= RoundN(currentGameParticipant.Chips, n - rnd) &&
                            this.raise >= RoundN(currentGameParticipant.Chips, n - rnd) / 2)
                        {
                            this.Call(currentGameParticipant);
                        }

                        if (this.raise <= RoundN(currentGameParticipant.Chips, n - rnd) / 2)
                        {
                            if (this.raise > 0)
                            {
                                this.raise = RoundN(currentGameParticipant.Chips, n - rnd);
                                Raised(currentGameParticipant);
                            }
                            else
                            {
                                this.raise = this.call * 2;
                                this.Raised(currentGameParticipant);
                            }
                        }
                    }
                }

                if (this.call <= 0)
                {
                    this.raise = RoundN(currentGameParticipant.Chips, r - rnd);
                    this.Raised(currentGameParticipant);
                }
            }

            if (currentGameParticipant.Chips <= 0)
            {
                currentGameParticipant.FoldTurn = true;
            }
        }

        void Smooth(
            IGameParticipant currentGameParticipant, 
            int call, 
            int raise)
        {
            //Random random = new Random();
            //int rnd = random.Next(1, 3);
            if (this.call <= 0)
            {
                this.Check(currentGameParticipant);
            }
            else
            {
                if (this.call >= RoundN(currentGameParticipant.Chips, call))
                {
                    if (currentGameParticipant.Chips > this.call)
                    {
                        this.Call(currentGameParticipant);
                    }
                    else if (currentGameParticipant.Chips <= this.call)
                    {
                        this.raising = false;
                        currentGameParticipant.Turn = false;
                        currentGameParticipant.Chips = 0;
                        currentGameParticipant.ParticipantPanel.StatusButton.Text = 
                            "Call " + currentGameParticipant.Chips;
                        this.potTextBox.Text = 
                            (int.Parse(this.potTextBox.Text) + currentGameParticipant.Chips).ToString();
                    }
                }
                else
                {
                    if (this.raise > 0)
                    {
                        if (currentGameParticipant.Chips >= this.raise * 2)
                        {
                            this.raise *= 2;
                            this.Raised(currentGameParticipant);
                        }
                        else
                        {
                            this.Call(currentGameParticipant);
                        }
                    }
                    else
                    {
                        this.raise = this.call * 2;
                        this.Raised(currentGameParticipant);
                    }
                }
            }

            if (currentGameParticipant.Chips <= 0)
            {
                currentGameParticipant.FoldTurn = true;
            }
        }

        #region UI
        private async void timerTick(object sender, object e)
        {
            if (this.timerProgressBar.Value <= 0)
            {
                this.player.FoldTurn = true;
                await this.Turns();
            }
            if (t > 0)
            {
                t--;
                this.timerProgressBar.Value = (t / 6) * 100;
            }
        }

        private void UpdateTick(object sender, object e)
        {
            if (this.player.Chips <= 0)
            {
                this.player.ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            }
            else
            {
                this.player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + this.player.Chips;
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                if (this.gameBots[bot].Chips <= 0)
                {
                    this.gameBots[bot].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
                }
                else
                {
                    this.gameBots[bot].ParticipantPanel.ChipsTextBox.Text = string.Format("Chips : {0}", this.gameBots[bot].Chips);
                }
            }

            if (this.player.Chips <= 0)
            {
                this.player.Turn = false;
                this.player.FoldTurn = true;
                this.callButton.Enabled = false;
                this.raiseButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.checkButton.Enabled = false;
            }

            //if (this.maxUp > 0)
            //{
            //    this.maxUp--;
            //}

            if (this.player.Chips >= this.call)
            {
                this.callButton.Text = "Call " + this.call;
            }
            else
            {
                this.callButton.Text = "All in";
                this.raiseButton.Enabled = false;
            }

            if (this.call > 0)
            {
                this.checkButton.Enabled = false;
            }
            else if (call <= 0)
            {
                this.checkButton.Enabled = true;
                this.callButton.Text = "Call";
                this.callButton.Enabled = false;
            }

            if (this.player.Chips <= 0)
            {
                this.raiseButton.Enabled = false;
            }

            int parsedValue;

            if (this.raiseTextBox.Text != string.Empty && 
                int.TryParse(raiseTextBox.Text, out parsedValue))
            {
                if (this.player.Chips <= int.Parse(this.raiseTextBox.Text))
                {
                    this.raiseButton.Text = "All in";
                }
                else
                {
                    this.raiseButton.Text = "raise";
                }
            }

            if (this.player.Chips < this.call)
            {
                this.raiseButton.Enabled = false;
            }
        }

        private async void botFoldOnClick(object sender, EventArgs e)
        {
            this.player.ParticipantPanel.StatusButton.Text = "Fold";
            this.player.Turn = false;
            this.player.FoldTurn = true;
            await this.Turns();
        }

        private async void botCheckOnClick(object sender, EventArgs e)
        {
            if (this.call <= 0)
            {
                this.player.Turn = false;
                this.player.ParticipantPanel.StatusButton.Text = "Check";
            }
            else
            {
                // pStatus.Text = "All in " + Chips;
                this.checkButton.Enabled = false;
            }

            await this.Turns();
        }

        private async void botCallOnClick(object sender, EventArgs e)
        {
            this.Rules(0, 1, this.player);
            if (this.player.Chips >= this.call)
            {
                this.player.Chips -= this.call;
                this.player.ParticipantPanel.ChipsTextBox.Text = 
                    "Chips : " + this.player.Chips;

                if (this.potTextBox.Text != string.Empty)
                {
                    this.potTextBox.Text = 
                        (int.Parse(this.potTextBox.Text) + this.call).ToString();
                }
                else
                {
                    this.potTextBox.Text = call.ToString();
                }

                this.player.Turn = false;
                this.player.ParticipantPanel.StatusButton.Text = "Call " + call;
                this.player.Call = this.call;
            }
            else if (this.player.Chips <= this.call && this.call > 0)
            {
                this.potTextBox.Text = 
                    (int.Parse(this.potTextBox.Text) + this.player.Chips).ToString();
                this.player.ParticipantPanel.StatusButton.Text = "All in " + this.player.Chips;
                this.player.Chips = 0;
                this.player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + this.player.Chips;
                this.player.Turn = false;
                this.foldButton.Enabled = false;
                this.player.Call = this.player.Chips;
            }

            await this.Turns();
        }

        private async void botRaiseOnClick(object sender, EventArgs e)
        {
            this.Rules(0, 1, this.player);
            int parsedValue;
            if (this.raiseTextBox.Text != string.Empty &&
                int.TryParse(this.raiseTextBox.Text, out parsedValue))
            {
                if (this.player.Chips > this.call)
                {
                    if (this.raise * 2 > int.Parse(this.raiseTextBox.Text))
                    {
                        this.raiseTextBox.Text = (this.raise*2).ToString();
                        this.writer.Print(
                            "You must raise at least twice as the current raise !");

                        return;
                    }
                    else
                    {
                        if (this.player.Chips >= int.Parse(this.raiseTextBox.Text))
                        {
                            this.call = int.Parse(this.raiseTextBox.Text);
                            this.raise = int.Parse(raiseTextBox.Text);
                            this.player.ParticipantPanel.StatusButton.Text = "raise " + this.call;
                            this.potTextBox.Text = (int.Parse(this.potTextBox.Text) + this.call).ToString();
                            this.callButton.Text = "Call";
                            this.player.Chips -= int.Parse(this.raiseTextBox.Text);
                            this.raising = true;
                            this.last = 0;
                            this.player.Raise = Convert.ToInt32(this.raise);
                        }
                        else
                        {
                            this.call = this.player.Chips;
                            this.raise = this.player.Chips;
                            this.potTextBox.Text =
                                (int.Parse(this.potTextBox.Text) + this.player.Chips).ToString();
                            this.player.ParticipantPanel.StatusButton.Text = "raise " + this.call;
                            this.player.Chips = 0;
                            this.raising = true;
                            this.last = 0;
                            this.player.Raise = Convert.ToInt32(this.raise);
                        }
                    }
                }
            }
            else
            {
                this.writer.Print("This is a number only field");
                return;
            }

            this.player.Turn = false;
            await this.Turns();
        }

        private void botAddOnClick(object sender, EventArgs e)
        {
            if (this.addChipsTextBox.Text != string.Empty)
            {
                this.player.Chips += int.Parse(this.addChipsTextBox.Text);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    this.gameBots[bot].Chips += int.Parse(this.addChipsButton.Text);
                }
            }

            this.player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + this.player.Chips;
        }

        private void botOptionsOnClick(object sender, EventArgs e)
        {
            this.bigBlindTextBox.Text = this.bigBlind.ToString();
            this.smallBlindTextBox.Text = this.smallBlind.ToString();
            if (this.bigBlindTextBox.Visible == false)
            {
                this.bigBlindTextBox.Visible = true;
                this.smallBlindTextBox.Visible = true;

                this.bigBlindButton.Visible = true;
                this.smallBlindButton.Visible = true;
            }
            else
            {
                this.bigBlindTextBox.Visible = false;
                this.smallBlindTextBox.Visible = false;

                this.bigBlindButton.Visible = false;
                this.smallBlindButton.Visible = false;
            }
        }

        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.smallBlindTextBox.Text.Contains(",") || 
                this.smallBlindTextBox.Text.Contains("."))
            {
                this.writer.Print("The Small Blind can be only round number !");
                this.smallBlindTextBox.Text = this.smallBlind.ToString();

                return;
            }

            if (!int.TryParse(this.smallBlindTextBox.Text, out parsedValue))
            {
                this.writer.Print("This is a number only field");
                this.smallBlindTextBox.Text = this.smallBlind.ToString();

                return;
            }

            if (int.Parse(this.smallBlindTextBox.Text) > 100000)
            {
                this.writer.Print("The maximum of the Small Blind is 100 000 $");
                this.smallBlindTextBox.Text = this.smallBlind.ToString();
            }
            else if (int.Parse(this.smallBlindTextBox.Text) < 250)
            {
                this.writer.Print("The minimum of the Small Blind is 250 $");
            }
            else
            {
                this.smallBlind = int.Parse(this.smallBlindTextBox.Text);
                this.writer.Print(
                    "The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void bBigBlindOnClick(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.bigBlindTextBox.Text.Contains(",") ||
                this.bigBlindTextBox.Text.Contains("."))
            {
                this.writer.Print("The Big Blind can be only round number !");
                this.bigBlindTextBox.Text = this.bigBlind.ToString();

                return;
            }

            if (!int.TryParse(this.smallBlindTextBox.Text, out parsedValue))
            { 
                this.writer.Print("This is a number only field");
                this.smallBlindTextBox.Text = this.bigBlind.ToString();

                return;
            }

            if (int.Parse(this.bigBlindTextBox.Text) > 200000)
            {
                this.writer.Print("The maximum of the Big Blind is 200 000");
                this.bigBlindTextBox.Text = this.bigBlind.ToString();
            }
            else if (int.Parse(this.bigBlindTextBox.Text) < 500)
            {
                this.writer.Print("The minimum of the Big Blind is 500 $");
            }
            else
            {
                this.bigBlind = int.Parse(this.bigBlindTextBox.Text);
                this.writer.Print(
                    "The changes have been saved ! They will become available the next hand you play. ");
            }
        }
      
        private void ChangeLayout(object sender, LayoutEventArgs e)
        {
            int width = this.Width;
            int height = this.Height;
        }
        #endregion

        private void SetBotCards(
            Bot botPanel, 
            PictureBox[] holder, 
            Bitmap backImage, 
            int[] reserve, 
            int horizontal, 
            int vertical, 
            int currentCard)
        {
            holder[currentCard].Tag = reserve[currentCard];
            holder[currentCard].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            holder[currentCard].Image = backImage;
            // holder[currentCard].Image = deck[currentCard];
            holder[currentCard].Location = new Point(horizontal, vertical);

            if (currentCard % 2 == 0)
            {
                horizontal += holder[currentCard].Width;
            }

            holder[currentCard].Visible = true;
            this.Controls.Add(botPanel.ParticipantPanel);
            botPanel.ParticipantPanel.Location = new Point(
                holder[currentCard].Left - 10,
                holder[currentCard].Top - 10);
            botPanel.ParticipantPanel.BackColor = Color.DarkBlue;
            botPanel.ParticipantPanel.Height = 150;
            botPanel.ParticipantPanel.Width = 180;
            botPanel.ParticipantPanel.Visible = false;
        }

        private void EnableButtons()
        {
            this.raiseButton.Enabled = true;
            this.callButton.Enabled = true;
            this.foldButton.Enabled = true;
        }
    }
}