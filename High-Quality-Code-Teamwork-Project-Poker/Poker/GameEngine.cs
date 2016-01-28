﻿namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Poker.Contracts;
    using Poker.Enums;
    using Poker.Models;
    using Poker.Models.Cards;
    using Poker.UI;

    public partial class GameEngine : Form
    {
        #region Constants
        private const int NumberOfBots = 5;
        private const int AllCardsOnTheTable = 17;
        #endregion

        #region Readonlyfloz
        private ProgressBar progressBar = new ProgressBar();
        private readonly ApplicationWriter writer = new ApplicationWriter();
        private readonly IBot[] gameBots = new Bot[5]
        { new Bot(), new Bot(), new Bot(), new Bot(), new Bot() };
        private readonly IPlayer player = new Player();
        private readonly IList<IType> win = new List<IType>();
        private readonly IList<string> winnersChecker = new List<string>();
        private readonly IList<int> ints = new List<int>();
        private readonly Image[] deck = new Image[52];
        private readonly Point[] cardLocations = new Point[AllCardsOnTheTable];
        #endregion
        
        #region Variables
        private int rounds;
        bool changed;
        int winners, maxLeft = 6; 
        private int last = 123;
        int raisedTurn = 1;
        private bool restart;

        IDeckOfCards deckOfCards;

        private Timer timer = new Timer();
        private Timer update = new Timer();
        private int timeForTurn = 60;

        private int bigBlind = 500;
        private int smallBlind = 250;
        private int turnCount;
        #endregion
        
        public GameEngine(IGameRules rule, IHandRanking handRank)
        {
            this.Rule = rule;
            this.HandRank = handRank;
            this.deckOfCards = new DeckOfCards("Assets\\Cards", "*.png");
            this.player.Call = this.bigBlind;
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                gameBots[bot].Call = this.bigBlind;
            }

            this.MaximizeBox = false;
            this.update.Start();
            this.InitializeComponent();
            this.InitializeCardLocations(cardLocations);
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
            this.timer.Tick += this.TimerTick;
            this.update.Interval = 1 * 1 * 100;
            this.update.Tick += this.UpdateTick;

            this.bigBlindTextBox.Visible = true;
            this.smallBlindTextBox.Visible = true;

            this.raiseTextBox.Text = (this.bigBlind * 2).ToString();
        }

        public IHandRanking HandRank { get; private set; }

        public IGameRules Rule { get; private set; }

        async Task Shuffle()
        {
            this.callButton.Enabled = false;
            this.raiseButton.Enabled = false;
            this.foldButton.Enabled = false;
            this.checkButton.Enabled = false;

            this.MaximizeBox = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

            // Shuffle the deck.
            this.deckOfCards.ShuffleDeck();

            for (int currentCard = 0; currentCard < AllCardsOnTheTable; currentCard++)
            {
                // Next card from the deck.
                ICard drawnCard = this.deckOfCards.DrawOneCard();

                this.deck[currentCard] = drawnCard.Image;

                this.HandRank.Reserve[currentCard] = int.Parse(drawnCard.Name) - 1;

                this.Rule.CardImages[currentCard] = new PictureBox();
                this.Rule.CardImages[currentCard].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Rule.CardImages[currentCard].Height = 130;
                this.Rule.CardImages[currentCard].Width = 80;
                this.Rule.CardImages[currentCard].Image = backImage;
                this.Rule.CardImages[currentCard].Location = this.cardLocations[currentCard];
                this.Controls.Add(this.Rule.CardImages[currentCard]);
                this.Rule.CardImages[currentCard].Name = "pb" + currentCard;
                await Task.Delay(200);

                // Throwing Cards
                if (currentCard < 2)
                {
                    if (this.Rule.CardImages[0].Tag != null)
                    {
                        this.Rule.CardImages[1].Tag = this.HandRank.Reserve[1];
                    }

                    this.Rule.CardImages[0].Tag = this.HandRank.Reserve[0];
                    this.Rule.CardImages[currentCard].Image = this.deck[currentCard];
                    this.Rule.CardImages[currentCard].Anchor = AnchorStyles.Bottom;
                }

                if (this.gameBots[0].Chips > 0)
                {
                    if (currentCard >= 2 && currentCard < 4)
                    {
                        this.SetBotCards(
                            this.gameBots[0],
                            this.Rule.CardImages,
                            backImage,
                            this.HandRank.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[1].Chips > 0)
                {
                    if (currentCard >= 4 && currentCard < 6)
                    {
                        this.SetBotCards(
                            this.gameBots[1],
                            this.Rule.CardImages, backImage,
                            this.HandRank.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[2].Chips > 0)
                {
                    if (currentCard >= 6 && currentCard < 8)
                    {
                        this.SetBotCards(this.gameBots[2],
                            this.Rule.CardImages, backImage,
                            this.HandRank.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[3].Chips > 0)
                {
                    if (currentCard >= 8 && currentCard < 10)
                    {
                        this.SetBotCards(this.gameBots[3],
                            this.Rule.CardImages,
                            backImage,
                            this.HandRank.Reserve,
                            currentCard);
                    }
                }

                if (this.gameBots[4].Chips > 0)
                {
                    if (currentCard >= 10 && currentCard < 12)
                    {
                        this.SetBotCards(this.gameBots[4],
                            this.Rule.CardImages,
                            backImage,
                            this.HandRank.Reserve,
                            currentCard);
                    }
                }

                // Printing the five river cards
                if (currentCard >= 12)
                {
                    this.Rule.CardImages[currentCard].Tag = this.HandRank.Reserve[currentCard];
                    if (this.Rule.CardImages[currentCard] != null)
                    {
                        this.Rule.CardImages[currentCard].Anchor = AnchorStyles.None;
                    }
                }

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

            // If all bots ran out of money, player wins.
            int leftBotsWithoutMoney = this.gameBots.Count(bot => bot.Chips <= 0);
            if (leftBotsWithoutMoney == this.gameBots.Length)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Would You Like To Play Again ?", "You Won , Congratulations ! ",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
        }

        async Task Turns()
        {
            // Rotating
            if (!this.player.FoldTurn)
            {
                if (this.player.Turn)
                {
                    this.FixParticipantCall(this.player, 1);
                    this.writer.Print("Player's Turn");

                    this.timerProgressBar.Visible = true;
                    this.timerProgressBar.Value = 1000;
                    this.timeForTurn = 60;
                    this.timer.Start();

                    this.EnableButtons();
                    this.turnCount++;
                    this.FixParticipantCall(this.player, 2);
                }
            }

            if (this.player.FoldTurn || !this.player.Turn)
            {
                await this.AllIn();
                if (this.player.FoldTurn)
                {
                    if (this.callButton.Text.Contains("All in") == false ||
                        this.raiseButton.Text.Contains("All in") == false)
                    {
                        this.maxLeft--;
                    }
                }

                await this.CheckRaise(0);
                this.timerProgressBar.Visible = false;
                this.raiseButton.Enabled = false;
                this.callButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.timer.Stop();
                // TODO: Maybe there is a bug here
                this.gameBots[0].Turn = true;
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    IBot currentBot = this.gameBots[bot];
                    int botIndex = bot + 1;
                    if (!currentBot.FoldTurn)
                    {
                        if (currentBot.Turn)
                        {
                            int firstCard = botIndex * 2;
                            int secondCard = botIndex * 2 + 1;

                            this.FixParticipantCall(currentBot, 1);
                            this.FixParticipantCall(currentBot, 2);
                            this.Rule.ExecuteGameRules(firstCard, secondCard, currentBot);

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

                        if (currentBot.FoldTurn)
                        {
                            this.maxLeft--;
                        }

                        if (currentBot.FoldTurn ||
                            !currentBot.Turn)
                        {
                            await this.CheckRaise(botIndex);
                            if (botIndex != NumberOfBots)
                            {
                                this.gameBots[bot + 1].Turn = true;
                            }
                        }
                    }
                }

                this.player.Turn = true;
                if (this.player.FoldTurn)
                {
                    if (!this.callButton.Text.Contains("All in") ||
                        !this.raiseButton.Text.Contains("All in"))
                    {
                        this.maxLeft--;
                    }
                }

                await this.AllIn();
                if (!this.restart)
                {
                    this.Turns();
                }

                this.restart = false;
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

            for (int card = 0; card < AllCardsOnTheTable; card++)
            {
                //await Task.Delay(5);
                if (this.Rule.CardImages[card].Visible)
                {
                    this.Rule.CardImages[card].Image = this.deck[card];
                }
            }

            if (currentGameParticipantType == this.HandRank.WinningHand.Current)
            {
                if (currentGameParticipantPower == this.HandRank.WinningHand.Power)
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
                        this.player.Chips += int.Parse(
                            this.potTextBox.Text) / this.winners;
                        this.player.ParticipantPanel.ChipsTextBox.Text = this.player.Chips.ToString();
                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (this.winnersChecker.Contains($"Bot {botIndex}"))
                        {
                            this.gameBots[bot].Chips += int.Parse(
                                this.potTextBox.Text) / this.winners;
                            this.gameBots[bot].ParticipantPanel.ChipsTextBox.Text =
                                this.gameBots[bot].Chips.ToString();
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
                    }

                    for (int bot = 0; bot < NumberOfBots; bot++)
                    {
                        int botIndex = bot + 1;
                        if (this.winnersChecker.Contains($"Bot {botIndex}"))
                        {
                            this.gameBots[bot].Chips += int.Parse(
                                this.potTextBox.Text) / this.winners;
                            //await Finish(1)
                        }
                    }
                }
            }
        }

        async Task CheckRaise(int currentTurn)
        {
            // TODO: currentTurn or raisedTurn
            bool hasBotRised = this.gameBots.Count(bot => bot.RaiseTurn == true) > 0;
            bool hasPlayerRised = this.player.RaiseTurn;
            if (hasPlayerRised || hasBotRised)
            {
                this.turnCount = 0;
                this.player.RaiseTurn = false;
                for (int i = 0; i < NumberOfBots; i++)
                {
                    this.gameBots[i].RaiseTurn = false;
                }

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
                        this.rounds++;

                        if (!this.player.FoldTurn)
                        {
                            this.player.ParticipantPanel.StatusButton.Text = string.Empty;
                            this.player.Raise = 0;
                        }

                        for (int bot = 0; bot < NumberOfBots; bot++)
                        {
                            if (!this.gameBots[bot].FoldTurn)
                            {
                                this.gameBots[bot].ParticipantPanel.StatusButton.Text = string.Empty;
                                this.gameBots[bot].Raise = 0;
                            }
                        }
                    }
                }
            }

            if (this.rounds == (int)PokerStages.Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.Rule.CardImages[j].Image != this.deck[j])
                    {
                        this.Rule.CardImages[j].Image = this.deck[j];

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

            if (this.rounds == (int)PokerStages.Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.Rule.CardImages[j].Image != this.deck[j])
                    {
                        this.Rule.CardImages[j].Image = this.deck[j];

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

            if (this.rounds == (int)PokerStages.River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.Rule.CardImages[j].Image != this.deck[j])
                    {
                        this.Rule.CardImages[j].Image = this.deck[j];

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

            if (this.rounds == (int)PokerStages.End && this.maxLeft == 6)
            {
                string fixedLast = string.Empty;
                if (!this.player.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    this.Rule.ExecuteGameRules(0, 1, this.player);
                }

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    IBot currentBot = this.gameBots[bot];
                    int botIndex = bot + 1;
                    int firstCard = botIndex * 2;
                    int seconCard = botIndex * 2 + 1;

                    if (!currentBot.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                    {
                        fixedLast = $"Bot {botIndex}";
                        this.Rule.ExecuteGameRules(firstCard, seconCard, currentBot);
                    }
                }

                this.ValidateWinner(this.player, "Player", fixedLast);
                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    IBot currentBot = this.gameBots[bot];
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

                        this.raiseButton.Text = "Raise";
                    }
                }

                this.player.ParticipantPanel.Visible = false;
                this.player.Call = this.bigBlind;
                this.player.Raise = 0;
                this.player.Power = 0;
                this.player.Type = -1;

                for (int bot = 0; bot < NumberOfBots; bot++)
                {
                    IBot currentBot = this.gameBots[bot];

                    currentBot.ParticipantPanel.Visible = false;
                    currentBot.Call = this.bigBlind;
                    currentBot.Raise = 0;
                    currentBot.Power = 0;
                    currentBot.Type = -1;
                }

                this.last = 0;
                this.deckOfCards.RenewUsedDeck();
                this.rounds = 0;
                this.HandRank.Type = 0;

                this.ints.Clear();
                this.winnersChecker.Clear();
                this.winners = 0;
                this.win.Clear();
                this.HandRank.WinningHand.Current = 0;
                this.HandRank.WinningHand.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    this.Rule.CardImages[os].Image = null;
                    this.Rule.CardImages[os].Invalidate();
                    this.Rule.CardImages[os].Visible = false;
                }

                this.potTextBox.Text = "0";
                this.player.ParticipantPanel.StatusButton.Text = string.Empty;

                await this.Shuffle();
                await this.Turns();
            }
        }
        
        void FixParticipantCall(IGameParticipant currentGameParticipant, int options)
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (currentGameParticipant.ParticipantPanel.StatusButton.Text.Contains("Raise"))
                    {
                        var changeRaise = currentGameParticipant.ParticipantPanel.StatusButton.Text.Substring(6);
                        currentGameParticipant.Raise = int.Parse(changeRaise);
                    }
                    else if (currentGameParticipant.ParticipantPanel.StatusButton.Text.Contains("Call"))
                    {
                        var changeCall = currentGameParticipant.ParticipantPanel.StatusButton.Text.Substring(5);
                        currentGameParticipant.Call = int.Parse(changeCall);
                    }
                    else if (currentGameParticipant.ParticipantPanel.StatusButton.Text.Contains("Check"))
                    {
                        currentGameParticipant.Raise = 0;
                        currentGameParticipant.Call = 0;
                    }
                }

                if (options == 2)
                {
                    // critical
                    if (currentGameParticipant.Raise != 0)
                    {
                        currentGameParticipant.Call = currentGameParticipant.Raise;
                    }

                    if (currentGameParticipant.Call != this.bigBlind)
                    {
                        currentGameParticipant.Call = this.bigBlind;
                    }

                    if (currentGameParticipant.Raise != 0 ||
                        currentGameParticipant.Call != this.bigBlind)
                    {
                        currentGameParticipant.Call = 0;
                        currentGameParticipant.Raise = 0;
                        this.callButton.Enabled = false;
                        this.callButton.Text = "Call button is unable.";
                    }
                }
            }
        }

        async Task AllIn()
        {
            if (this.player.Chips <= 0)
            {
                if (this.player.ParticipantPanel.StatusButton.Text.Contains("Raise"))
                {
                    this.ints.Add(this.player.Chips);
                }
                else if (this.player.ParticipantPanel.StatusButton.Text.Contains("Call"))
                {
                    this.ints.Add(this.player.Chips);
                }
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                IBot currentBot = this.gameBots[bot];

                if (currentBot.Chips <= 0 &&
                    !currentBot.FoldTurn)
                {
                    this.ints.Add(currentBot.Chips);
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

            int leftNotFoldedParticipants = this.gameBots.Count(x => x.FoldTurn == false);
            if (!this.player.FoldTurn)
            {
                leftNotFoldedParticipants++;
            }

            // LastManStanding
            if (leftNotFoldedParticipants == 1)
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
                        if (!this.gameBots[bot].FoldTurn)
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
                    this.Rule.CardImages[j].Visible = false;
                }

                await this.Finish(1);
            }

            // FiveOrLessLeft
            if (leftNotFoldedParticipants < 6 && leftNotFoldedParticipants > 1 &&
                this.rounds >= (int)PokerStages.End)
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
            this.player.Call = this.bigBlind;
            this.HandRank.Type = 0;
            this.rounds = 0;

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                IBot currentBot = this.gameBots[bot];

                currentBot.ParticipantPanel.Visible = false;
                currentBot.Power = 0;
                currentBot.Type = -1;
                currentBot.Turn = false;
                currentBot.FoldTurn = false;
                currentBot.Call = this.bigBlind;
                currentBot.Raise = 0;
            }

            this.player.Power = 0;
            this.player.Type = -1;
            this.player.Raise = 0;
            this.player.Call = 0;
            this.player.FoldTurn = false;
            this.player.Turn = true;
            this.player.RaiseTurn = false;

            this.restart = false;

            this.winners = 0;
            this.maxLeft = 6;
            this.last = 123;

            this.winnersChecker.Clear();
            this.ints.Clear();
            this.win.Clear();

            this.HandRank.WinningHand.Current = 0;
            this.HandRank.WinningHand.Power = 0;
            this.potTextBox.Text = "0";
            this.timeForTurn = 60;
            this.turnCount = 0;
            this.player.ParticipantPanel.StatusButton.Text = string.Empty;

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                this.gameBots[bot].ParticipantPanel.StatusButton.Text = string.Empty;
            }

            if (this.player.Chips <= 0)
            {
                AddChips chipsAdder = new AddChips();
                this.AddMoreChips(chipsAdder);
            }

            this.deckOfCards.RenewUsedDeck();
            for (int card = 0; card < AllCardsOnTheTable; card++)
            {
                this.Rule.CardImages[card].Image = null;
                this.Rule.CardImages[card].Invalidate();
                this.Rule.CardImages[card].Visible = false;
            }

            await this.Shuffle();
            //await Turns();
        }

        private void FixWinners()
        {
            this.win.Clear();
            this.HandRank.WinningHand.Current = 0;
            this.HandRank.WinningHand.Power = 0;
            string fixedLast = string.Empty;

            if (!this.player.ParticipantPanel.StatusButton.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rule.ExecuteGameRules(0, 1, this.player);
            }

            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                IBot currentBot = this.gameBots[bot];
                int botIndex = bot + 1;
                int firstCard = botIndex * 2;
                int secondCard = botIndex * 2 + 1;

                if (!currentBot.ParticipantPanel.StatusButton.Text.Contains("Fold"))
                {
                    fixedLast = $"Bot {botIndex}";
                    this.Rule.ExecuteGameRules(firstCard, secondCard, currentBot);
                }
            }

            this.ValidateWinner(this.player, "Player", fixedLast);
            for (int bot = 0; bot < NumberOfBots; bot++)
            {
                if (!gameBots[bot].FoldTurn)
                {
                    this.ValidateWinner(this.gameBots[bot], $"Bot {bot + 1}", fixedLast);
                }
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
                // TODO: botType will never be 5.5, because it is int
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
                    this.StraightFlush(currentGameParticipant);
                }
            }

            if (currentGameParticipant.FoldTurn)
            {
                this.Rule.CardImages[firstCard].Visible = false;
                this.Rule.CardImages[secondCard].Visible = false;
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


        private void ChooseToCall(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.RaiseTurn = false;
            currentGameParticipant.Turn = false;
            currentGameParticipant.Chips -= currentGameParticipant.Call;
            currentGameParticipant.ParticipantPanel.Text = "Call " + currentGameParticipant.Call;
            this.potTextBox.Text = (int.Parse(this.potTextBox.Text) + currentGameParticipant.Call).ToString();
        }

        private void Raised(IGameParticipant currentGameParticipant)
        {
            currentGameParticipant.Chips -= currentGameParticipant.Raise;
            currentGameParticipant.ParticipantPanel.StatusButton.Text =
                "Raise " + currentGameParticipant.Raise;
            this.potTextBox.Text =
                (int.Parse(this.potTextBox.Text) + currentGameParticipant.Raise).ToString();
            //this.call = currentGameParticipant.Raise;
            currentGameParticipant.RaiseTurn = true;
            currentGameParticipant.Turn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        private void HP(
            IGameParticipant currrentGameParticipant,
            int n,
            int n1)
        {
            Random random = new Random();
            int rnd = random.Next(1, 4);

            if (currrentGameParticipant.Call <= 0)
            {
                currrentGameParticipant.ChooseToCheck();
            }

            if (currrentGameParticipant.Call > 0)
            {
                if (rnd == 1)
                {
                    if (currrentGameParticipant.Call <= RoundN(currrentGameParticipant.Chips, n))
                    {
                        this.ChooseToCall(currrentGameParticipant);
                    }
                    else
                    {
                        currrentGameParticipant.ChooseToFold();
                    }
                }

                if (rnd == 2)
                {
                    if (currrentGameParticipant.Call <= RoundN(currrentGameParticipant.Chips, n1))
                    {
                        this.ChooseToCall(currrentGameParticipant);
                    }
                    else
                    {
                        currrentGameParticipant.ChooseToFold();
                    }
                }
            }

            if (rnd == 3)
            {
                if (currrentGameParticipant.Raise == 0)
                {
                    currrentGameParticipant.Raise = currrentGameParticipant.Call * 2;
                    this.Raised(currrentGameParticipant);
                }
                else
                {
                    if (currrentGameParticipant.Raise <= RoundN(currrentGameParticipant.Chips, n))
                    {
                        currrentGameParticipant.Raise = currrentGameParticipant.Call * 2;
                        this.Raised(currrentGameParticipant);
                    }
                    else
                    {
                        currrentGameParticipant.ChooseToFold();
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
                if (currentGameParticipant.Call <= 0)
                {
                    currentGameParticipant.ChooseToCheck();
                }

                if (currentGameParticipant.Call > 0)
                {
                    if (currentGameParticipant.Call >= RoundN(currentGameParticipant.Chips, n1))
                    {
                        currentGameParticipant.ChooseToFold();
                    }

                    if (currentGameParticipant.Raise > RoundN(currentGameParticipant.Chips, n))
                    {
                        currentGameParticipant.ChooseToFold();
                    }

                    if (!currentGameParticipant.FoldTurn)
                    {
                        if (currentGameParticipant.Call >= RoundN(currentGameParticipant.Chips, n) &&
                            currentGameParticipant.Call <= RoundN(currentGameParticipant.Chips, n1))
                        {
                            this.ChooseToCall(currentGameParticipant);
                        }

                        if (currentGameParticipant.Raise <= RoundN(currentGameParticipant.Chips, n) &&
                            currentGameParticipant.Raise >= RoundN(currentGameParticipant.Chips, n) / 2)
                        {
                            this.ChooseToCall(currentGameParticipant);
                        }

                        if (currentGameParticipant.Raise <= RoundN(currentGameParticipant.Chips, n) / 2)
                        {
                            if (currentGameParticipant.Raise > 0)
                            {
                                currentGameParticipant.Raise = (int)RoundN(currentGameParticipant.Chips, n);
                                this.Raised(currentGameParticipant);
                            }
                            else
                            {
                                currentGameParticipant.Raise = currentGameParticipant.Call * 2;
                                this.Raised(currentGameParticipant);
                            }
                        }
                    }
                }
            }

            if (this.rounds >= 2)
            {
                if (currentGameParticipant.Call > 0)
                {
                    if (currentGameParticipant.Call >= RoundN(currentGameParticipant.Chips, n1 - rnd))
                    {
                        currentGameParticipant.ChooseToFold();
                    }

                    if (currentGameParticipant.Raise > RoundN(currentGameParticipant.Chips, n - rnd))
                    {
                        currentGameParticipant.ChooseToFold();
                    }

                    if (!currentGameParticipant.FoldTurn)
                    {
                        if (currentGameParticipant.Call >= RoundN(currentGameParticipant.Chips, n - rnd) &&
                            currentGameParticipant.Call <= RoundN(currentGameParticipant.Chips, n1 - rnd))
                        {
                            this.ChooseToCall(currentGameParticipant);
                        }

                        if (currentGameParticipant.Raise <= RoundN(currentGameParticipant.Chips, n - rnd) &&
                            currentGameParticipant.Raise >= RoundN(currentGameParticipant.Chips, n - rnd) / 2)
                        {
                            this.ChooseToCall(currentGameParticipant);
                        }

                        if (currentGameParticipant.Raise <= RoundN(currentGameParticipant.Chips, n - rnd) / 2)
                        {
                            if (currentGameParticipant.Raise > 0)
                            {
                                currentGameParticipant.Raise = (int)RoundN(currentGameParticipant.Chips, n - rnd);
                                Raised(currentGameParticipant);
                            }
                            else
                            {
                                currentGameParticipant.Raise = currentGameParticipant.Call * 2;
                                this.Raised(currentGameParticipant);
                            }
                        }
                    }
                }

                if (currentGameParticipant.Call <= 0)
                {
                    currentGameParticipant.Raise = (int)RoundN(currentGameParticipant.Chips, r - rnd);
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
            if (currentGameParticipant.Call <= 0)
            {
                currentGameParticipant.ChooseToCheck();
            }
            else
            {
                if (currentGameParticipant.Call >= RoundN(currentGameParticipant.Chips, call))
                {
                    if (currentGameParticipant.Chips > currentGameParticipant.Call)
                    {
                        this.ChooseToCall(currentGameParticipant);
                    }
                    else if (currentGameParticipant.Chips <= currentGameParticipant.Call)
                    {
                        currentGameParticipant.RaiseTurn = false;
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
                    if (currentGameParticipant.Raise > 0)
                    {
                        if (currentGameParticipant.Chips >= currentGameParticipant.Raise * 2)
                        {
                            currentGameParticipant.Raise *= 2;
                            this.Raised(currentGameParticipant);
                        }
                        else
                        {
                            this.ChooseToCall(currentGameParticipant);
                        }
                    }
                    else
                    {
                        currentGameParticipant.Raise = currentGameParticipant.Call * 2;
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
        private async void TimerTick(object sender, object e)
        {
            if (this.timerProgressBar.Value <= 0)
            {
                this.player.FoldTurn = true;
                await this.Turns();
            }
            if (this.timeForTurn > 0)
            {
                this.timeForTurn--;
                this.timerProgressBar.Value = this.timeForTurn / 6 * 100;
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

            if (this.player.Chips >= this.bigBlind)
            {
                this.callButton.Text = "Call " + this.bigBlind;
            }
            else
            {
                this.callButton.Text = "All in";
                this.raiseButton.Enabled = false;
            }

            if (this.player.Call > 0)
            {
                this.checkButton.Enabled = false;
            }
            else if (this.player.Call <= 0)
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
                    this.raiseButton.Text = "Raise";
                }
            }

            // critical
            if (this.player.Chips < this.player.Raise)
            {
                this.raiseButton.Enabled = false;
            }
        }

        private async void ButtonFoldOnClick(object sender, EventArgs e)
        {
            this.player.ParticipantPanel.StatusButton.Text = "Fold";
            this.player.Turn = false;
            this.player.FoldTurn = true;

            await this.Turns();
        }

        private async void ButtonCheckOnClick(object sender, EventArgs e)
        {
            // critical
            if (this.player.Call <= 0)
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

        private async void ButtonCallOnClick(object sender, EventArgs e)
        {
            this.Rule.ExecuteGameRules(0, 1, this.player);
            if (this.player.Chips >= this.player.Call)
            {
                this.player.Chips -= this.player.Call;
                this.player.ParticipantPanel.ChipsTextBox.Text =
                    "Chips : " + this.player.Chips;

                if (this.potTextBox.Text != string.Empty)
                {
                    this.potTextBox.Text =
                        (int.Parse(this.potTextBox.Text) + this.player.Call).ToString();
                }
                else
                {
                    this.potTextBox.Text = this.player.Call.ToString();
                }

                this.player.Turn = false;
                this.player.ParticipantPanel.StatusButton.Text = "Call " + this.player.Call;
                //this.player.Call = this.call;
            }
            else if (this.player.Chips <= this.player.Call && this.player.Call > 0)
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

        private async void ButtonRaiseOnClick(object sender, EventArgs e)
        {
            this.Rule.ExecuteGameRules(0, 1, this.player);
            int parsedValue;
            if (this.raiseTextBox.Text != string.Empty &&
                int.TryParse(this.raiseTextBox.Text, out parsedValue))
            {
                if (this.player.Chips > this.player.Call)
                {
                    if (this.bigBlind * 2 > int.Parse(this.raiseTextBox.Text))
                    {
                        this.raiseTextBox.Text = (this.player.Raise * 2).ToString();
                        this.writer.Print(
                            "You must raise at least twice as the current big blind!");

                        return;
                    }
                    else
                    {
                        if (this.player.Chips >= int.Parse(this.raiseTextBox.Text))
                        {
                            //this.call = int.Parse(this.raiseTextBox.Text);
                            this.player.Raise = int.Parse(this.raiseTextBox.Text);
                            this.player.ParticipantPanel.StatusButton.Text = "Raise " + this.player.Raise;
                            this.potTextBox.Text = (int.Parse(this.potTextBox.Text) + this.player.Raise).ToString();
                            //this.callButton.Text = "Call";
                            this.player.Chips -= int.Parse(this.raiseTextBox.Text);
                            this.player.RaiseTurn = true;
                            this.last = 0;
                            //this.player.Raise = Convert.ToInt32(this.raise);
                        }
                        else
                        {
                            //this.player.Call = this.player.Chips;
                            this.player.Raise = this.player.Chips;
                            this.potTextBox.Text =
                                (int.Parse(this.potTextBox.Text) + this.player.Chips).ToString();
                            this.player.ParticipantPanel.StatusButton.Text = "Raise " + this.player.Raise;
                            this.player.Chips = 0;
                            this.player.RaiseTurn = true;
                            this.last = 0;
                            //this.player.Raise = Convert.ToInt32(this.raise);
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

        /// <summary>
        /// With using this method the player can add to his pocket more chips.
        /// </summary>
        /// <param name="sender">Command "add chips"</param>
        private void ButtonAddOnClick(object sender, EventArgs e)
        {
            if (this.addChipsTextBox.Text != string.Empty)
            {
                this.player.Chips += int.Parse(this.addChipsTextBox.Text);
            }

            this.player.ParticipantPanel.ChipsTextBox.Text = "Chips : " + this.player.Chips;
        }

        private void ButtonOptionsOnClick(object sender, EventArgs e)
        {
            this.bigBlindTextBox.Text = this.bigBlind.ToString();
            this.smallBlindTextBox.Text = this.smallBlind.ToString();
            if (this.bigBlindTextBox.Visible == false)
            {
                this.bigBlindTextBox.Visible = true;
                this.smallBlindTextBox.Visible = true;
            }
            else
            {
                this.bigBlindTextBox.Visible = false;
                this.smallBlindTextBox.Visible = false;
            }
        }

        private void ButtonSmallBlindOnClick(object sender, EventArgs e)
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
                    "The changes have been saved! They will become available the next hand you play. ");
            }
        }

        private void ButtonBigBlindOnClick(object sender, EventArgs e)
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
            IBot bot,
            PictureBox[] cardImages,
            Bitmap backImage,
            int[] reserve,
            int currentCard)
        {
            cardImages[currentCard].Tag = reserve[currentCard];
            cardImages[currentCard].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cardImages[currentCard].Visible = true;
        }

        private void InitializeCardLocations(Point[] cardLocations)
        {
            // Player cards
            cardLocations[0] = new Point(580, 480);
            cardLocations[1] = new Point(660, 480);
            // Bot1 cards
            cardLocations[2] = new Point(15, 420);
            cardLocations[3] = new Point(95, 420);
            // Bot2 cards
            cardLocations[4] = new Point(75, 65);
            cardLocations[5] = new Point(155, 65);
            // Bot3 cards
            cardLocations[6] = new Point(590, 25);
            cardLocations[7] = new Point(670, 25);
            // Bot4 cards
            cardLocations[8] = new Point(1115, 65);
            cardLocations[9] = new Point(1195, 65);
            // Bot5 cards
            cardLocations[10] = new Point(1160, 420);
            cardLocations[11] = new Point(1240, 420);
            // River card locations
            cardLocations[12] = new Point(410, 265);
            cardLocations[13] = new Point(520, 265);
            cardLocations[14] = new Point(630, 265);
            cardLocations[15] = new Point(740, 265);
            cardLocations[16] = new Point(850, 265);
        }

        private void EnableButtons()
        {
            this.raiseButton.Enabled = true;
            this.callButton.Enabled = true;
            this.foldButton.Enabled = true;
        }

        private void AddMoreChips(AddChips chipsAdder)
        {
            chipsAdder.ShowDialog();
            if (chipsAdder.NewChips != 0)
            {
                this.player.Chips = chipsAdder.NewChips;

                this.player.FoldTurn = false;
                this.player.Turn = true;
                this.raiseButton.Enabled = true;
                this.foldButton.Enabled = true;
                this.checkButton.Enabled = true;
                this.raiseButton.Text = "Raise";
            }
        }
    }
}