namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Poker.Models;

    public partial class GameEngine : Form
    {
        #region Constants
        private const int StartChips = 10000;
        private const int NumberOfBots = 5;
        private const int AllCardsOnTheTable = 17;
        #endregion

        #region Variables
        ProgressBar progressBar = new ProgressBar();

        Bot[] botsPanel = new Bot[5];

        // Moving playerPanel, playerChips, playerCall and playerRaise to the Player class
        // all references to the forementioned fields updated and attached to the class
        Player player = new Player(StartChips);

        private int call = 500;
        private int foldedPlayers = 5;
        private double type;
        private double rounds = 0;
        private double Raise = 0;
        private double playerType = -1;
        private double botType = -1;
        private double playerPower = 0;
        private double botPower = 0;

        private int botChips = StartChips;
        private bool botTurn = false;
        private bool botFoldTurn = false;
        private bool botFolded;
        private bool intsadded;
        bool changed;
        private int botCall = 0;
        int botRaise = 0;
        int height, width, winners = 0, Flop = 1, Turn = 2, River = 3, End = 4, maxLeft = 6;
        private int last = 123;
        int raisedTurn = 1;

        List<bool?> bools = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        bool playerFoldTurn = false, playerTurn = true;
        bool restart = false, raising = false;
        Poker.Type winningHand;
        string[] ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        /*string[] ImgLocation ={card
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/
        int[] Reserve = new int[17];
        Image[] Deck = new Image[52];
        PictureBox[] Holder = new PictureBox[52];
        Timer timer = new Timer();
        Timer Updates = new Timer();

        // GLOBAL I -> WTF ?! :D
        int t = 60, i, bigBlind = 500, sb = 250, maxUp = 10000000, turnCount = 0;
        #endregion
        public GameEngine()
        {
            //bools.Add(PFturn); bools.Add(B1Fturn); bools.Add(B2Fturn); bools.Add(B3Fturn); bools.Add(B4Fturn); bools.Add(B5Fturn);
            call = this.bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;
            Updates.Start();
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            Shuffle();
            potTextBox.Enabled = false;
            player_ChipsTextBox.Enabled = false;
            player_ChipsTextBox.Text = "Chips : " + player.Chips.ToString();

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                botsPanel[bot].ChipsTextBox.Enabled = false;
                botsPanel[bot].ChipsTextBox.Text = string.Format("Chips: {0}", botChips);
            }

            timer.Interval = 1 * 1 * 1000;
            timer.Tick += timer_Tick;
            Updates.Interval = 1 * 1 * 100;
            Updates.Tick += Update_Tick;

            // TODO: unnecessary things
            bigBlind_TextBox.Visible = true;
            smallBlind_TextBox.Visible = true;

            bigBlind_Button.Visible = true;
            smallBlind_Button.Visible = true;

            bigBlind_TextBox.Visible = true;
            smallBlind_TextBox.Visible = true;

            bigBlind_Button.Visible = true;
            smallBlind_Button.Visible = true;

            bigBlind_TextBox.Visible = false;
            smallBlind_TextBox.Visible = false;

            bigBlind_Button.Visible = false;
            smallBlind_Button.Visible = false;

            raiseTextBox.Text = (this.bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            bools.Add(playerFoldTurn);
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                botsPanel[bot].Bools.Add(botFoldTurn);
            }

            callButton.Enabled = false;
            raiseButton.Enabled = false;
            foldButton.Enabled = false;
            checkButton.Enabled = false;

            MaximizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580;
            int vertical = 480;
            Random random = new Random();
            for (int i = ImgLocation.Length; i > 0; i--)
            {
                int generatedNumber = random.Next(i);
                var k = ImgLocation[generatedNumber];
                ImgLocation[generatedNumber] = ImgLocation[i - 1];
                ImgLocation[i - 1] = k;
            }

            for (int currentCard = 0; currentCard < 17; currentCard++)
            {
                Deck[currentCard] = Image.FromFile(ImgLocation[currentCard]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    ImgLocation[currentCard] = ImgLocation[currentCard].Replace(c, string.Empty);
                }

                Reserve[currentCard] = int.Parse(ImgLocation[currentCard]) - 1;
                Holder[currentCard] = new PictureBox();
                Holder[currentCard].SizeMode = PictureBoxSizeMode.StretchImage;
                Holder[currentCard].Height = 130;
                Holder[currentCard].Width = 80;
                this.Controls.Add(Holder[currentCard]);
                Holder[currentCard].Name = "pb" + currentCard.ToString();
                await Task.Delay(200);

                // Throwing Cards
                if (currentCard < 2)
                {
                    if (Holder[0].Tag != null)
                    {
                        Holder[1].Tag = Reserve[1];
                    }

                    Holder[0].Tag = Reserve[0];
                    Holder[currentCard].Image = Deck[currentCard];
                    Holder[currentCard].Anchor = AnchorStyles.Bottom;
                    //Holder[i].Dock = DockStyle.Top;
                    Holder[currentCard].Location = new Point(horizontal, vertical);
                    horizontal += Holder[currentCard].Width;
                    this.Controls.Add(player.PlayerPanel);
                    player.PlayerPanel.Location = new Point(Holder[0].Left - 10, Holder[0].Top - 10);
                    player.PlayerPanel.BackColor = Color.DarkBlue;
                    player.PlayerPanel.Height = 150;
                    player.PlayerPanel.Width = 180;
                    player.PlayerPanel.Visible = false;
                }

                //for (int bot = 0; bot < NumberOfBots; bot++)
                //{
                //    if (botsPanel[bot].Chips > 0)
                //    {
                        
                //    }
                //}

                if (botsPanel[0].Chips > 0)
                {
                    foldedPlayers--;
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
                            horizontal += Holder[currentCard].Width;
                        }

                        this.SetBotCards(botsPanel[0], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (botsPanel[1].Chips > 0)
                {
                    foldedPlayers--;
                    if (currentCard >= 4 && currentCard < 6)
                    {
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        if (currentCard % 2 == 1)
                        {
                            horizontal += Holder[currentCard].Width;
                        }

                        this.SetBotCards(botsPanel[1], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (botsPanel[2].Chips > 0)
                {
                    foldedPlayers--;
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
                            horizontal += Holder[currentCard].Width;
                        }

                        this.SetBotCards(botsPanel[2], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (botsPanel[3].Chips > 0)
                {
                    foldedPlayers--;
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
                            horizontal += Holder[currentCard].Width;
                        }

                        this.SetBotCards(botsPanel[3], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (botsPanel[4].Chips > 0)
                {
                    foldedPlayers--;
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
                            horizontal += Holder[currentCard].Width;
                        }

                        this.SetBotCards(botsPanel[4], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 11)
                        {
                            check = false;
                        }
                    }
                }

                // Printing five cards on the desk
                if (currentCard >= 12)
                {
                    Holder[currentCard].Tag = Reserve[currentCard];

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (Holder[currentCard] != null)
                    {
                        Holder[currentCard].Anchor = AnchorStyles.None;
                        Holder[currentCard].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[currentCard].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
               
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    // This loop skips the first two cards (player's cards)
                    for (int card = 2; card < 12; card += 2)
                    {
                        if (botsPanel[bot].Chips <= 0)
                        {
                            botsPanel[bot].FoldTurn = true;
                            Holder[card].Visible = false;
                            Holder[card + 1].Visible = false;
                        }
                        else
                        {
                            botsPanel[0].FoldTurn = false;
                            if (currentCard == card)
                            {
                                if (Holder[card + 1] != null)
                                {
                                    Holder[card].Visible = true;
                                    Holder[card + 1].Visible = true;
                                }
                            }
                        }
                    }                   
                }

                if (currentCard == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                    }

                    this.EnableButtons();
                    timer.Start();
                }
            }

            if (foldedPlayers == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                foldedPlayers = 5;
            }
        }

        async Task Turns()
        {
            // Rotating
            if (!playerFoldTurn)
            {
                if (playerTurn)
                {
                    FixCallPlayer(1);
                    //MessageBox.Show("Player's Turn");
                    timerProgressBar.Visible = true;
                    timerProgressBar.Value = 1000;
                    t = 60;
                    maxUp = 10000000;
                    timer.Start();

                    raiseButton.Enabled = true;
                    callButton.Enabled = true;
                    foldButton.Enabled = true;
                    turnCount++;
                    FixCallPlayer(2);
                }
            }

            if (playerFoldTurn || !playerTurn)
            {
                await AllIn();
                if (playerFoldTurn && !player.Folded)
                {
                    if (callButton.Text.Contains("All in") == false || raiseButton.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        player.Folded = true;
                    }
                }

                await CheckRaise(0, 0);

                timerProgressBar.Visible = false;
                raiseButton.Enabled = false;
                callButton.Enabled = false;
                foldButton.Enabled = false;

                timer.Stop();

                botsPanel[0].Turn = true;
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = botsPanel[bot];
                    int botIndex = bot + 1;
                    if (!currentBot.FoldTurn)
                    {
                        if (currentBot.Turn)
                        {
                            // These are just magic values, but it's better than no info for them
                            int firstCard = botIndex * 2;
                            int secondCard = botIndex * 2 + 1;

                            FixCallBot(currentBot, 1);
                            FixCallBot(currentBot, 2);
                            Rules(firstCard, secondCard, $"Bot {botIndex}", ref currentBot.Type, ref currentBot.Type, currentBot.FoldTurn);

                            // TODO: Maybe we can remove the message
                            MessageBox.Show($"Bot {botIndex}'s Turn");

                            AI(firstCard, secondCard, ref currentBot.Chips, ref currentBot.Turn, ref currentBot.FoldTurn, currentBot.StatusButton, bot, currentBot.Power, currentBot.Type);
                            turnCount++;
                            last = botIndex;
                            currentBot.Turn = false;
                            if (botIndex != NumberOfBots)
                            {
                                botsPanel[bot + 1].Turn = true;
                            }
                        }

                        if (currentBot.FoldTurn && !currentBot.Folded)
                        {
                            bools.RemoveAt(botIndex);
                            bools.Insert(botIndex, null);
                            maxLeft--;
                            currentBot.Folded = true;
                        }

                        if (currentBot.FoldTurn || !currentBot.Turn)
                        {
                            await CheckRaise(botIndex, botIndex);
                            botsPanel[bot + 1].Turn = true;
                        }
                    }
                }

                if (playerFoldTurn && !player.Folded)
                {
                    if (!callButton.Text.Contains("All in") || !raiseButton.Text.Contains("All in"))
                    {
                        // TODO: Create PlayerClass and work with its bools
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        player.Folded = true;
                    }
                }

                await AllIn();
                if (!restart)
                {
                    await Turns();
                }

                restart = false;
            }
        }

        void Rules(int firstCard, int secondCard, string currentText, ref double current, ref double Power, bool foldedTurn)
        {
            if (firstCard == 0 && secondCard == 1)
            {
            }

            if (!foldedTurn || firstCard == 0 && secondCard == 1 && playerStatusButton.Text.Contains("Fold") == false)
            {
                // Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = Reserve[firstCard];
                Straight[1] = Reserve[secondCard];
                Straight1[0] = Straight[2] = Reserve[12];
                Straight1[1] = Straight[3] = Reserve[13];
                Straight1[2] = Straight[4] = Reserve[14];
                Straight1[3] = Straight[5] = Reserve[15];
                Straight1[4] = Straight[6] = Reserve[16];

                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();

                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();

                Array.Sort(Straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);

                for (int i = 0; i < 16; i++)
                {
                    if (Reserve[i] == int.Parse(Holder[firstCard].Tag.ToString()) && Reserve[i + 1] == int.Parse(Holder[secondCard].Tag.ToString()))
                    {
                        //Pair from Hand current = 1
                        rPairFromHand(ref current, ref Power);

                        // Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(ref current, ref Power);

                        // Two Pair current = 2
                        rTwoPair(ref current, ref Power);

                        // Three of a kind current = 3
                        rThreeOfAKind(ref current, ref Power, Straight);

                        // Straight current = 4
                        rStraight(ref current, ref Power, Straight);

                        // Flush current = 5 || 5.5
                        rFlush(ref current, ref Power, ref vf, Straight1);

                        // Full House current = 6
                        rFullHouse(ref current, ref Power, ref done, Straight);

                        // Four of a Kind current = 7
                        rFourOfAKind(ref current, ref Power, Straight);

                        // Straight Flush current = 8 || 9
                        rStraightFlush(ref current, ref Power, st1, st2, st3, st4);

                        // High Card current = -1
                        rHighCard(ref current, ref Power);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a winning hand to the collection of <see cref="Type"/>.
        /// </summary>
        /// <param name="power"></param>
        /// <param name="current"></param>
        private void AddWin(double power, double current)
        {
            Type typeToAdd = new Type(power, current);
            Win.Add(typeToAdd);
            // Returns the best hand so far by sorting the Win collection.
            winningHand = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        }

        private void rStraightFlush(ref double current, ref double Power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        Power = st1.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        Power = st1.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        Power = st2.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        Power = st2.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        Power = st3.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        Power = st3.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        Power = st4.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        Power = st4.Max() / 4 + current * 100;
                        AddWin(Power, current);
                    }
                }
            }
        }

        private void rFourOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        Power = (Straight[j] / 4) * 4 + current * 100;
                        AddWin(Power, current);
                    }

                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        Power = 13 * 4 + current * 100;
                        AddWin(Power, current);
                    }
                }
            }
        }

        private void rFullHouse(ref double current, ref double Power, ref bool done, int[] Straight)
        {
            if (current >= -1)
            {
                type = Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                Power = 13 * 2 + current * 100;
                                AddWin(Power, current);
                                break;
                            }
                            else if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                Power = fh.Max() / 4 * 2 + current * 100;
                                AddWin(Power, current);
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (current != 6)
                {
                    Power = type;
                }
            }
        }

        private void rFlush(ref double current, ref double Power, ref bool vf, int[] Straight1)
        {
            if (current >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();

                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f1.Max() / 4 && Reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4) // different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (Reserve[i] % 4 == f1[0] % 4 && Reserve[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f1[0] % 4 && Reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f1.Min() / 4 && Reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        Power = f1.Max() + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f2.Max() / 4 && Reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4) // different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (Reserve[i] % 4 == f2[0] % 4 && Reserve[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f2[0] % 4 && Reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f2.Min() / 4 && Reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        Power = f2.Max() + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f3.Max() / 4 && Reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (Reserve[i] % 4 == f3[0] % 4 && Reserve[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f3[0] % 4 && Reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f3.Min() / 4 && Reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        Power = f3.Max() + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f4.Max() / 4 && Reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            AddWin(Power, current);
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (Reserve[i] % 4 == f4[0] % 4 && Reserve[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f4[0] % 4 && Reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f4.Min() / 4 && Reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        Power = f4.Max() + current * 100;
                        AddWin(Power, current);
                        vf = true;
                    }
                }

                // ace
                if (f1.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }
                }

                if (f2.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }
                }

                if (f3.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }
                }

                if (f4.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }
                }
            }
        }

        private void rStraight(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            Power = op.Max() + current * 100;
                            AddWin(Power, current);
                        }
                        else
                        {
                            current = 4;
                            Power = op[j + 4] + current * 100;
                            AddWin(Power, current);
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        Power = 13 + current * 100;
                        AddWin(Power, current);
                    }
                }
            }
        }

        private void rThreeOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            Power = 13 * 3 + current * 100;
                            AddWin(Power, current);
                        }
                        else
                        {
                            current = 3;
                            Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            AddWin(Power, current);
                        }
                    }
                }
            }
        }

        private void rTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (Reserve[i] / 4 != Reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            else
                            {
                                if (Reserve[i] / 4 == Reserve[tc] / 4 && Reserve[i + 1] / 4 == Reserve[tc - k] / 4 ||
                                    Reserve[i + 1] / 4 == Reserve[tc] / 4 && Reserve[i] / 4 == Reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            AddWin(Power, current);
                                        }
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (Reserve[i] / 4) * 2 + current * 100;
                                            AddWin(Power, current);
                                        }
                                        if (Reserve[i + 1] / 4 != 0 && Reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            AddWin(Power, current);
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

        private void rPairTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
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
                            if (Reserve[tc] / 4 == Reserve[tc - k] / 4)
                            {
                                if (Reserve[tc] / 4 != Reserve[i] / 4 && Reserve[tc] / 4 != Reserve[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i] / 4) * 2 + 13 * 4 + current * 100;
                                            AddWin(Power, current);
                                        }
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            AddWin(Power, current);
                                        }
                                        if (Reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[tc] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            AddWin(Power, current);
                                        }
                                        if (Reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[tc] / 4) * 2 + (Reserve[i] / 4) * 2 + current * 100;
                                            AddWin(Power, current);
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + Reserve[i] / 4 + current * 100;
                                                // using AddWin method with current value = 1 was intended
                                                // to match with default game logic 
                                                AddWin(Power, 1);
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = Reserve[tc] / 4 + Reserve[i] / 4 + current * 100;
                                                AddWin(Power, 1);
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + Reserve[i + 1] + current * 100;
                                                AddWin(Power, 1);
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = Reserve[tc] / 4 + Reserve[i + 1] / 4 + current * 100;
                                                AddWin(Power, 1);
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

        private void rPairFromHand(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (Reserve[i] / 4 == Reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (Reserve[i] / 4 == 0)
                        {
                            current = 1;
                            Power = 13 * 4 + current * 100;
                            AddWin(Power, current);
                        }
                        else
                        {
                            current = 1;
                            Power = (Reserve[i + 1] / 4) * 4 + current * 100;
                            AddWin(Power, current);
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (Reserve[i + 1] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[i + 1] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + Reserve[i] / 4 + current * 100;
                                AddWin(Power, current);
                            }
                            else
                            {
                                current = 1;
                                Power = (Reserve[i + 1] / 4) * 4 + Reserve[i] / 4 + current * 100;
                                AddWin(Power, current);
                            }
                        }
                        msgbox = true;
                    }
                    if (Reserve[i] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[i] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + Reserve[i + 1] / 4 + current * 100;
                                AddWin(Power, current);
                            }
                            else
                            {
                                current = 1;
                                Power = (Reserve[tc] / 4) * 4 + Reserve[i + 1] / 4 + current * 100;
                                AddWin(Power, current);
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }

        private void rHighCard(ref double current, ref double Power)
        {
            if (current == -1)
            {
                if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                {
                    current = -1;
                    Power = Reserve[i] / 4;
                    AddWin(Power, current);
                }
                else
                {
                    current = -1;
                    Power = Reserve[i + 1] / 4;
                    AddWin(Power, current);
                }
                if (Reserve[i] / 4 == 0 || Reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    Power = 13;
                    AddWin(Power, current);
                }
            }
        }

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (Holder[j].Visible)
                    Holder[j].Image = Deck[j];
            }
            if (current == winningHand.Current)
            {
                if (Power == winningHand.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    else if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    else if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    else if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    else if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    else if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    else if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    else if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    else if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    else if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (winners > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(potTextBox.Text) / winners;
                        player_ChipsTextBox.Text = player.Chips.ToString();
                        //pPanel.Visible = true;

                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (CheckWinners.Contains($"Bot {botIndex}"))
                        {
                            botsPanel[bot].Chips += int.Parse(potTextBox.Text)/winners;
                            botsPanel[bot].ChipsTextBox.Text = botsPanel[bot].Chips.ToString();
                            //botsPanel[bot].Visible = true;
                        }
                    }

                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(potTextBox.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (CheckWinners.Contains($"Bot {botIndex}"))
                        {
                            botsPanel[bot].Chips += int.Parse(potTextBox.Text) / winners;
                            //await Finish(1)
                            //botsPanel[bot].Visible = true;
                        }
                    }                   
                }
            }
        }

        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        Raise = 0;
                        call = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!playerFoldTurn)
                        {
                            playerStatusButton.Text = string.Empty;
                        }
                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            if (!botsPanel[bot].FoldTurn)
                            {
                                botsPanel[bot].StatusButton.Text = string.Empty;
                            }
                        }
                    }
                }
            }

            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (Holder[j].Image != Deck[j])
                    {
                        Holder[j].Image = Deck[j];

                        player.Call = 0;  
                        player.Raise = 0;

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            botsPanel[bot].Call = 0;
                            botsPanel[bot].Raise = 0;
                        }
                    }
                }
            }

            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (Holder[j].Image != Deck[j])
                    {
                        Holder[j].Image = Deck[j];

                        player.Call = 0;
                        player.Raise = 0;

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            botsPanel[bot].Call = 0;
                            botsPanel[bot].Raise = 0;
                        }
                    }
                }
            }

            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (Holder[j].Image != Deck[j])
                    {
                        Holder[j].Image = Deck[j];

                        player.Call = 0;
                        player.Raise = 0;

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            botsPanel[bot].Call = 0;
                            botsPanel[bot].Raise = 0;
                        }
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!playerStatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
                }

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = botsPanel[bot];
                    int botIndex = bot + 1;
                    int firstCard = botIndex*2;
                    int seconCard = botIndex*2 + 1;
                    if (!currentBot.StatusButton.Text.Contains("Fold"))
                    {
                        fixedLast = $"Bot {botIndex}";
                        Rules(firstCard, seconCard, ref currentBot.Type, ref currentBot.Power, currentBot.FoldTurn);
                    } 
                }
                
                Winner(playerType, playerPower, "Player", player.Chips, fixedLast);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = botsPanel[bot];
                    Winner(currentBot.Type, currentBot.Power, $"Bot {bot}", currentBot.Chips, fixedLast);
                    currentBot.FoldTurn = false;
                }
                
                restart = true;
                playerTurn = true;
                playerFoldTurn = false;

                if (player.Chips <= 0)
                {
                    AddChips addMoreChipsForm = new AddChips();
                    addMoreChipsForm.ShowDialog();
                    if (addMoreChipsForm.NewChips != 0)
                    {
                        player.Chips = addMoreChipsForm.NewChips;
                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            botsPanel[bot].Chips = addMoreChipsForm.NewChips;
                        }

                        playerFoldTurn = false;
                        playerTurn = true;
                        raiseButton.Enabled = true;
                        foldButton.Enabled = true;
                        checkButton.Enabled = true;

                        raiseButton.Text = "Raise";
                    }
                }

                player.PlayerPanel.Visible = false;
                player.Call = 0;
                player.Raise = 0;
                playerPower = 0;
                playerType = -1;

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = botsPanel[bot];

                    currentBot.Visible = false;
                    currentBot.Call = 0;
                    currentBot.Raise = 0;

                    // Moved up (they were below the variable type)
                    currentBot.Power = 0;
                    currentBot.Type = -1;
                }

                last = 0;
                call = this.bigBlind;
                Raise = 0;
                ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bools.Clear();
                rounds = 0;
                type = 0;                

                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                Win.Clear();
                winningHand.Current = 0;
                winningHand.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    Holder[os].Image = null;
                    Holder[os].Invalidate();
                    Holder[os].Visible = false;
                }

                potTextBox.Text = "0";
                playerStatusButton.Text = string.Empty;

                await Shuffle();
                await Turns();
            }
        }

        //void FixCall(Label status, ref int currentCall, ref int currentRaise, int options)
        //{
        //    if (rounds != 4)
        //    {
        //        if (options == 1)
        //        {
        //            if (status.Text.Contains("Raise"))
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
        //            if (currentRaise != Raise && currentRaise <= Raise)
        //            {
        //                call = Convert.ToInt32(Raise) - currentRaise;
        //            }

        //            if (currentCall != call || currentCall <= call)
        //            {
        //                call = call - currentCall;
        //            }

        //            if (currentRaise == Raise && Raise > 0)
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
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (playerStatusButton.Text.Contains("Raise"))
                    {
                        var changeRaise = playerStatusButton.Text.Substring(6);
                        player.Raise = int.Parse(changeRaise);
                    }
                    else if (playerStatusButton.Text.Contains("Call"))
                    {
                        var changeCall = playerStatusButton.Text.Substring(5);
                        player.Call = int.Parse(changeCall);
                    }
                    else if (playerStatusButton.Text.Contains("Check"))
                    {
                        player.Raise = 0;
                        player.Call = 0;
                    }
                }
                if (options == 2)
                {
                    if (player.Raise != Raise && player.Raise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - player.Raise;
                    }

                    if (player.Call != call || player.Call <= call)
                    {
                        call = call - player.Call;
                    }

                    if (player.Raise == Raise && Raise > 0)
                    {
                        call = 0;
                        callButton.Enabled = false;
                        callButton.Text = "Call button is unable.";
                    }
                }
            }
        }

        void FixCallBot(Bot currentBot, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (currentBot.StatusButton.Text.Contains("Raise"))
                    {
                        var changeRaise = currentBot.StatusButton.Text.Substring(6);
                        currentBot.Raise = int.Parse(changeRaise);
                    }
                    else if (currentBot.StatusButton.Text.Contains("Call"))
                    {
                        var changeCall = currentBot.StatusButton.Text.Substring(5);
                        currentBot.Call = int.Parse(changeCall);
                    }
                    else if (currentBot.StatusButton.Text.Contains("Check"))
                    {
                        currentBot.Raise = 0;
                        currentBot.Call = 0;
                    }
                }
                if (options == 2)
                {
                    if (currentBot.Raise != Raise && currentBot.Raise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - currentBot.Raise;
                    }

                    if (currentBot.Call != call || currentBot.Call <= call)
                    {
                        call = call - currentBot.Call;
                    }

                    if (currentBot.Raise == Raise && Raise > 0)
                    {
                        call = 0;
                        callButton.Enabled = false;
                        callButton.Text = "Call button is unable.";
                    }
                }
            }
        }

        async Task AllIn()
        {
            // All in
            if (player.Chips <= 0 && !intsadded)
            {
                if (playerStatusButton.Text.Contains("Raise"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
                else if (playerStatusButton.Text.Contains("Call"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
            }

            intsadded = false;
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = botsPanel[bot];

                if (currentBot.Chips <= 0 && !currentBot.FoldTurn)
                {
                    if (!intsadded)
                    {
                        ints.Add(currentBot.Chips);
                        intsadded = true;
                    }
                    intsadded = false;
                }
            }
            
            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }

            var abc = bools.Count(x => x == false);

            // LastManStanding
            if (abc == 1)
            {
                int index = bools.IndexOf(false);
                if (index == 0)
                {
                    player.Chips += int.Parse(potTextBox.Text);
                    player_ChipsTextBox.Text = player.Chips.ToString();
                    player.PlayerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    int botIndex = bot + 1;
                    if (index == botIndex)
                    {
                        botsPanel[bot].Chips += int.Parse(potTextBox.Text);
                        player_ChipsTextBox.Text = botsPanel[bot].Chips.ToString();
                        botsPanel[bot].Visible = true;
                        MessageBox.Show($"Bot {botIndex} wins");
                    }
                }

                for (int j = 0; j <= 16; j++)
                {
                    Holder[j].Visible = false;
                }
                await Finish(1);
            }

            intsadded = false;

            // FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
            {
                await Finish(2);
            }
        }

        async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }

            player.PlayerPanel.Visible = false;

            call = this.bigBlind;
            Raise = 0;
            foldedPlayers = 5;
            type = 0;
            rounds = 0;

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = botsPanel[bot];

                currentBot.Visible = false;
                currentBot.Power = 0;

                // Moved up (they were below variable Raise)
                currentBot.Type = -1;
                currentBot.Turn = false;
                currentBot.FoldTurn = false;
                currentBot.Folded = false;
                currentBot.Call = 0;
                currentBot.Raise = 0;
            }

            playerPower = 0;
            playerType = -1;
            Raise = 0;
            
            player.Folded = false;

            playerFoldTurn = false;
            playerTurn = true;
            restart = false;
            raising = false;

            player.Call = 0;
            
            player.Raise = 0;

            height = 0;
            width = 0;
            winners = 0;
            Flop = 1;
            Turn = 2;
            River = 3;
            End = 4;
            maxLeft = 6;
            last = 123;
            raisedTurn = 1;

            bools.Clear();
            CheckWinners.Clear();
            ints.Clear();
            Win.Clear();

            winningHand.Current = 0;
            winningHand.Power = 0;
            potTextBox.Text = "0";
            t = 60;
            maxUp = 10000000;
            turnCount = 0;

            playerStatusButton.Text = string.Empty;
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                botsPanel[bot].StatusButton.Text = string.Empty;
            }

            if (player.Chips <= 0)
            {
                AddChips chipsAdder = new AddChips();
                chipsAdder.ShowDialog();
                if (chipsAdder.NewChips != 0)
                {
                    player.Chips = chipsAdder.NewChips;
                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        botsPanel[bot].Chips = chipsAdder.NewChips;
                    }

                    playerFoldTurn = false;
                    playerTurn = true;
                    raiseButton.Enabled = true;
                    foldButton.Enabled = true;
                    checkButton.Enabled = true;
                    raiseButton.Text = "Raise";
                }
            }
            ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                Holder[os].Image = null;
                Holder[os].Invalidate();
                Holder[os].Visible = false;
            }
            await Shuffle();
            //await Turns();
        }

        void FixWinners()
        {
            Win.Clear();
            winningHand.Current = 0;
            winningHand.Power = 0;
            string fixedLast = "qwerty";
            if (!playerStatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = botsPanel[bot];
                int botIndex = bot + 1;
                int firstCard = botIndex*2;
                int secondCard = botIndex*2 + 1;

                if (!currentBot.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = $"Bot {botIndex}";
                    Rules(firstCard, secondCard, $"Bot {botIndex}", ref currentBot.Type, ref currentBot.Power, currentBot.FoldTurn);
                }
            }
           
            Winner(playerType, playerPower, "Player", player.Chips, fixedLast);
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Winner(botsPanel[bot].Type, botsPanel[bot].Power, $"Bot {bot + 1}", botsPanel[bot].Chips, fixedLast);
            }
        }

        void AI(int firstCard, int secondCard, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                else if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                else if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                else if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                else if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                else if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                else if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                else if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                else if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                else if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
            {
                Holder[firstCard].Visible = false;
                Holder[secondCard].Visible = false;
            }
        }

        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }

        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }

        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }

        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }

        private void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }

        private void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            else if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            else if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }

        private void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }

        private void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }

        private void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }

        private void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }

        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            raising = false;
        }

        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= call;
            sStatus.Text = "Call " + call;
            potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
        }

        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(Raise);
            sStatus.Text = "Raise " + Raise;
            potTextBox.Text = (int.Parse(potTextBox.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
            raising = true;
            sTurn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(ref sTurn, sStatus);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (call <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (Raise <= RoundN(sChips, n))
                    {
                        Raise = call * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        private void PH(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(ref sTurn, sStatus);
                }
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (Raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n) && Raise >= RoundN(sChips, n) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (Raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n - rnd) && Raise >= RoundN(sChips, n - rnd) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n - rnd) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    Raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        void Smooth(ref int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= call)
                    {
                        raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        potTextBox.Text = (int.Parse(potTextBox.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (botChips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        Raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (timerProgressBar.Value <= 0)
            {
                playerFoldTurn = true;
                await Turns();
            }
            if (t > 0)
            {
                t--;
                timerProgressBar.Value = (t / 6) * 100;
            }
        }

        private void Update_Tick(object sender, object e)
        {
            if (player.Chips <= 0)
            {
                player_ChipsTextBox.Text = "Chips : 0";
            }
            else
            {
                player_ChipsTextBox.Text = "Chips : " + player.Chips;
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                if (botsPanel[bot].Chips <= 0)
                {
                    botsPanel[bot].ChipsTextBox.Text = "Chips : 0";
                }
                else
                {
                    botsPanel[bot].ChipsTextBox.Text = string.Format("Chips : {0}", botsPanel[bot].Chips);
                }
            }

            if (player.Chips <= 0)
            {
                playerTurn = false;
                playerFoldTurn = true;
                callButton.Enabled = false;
                raiseButton.Enabled = false;
                foldButton.Enabled = false;
                checkButton.Enabled = false;
            }
            if (maxUp > 0)
            {
                maxUp--;
            }
            if (player.Chips >= call)
            {
                callButton.Text = "Call " + call.ToString();
            }
            else
            {
                callButton.Text = "All in";
                raiseButton.Enabled = false;
            }

            if (call > 0)
            {
                checkButton.Enabled = false;
            }
            else if (call <= 0)
            {
                checkButton.Enabled = true;
                callButton.Text = "Call";
                callButton.Enabled = false;
            }

            if (player.Chips <= 0)
            {
                raiseButton.Enabled = false;
            }

            int parsedValue;

            if (raiseTextBox.Text != string.Empty && int.TryParse(raiseTextBox.Text, out parsedValue))
            {
                if (player.Chips <= int.Parse(raiseTextBox.Text))
                {
                    raiseButton.Text = "All in";
                }
                else
                {
                    raiseButton.Text = "Raise";
                }
            }

            if (player.Chips < call)
            {
                raiseButton.Enabled = false;
            }
        }

        private async void botFoldOnClick(object sender, EventArgs e)
        {
            playerStatusButton.Text = "Fold";
            playerTurn = false;
            playerFoldTurn = true;
            await Turns();
        }

        private async void botCheckOnClick(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                playerTurn = false;
                playerStatusButton.Text = "Check";
            }
            else
            {
                // pStatus.Text = "All in " + Chips;
                checkButton.Enabled = false;
            }
            await Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
            if (player.Chips >= call)
            {
                player.Chips -= call;
                player_ChipsTextBox.Text = "Chips : " + player.Chips.ToString();
                if (potTextBox.Text != string.Empty)
                {
                    potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
                }
                else
                {
                    potTextBox.Text = call.ToString();
                }
                playerTurn = false;
                playerStatusButton.Text = "Call " + call;
                player.Call = call;
            }
            else if (player.Chips <= call && call > 0)
            {
                potTextBox.Text = (int.Parse(potTextBox.Text) + player.Chips).ToString();
                playerStatusButton.Text = "All in " + player.Chips;
                player.Chips = 0;
                player_ChipsTextBox.Text = "Chips : " + player.Chips.ToString();
                playerTurn = false;
                foldButton.Enabled = false;
                player.Call = player.Chips;
            }
            await Turns();
        }

        private async void botRaiseOnClick(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
            int parsedValue;
            if (raiseTextBox.Text != string.Empty && int.TryParse(raiseTextBox.Text, out parsedValue))
            {
                if (player.Chips > call)
                {
                    if (Raise * 2 > int.Parse(raiseTextBox.Text))
                    {
                        raiseTextBox.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise at least twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (player.Chips >= int.Parse(raiseTextBox.Text))
                        {
                            call = int.Parse(raiseTextBox.Text);
                            Raise = int.Parse(raiseTextBox.Text);
                            playerStatusButton.Text = "Raise " + call.ToString();
                            potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
                            callButton.Text = "Call";
                            player.Chips -= int.Parse(raiseTextBox.Text);
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = player.Chips;
                            Raise = player.Chips;
                            potTextBox.Text = (int.Parse(potTextBox.Text) + player.Chips).ToString();
                            playerStatusButton.Text = "Raise " + call.ToString();
                            player.Chips = 0;
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            playerTurn = false;
            await Turns();
        }

        private void botAddOnClick(object sender, EventArgs e)
        {
            if (addChips_TextBox.Text != string.Empty)
            {
                player.Chips += int.Parse(addChips_TextBox.Text);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    botsPanel[bot].Chips += int.Parse(addChips_Button.Text);
                }
            }

            player_ChipsTextBox.Text = "Chips : " + player.Chips.ToString();
        }

        private void botOptionsOnClick(object sender, EventArgs e)
        {
            bigBlind_TextBox.Text = this.bigBlind.ToString();
            smallBlind_TextBox.Text = sb.ToString();
            if (bigBlind_TextBox.Visible == false)
            {
                bigBlind_TextBox.Visible = true;
                smallBlind_TextBox.Visible = true;

                bigBlind_Button.Visible = true;
                smallBlind_Button.Visible = true;
            }
            else
            {
                bigBlind_TextBox.Visible = false;
                smallBlind_TextBox.Visible = false;

                bigBlind_Button.Visible = false;
                smallBlind_Button.Visible = false;
            }
        }

        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (smallBlind_TextBox.Text.Contains(",") || smallBlind_TextBox.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                smallBlind_TextBox.Text = sb.ToString();
                return;
            }
            if (!int.TryParse(smallBlind_TextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                smallBlind_TextBox.Text = sb.ToString();
                return;
            }

            if (int.Parse(smallBlind_TextBox.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                smallBlind_TextBox.Text = sb.ToString();
            }
            else if (int.Parse(smallBlind_TextBox.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            else
            {
                sb = int.Parse(smallBlind_TextBox.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (bigBlind_TextBox.Text.Contains(",") || bigBlind_TextBox.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                bigBlind_TextBox.Text = this.bigBlind.ToString();
                return;
            }
            if (!int.TryParse(smallBlind_TextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                smallBlind_TextBox.Text = this.bigBlind.ToString();
                return;
            }

            if (int.Parse(bigBlind_TextBox.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                bigBlind_TextBox.Text = this.bigBlind.ToString();
            }
            else if (int.Parse(bigBlind_TextBox.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            else
            {
                this.bigBlind = int.Parse(bigBlind_TextBox.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            width = this.Width;
            height = this.Height;
        }
        #endregion

        private void SetBotCards(Panel botPanel, PictureBox[] Holder, Bitmap backImage, int[] Reserve, int horizontal, int vertical, int currentCard)
        {
            Holder[currentCard].Tag = Reserve[currentCard];

            Holder[currentCard].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Holder[currentCard].Image = backImage;
            // Holder[currentCard].Image = Deck[currentCard];
            Holder[currentCard].Location = new Point(horizontal, vertical);

            if (currentCard % 2 == 0)
            {
                horizontal += Holder[currentCard].Width;
            }

            Holder[currentCard].Visible = true;
            Controls.Add(botPanel);
            botPanel.Location = new Point(Holder[currentCard].Left - 10, Holder[currentCard].Top - 10);
            botPanel.BackColor = Color.DarkBlue;
            botPanel.Height = 150;
            botPanel.Width = 180;
            botPanel.Visible = false;
        }

        private void EnableButtons()
        {
            raiseButton.Enabled = true;
            callButton.Enabled = true;
            foldButton.Enabled = true;
        }
    }
}