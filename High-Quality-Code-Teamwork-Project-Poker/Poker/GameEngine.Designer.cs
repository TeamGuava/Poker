namespace Poker
{
    public partial class GameEngine
    {
        private System.Windows.Forms.Button foldButton;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button callButton;
        private System.Windows.Forms.Button raiseButton;
        private System.Windows.Forms.Button addChipsButton;
        private System.Windows.Forms.Button optionsButton;
        private System.Windows.Forms.Button bigBlindButton;
        private System.Windows.Forms.Button smallBlindButton;
        private System.Windows.Forms.TextBox potTextBox;
        private System.Windows.Forms.TextBox addChipsTextBox;
        private System.Windows.Forms.TextBox smallBlindTextBox;
        private System.Windows.Forms.TextBox bigBlindTextBox;
        private System.Windows.Forms.TextBox raiseTextBox;
        private System.Windows.Forms.Label potLabel;
        private System.Windows.Forms.ProgressBar timerProgressBar;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.foldButton = new System.Windows.Forms.Button();
            this.checkButton = new System.Windows.Forms.Button();
            this.callButton = new System.Windows.Forms.Button();
            this.raiseButton = new System.Windows.Forms.Button();
            this.timerProgressBar = new System.Windows.Forms.ProgressBar();
            this.addChipsButton = new System.Windows.Forms.Button();
            this.addChipsTextBox = new System.Windows.Forms.TextBox();
            this.potTextBox = new System.Windows.Forms.TextBox();
            this.optionsButton = new System.Windows.Forms.Button();
            this.bigBlindButton = new System.Windows.Forms.Button();
            this.smallBlindTextBox = new System.Windows.Forms.TextBox();
            this.smallBlindButton = new System.Windows.Forms.Button();
            this.bigBlindTextBox = new System.Windows.Forms.TextBox();
            this.potLabel = new System.Windows.Forms.Label();
            this.raiseTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // fold_Button
            // 
            this.foldButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.foldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.foldButton.Location = new System.Drawing.Point(335, 660);
            this.foldButton.Name = "fold_Button";
            this.foldButton.Size = new System.Drawing.Size(130, 62);
            this.foldButton.TabIndex = 0;
            this.foldButton.Text = "Fold";
            this.foldButton.UseVisualStyleBackColor = true;
            this.foldButton.Click += new System.EventHandler(this.buttonFoldOnClick);
            // 
            // check_Button
            // 
            this.checkButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkButton.Location = new System.Drawing.Point(494, 660);
            this.checkButton.Name = "check_Button";
            this.checkButton.Size = new System.Drawing.Size(134, 62);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "Check";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.buttonCheckOnClick);
            // 
            // call_Button
            // 
            this.callButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.callButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.callButton.Location = new System.Drawing.Point(667, 661);
            this.callButton.Name = "call_Button";
            this.callButton.Size = new System.Drawing.Size(126, 62);
            this.callButton.TabIndex = 3;
            this.callButton.Text = "Call";
            this.callButton.UseVisualStyleBackColor = true;
            this.callButton.Click += new System.EventHandler(this.buttonCallOnClick);
            // 
            // raise_Button
            // 
            this.raiseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.raiseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.raiseButton.Location = new System.Drawing.Point(835, 661);
            this.raiseButton.Name = "raise_Button";
            this.raiseButton.Size = new System.Drawing.Size(124, 62);
            this.raiseButton.TabIndex = 4;
            this.raiseButton.Text = "raise";
            this.raiseButton.UseVisualStyleBackColor = true;
            this.raiseButton.Click += new System.EventHandler(this.buttonRaiseOnClick);
            // 
            // timer_ProgressBar
            // 
            this.timerProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.timerProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.timerProgressBar.Location = new System.Drawing.Point(335, 631);
            this.timerProgressBar.Maximum = 1000;
            this.timerProgressBar.Name = "timer_ProgressBar";
            this.timerProgressBar.Size = new System.Drawing.Size(667, 23);
            this.timerProgressBar.TabIndex = 5;
            this.timerProgressBar.Value = 1000;
            // 
            // player_ChipsTextBox
            // 
            this.player.ParticipantPanel.ChipsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.player.ParticipantPanel.ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player.ParticipantPanel.ChipsTextBox.Location = new System.Drawing.Point(755, 553);
            this.player.ParticipantPanel.ChipsTextBox.Name = "player_ChipsTextBox";
            this.player.ParticipantPanel.ChipsTextBox.Size = new System.Drawing.Size(163, 23);
            this.player.ParticipantPanel.ChipsTextBox.TabIndex = 6;
            this.player.ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            // 
            // addChipsButton
            // 
            this.addChipsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addChipsButton.Location = new System.Drawing.Point(12, 697);
            this.addChipsButton.Name = "addChipsButton";
            this.addChipsButton.Size = new System.Drawing.Size(75, 25);
            this.addChipsButton.TabIndex = 7;
            this.addChipsButton.Text = "AddChips";
            this.addChipsButton.UseVisualStyleBackColor = true;
            this.addChipsButton.Click += new System.EventHandler(this.buttonAddOnClick);
            // 
            // addChipsTextBox
            // 
            this.addChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addChipsTextBox.Location = new System.Drawing.Point(93, 700);
            this.addChipsTextBox.Name = "addChipsTextBox";
            this.addChipsTextBox.Size = new System.Drawing.Size(125, 20);
            this.addChipsTextBox.TabIndex = 8;
            // 
            // bot5_ChipsTextBox
            // 
            this.gameBots[4].ParticipantPanel.ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gameBots[4].ParticipantPanel.ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gameBots[4].ParticipantPanel.ChipsTextBox.Location = new System.Drawing.Point(1012, 553);
            this.gameBots[4].ParticipantPanel.ChipsTextBox.Name = "bot5_ChipsTextBox";
            this.gameBots[4].ParticipantPanel.ChipsTextBox.Size = new System.Drawing.Size(152, 23);
            this.gameBots[4].ParticipantPanel.ChipsTextBox.TabIndex = 9;
            this.gameBots[4].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            // 
            // bot4_ChipsTextBox
            // 
            this.gameBots[3].ParticipantPanel.ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gameBots[3].ParticipantPanel.ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gameBots[3].ParticipantPanel.ChipsTextBox.Location = new System.Drawing.Point(970, 81);
            this.gameBots[3].ParticipantPanel.ChipsTextBox.Name = "bot4_ChipsTextBox";
            this.gameBots[3].ParticipantPanel.ChipsTextBox.Size = new System.Drawing.Size(123, 23);
            this.gameBots[3].ParticipantPanel.ChipsTextBox.TabIndex = 10;
            this.gameBots[3].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            // 
            // bot3_ChipsTextBox
            // 
            this.gameBots[2].ParticipantPanel.ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gameBots[2].ParticipantPanel.ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gameBots[2].ParticipantPanel.ChipsTextBox.Location = new System.Drawing.Point(755, 81);
            this.gameBots[2].ParticipantPanel.ChipsTextBox.Name = "bot3_ChipsTextBox";
            this.gameBots[2].ParticipantPanel.ChipsTextBox.Size = new System.Drawing.Size(125, 23);
            this.gameBots[2].ParticipantPanel.ChipsTextBox.TabIndex = 11;
            this.gameBots[2].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            // 
            // bot2_ChipsTextBox
            // 
            this.gameBots[1].ParticipantPanel.ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gameBots[1].ParticipantPanel.ChipsTextBox.Location = new System.Drawing.Point(276, 81);
            this.gameBots[1].ParticipantPanel.ChipsTextBox.Name = "bot2_ChipsTextBox";
            this.gameBots[1].ParticipantPanel.ChipsTextBox.Size = new System.Drawing.Size(133, 23);
            this.gameBots[1].ParticipantPanel.ChipsTextBox.TabIndex = 12;
            this.gameBots[1].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            //   
            // bot1_ChipsTextBox
            // 
            this.gameBots[0].ParticipantPanel.ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gameBots[0].ParticipantPanel.ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gameBots[0].ParticipantPanel.ChipsTextBox.Location = new System.Drawing.Point(181, 553);
            this.gameBots[0].ParticipantPanel.ChipsTextBox.Name = "bot1_ChipsTextBox";
            this.gameBots[0].ParticipantPanel.ChipsTextBox.Size = new System.Drawing.Size(142, 23);
            this.gameBots[0].ParticipantPanel.ChipsTextBox.TabIndex = 13;
            this.gameBots[0].ParticipantPanel.ChipsTextBox.Text = "Chips : 0";
            // 
            // pot_TextBox
            // 
            this.potTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.potTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potTextBox.Location = new System.Drawing.Point(606, 212);
            this.potTextBox.Name = "pot_TextBox";
            this.potTextBox.Size = new System.Drawing.Size(125, 23);
            this.potTextBox.TabIndex = 14;
            this.potTextBox.Text = "0";
            // 
            // optionsButton
            // 
            this.optionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.optionsButton.Location = new System.Drawing.Point(12, 12);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(75, 36);
            this.optionsButton.TabIndex = 15;
            this.optionsButton.Text = "BB/SB";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.buttonOptionsOnClick);
            // 
            // bigBlindButton
            // 
            this.bigBlindButton.Location = new System.Drawing.Point(12, 254);
            this.bigBlindButton.Name = "bigBlindButton";
            this.bigBlindButton.Size = new System.Drawing.Size(75, 23);
            this.bigBlindButton.TabIndex = 16;
            this.bigBlindButton.Text = "Big Blind";
            this.bigBlindButton.UseVisualStyleBackColor = true;
            this.bigBlindButton.Click += new System.EventHandler(this.buttonBigBlindOnClick);
            // 
            // smallBlindTextBox
            // 
            this.smallBlindTextBox.Location = new System.Drawing.Point(12, 228);
            this.smallBlindTextBox.Name = "smallBlindTextBox";
            this.smallBlindTextBox.Size = new System.Drawing.Size(75, 20);
            this.smallBlindTextBox.TabIndex = 17;
            this.smallBlindTextBox.Text = "250";
            // 
            // smallBlindButton
            // 
            this.smallBlindButton.Location = new System.Drawing.Point(12, 199);
            this.smallBlindButton.Name = "smallBlindButton";
            this.smallBlindButton.Size = new System.Drawing.Size(75, 23);
            this.smallBlindButton.TabIndex = 18;
            this.smallBlindButton.Text = "Small Blind";
            this.smallBlindButton.UseVisualStyleBackColor = true;
            this.smallBlindButton.Click += new System.EventHandler(this.buttonSmallBlindOnClick);
            // 
            // bigBlindTextBox
            // 
            this.bigBlindTextBox.Location = new System.Drawing.Point(12, 283);
            this.bigBlindTextBox.Name = "bigBlindTextBox";
            this.bigBlindTextBox.Size = new System.Drawing.Size(75, 20);
            this.bigBlindTextBox.TabIndex = 19;
            this.bigBlindTextBox.Text = "500";
            // 
            // bot5_StatusButton
            // 
            this.gameBots[4].ParticipantPanel.StatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gameBots[4].ParticipantPanel.StatusButton.Location = new System.Drawing.Point(1012, 579);
            this.gameBots[4].ParticipantPanel.StatusButton.Name = "bot5_StatusButton";
            this.gameBots[4].ParticipantPanel.StatusButton.Size = new System.Drawing.Size(152, 32);
            this.gameBots[4].ParticipantPanel.StatusButton.TabIndex = 26;
            // 
            // bot4_StatusButton
            // 
            this.gameBots[3].ParticipantPanel.StatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gameBots[3].ParticipantPanel.StatusButton.Location = new System.Drawing.Point(970, 107);
            this.gameBots[3].ParticipantPanel.StatusButton.Name = "bot4_StatusButton";
            this.gameBots[3].ParticipantPanel.StatusButton.Size = new System.Drawing.Size(123, 32);
            this.gameBots[3].ParticipantPanel.StatusButton.TabIndex = 27;
            // 
            // bot3_StatusButton
            // 
            this.gameBots[2].ParticipantPanel.StatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gameBots[2].ParticipantPanel.StatusButton.Location = new System.Drawing.Point(755, 107);
            this.gameBots[2].ParticipantPanel.StatusButton.Name = "bot3_StatusButton";
            this.gameBots[2].ParticipantPanel.StatusButton.Size = new System.Drawing.Size(125, 32);
            this.gameBots[2].ParticipantPanel.StatusButton.TabIndex = 28;
            // 
            // bot2_StatusButton
            // 
            this.gameBots[1].ParticipantPanel.StatusButton.Location = new System.Drawing.Point(276, 107);
            this.gameBots[1].ParticipantPanel.StatusButton.Name = "bot2_StatusButton";
            this.gameBots[1].ParticipantPanel.StatusButton.Size = new System.Drawing.Size(133, 32);
            this.gameBots[1].ParticipantPanel.StatusButton.TabIndex = 31;
            //   
            // bot1_StatusButton
            // 
            this.gameBots[0].ParticipantPanel.StatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gameBots[0].ParticipantPanel.StatusButton.Location = new System.Drawing.Point(181, 579);
            this.gameBots[0].ParticipantPanel.StatusButton.Name = "bot1_StatusButton";
            this.gameBots[0].ParticipantPanel.StatusButton.Size = new System.Drawing.Size(142, 32);
            this.gameBots[0].ParticipantPanel.StatusButton.TabIndex = 29;
            // 
            // player_StatusButton
            // 
            this.player.ParticipantPanel.StatusButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.player.ParticipantPanel.StatusButton.Location = new System.Drawing.Point(755, 579);
            this.player.ParticipantPanel.StatusButton.Name = "player_StatusButton";
            this.player.ParticipantPanel.StatusButton.Size = new System.Drawing.Size(163, 32);
            this.player.ParticipantPanel.StatusButton.TabIndex = 30;
            // 
            // potLabel
            // 
            this.potLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.potLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potLabel.Location = new System.Drawing.Point(654, 188);
            this.potLabel.Name = "potLabel";
            this.potLabel.Size = new System.Drawing.Size(31, 21);
            this.potLabel.TabIndex = 0;
            this.potLabel.Text = "Pot";
            // 
            // raise_TextBox
            // 
            this.raiseTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.raiseTextBox.Location = new System.Drawing.Point(965, 703);
            this.raiseTextBox.Name = "raise_TextBox";
            this.raiseTextBox.Size = new System.Drawing.Size(108, 20);
            this.raiseTextBox.TabIndex = 0;
            // 
            // GameEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.raiseTextBox);
            this.Controls.Add(this.potLabel);

            this.Controls.Add(this.player.ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[0].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[1].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[2].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[3].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[4].ParticipantPanel.StatusButton);

            this.Controls.Add(this.bigBlindTextBox);
            this.Controls.Add(this.smallBlindButton);
            this.Controls.Add(this.smallBlindTextBox);
            this.Controls.Add(this.bigBlindButton);
            this.Controls.Add(this.optionsButton);
            this.Controls.Add(this.potTextBox);

            this.Controls.Add(this.player.ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[0].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[1].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[2].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[3].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[4].ParticipantPanel.ChipsTextBox);

            this.Controls.Add(this.addChipsTextBox);
            this.Controls.Add(this.addChipsButton);
            this.Controls.Add(this.timerProgressBar);
            this.Controls.Add(this.raiseButton);
            this.Controls.Add(this.callButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.foldButton);
            this.DoubleBuffered = true;
            this.Name = "GameEngine";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ChangeLayout);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    } 
} 

