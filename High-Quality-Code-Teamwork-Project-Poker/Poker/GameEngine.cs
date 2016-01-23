using Poker.UI;

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
    using Poker.Contracts;


    public partial class GameEngine : Form
    {
        #region Constants
        private const int NumberOfBots = 5;
        private const int AllCardsOnTheTable = 17;
        #endregion

        #region Variables
        ProgressBar progressBar = new ProgressBar();
        ApplicationWriter writer = new ApplicationWriter();

        private Bot[] gameBots = new Bot[5]
        {
            new Bot(),
            new Bot(),
            new Bot(),
            new Bot(),
            new Bot()
        };

        Player player = new Player();
        private int call = 500;
        private int foldedPlayers = 5;
        private double type;
        private double rounds = 0;
        private double Raise = 0;

        private bool intsadded;
        bool changed;

        int winners = 0, Flop = 1, Turn = 2, River = 3, End = 4, maxLeft = 6;
        private int last = 123;
        int raisedTurn = 1;

        //List<bool?> bools = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        //bool playerFoldTurn = false, playerTurn = true;
        bool restart = false, raising = false;
        Type winningHand;
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
            //int width = this.Width;
            //int height = this.Height;
            Shuffle();
            potTextBox.Enabled = false;

            player.ParticipantPanel.ChipsTextBox.Enabled = false;
            player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + player.Chips.ToString();

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                gameBots[bot].ParticipantPanel.ChipsTextBox.Enabled = false;
                gameBots[bot].ParticipantPanel.ChipsTextBox.Text = string.Format("Chips: {0}", gameBots[bot].Chips);
            }

            timer.Interval = 1 * 1 * 1000;
            timer.Tick += timer_Tick;
            Updates.Interval = 1 * 1 * 100;
            Updates.Tick += Update_Tick;

            this.bigBlindTextBox.Visible = true;
            this.smallBlindTextBox.Visible = true;

            raiseTextBox.Text = (this.bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            //bools.Add(player.FoldTurn);
            //for (int bot = 0; bot < NumberOfBots; bot++)
            //{
            //    bools.Add(gameBots[bot].FoldTurn);
            //}

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

            for (int currentCard = 0; currentCard < AllCardsOnTheTable; currentCard++)
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
                    this.Controls.Add(player.ParticipantPanel);
                    player.ParticipantPanel.Location = new Point(Holder[0].Left - 10, Holder[0].Top - 10);
                    player.ParticipantPanel.BackColor = Color.DarkBlue;
                    player.ParticipantPanel.Height = 150;
                    player.ParticipantPanel.Width = 180;
                    player.ParticipantPanel.Visible = false;
                }

                //for (int bot = 0; bot < NumberOfBots; bot++)
                //{
                //    if (gameBots[bot].Chips > 0)
                //    {
                        
                //    }
                //}

                if (gameBots[0].Chips > 0)
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

                        this.SetBotCards(gameBots[0], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (gameBots[1].Chips > 0)
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

                        this.SetBotCards(gameBots[1], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (gameBots[2].Chips > 0)
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

                        this.SetBotCards(gameBots[2], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (gameBots[3].Chips > 0)
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

                        this.SetBotCards(gameBots[3], Holder, backImage, Reserve, horizontal, vertical, currentCard);

                        if (currentCard == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (gameBots[4].Chips > 0)
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

                        this.SetBotCards(gameBots[4], Holder, backImage, Reserve, horizontal, vertical, currentCard);

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
                        if (gameBots[bot].Chips <= 0)
                        {
                            gameBots[bot].FoldTurn = true;
                            Holder[card].Visible = false;
                            Holder[card + 1].Visible = false;
                        }
                        else
                        {
                            gameBots[0].FoldTurn = false;
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
            if (!player.FoldTurn)
            {
                if (player.Turn)
                {
                    FixCallPlayer(1);
                    writer.Print("Player's Turn");
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

            if (player.FoldTurn || !player.Turn)
            {
                await AllIn();
                if (player.FoldTurn && !player.IsFolded)
                {
                    if (callButton.Text.Contains("All in") == false || raiseButton.Text.Contains("All in") == false)
                    {
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        maxLeft--;
                        player.IsFolded = true;
                    }
                }

                await CheckRaise(0, 0);

                timerProgressBar.Visible = false;
                raiseButton.Enabled = false;
                callButton.Enabled = false;
                foldButton.Enabled = false;

                timer.Stop();

                gameBots[0].Turn = true;
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = gameBots[bot];
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
                            Rules(firstCard, secondCard, currentBot);

                            // TODO: Maybe we can remove this
                            writer.Print($"Bot {botIndex}'s Turn");

                            AI(firstCard, secondCard, currentBot);
                            turnCount++;
                            last = botIndex;
                            currentBot.Turn = false;
                            if (botIndex != NumberOfBots)
                            {
                                gameBots[bot + 1].Turn = true;
                            }
                        }

                        if (currentBot.FoldTurn && !currentBot.IsFolded)
                        {
                            //bools.RemoveAt(botIndex);
                            //bools.Insert(botIndex, null);
                            maxLeft--;
                            currentBot.IsFolded = true;
                        }

                        if (currentBot.FoldTurn || !currentBot.Turn)
                        {
                            await CheckRaise(botIndex, botIndex);
                            if(botIndex != NumberOfBots)
                            {
                                gameBots[bot + 1].Turn = true;
                            }
                        }
                    }
                }

                this.player.Turn = true;
                if (player.FoldTurn && !player.IsFolded)
                {
                    if (!callButton.Text.Contains("All in") || !raiseButton.Text.Contains("All in"))
                    {
                        // TODO: Create PlayerClass and work with its bools
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        maxLeft--;
                        player.IsFolded = true;
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

        void Rules(int firstCard, int secondCard, IGameParticipant currentGameParticipant)
        {
            if (firstCard == 0 && secondCard == 1)
            {
            }

            if (!currentGameParticipant.FoldTurn || firstCard == 0 && secondCard == 1 && player.ParticipantPanel.StatusButton.Text.Contains("Fold") == false)
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
                        rPairFromHand(currentGameParticipant);

                        // Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(currentGameParticipant);

                        // Two Pair current = 2
                        rTwoPair(currentGameParticipant);

                        // Three of a kind current = 3
                        rThreeOfAKind(currentGameParticipant, Straight);

                        // Straight current = 4
                        rStraight(currentGameParticipant, Straight);

                        // Flush current = 5 || 5.5
                        rFlush(currentGameParticipant, ref vf, Straight1);

                        // Full House current = 6
                        rFullHouse(currentGameParticipant, ref done, Straight);

                        // Four of a Kind current = 7
                        rFourOfAKind(currentGameParticipant, Straight);

                        // Straight Flush current = 8 || 9
                        rStraightFlush(currentGameParticipant, st1, st2, st3, st4);

                        // High Card current = -1
                        rHighCard(currentGameParticipant);
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
            Win.Add(typeToAdd);
            // Returns the best hand so far by sorting the Win collection.
            winningHand = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        }

        private void rStraightFlush(IGameParticipant currentGameParticipant, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (currentGameParticipant.Type >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power = (int)(st1.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power = (int)(st1.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power = (int)(st2.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power = (int)(st2.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power = (int)(st3.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power = (int)(st3.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentGameParticipant.Type = 8;
                        currentGameParticipant.Power = (int)(st4.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        currentGameParticipant.Type = 9;
                        currentGameParticipant.Power = (int)(st4.Max() / 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rFourOfAKind(IGameParticipant currentGameParticipant, int[] Straight)
        {
            if (currentGameParticipant.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        currentGameParticipant.Type = 7;
                        currentGameParticipant.Power = (int)((Straight[j] / 4) * 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        currentGameParticipant.Type = 7;
                        currentGameParticipant.Power = (int)(13 * 4 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rFullHouse(IGameParticipant currentGameParticipant, ref bool done, int[] Straight)
        {
            if (currentGameParticipant.Type >= -1)
            {
                type = currentGameParticipant.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentGameParticipant.Type = 6;
                                currentGameParticipant.Power = (int)(13 * 2 + currentGameParticipant.Type * 100);
                                AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                break;
                            }
                            else if (fh.Max() / 4 > 0)
                            {
                                currentGameParticipant.Type = 6;
                                currentGameParticipant.Power = (int)(fh.Max() / 4 * 2 + currentGameParticipant.Type * 100);
                                AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                    currentGameParticipant.Power = (int) type;
                }
            }
        }

        private void rFlush(IGameParticipant currentGameParticipant, ref bool vf, int[] Straight1)
        {
            if (currentGameParticipant.Type >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();

                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        currentGameParticipant.Type = 5;

                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                        }

                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                        }
                        else if (Reserve[i] / 4 < f1.Max() / 4 && Reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            currentGameParticipant.Power = (int)(f1.Max() + currentGameParticipant.Type * 100);
                        }

                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                }

                if (f1.Length == 4) // different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        currentGameParticipant.Type = 5;

                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                        }
                        else
                        {
                            currentGameParticipant.Power = (int)(f1.Max() + currentGameParticipant.Type * 100);
                        }

                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        currentGameParticipant.Type = 5;
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f1.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (Reserve[i] % 4 == f1[0] % 4 && Reserve[i] / 4 > f1.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f1[0] % 4 && Reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f1.Min() / 4 && Reserve[i + 1] / 4 < f1.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(f1.Max() + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f2.Max() / 4 && Reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f2.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f2.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f2.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (Reserve[i] % 4 == f2[0] % 4 && Reserve[i] / 4 > f2.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f2[0] % 4 && Reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f2.Min() / 4 && Reserve[i + 1] / 4 < f2.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(f2.Max() + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f3.Max() / 4 && Reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f3.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f3.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f3.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (Reserve[i] % 4 == f3[0] % 4 && Reserve[i] / 4 > f3.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f3[0] % 4 && Reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f3.Min() / 4 && Reserve[i + 1] / 4 < f3.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(f3.Max() + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f4.Max() / 4 && Reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f4.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f4.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                        else
                        {
                            currentGameParticipant.Type = 5;
                            currentGameParticipant.Power = (int)(f4.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (Reserve[i] % 4 == f4[0] % 4 && Reserve[i] / 4 > f4.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f4[0] % 4 && Reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(Reserve[i + 1] + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f4.Min() / 4 && Reserve[i + 1] / 4 < f4.Min())
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(f4.Max() + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        vf = true;
                    }
                }

                // ace
                if (f1.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (f2.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (f3.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (f4.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        currentGameParticipant.Type = 5.5;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rStraight(IGameParticipant currentGameParticipant, int[] Straight)
        {
            if (currentGameParticipant.Type >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            currentGameParticipant.Type = 4;
                            currentGameParticipant.Power = (int)(op.Max() + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 4;
                            currentGameParticipant.Power = (int)(op[j + 4] + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        currentGameParticipant.Type = 4;
                        currentGameParticipant.Power = (int)(13 + currentGameParticipant.Type * 100);
                        AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }
            }
        }

        private void rThreeOfAKind(IGameParticipant currentGameParticipant, int[] Straight)
        {
            if (currentGameParticipant.Type >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            currentGameParticipant.Type = 3;
                            currentGameParticipant.Power = (int)(13 * 3 + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 3;
                            currentGameParticipant.Power = (int)(fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)(13 * 4 + (Reserve[i + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                        }
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)(13 * 4 + (Reserve[i] / 4) * 2 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                        }
                                        if (Reserve[i + 1] / 4 != 0 && Reserve[i] / 4 != 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)((Reserve[i] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                                if (Reserve[tc] / 4 != Reserve[i] / 4 && Reserve[tc] / 4 != Reserve[i + 1] / 4 && currentGameParticipant.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)((Reserve[i] / 4) * 2 + 13 * 4 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                        }
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)((Reserve[i + 1] / 4) * 2 + 13 * 4 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                        }
                                        if (Reserve[i + 1] / 4 != 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)((Reserve[tc] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                        }
                                        if (Reserve[i] / 4 != 0)
                                        {
                                            currentGameParticipant.Type = 2;
                                            currentGameParticipant.Power = (int)((Reserve[tc] / 4) * 2 + (Reserve[i] / 4) * 2 + currentGameParticipant.Type * 100);
                                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (currentGameParticipant.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = (int)(13 + Reserve[i] / 4 + currentGameParticipant.Type * 100);
                                                // using AddWin method with current value = 1 was intended
                                                // to match with default game logic 
                                                AddWin(currentGameParticipant.Power, 1);
                                            }
                                            else
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = (int)(Reserve[tc] / 4 + Reserve[i] / 4 + currentGameParticipant.Type * 100);
                                                AddWin(currentGameParticipant.Power, 1);
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = (int)(13 + Reserve[i + 1] + currentGameParticipant.Type * 100);
                                                AddWin(currentGameParticipant.Power, 1);
                                            }
                                            else
                                            {
                                                currentGameParticipant.Type = 0;
                                                currentGameParticipant.Power = (int)(Reserve[tc] / 4 + Reserve[i + 1] / 4 + currentGameParticipant.Type * 100);
                                                AddWin(currentGameParticipant.Power, 1);
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
                if (Reserve[i] / 4 == Reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (Reserve[i] / 4 == 0)
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power = (int) (13 * 4 + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power = (int) ((Reserve[i + 1] / 4) * 4 + currentGameParticipant.Type * 100);
                            AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = (int)(13 * 4 + Reserve[i] / 4 + currentGameParticipant.Type * 100);
                                AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            }
                            else
                            {
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = (int)((Reserve[i + 1] / 4) * 4 + Reserve[i] / 4 + currentGameParticipant.Type * 100);
                                AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = (int)(13 * 4 + Reserve[i + 1] / 4 + currentGameParticipant.Type * 100);
                                AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                            }
                            else
                            {
                                currentGameParticipant.Type = 1;
                                currentGameParticipant.Power = (int)((Reserve[tc] / 4) * 4 + Reserve[i + 1] / 4 + currentGameParticipant.Type * 100);
                                AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
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
                if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = Reserve[i] / 4;
                    AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                }
                else
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = Reserve[i + 1] / 4;
                    AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                }
                if (Reserve[i] / 4 == 0 || Reserve[i + 1] / 4 == 0)
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = 13;
                    AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                }
            }
        }

        void ValidateWinner(IGameParticipant currentGameParticipant, string currentText, string lastly)
        {
            //TODO: needs some upgrading
            double current = currentGameParticipant.Type;
            int Power = currentGameParticipant.Power;

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
                        writer.Print(currentText + " High Card ");
                    }
                    else if (current == 1 || current == 0)
                    {
                        writer.Print(currentText + " Pair ");
                    }
                    else if (current == 2)
                    {
                        writer.Print(currentText + " Two Pair ");
                    }
                    else if (current == 3)
                    {
                        writer.Print(currentText + " Three of a Kind ");
                    }
                    else if (current == 4)
                    {
                        writer.Print(currentText + " Straight ");
                    }
                    else if (current == 5 || current == 5.5)
                    {
                        writer.Print(currentText + " Flush ");
                    }
                    else if (current == 6)
                    {
                        writer.Print(currentText + " Full House ");
                    }
                    else if (current == 7)
                    {
                        writer.Print(currentText + " Four of a Kind ");
                    }
                    else if (current == 8)
                    {
                        writer.Print(currentText + " Straight Flush ");
                    }
                    else if (current == 9)
                    {
                        writer.Print(currentText + " Royal Flush ! ");
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
                        player.ParticipantPanel.ChipsTextBox.Text = player.Chips.ToString();
                        //pPanel.Visible = true;

                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (CheckWinners.Contains($"Bot {botIndex}"))
                        {
                            gameBots[bot].Chips += int.Parse(potTextBox.Text)/winners;
                            gameBots[bot].ParticipantPanel.ChipsTextBox.Text = gameBots[bot].Chips.ToString();
                            //gameBots[bot].Visible = true;
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
                            gameBots[bot].Chips += int.Parse(potTextBox.Text) / winners;
                            //await Finish(1)
                            //gameBots[bot].Visible = true;
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
                        if (!player.FoldTurn)
                        {
                            player.ParticipantPanel.StatusButton.Text = string.Empty;
                        }

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            if (!gameBots[bot].FoldTurn)
                            {
                                gameBots[bot].ParticipantPanel.StatusButton.Text = string.Empty;
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
                            gameBots[bot].Call = 0;
                            gameBots[bot].Raise = 0;
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
                            gameBots[bot].Call = 0;
                            gameBots[bot].Raise = 0;
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
                            gameBots[bot].Call = 0;
                            gameBots[bot].Raise = 0;
                        }
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = string.Empty;
                if (!player.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, player);
                }

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = gameBots[bot];
                    int botIndex = bot + 1;
                    int firstCard = botIndex*2;
                    int seconCard = botIndex*2 + 1;
                    if (!currentBot.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                    {
                        fixedLast = $"Bot {botIndex}";
                        Rules(firstCard, seconCard, currentBot);
                    } 
                }
                
                ValidateWinner(player, "Player", fixedLast);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = gameBots[bot];
                    ValidateWinner(currentBot, $"Bot {bot + 1}", fixedLast);
                    currentBot.FoldTurn = false;
                }
                
                restart = true;
                player.Turn = true;
                player.FoldTurn = false;

                if (player.Chips <= 0)
                {
                    AddChips addMoreChipsForm = new AddChips();
                    addMoreChipsForm.ShowDialog();
                    if (addMoreChipsForm.NewChips != 0)
                    {
                        player.Chips = addMoreChipsForm.NewChips;
                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            gameBots[bot].Chips = addMoreChipsForm.NewChips;
                        }

                        player.FoldTurn = false;
                        player.Turn = true;
                        raiseButton.Enabled = true;
                        foldButton.Enabled = true;
                        checkButton.Enabled = true;

                        raiseButton.Text = "Raise";
                    }
                }

                player.ParticipantPanel.Visible = false;
                player.Call = 0;
                player.Raise = 0;
                player.Power = 0;
                player.Type = -1;

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    Bot currentBot = gameBots[bot];

                    currentBot.ParticipantPanel.Visible = false;
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
                //bools.Clear();
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
                player.ParticipantPanel.StatusButton.Text = string.Empty;

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
                    if (player.ParticipantPanel.StatusButton.Text.Contains("Raise"))
                    {
                        var changeRaise = player.ParticipantPanel.StatusButton.Text.Substring(6);
                        player.Raise = int.Parse(changeRaise);
                    }
                    else if (player.ParticipantPanel.StatusButton.Text.Contains("Call"))
                    {
                        var changeCall = player.ParticipantPanel.StatusButton.Text.Substring(5);
                        player.Call = int.Parse(changeCall);
                    }
                    else if (player.ParticipantPanel.StatusButton.Text.Contains("Check"))
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
                    if (currentBot.ParticipantPanel.StatusButton.Text.Contains("Raise"))
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
                if (player.ParticipantPanel.StatusButton.Text.Contains("Raise"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
                else if (player.ParticipantPanel.StatusButton.Text.Contains("Call"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
            }

            intsadded = false;
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = gameBots[bot];

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

            int leftOneNotFoldedPlayer = gameBots.Count(x => x.FoldTurn == false);
            if (!player.FoldTurn)
            {
                leftOneNotFoldedPlayer++;
            }
            // LastManStanding
            if (leftOneNotFoldedPlayer == 1)
            {
                if (!player.FoldTurn)
                {
                    player.Chips += int.Parse(potTextBox.Text);
                    player.ParticipantPanel.ChipsTextBox.Text = player.Chips.ToString();
                    player.ParticipantPanel.Visible = true;
                    writer.Print("Player Wins");
                }
                else
                {
                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (!gameBots[bot].FoldTurn || !gameBots[bot].IsFolded)
                        {
                            gameBots[bot].Chips += int.Parse(potTextBox.Text);

                            // why is this here?!
                            player.ParticipantPanel.ChipsTextBox.Text = gameBots[bot].Chips.ToString();

                            gameBots[bot].ParticipantPanel.Visible = true;
                            writer.Print($"Bot {botIndex} wins");
                        }
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
            if (leftOneNotFoldedPlayer < 6 && leftOneNotFoldedPlayer > 1 && rounds >= End)
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

            player.ParticipantPanel.Visible = false;

            call = this.bigBlind;
            Raise = 0;
            foldedPlayers = 5;
            type = 0;
            rounds = 0;

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = gameBots[bot];

                currentBot.ParticipantPanel.Visible = false;
                currentBot.Power = 0;

                // Moved up (they were below variable Raise)
                currentBot.Type = -1;
                currentBot.Turn = false;
                currentBot.FoldTurn = false;
                currentBot.IsFolded = false;
                currentBot.Call = 0;
                currentBot.Raise = 0;
            }

            player.Power = 0;
            player.Type = -1;
            Raise = 0;
            
            player.IsFolded = false;

            player.FoldTurn = false;
            player.Turn = true;
            restart = false;
            raising = false;

            player.Call = 0;
            
            player.Raise = 0;

            //height = 0;
            //width = 0;
            winners = 0;
            Flop = 1;
            Turn = 2;
            River = 3;
            End = 4;
            maxLeft = 6;
            last = 123;
            raisedTurn = 1;

            //bools.Clear();
            CheckWinners.Clear();
            ints.Clear();
            Win.Clear();

            winningHand.Current = 0;
            winningHand.Power = 0;
            potTextBox.Text = "0";
            t = 60;
            maxUp = 10000000;
            turnCount = 0;

            player.ParticipantPanel.StatusButton.Text = string.Empty;
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                gameBots[bot].ParticipantPanel.StatusButton.Text = string.Empty;
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
                        gameBots[bot].Chips = chipsAdder.NewChips;
                    }

                    player.FoldTurn = false;
                    player.Turn = true;
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
            string fixedLast = string.Empty;
            if (!player.ParticipantPanel.StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, player);
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                Bot currentBot = gameBots[bot];
                int botIndex = bot + 1;
                int firstCard = botIndex*2;
                int secondCard = botIndex*2 + 1;

                if (!currentBot.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = $"Bot {botIndex}";
                    Rules(firstCard, secondCard, currentBot);
                }
            }
           
            ValidateWinner(player, "Player", fixedLast);
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                ValidateWinner(gameBots[bot], $"Bot {bot + 1}", fixedLast);
            }
        }

        void AI(int firstCard, int secondCard, IGameParticipant currentGameParticipant)
        {
            int botType = (int)currentGameParticipant.Type;

            if (!currentGameParticipant.FoldTurn)
            {
                if (botType == -1)
                {
                    HighCard(currentGameParticipant);
                }
                else if (botType == 0)
                {
                    PairTable(currentGameParticipant);
                }
                else if (botType == 1)
                {
                    PairHand(currentGameParticipant);
                }
                else if (botType == 2)
                {
                    TwoPair(currentGameParticipant);
                }
                else if (botType == 3)
                {
                    ThreeOfAKind(currentGameParticipant);
                }
                else if (botType == 4)
                {
                    Straight(currentGameParticipant);
                }
                else if (botType == 5 || botType == 5.5)
                {
                    Flush(currentGameParticipant);
                }
                else if (botType == 6)
                {
                    FullHouse(currentGameParticipant);
                }
                else if (botType == 7)
                {
                    FourOfAKind(currentGameParticipant);
                }
                else if (botType == 8 || botType == 9)
                {
                    StraightFlush(currentGameParticipant);
                }
            }

            if (currentGameParticipant.FoldTurn)
            {
                Holder[firstCard].Visible = false;
                Holder[secondCard].Visible = false;
            }
        }

        private void HighCard(IGameParticipant currentGameParticipant)
        {
            HP(currentGameParticipant, 20, 25);
        }

        private void PairTable(IGameParticipant currentGameParticipant)
        {
            HP(currentGameParticipant, 16, 25);
        }

        private void PairHand(IGameParticipant currentGameParticipant)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (currentGameParticipant.Power <= 199 && currentGameParticipant.Power >= 140)
            {
                PH(currentGameParticipant, rCall, 6, rRaise);
            }
            else if (currentGameParticipant.Power <= 139 && currentGameParticipant.Power >= 128)
            {
                PH(currentGameParticipant, rCall, 7, rRaise);
            }
            else if (currentGameParticipant.Power < 128 && currentGameParticipant.Power >= 101)
            {
                PH(currentGameParticipant, rCall, 9, rRaise);
            }
        }

        private void TwoPair(IGameParticipant currentGameParticipant)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (currentGameParticipant.Power <= 290 && currentGameParticipant.Power >= 246)
            {
                PH(currentGameParticipant, rCall, 3, rRaise);
            }
            else if (currentGameParticipant.Power <= 244 && currentGameParticipant.Power >= 234)
            {
                PH(currentGameParticipant, rCall, 4, rRaise);
            }
            else if (currentGameParticipant.Power < 234 && currentGameParticipant.Power >= 201)
            {
                PH(currentGameParticipant, rCall, 4, rRaise);
            }
        }

        private void ThreeOfAKind(IGameParticipant currentGameParticipant)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (currentGameParticipant.Power <= 390 && currentGameParticipant.Power >= 330)
            {
                Smooth(currentGameParticipant, tCall, tRaise);
            }
            if (currentGameParticipant.Power <= 327 && currentGameParticipant.Power >= 321)//10  8
            {
                Smooth(currentGameParticipant, tCall, tRaise);
            }
            if (currentGameParticipant.Power < 321 && currentGameParticipant.Power >= 303)//7 2
            {
                Smooth(currentGameParticipant, tCall, tRaise);
            }
        }

        private void Straight(IGameParticipant currentGameParticipant)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (currentGameParticipant.Power <= 480 && currentGameParticipant.Power >= 410)
            {
                Smooth(currentGameParticipant, sCall, sRaise);
            }
            else if (currentGameParticipant.Power <= 409 && currentGameParticipant.Power >= 407)//10  8
            {
                Smooth(currentGameParticipant, sCall, sRaise);
            }
            else if (currentGameParticipant.Power < 407 && currentGameParticipant.Power >= 404)
            {
                Smooth(currentGameParticipant, sCall, sRaise);
            }
        }

        private void Flush(IGameParticipant currentGameParticipant)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(currentGameParticipant, fCall, fRaise);
        }

        private void FullHouse(IGameParticipant currentGameParticipant)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (currentGameParticipant.Power <= 626 && currentGameParticipant.Power >= 620)
            {
                Smooth(currentGameParticipant, fhCall, fhRaise);
            }
            if (currentGameParticipant.Power < 620 && currentGameParticipant.Power >= 602)
            {
                Smooth(currentGameParticipant, fhCall, fhRaise);
            }
        }

        private void FourOfAKind(IGameParticipant currentGameParticipant)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (currentGameParticipant.Power <= 752 && currentGameParticipant.Power >= 704)
            {
                Smooth(currentGameParticipant, fkCall, fkRaise);
            }
        }

        private void StraightFlush(IGameParticipant currentGameParticipant)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (currentGameParticipant.Power <= 913 && currentGameParticipant.Power >= 804)
            {
                Smooth(currentGameParticipant, sfCall, sfRaise);
            }
        }

        private void Fold(IGameParticipant currentGameParticipant)
        {
            raising = false;
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Is Folded";
            currentGameParticipant.Turn = false;
            currentGameParticipant.FoldTurn = true;
        }

        private void Check(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Check";
            currentGameParticipant.Turn = false;
            raising = false;
        }

        private void Call(IGameParticipant currentGameParticipant)
        {
            raising = false;
            currentGameParticipant.Turn = false;
            currentGameParticipant.Chips -= call;
            currentGameParticipant.ParticipantPanel.Text = "Call " + call;
            potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
        }

        private void Raised(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.Chips -= Convert.ToInt32(Raise);
            currentGameParticipant.ParticipantPanel.StatusButton.Text = "Raise " + Raise;
            potTextBox.Text = (int.Parse(potTextBox.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
            raising = true;
            currentGameParticipant.Turn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        // private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        private void HP(IGameParticipant currrentGameParticipant, int n, int n1)
        {
            Random random = new Random();
            int rnd = random.Next(1, 4);
            if (call <= 0)
            {
                Check(currrentGameParticipant);
            }

            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(currrentGameParticipant.Chips, n))
                    {
                        Call(currrentGameParticipant);
                    }
                    else
                    {
                        Fold(currrentGameParticipant);
                    }
                }
                if (rnd == 2)
                {
                    if (call <= RoundN(currrentGameParticipant.Chips, n1))
                    {
                        Call(currrentGameParticipant);
                    }
                    else
                    {
                        Fold(currrentGameParticipant);
                    }
                }
            }
            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(currrentGameParticipant);
                }
                else
                {
                    if (Raise <= RoundN(currrentGameParticipant.Chips, n))
                    {
                        Raise = call * 2;
                        Raised(currrentGameParticipant);
                    }
                    else
                    {
                        Fold(currrentGameParticipant);
                    }
                }
            }

            if (currrentGameParticipant.Chips <= 0)
            {
                currrentGameParticipant.FoldTurn = true;
            }
        }

        private void PH(IGameParticipant currentGameParticipant, int n, int n1, int r)
        {
            Random random = new Random();
            int rnd = random.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(currentGameParticipant);
                }
                if (call > 0)
                {
                    if (call >= RoundN(currentGameParticipant.Chips, n1))
                    {
                        Fold(currentGameParticipant);
                    }
                    if (Raise > RoundN(currentGameParticipant.Chips, n))
                    {
                        Fold(currentGameParticipant);
                    }
                    if (!currentGameParticipant.FoldTurn)
                    {
                        if (call >= RoundN(currentGameParticipant.Chips, n) && call <= RoundN(currentGameParticipant.Chips, n1))
                        {
                            Call(currentGameParticipant);
                        }
                        if (Raise <= RoundN(currentGameParticipant.Chips, n) && Raise >= RoundN(currentGameParticipant.Chips, n) / 2)
                        {
                            Call(currentGameParticipant);
                        }
                        if (Raise <= RoundN(currentGameParticipant.Chips, n) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(currentGameParticipant.Chips, n);
                                Raised(currentGameParticipant);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(currentGameParticipant);
                            }
                        }
                    }
                }
            }

            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(currentGameParticipant.Chips, n1 - rnd))
                    {
                        Fold(currentGameParticipant);
                    }
                    if (Raise > RoundN(currentGameParticipant.Chips, n - rnd))
                    {
                        Fold(currentGameParticipant);
                    }
                    if (!currentGameParticipant.FoldTurn)
                    {
                        if (call >= RoundN(currentGameParticipant.Chips, n - rnd) && call <= RoundN(currentGameParticipant.Chips, n1 - rnd))
                        {
                            Call(currentGameParticipant);
                        }
                        if (Raise <= RoundN(currentGameParticipant.Chips, n - rnd) && Raise >= RoundN(currentGameParticipant.Chips, n - rnd) / 2)
                        {
                            Call(currentGameParticipant);
                        }
                        if (Raise <= RoundN(currentGameParticipant.Chips, n - rnd) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(currentGameParticipant.Chips, n - rnd);
                                Raised(currentGameParticipant);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(currentGameParticipant);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    Raise = RoundN(currentGameParticipant.Chips, r - rnd);
                    Raised(currentGameParticipant);
                }
            }

            if (currentGameParticipant.Chips <= 0)
            {
                currentGameParticipant.FoldTurn = true;
            }
        }

        void Smooth(IGameParticipant currentGameParticipant, int call, int raise)
        {
            //Random random = new Random();
            //int rnd = random.Next(1, 3);
            if (this.call <= 0)
            {
                Check(currentGameParticipant);
            }
            else
            {
                if (this.call >= RoundN(currentGameParticipant.Chips, call))
                {
                    if (currentGameParticipant.Chips > this.call)
                    {
                        Call(currentGameParticipant);
                    }
                    else if (currentGameParticipant.Chips <= this.call)
                    {
                        raising = false;
                        currentGameParticipant.Turn = false;
                        currentGameParticipant.Chips = 0;
                        currentGameParticipant.ParticipantPanel.StatusButton.Text = "Call " + currentGameParticipant.Chips;
                        potTextBox.Text = (int.Parse(potTextBox.Text) + currentGameParticipant.Chips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (currentGameParticipant.Chips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(currentGameParticipant);
                        }
                        else
                        {
                            Call(currentGameParticipant);
                        }
                    }
                    else
                    {
                        Raise = this.call * 2;
                        Raised(currentGameParticipant);
                    }
                }
            }

            if (currentGameParticipant.Chips <= 0)
            {
                currentGameParticipant.FoldTurn = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (timerProgressBar.Value <= 0)
            {
                player.FoldTurn = true;
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
                player.ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            }
            else
            {
                player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + player.Chips;
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                if (gameBots[bot].Chips <= 0)
                {
                    gameBots[bot].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
                }
                else
                {
                    gameBots[bot].ParticipantPanel.ChipsTextBox.Text = string.Format("Chips : {0}", gameBots[bot].Chips);
                }
            }

            if (player.Chips <= 0)
            {
                player.Turn = false;
                player.FoldTurn = true;
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
            player.ParticipantPanel.StatusButton.Text = "Fold";
            player.Turn = false;
            player.FoldTurn = true;
            await Turns();
        }

        private async void botCheckOnClick(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                player.Turn = false;
                player.ParticipantPanel.StatusButton.Text = "Check";
            }
            else
            {
                // pStatus.Text = "All in " + Chips;
                checkButton.Enabled = false;
            }

            await Turns();
        }

        private async void botCallOnClick(object sender, EventArgs e)
        {
            Rules(0, 1, player);
            if (player.Chips >= call)
            {
                player.Chips -= call;
                player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + player.Chips.ToString();
                if (potTextBox.Text != string.Empty)
                {
                    potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
                }
                else
                {
                    potTextBox.Text = call.ToString();
                }

                player.Turn = false;
                player.ParticipantPanel.StatusButton.Text = "Call " + call;
                player.Call = call;
            }
            else if (player.Chips <= call && call > 0)
            {
                potTextBox.Text = (int.Parse(potTextBox.Text) + player.Chips).ToString();
                player.ParticipantPanel.StatusButton.Text = "All in " + player.Chips;
                player.Chips = 0;
                player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + player.Chips.ToString();
                player.Turn = false;
                foldButton.Enabled = false;
                player.Call = player.Chips;
            }

            await Turns();
        }

        private async void botRaiseOnClick(object sender, EventArgs e)
        {
            Rules(0, 1, player);
            int parsedValue;
            if (raiseTextBox.Text != string.Empty && int.TryParse(raiseTextBox.Text, out parsedValue))
            {
                if (player.Chips > call)
                {
                    if (Raise * 2 > int.Parse(raiseTextBox.Text))
                    {
                        raiseTextBox.Text = (Raise*2).ToString();
                        writer.Print("You must raise at least twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (player.Chips >= int.Parse(raiseTextBox.Text))
                        {
                            call = int.Parse(raiseTextBox.Text);
                            Raise = int.Parse(raiseTextBox.Text);
                            player.ParticipantPanel.StatusButton.Text = "Raise " + call.ToString();
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
                            player.ParticipantPanel.StatusButton.Text = "Raise " + call.ToString();
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
                writer.Print("This is a number only field");
                return;
            }

            player.Turn = false;
            await Turns();
        }

        private void botAddOnClick(object sender, EventArgs e)
        {
            if (this.addChipsTextBox.Text != string.Empty)
            {
                player.Chips += int.Parse(this.addChipsTextBox.Text);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    gameBots[bot].Chips += int.Parse(this.addChipsButton.Text);
                }
            }

            player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + player.Chips.ToString();
        }

        private void botOptionsOnClick(object sender, EventArgs e)
        {
            this.bigBlindTextBox.Text = this.bigBlind.ToString();
            this.smallBlindTextBox.Text = sb.ToString();
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
            if (this.smallBlindTextBox.Text.Contains(",") || this.smallBlindTextBox.Text.Contains("."))
            {
                writer.Print("The Small Blind can be only round number !");
                this.smallBlindTextBox.Text = sb.ToString();
                return;
            }
            if (!int.TryParse(this.smallBlindTextBox.Text, out parsedValue))
            {
                writer.Print("This is a number only field");
                this.smallBlindTextBox.Text = sb.ToString();
                return;
            }

            if (int.Parse(this.smallBlindTextBox.Text) > 100000)
            {
                writer.Print("The maximum of the Small Blind is 100 000 $");
                this.smallBlindTextBox.Text = sb.ToString();
            }
            else if (int.Parse(this.smallBlindTextBox.Text) < 250)
            {
                writer.Print("The minimum of the Small Blind is 250 $");
            }
            else
            {
                sb = int.Parse(this.smallBlindTextBox.Text);
                writer.Print("The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void bBigBlindOnClick(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.bigBlindTextBox.Text.Contains(",") || this.bigBlindTextBox.Text.Contains("."))
            {
                writer.Print("The Big Blind can be only round number !");
                this.bigBlindTextBox.Text = this.bigBlind.ToString();
                return;
            }
            if (!int.TryParse(this.smallBlindTextBox.Text, out parsedValue))
            { 
                writer.Print("This is a number only field");
                this.smallBlindTextBox.Text = this.bigBlind.ToString();
                return;
            }

            if (int.Parse(this.bigBlindTextBox.Text) > 200000)
            {
                writer.Print("The maximum of the Big Blind is 200 000");
                this.bigBlindTextBox.Text = this.bigBlind.ToString();
            }
            else if (int.Parse(this.bigBlindTextBox.Text) < 500)
            {
                writer.Print("The minimum of the Big Blind is 500 $");
            }
            else
            {
                this.bigBlind = int.Parse(this.bigBlindTextBox.Text);
                writer.Print("The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            int width = this.Width;
            int height = this.Height;
        }
        #endregion

        private void SetBotCards(Bot botPanel, PictureBox[] Holder, Bitmap backImage, int[] Reserve, int horizontal, int vertical, int currentCard)
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
            Controls.Add(botPanel.ParticipantPanel);
            botPanel.ParticipantPanel.Location = new Point(Holder[currentCard].Left - 10, Holder[currentCard].Top - 10);
            botPanel.ParticipantPanel.BackColor = Color.DarkBlue;
            botPanel.ParticipantPanel.Height = 150;
            botPanel.ParticipantPanel.Width = 180;
            botPanel.ParticipantPanel.Visible = false;
        }

        private void EnableButtons()
        {
            raiseButton.Enabled = true;
            callButton.Enabled = true;
            foldButton.Enabled = true;
        }
    }
}