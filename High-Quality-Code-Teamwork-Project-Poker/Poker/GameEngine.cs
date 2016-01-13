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

    public partial class GameEngine : Form
    {
        #region Constants
        private const int StartChips = 10000;
        #endregion

        #region Variables
        ProgressBar progressBar = new ProgressBar();

        Panel playerPanel = new Panel();
        Panel bot1Panel = new Panel();
        Panel bot2Panel = new Panel();
        Panel bot3Panel = new Panel();
        Panel bot4Panel = new Panel();
        Panel bot5Panel = new Panel();

        int call = 500, foldedPlayers = 5;
        double type, rounds = 0, Raise = 0;
        double playerType = -1, bot1Type = -1, bot2Type = -1, bot3Type = -1, bot4Type = -1, bot5Type = -1;
        double playerPower = 0, bot1Power, bot2Power, bot3Power, bot4Power, bot5Power;
        public int playerChips = StartChips, bot1Chips = StartChips, bot2Chips = StartChips, bot3Chips = StartChips, bot4Chips = StartChips, bot5Chips = StartChips;
        bool bot1turn = false, bot2turn = false, bot3turn = false, bot4turn = false, bot5turn = false;
        bool bot1FoldTurn = false, bot2FoldTurn = false, bot3FoldTurn = false, bot4FoldTurn = false, bot5FoldTurn = false;
        bool playerFolded, bot1Folded, bot2Folded, bot3Folded, bot4Folded, bot5Folded;
        bool intsadded, changed;
        int playerCall = 0, bot1Call = 0, bot2Call = 0, bot3Call = 0, bot4Call = 0, bot5Call = 0;
        int playerRaise = 0, bot1Raise = 0, bot2Raise = 0, bot3Raise = 0, bot4Raise = 0, bot5Raise = 0;
        int height, width, winners = 0, Flop = 1, Turn = 2, River = 3, End = 4, maxLeft = 6;
        int last = 123, raisedTurn = 1;

        List<bool?> bools = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        bool playerFoldTurn = false, playerTurn = true;
        bool restart = false, raising = false;
        Poker.Type sorted;
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
            pot_TextBox.Enabled = false;
            player_ChipsTextBox.Enabled = false;
            bot1_ChipsTextBox.Enabled = false;
            bot2_ChipsTextBox.Enabled = false;
            bot3_ChipsTextBox.Enabled = false;
            bot4_ChipsTextBox.Enabled = false;
            bot5_ChipsTextBox.Enabled = false;
            player_ChipsTextBox.Text = "Chips : " + playerChips.ToString();
            bot1_ChipsTextBox.Text = "Chips : " + bot1Chips.ToString();
            bot2_ChipsTextBox.Text = "Chips : " + bot2Chips.ToString();
            bot3_ChipsTextBox.Text = "Chips : " + bot3Chips.ToString();
            bot4_ChipsTextBox.Text = "Chips : " + bot4Chips.ToString();
            bot5_ChipsTextBox.Text = "Chips : " + bot5Chips.ToString();
            timer.Interval = 1 * 1 * 1000;
            timer.Tick += timer_Tick;
            Updates.Interval = 1 * 1 * 100;
            Updates.Tick += Update_Tick;
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
            raise_TextBox.Text = (this.bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            bools.Add(playerFoldTurn);
            bools.Add(bot1FoldTurn);
            bools.Add(bot2FoldTurn);
            bools.Add(bot3FoldTurn);
            bools.Add(bot4FoldTurn);
            bools.Add(bot5FoldTurn);

            call_Button.Enabled = false;
            raise_Button.Enabled = false;
            fold_Button.Enabled = false;
            check_Button.Enabled = false;

            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580, vertical = 480;
            Random r = new Random();
            for (i = ImgLocation.Length; i > 0; i--)
            {
                int j = r.Next(i);
                var k = ImgLocation[j];
                ImgLocation[j] = ImgLocation[i - 1];
                ImgLocation[i - 1] = k;
            }

            for (i = 0; i < 17; i++)
            {
                Deck[i] = Image.FromFile(ImgLocation[i]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    ImgLocation[i] = ImgLocation[i].Replace(c, string.Empty);
                }

                Reserve[i] = int.Parse(ImgLocation[i]) - 1;
                Holder[i] = new PictureBox();
                Holder[i].SizeMode = PictureBoxSizeMode.StretchImage;
                Holder[i].Height = 130;
                Holder[i].Width = 80;
                this.Controls.Add(Holder[i]);
                Holder[i].Name = "pb" + i.ToString();
                await Task.Delay(200);

                // Throwing Cards
                if (i < 2)
                {
                    if (Holder[0].Tag != null)
                    {
                        Holder[1].Tag = Reserve[1];
                    }

                    Holder[0].Tag = Reserve[0];
                    Holder[i].Image = Deck[i];
                    Holder[i].Anchor = AnchorStyles.Bottom;
                    //Holder[i].Dock = DockStyle.Top;
                    Holder[i].Location = new Point(horizontal, vertical);
                    horizontal += Holder[i].Width;
                    this.Controls.Add(playerPanel);
                    playerPanel.Location = new Point(Holder[0].Left - 10, Holder[0].Top - 10);
                    playerPanel.BackColor = Color.DarkBlue;
                    playerPanel.Height = 150;
                    playerPanel.Width = 180;
                    playerPanel.Visible = false;
                }

                if (bot1Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 2 && i < 4)
                    {
                        if (Holder[2].Tag != null)
                        {
                            Holder[3].Tag = Reserve[3];
                        }

                        Holder[2].Tag = Reserve[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        Holder[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        Holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[i].Location = new Point(horizontal, vertical);
                        horizontal += Holder[i].Width;
                        Holder[i].Visible = true;
                        this.Controls.Add(bot1Panel);
                        bot1Panel.Location = new Point(Holder[2].Left - 10, Holder[2].Top - 10);
                        bot1Panel.BackColor = Color.DarkBlue;
                        bot1Panel.Height = 150;
                        bot1Panel.Width = 180;
                        bot1Panel.Visible = false;
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (bot2Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 4 && i < 6)
                    {
                        if (Holder[4].Tag != null)
                        {
                            Holder[5].Tag = Reserve[5];
                        }

                        Holder[4].Tag = Reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        check = true;
                        Holder[i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        Holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[i].Location = new Point(horizontal, vertical);
                        horizontal += Holder[i].Width;
                        Holder[i].Visible = true;
                        this.Controls.Add(bot2Panel);
                        bot2Panel.Location = new Point(Holder[4].Left - 10, Holder[4].Top - 10);
                        bot2Panel.BackColor = Color.DarkBlue;
                        bot2Panel.Height = 150;
                        bot2Panel.Width = 180;
                        bot2Panel.Visible = false;
                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (bot3Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 6 && i < 8)
                    {
                        if (Holder[6].Tag != null)
                        {
                            Holder[7].Tag = Reserve[7];
                        }

                        Holder[6].Tag = Reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        Holder[i].Anchor = AnchorStyles.Top;
                        Holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[i].Location = new Point(horizontal, vertical);
                        horizontal += Holder[i].Width;
                        Holder[i].Visible = true;
                        this.Controls.Add(bot3Panel);
                        bot3Panel.Location = new Point(Holder[6].Left - 10, Holder[6].Top - 10);
                        bot3Panel.BackColor = Color.DarkBlue;
                        bot3Panel.Height = 150;
                        bot3Panel.Width = 180;
                        bot3Panel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (bot4Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 8 && i < 10)
                    {
                        if (Holder[8].Tag != null)
                        {
                            Holder[9].Tag = Reserve[9];
                        }

                        Holder[8].Tag = Reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;
                        Holder[i].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        Holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[i].Location = new Point(horizontal, vertical);
                        horizontal += Holder[i].Width;
                        Holder[i].Visible = true;
                        this.Controls.Add(bot4Panel);
                        bot4Panel.Location = new Point(Holder[8].Left - 10, Holder[8].Top - 10);
                        bot4Panel.BackColor = Color.DarkBlue;
                        bot4Panel.Height = 150;
                        bot4Panel.Width = 180;
                        bot4Panel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (bot5Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 10 && i < 12)
                    {
                        if (Holder[10].Tag != null)
                        {
                            Holder[11].Tag = Reserve[11];
                        }

                        Holder[10].Tag = Reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        Holder[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        Holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[i].Location = new Point(horizontal, vertical);
                        horizontal += Holder[i].Width;
                        Holder[i].Visible = true;
                        this.Controls.Add(bot5Panel);
                        bot5Panel.Location = new Point(Holder[10].Left - 10, Holder[10].Top - 10);
                        bot5Panel.BackColor = Color.DarkBlue;
                        bot5Panel.Height = 150;
                        bot5Panel.Width = 180;
                        bot5Panel.Visible = false;
                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }

                if (i >= 12)
                {
                    if (i == 12)
                    {
                        Holder[12].Tag = Reserve[12];
                    }
                    else if (i > 13)
                    {
                        Holder[13].Tag = Reserve[13];
                    }
                    else if (i > 14)
                    {
                        Holder[14].Tag = Reserve[14];
                    }
                    else if (i > 15)
                    {
                        Holder[15].Tag = Reserve[15];
                    }
                    else if (i == 16)
                    {
                        Holder[16].Tag = Reserve[16];

                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (Holder[i] != null)
                    {
                        Holder[i].Anchor = AnchorStyles.None;
                        Holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }

                if (bot1Chips <= 0)
                {
                    bot1FoldTurn = true;
                    Holder[2].Visible = false;
                    Holder[3].Visible = false;
                }
                else
                {
                    bot1FoldTurn = false;
                    if (i == 3)
                    {
                        if (Holder[3] != null)
                        {
                            Holder[2].Visible = true;
                            Holder[3].Visible = true;
                        }
                    }
                }

                if (bot2Chips <= 0)
                {
                    bot2FoldTurn = true;
                    Holder[4].Visible = false;
                    Holder[5].Visible = false;
                }
                else
                {
                    bot2FoldTurn = false;
                    if (i == 5)
                    {
                        if (Holder[5] != null)
                        {
                            Holder[4].Visible = true;
                            Holder[5].Visible = true;
                        }
                    }
                }

                if (bot3Chips <= 0)
                {
                    bot3FoldTurn = true;
                    Holder[6].Visible = false;
                    Holder[7].Visible = false;
                }
                else
                {
                    bot3FoldTurn = false;
                    if (i == 7)
                    {
                        if (Holder[7] != null)
                        {
                            Holder[6].Visible = true;
                            Holder[7].Visible = true;
                        }
                    }
                }

                if (bot4Chips <= 0)
                {
                    bot4FoldTurn = true;
                    Holder[8].Visible = false;
                    Holder[9].Visible = false;
                }
                else
                {
                    bot4FoldTurn = false;
                    if (i == 9)
                    {
                        if (Holder[9] != null)
                        {
                            Holder[8].Visible = true;
                            Holder[9].Visible = true;
                        }
                    }
                }

                if (bot5Chips <= 0)
                {
                    bot5FoldTurn = true;
                    Holder[10].Visible = false;
                    Holder[11].Visible = false;
                }

                else
                {
                    bot5FoldTurn = false;
                    if (i == 11)
                    {
                        if (Holder[11] != null)
                        {
                            Holder[10].Visible = true;
                            Holder[11].Visible = true;
                        }
                    }
                }

                if (i == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }

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

            if (i == 17)
            {
                raise_Button.Enabled = true;
                call_Button.Enabled = true;
                raise_Button.Enabled = true;
                raise_Button.Enabled = true;
                fold_Button.Enabled = true;
            }
        }

        async Task Turns()
        {
            // Rotating
            if (!playerFoldTurn)
            {
                if (playerTurn)
                {
                    FixCall(player_StatusButton, ref playerCall, ref playerRaise, 1);
                    //MessageBox.Show("Player's Turn");
                    timer_ProgressBar.Visible = true;
                    timer_ProgressBar.Value = 1000;
                    t = 60;
                    maxUp = 10000000;
                    timer.Start();
                    raise_Button.Enabled = true;
                    call_Button.Enabled = true;
                    raise_Button.Enabled = true;
                    raise_Button.Enabled = true;
                    fold_Button.Enabled = true;
                    turnCount++;
                    FixCall(player_StatusButton, ref playerCall, ref playerRaise, 2);
                }
            }

            if (playerFoldTurn || !playerTurn)
            {
                await AllIn();
                if (playerFoldTurn && !playerFolded)
                {
                    if (call_Button.Text.Contains("All in") == false || raise_Button.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        playerFolded = true;
                    }
                }

                await CheckRaise(0, 0);
                timer_ProgressBar.Visible = false;
                raise_Button.Enabled = false;
                call_Button.Enabled = false;
                raise_Button.Enabled = false;
                raise_Button.Enabled = false;
                fold_Button.Enabled = false;
                timer.Stop();
                bot1turn = true;
                if (!bot1FoldTurn)
                {
                    if (bot1turn)
                    {
                        FixCall(bot1_StatusButton, ref bot1Call, ref bot1Raise, 1);
                        FixCall(bot1_StatusButton, ref bot1Call, ref bot1Raise, 2);
                        Rules(2, 3, "Bot 1", ref bot1Type, ref bot1Power, bot1FoldTurn);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, ref bot1Chips, ref bot1turn, ref  bot1FoldTurn, bot1_StatusButton, 0, bot1Power, bot1Type);
                        turnCount++;
                        last = 1;
                        bot1turn = false;
                        bot2turn = true;
                    }
                }

                if (bot1FoldTurn && !bot1Folded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    bot1Folded = true;
                }

                if (bot1FoldTurn || !bot1turn)
                {
                    await CheckRaise(1, 1);
                    bot2turn = true;
                }

                if (!bot2FoldTurn)
                {
                    if (bot2turn)
                    {
                        FixCall(bot2_StatusButton, ref bot2Call, ref bot2Raise, 1);
                        FixCall(bot2_StatusButton, ref bot2Call, ref bot2Raise, 2);
                        Rules(4, 5, "Bot 2", ref bot2Type, ref bot2Power, bot2FoldTurn);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, ref bot2Chips, ref bot2turn, ref  bot2FoldTurn, bot2_StatusButton, 1, bot2Power, bot2Type);
                        turnCount++;
                        last = 2;
                        bot2turn = false;
                        bot3turn = true;
                    }
                }

                if (bot2FoldTurn && !bot2Folded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    bot2Folded = true;
                }

                if (bot2FoldTurn || !bot2turn)
                {
                    await CheckRaise(2, 2);
                    bot3turn = true;
                }

                if (!bot3FoldTurn)
                {
                    if (bot3turn)
                    {
                        FixCall(bot3_StatusButton, ref bot3Call, ref bot3Raise, 1);
                        FixCall(bot3_StatusButton, ref bot3Call, ref bot3Raise, 2);
                        Rules(6, 7, "Bot 3", ref bot3Type, ref bot3Power, bot3FoldTurn);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, ref bot3Chips, ref bot3turn, ref  bot3FoldTurn, bot3_StatusButton, 2, bot3Power, bot3Type);
                        turnCount++;
                        last = 3;
                        bot3turn = false;
                        bot4turn = true;
                    }
                }

                if (bot3FoldTurn && !bot3Folded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    bot3Folded = true;
                }

                if (bot3FoldTurn || !bot3turn)
                {
                    await CheckRaise(3, 3);
                    bot4turn = true;
                }

                if (!bot4FoldTurn)
                {
                    if (bot4turn)
                    {
                        FixCall(bot4_StatusButton, ref bot4Call, ref bot4Raise, 1);
                        FixCall(bot4_StatusButton, ref bot4Call, ref bot4Raise, 2);
                        Rules(8, 9, "Bot 4", ref bot4Type, ref bot4Power, bot4FoldTurn);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, ref bot4Chips, ref bot4turn, ref  bot4FoldTurn, bot4_StatusButton, 3, bot4Power, bot4Type);
                        turnCount++;
                        last = 4;
                        bot4turn = false;
                        bot5turn = true;
                    }
                }

                if (bot4FoldTurn && !bot4Folded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    bot4Folded = true;
                }

                if (bot4FoldTurn || !bot4turn)
                {
                    await CheckRaise(4, 4);
                    bot5turn = true;
                }

                if (!bot5FoldTurn)
                {
                    if (bot5turn)
                    {
                        FixCall(bot5_StatusButton, ref bot5Call, ref bot5Raise, 1);
                        FixCall(bot5_StatusButton, ref bot5Call, ref bot5Raise, 2);
                        Rules(10, 11, "Bot 5", ref bot5Type, ref bot5Power, bot5FoldTurn);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, ref bot5Chips, ref bot5turn, ref  bot5FoldTurn, bot5_StatusButton, 4, bot5Power, bot5Type);
                        turnCount++;
                        last = 5;
                        bot5turn = false;
                    }
                }

                if (bot5FoldTurn && !bot5Folded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    bot5Folded = true;
                }

                if (bot5FoldTurn || !bot5turn)
                {
                    await CheckRaise(5, 5);
                    playerTurn = true;
                }

                if (playerFoldTurn && !playerFolded)
                {
                    if (call_Button.Text.Contains("All in") == false || raise_Button.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        playerFolded = true;
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

        void Rules(int c1, int c2, string currentText, ref double current, ref double Power, bool foldedTurn)
        {
            if (c1 == 0 && c2 == 1)
            {
            }

            if (!foldedTurn || c1 == 0 && c2 == 1 && player_StatusButton.Text.Contains("Fold") == false)
            {
                // Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = Reserve[c1];
                Straight[1] = Reserve[c2];
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

                for (i = 0; i < 16; i++)
                {
                    if (Reserve[i] == int.Parse(Holder[c1].Tag.ToString()) && Reserve[i + 1] == int.Parse(Holder[c2].Tag.ToString()))
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
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        Power = st1.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        Power = st2.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        Power = st2.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        Power = st3.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        Power = st3.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        Power = st4.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        Power = st4.Max() / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        Win.Add(new Type() { Power = Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        Power = 13 * 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                                Win.Add(new Type() { Power = Power, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            else if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                Power = fh.Max() / 4 * 2 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f1.Max() / 4 && Reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f1[0] % 4 && Reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f1.Min() / 4 && Reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        Power = f1.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f2.Max() / 4 && Reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f2[0] % 4 && Reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f2.Min() / 4 && Reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        Power = f2.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f3.Max() / 4 && Reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f3[0] % 4 && Reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f3.Min() / 4 && Reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        Power = f3.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f4.Max() / 4 && Reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[i + 1] % 4 == f4[0] % 4 && Reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f4.Min() / 4 && Reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        Power = f4.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            Power = op[j + 4] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 4 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (Reserve[i] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 != 0 && Reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[tc] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[tc] / 4) * 2 + (Reserve[i] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = Reserve[tc] / 4 + Reserve[i] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + Reserve[i + 1] + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = Reserve[tc] / 4 + Reserve[i + 1] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                            Win.Add(new Type() { Power = Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            Power = (Reserve[i + 1] / 4) * 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (Reserve[i + 1] / 4) * 4 + Reserve[i] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (Reserve[tc] / 4) * 4 + Reserve[i + 1] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    Power = Reserve[i + 1] / 4;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (Reserve[i] / 4 == 0 || Reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    Power = 13;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
            if (current == sorted.Current)
            {
                if (Power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    if (current == 9)
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
                        playerChips += int.Parse(pot_TextBox.Text) / winners;
                        player_ChipsTextBox.Text = playerChips.ToString();
                        //pPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1Chips += int.Parse(pot_TextBox.Text) / winners;
                        bot1_ChipsTextBox.Text = bot1Chips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2Chips += int.Parse(pot_TextBox.Text) / winners;
                        bot2_ChipsTextBox.Text = bot2Chips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3Chips += int.Parse(pot_TextBox.Text) / winners;
                        bot3_ChipsTextBox.Text = bot3Chips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4Chips += int.Parse(pot_TextBox.Text) / winners;
                        bot4_ChipsTextBox.Text = bot4Chips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5Chips += int.Parse(pot_TextBox.Text) / winners;
                        bot5_ChipsTextBox.Text = bot5Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        playerChips += int.Parse(pot_TextBox.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1Chips += int.Parse(pot_TextBox.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2Chips += int.Parse(pot_TextBox.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3Chips += int.Parse(pot_TextBox.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4Chips += int.Parse(pot_TextBox.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5Chips += int.Parse(pot_TextBox.Text);
                        //await Finish(1);
                        //b5Panel.Visible = true;
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
                            player_StatusButton.Text = string.Empty;
                        }
                        else if (!bot1FoldTurn)
                        {
                            bot1_StatusButton.Text = string.Empty;
                        }
                        else if (!bot2FoldTurn)
                        {
                            bot2_StatusButton.Text = string.Empty;
                        }
                        else if (!bot3FoldTurn)
                        {
                            bot3_StatusButton.Text = string.Empty;
                        }
                        else if (!bot4FoldTurn)
                        {
                            bot4_StatusButton.Text = string.Empty;
                        }
                        if (!bot5FoldTurn)
                        {
                            bot5_StatusButton.Text = string.Empty;
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

                        playerCall = 0;  
                        bot1Call = 0; 
                        bot2Call = 0; 
                        bot3Call = 0; 
                        bot4Call = 0; 
                        bot5Call = 0;

                        playerRaise = 0;
                        bot1Raise = 0;
                        bot2Raise = 0;
                        bot3Raise = 0;
                        bot4Raise = 0;
                        bot5Raise = 0;
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
                        playerCall = 0;  
                        bot1Call = 0; 
                        bot2Call = 0; 
                        bot3Call = 0; 
                        bot4Call = 0; 
                        bot5Call = 0;

                        playerRaise = 0;
                        bot1Raise = 0;
                        bot2Raise = 0;
                        bot3Raise = 0;
                        bot4Raise = 0;
                        bot5Raise = 0;
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

                        playerCall = 0;  
                        bot1Call = 0; 
                        bot2Call = 0; 
                        bot3Call = 0; 
                        bot4Call = 0; 
                        bot5Call = 0;

                        playerRaise = 0;
                        bot1Raise = 0;
                        bot2Raise = 0;
                        bot3Raise = 0;
                        bot4Raise = 0;
                        bot5Raise = 0;
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!player_StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
                }

                if (!bot1_StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref bot1Type, ref bot1Power, bot1FoldTurn);
                }

                if (!bot2_StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref bot2Type, ref bot2Power, bot2FoldTurn);
                }

                if (!bot3_StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref bot3Type, ref bot3Power, bot3FoldTurn);
                }

                if (!bot4_StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref bot4Type, ref bot4Power, bot4FoldTurn);
                }

                if (!bot5_StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref bot5Type, ref bot5Power, bot5FoldTurn);
                }

                Winner(playerType, playerPower, "Player", playerChips, fixedLast);
                Winner(bot1Type, bot1Power, "Bot 1", bot1Chips, fixedLast);
                Winner(bot2Type, bot2Power, "Bot 2", bot2Chips, fixedLast);
                Winner(bot3Type, bot3Power, "Bot 3", bot3Chips, fixedLast);
                Winner(bot4Type, bot4Power, "Bot 4", bot4Chips, fixedLast);
                Winner(bot5Type, bot5Power, "Bot 5", bot5Chips, fixedLast);

                restart = true;
                playerTurn = true;
                playerFoldTurn = false;
                bot1FoldTurn = false;
                bot2FoldTurn = false;
                bot3FoldTurn = false;
                bot4FoldTurn = false;
                bot5FoldTurn = false;

                if (playerChips <= 0)
                {
                    AddChips addMoreChipsForm = new AddChips();
                    addMoreChipsForm.ShowDialog();
                    if (addMoreChipsForm.NewChips != 0)
                    {
                        playerChips = addMoreChipsForm.NewChips;
                        bot1Chips += addMoreChipsForm.NewChips;
                        bot2Chips += addMoreChipsForm.NewChips;
                        bot3Chips += addMoreChipsForm.NewChips;
                        bot4Chips += addMoreChipsForm.NewChips;
                        bot5Chips += addMoreChipsForm.NewChips;
                        playerFoldTurn = false;
                        playerTurn = true;
                        raise_Button.Enabled = true;
                        fold_Button.Enabled = true;
                        check_Button.Enabled = true;
                        raise_Button.Text = "Raise";
                    }
                }

                playerPanel.Visible = false;
                bot1Panel.Visible = false;
                bot2Panel.Visible = false;
                bot3Panel.Visible = false;
                bot4Panel.Visible = false;
                bot5Panel.Visible = false;

                playerCall = 0; 
                bot1Call = 0;
                bot2Call = 0;
                bot3Call = 0;
                bot4Call = 0;
                bot5Call = 0;

                playerRaise = 0;
                bot1Raise = 0;
                bot2Raise = 0;
                bot3Raise = 0;
                bot4Raise = 0;
                bot5Raise = 0;
                last = 0;
                call = this.bigBlind;
                Raise = 0;
                ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bools.Clear();
                rounds = 0;
                playerPower = 0;
                playerType = -1;
                type = 0;

                bot1Power = 0;
                bot2Power = 0;
                bot3Power = 0;
                bot4Power = 0;
                bot5Power = 0;

                bot1Type = -1;
                bot2Type = -1;
                bot3Type = -1;
                bot4Type = -1;
                bot5Type = -1;

                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    Holder[os].Image = null;
                    Holder[os].Invalidate();
                    Holder[os].Visible = false;
                }

                pot_TextBox.Text = "0";
                player_StatusButton.Text = string.Empty;

                await Shuffle();
                await Turns();
            }
        }

        void FixCall(Label status, ref int cCall, ref int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }
                    else if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }
                    else if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != Raise && cRaise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - cRaise;
                    }

                    if (cCall != call || cCall <= call)
                    {
                        call = call - cCall;
                    }

                    if (cRaise == Raise && Raise > 0)
                    {
                        call = 0;
                        call_Button.Enabled = false;
                        call_Button.Text = "Callisfuckedup";
                    }
                }
            }
        }

        async Task AllIn()
        {
            // All in
            if (playerChips <= 0 && !intsadded)
            {
                if (player_StatusButton.Text.Contains("Raise"))
                {
                    ints.Add(playerChips);
                    intsadded = true;
                }
                else if (player_StatusButton.Text.Contains("Call"))
                {
                    ints.Add(playerChips);
                    intsadded = true;
                }
            }

            intsadded = false;
            if (bot1Chips <= 0 && !bot1FoldTurn)
            {
                if (!intsadded)
                {
                    ints.Add(bot1Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            else if (bot2Chips <= 0 && !bot2FoldTurn)
            {
                if (!intsadded)
                {
                    ints.Add(bot2Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            else if (bot3Chips <= 0 && !bot3FoldTurn)
            {
                if (!intsadded)
                {
                    ints.Add(bot3Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            else if (bot4Chips <= 0 && !bot4FoldTurn)
            {
                if (!intsadded)
                {
                    ints.Add(bot4Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            else if (bot5Chips <= 0 && !bot5FoldTurn)
            {
                if (!intsadded)
                {
                    ints.Add(bot5Chips);
                    intsadded = true;
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
                    playerChips += int.Parse(pot_TextBox.Text);
                    player_ChipsTextBox.Text = playerChips.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    bot1Chips += int.Parse(pot_TextBox.Text);
                    player_ChipsTextBox.Text = bot1Chips.ToString();
                    bot1Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    bot2Chips += int.Parse(pot_TextBox.Text);
                    player_ChipsTextBox.Text = bot2Chips.ToString();
                    bot2Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    bot3Chips += int.Parse(pot_TextBox.Text);
                    player_ChipsTextBox.Text = bot3Chips.ToString();
                    bot3Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    bot4Chips += int.Parse(pot_TextBox.Text);
                    player_ChipsTextBox.Text = bot4Chips.ToString();
                    bot4Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    bot5Chips += int.Parse(pot_TextBox.Text);
                    player_ChipsTextBox.Text = bot5Chips.ToString();
                    bot5Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
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

            playerPanel.Visible = false;
            bot1Panel.Visible = false;
            bot2Panel.Visible = false;
            bot3Panel.Visible = false;
            bot4Panel.Visible = false;
            bot5Panel.Visible = false;

            call = this.bigBlind;
            Raise = 0;
            foldedPlayers = 5;
            type = 0;
            rounds = 0;

            bot1Power = 0;
            bot2Power = 0;
            bot3Power = 0;
            bot4Power = 0;
            bot5Power = 0;

            playerPower = 0;
            playerType = -1;
            Raise = 0;

            bot1Type = -1;
            bot2Type = -1;
            bot3Type = -1;
            bot4Type = -1;
            bot5Type = -1;

            bot1turn = false;
            bot2turn = false;
            bot3turn = false;
            bot4turn = false;
            bot5turn = false;

            bot1FoldTurn = false;
            bot2FoldTurn = false;
            bot3FoldTurn = false;
            bot4FoldTurn = false;
            bot5FoldTurn = false;

            playerFolded = false;
            bot1Folded = false;
            bot2Folded = false;
            bot3Folded = false;
            bot4Folded = false;
            bot5Folded = false;

            playerFoldTurn = false;
            playerTurn = true;
            restart = false;
            raising = false;

            playerCall = 0;
            bot1Call = 0;
            bot2Call = 0;
            bot3Call = 0;
            bot4Call = 0;
            bot5Call = 0;

            playerRaise = 0;
            bot1Raise = 0;
            bot2Raise = 0;
            bot3Raise = 0;
            bot4Raise = 0;
            bot5Raise = 0;

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

            sorted.Current = 0;
            sorted.Power = 0;
            pot_TextBox.Text = "0";
            t = 60;
            maxUp = 10000000;
            turnCount = 0;
            player_StatusButton.Text = string.Empty;
            bot1_StatusButton.Text = string.Empty;
            bot2_StatusButton.Text = string.Empty;
            bot3_StatusButton.Text = string.Empty;
            bot4_StatusButton.Text = string.Empty;
            bot5_StatusButton.Text = string.Empty;
            if (playerChips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.NewChips != 0)
                {
                    playerChips = f2.NewChips;
                    bot1Chips += f2.NewChips;
                    bot2Chips += f2.NewChips;
                    bot3Chips += f2.NewChips;
                    bot4Chips += f2.NewChips;
                    bot5Chips += f2.NewChips;
                    playerFoldTurn = false;
                    playerTurn = true;
                    raise_Button.Enabled = true;
                    fold_Button.Enabled = true;
                    check_Button.Enabled = true;
                    raise_Button.Text = "Raise";
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
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!player_StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
            }
            if (!bot1_StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref bot1Type, ref bot1Power, bot1FoldTurn);
            }
            if (!bot2_StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref bot2Type, ref bot2Power, bot2FoldTurn);
            }
            if (!bot3_StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref bot3Type, ref bot3Power, bot3FoldTurn);
            }
            if (!bot4_StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref bot4Type, ref bot4Power, bot4FoldTurn);
            }
            if (!bot5_StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref bot5Type, ref bot5Power, bot5FoldTurn);
            }
            Winner(playerType, playerPower, "Player", playerChips, fixedLast);
            Winner(bot1Type, bot1Power, "Bot 1", bot1Chips, fixedLast);
            Winner(bot2Type, bot2Power, "Bot 2", bot2Chips, fixedLast);
            Winner(bot3Type, bot3Power, "Bot 3", bot3Chips, fixedLast);
            Winner(bot4Type, bot4Power, "Bot 4", bot4Chips, fixedLast);
            Winner(bot5Type, bot5Power, "Bot 5", bot5Chips, fixedLast);
        }

        void AI(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
            {
                Holder[c1].Visible = false;
                Holder[c2].Visible = false;
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
            pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + call).ToString();
        }

        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(Raise);
            sStatus.Text = "Raise " + Raise;
            pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + Convert.ToInt32(Raise)).ToString();
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
                        pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + botChips).ToString();
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
            if (timer_ProgressBar.Value <= 0)
            {
                playerFoldTurn = true;
                await Turns();
            }
            if (t > 0)
            {
                t--;
                timer_ProgressBar.Value = (t / 6) * 100;
            }
        }

        private void Update_Tick(object sender, object e)
        {
            if (playerChips <= 0)
            {
                player_ChipsTextBox.Text = "Chips : 0";
            }
            if (bot1Chips <= 0)
            {
                bot1_ChipsTextBox.Text = "Chips : 0";
            }
            if (bot2Chips <= 0)
            {
                bot2_ChipsTextBox.Text = "Chips : 0";
            }
            if (bot3Chips <= 0)
            {
                bot3_ChipsTextBox.Text = "Chips : 0";
            }
            if (bot4Chips <= 0)
            {
                bot4_ChipsTextBox.Text = "Chips : 0";
            }
            if (bot5Chips <= 0)
            {
                bot5_ChipsTextBox.Text = "Chips : 0";
            }
            player_ChipsTextBox.Text = "Chips : " + playerChips.ToString();
            bot1_ChipsTextBox.Text = "Chips : " + bot1Chips.ToString();
            bot2_ChipsTextBox.Text = "Chips : " + bot2Chips.ToString();
            bot3_ChipsTextBox.Text = "Chips : " + bot3Chips.ToString();
            bot4_ChipsTextBox.Text = "Chips : " + bot4Chips.ToString();
            bot5_ChipsTextBox.Text = "Chips : " + bot5Chips.ToString();
            if (playerChips <= 0)
            {
                playerTurn = false;
                playerFoldTurn = true;
                call_Button.Enabled = false;
                raise_Button.Enabled = false;
                fold_Button.Enabled = false;
                check_Button.Enabled = false;
            }
            if (maxUp > 0)
            {
                maxUp--;
            }
            if (playerChips >= call)
            {
                call_Button.Text = "Call " + call.ToString();
            }
            else
            {
                call_Button.Text = "All in";
                raise_Button.Enabled = false;
            }

            if (call > 0)
            {
                check_Button.Enabled = false;
            }
            else if (call <= 0)
            {
                check_Button.Enabled = true;
                call_Button.Text = "Call";
                call_Button.Enabled = false;
            }

            if (playerChips <= 0)
            {
                raise_Button.Enabled = false;
            }

            int parsedValue;

            if (raise_TextBox.Text != string.Empty && int.TryParse(raise_TextBox.Text, out parsedValue))
            {
                if (playerChips <= int.Parse(raise_TextBox.Text))
                {
                    raise_Button.Text = "All in";
                }
                else
                {
                    raise_Button.Text = "Raise";
                }
            }

            if (playerChips < call)
            {
                raise_Button.Enabled = false;
            }
        }

        private async void bFold_Click(object sender, EventArgs e)
        {
            player_StatusButton.Text = "Fold";
            playerTurn = false;
            playerFoldTurn = true;
            await Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                playerTurn = false;
                player_StatusButton.Text = "Check";
            }
            else
            {
                // pStatus.Text = "All in " + Chips;
                check_Button.Enabled = false;
            }
            await Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
            if (playerChips >= call)
            {
                playerChips -= call;
                player_ChipsTextBox.Text = "Chips : " + playerChips.ToString();
                if (pot_TextBox.Text != string.Empty)
                {
                    pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + call).ToString();
                }
                else
                {
                    pot_TextBox.Text = call.ToString();
                }
                playerTurn = false;
                player_StatusButton.Text = "Call " + call;
                playerCall = call;
            }
            else if (playerChips <= call && call > 0)
            {
                pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + playerChips).ToString();
                player_StatusButton.Text = "All in " + playerChips;
                playerChips = 0;
                player_ChipsTextBox.Text = "Chips : " + playerChips.ToString();
                playerTurn = false;
                fold_Button.Enabled = false;
                playerCall = playerChips;
            }
            await Turns();
        }

        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldTurn);
            int parsedValue;
            if (raise_TextBox.Text != string.Empty && int.TryParse(raise_TextBox.Text, out parsedValue))
            {
                if (playerChips > call)
                {
                    if (Raise * 2 > int.Parse(raise_TextBox.Text))
                    {
                        raise_TextBox.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (playerChips >= int.Parse(raise_TextBox.Text))
                        {
                            call = int.Parse(raise_TextBox.Text);
                            Raise = int.Parse(raise_TextBox.Text);
                            player_StatusButton.Text = "Raise " + call.ToString();
                            pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + call).ToString();
                            call_Button.Text = "Call";
                            playerChips -= int.Parse(raise_TextBox.Text);
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = playerChips;
                            Raise = playerChips;
                            pot_TextBox.Text = (int.Parse(pot_TextBox.Text) + playerChips).ToString();
                            player_StatusButton.Text = "Raise " + call.ToString();
                            playerChips = 0;
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(Raise);
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

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (addChips_TextBox.Text == string.Empty)
            {
            }
            else
            {
                playerChips += int.Parse(addChips_TextBox.Text);
                bot1Chips += int.Parse(addChips_TextBox.Text);
                bot2Chips += int.Parse(addChips_TextBox.Text);
                bot3Chips += int.Parse(addChips_TextBox.Text);
                bot4Chips += int.Parse(addChips_TextBox.Text);
                bot5Chips += int.Parse(addChips_TextBox.Text);
            }
            player_ChipsTextBox.Text = "Chips : " + playerChips.ToString();
        }

        private void bOptions_Click(object sender, EventArgs e)
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
            if (int.Parse(smallBlind_TextBox.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(smallBlind_TextBox.Text) >= 250 && int.Parse(smallBlind_TextBox.Text) <= 100000)
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
            if (int.Parse(bigBlind_TextBox.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(bigBlind_TextBox.Text) >= 500 && int.Parse(bigBlind_TextBox.Text) <= 200000)
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
    }
}