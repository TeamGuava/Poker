namespace Poker
{
    partial class GameEngine
    {
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.addChips_Button = new System.Windows.Forms.Button();
            this.addChips_TextBox = new System.Windows.Forms.TextBox();

            //this.player.ParticipantPanel.ChipsTextBox = new System.Windows.Forms.TextBox();
            //this.gameBots[0].ParticipantPanel.ChipsTextBox = new System.Windows.Forms.TextBox();
            //this.gameBots[1].ParticipantPanel.ChipsTextBox = new System.Windows.Forms.TextBox();
            //this.gameBots[2].ParticipantPanel.ChipsTextBox = new System.Windows.Forms.TextBox();
            //this.gameBots[3].ParticipantPanel.ChipsTextBox = new System.Windows.Forms.TextBox();
            //this.gameBots[4].ParticipantPanel.ChipsTextBox = new System.Windows.Forms.TextBox();

            this.potTextBox = new System.Windows.Forms.TextBox();
            this.options_Button = new System.Windows.Forms.Button();
            this.bigBlind_Button = new System.Windows.Forms.Button();
            this.smallBlind_TextBox = new System.Windows.Forms.TextBox();
            this.smallBlind_Button = new System.Windows.Forms.Button();
            this.bigBlind_TextBox = new System.Windows.Forms.TextBox();

            //this.player.ParticipantPanel.StatusButton = new System.Windows.Forms.Label();
            //this.gameBots[0].ParticipantPanel.StatusButton = new System.Windows.Forms.Label();
            //this.gameBots[1].ParticipantPanel.StatusButton = new System.Windows.Forms.Label();
            //this.gameBots[2].ParticipantPanel.StatusButton = new System.Windows.Forms.Label();
            //this.gameBots[3].ParticipantPanel.StatusButton = new System.Windows.Forms.Label();
            //this.gameBots[4].ParticipantPanel.StatusButton = new System.Windows.Forms.Label();

            this.pot_Label = new System.Windows.Forms.Label();
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
            this.foldButton.Click += new System.EventHandler(this.botFoldOnClick);
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
            this.checkButton.Click += new System.EventHandler(this.botCheckOnClick);
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
            this.callButton.Click += new System.EventHandler(this.bCall_Click);
            // 
            // raise_Button
            // 
            this.raiseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.raiseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.raiseButton.Location = new System.Drawing.Point(835, 661);
            this.raiseButton.Name = "raise_Button";
            this.raiseButton.Size = new System.Drawing.Size(124, 62);
            this.raiseButton.TabIndex = 4;
            this.raiseButton.Text = "Raise";
            this.raiseButton.UseVisualStyleBackColor = true;
            this.raiseButton.Click += new System.EventHandler(this.botRaiseOnClick);
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
            // addChips_Button
            // 
            this.addChips_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addChips_Button.Location = new System.Drawing.Point(12, 697);
            this.addChips_Button.Name = "addChips_Button";
            this.addChips_Button.Size = new System.Drawing.Size(75, 25);
            this.addChips_Button.TabIndex = 7;
            this.addChips_Button.Text = "AddChips";
            this.addChips_Button.UseVisualStyleBackColor = true;
            this.addChips_Button.Click += new System.EventHandler(this.botAddOnClick);
            // 
            // addChips_TextBox
            // 
            this.addChips_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addChips_TextBox.Location = new System.Drawing.Point(93, 700);
            this.addChips_TextBox.Name = "addChips_TextBox";
            this.addChips_TextBox.Size = new System.Drawing.Size(125, 20);
            this.addChips_TextBox.TabIndex = 8;
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
            // options_Button
            // 
            this.options_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.options_Button.Location = new System.Drawing.Point(12, 12);
            this.options_Button.Name = "options_Button";
            this.options_Button.Size = new System.Drawing.Size(75, 36);
            this.options_Button.TabIndex = 15;
            this.options_Button.Text = "BB/SB";
            this.options_Button.UseVisualStyleBackColor = true;
            this.options_Button.Click += new System.EventHandler(this.botOptionsOnClick);
            // 
            // bigBlind_Button
            // 
            this.bigBlind_Button.Location = new System.Drawing.Point(12, 254);
            this.bigBlind_Button.Name = "bigBlind_Button";
            this.bigBlind_Button.Size = new System.Drawing.Size(75, 23);
            this.bigBlind_Button.TabIndex = 16;
            this.bigBlind_Button.Text = "Big Blind";
            this.bigBlind_Button.UseVisualStyleBackColor = true;
            this.bigBlind_Button.Click += new System.EventHandler(this.bBB_Click);
            // 
            // smallBlind_TextBox
            // 
            this.smallBlind_TextBox.Location = new System.Drawing.Point(12, 228);
            this.smallBlind_TextBox.Name = "smallBlind_TextBox";
            this.smallBlind_TextBox.Size = new System.Drawing.Size(75, 20);
            this.smallBlind_TextBox.TabIndex = 17;
            this.smallBlind_TextBox.Text = "250";
            // 
            // smallBlind_Button
            // 
            this.smallBlind_Button.Location = new System.Drawing.Point(12, 199);
            this.smallBlind_Button.Name = "smallBlind_Button";
            this.smallBlind_Button.Size = new System.Drawing.Size(75, 23);
            this.smallBlind_Button.TabIndex = 18;
            this.smallBlind_Button.Text = "Small Blind";
            this.smallBlind_Button.UseVisualStyleBackColor = true;
            this.smallBlind_Button.Click += new System.EventHandler(this.bSB_Click);
            // 
            // bigBlind_TextBox
            // 
            this.bigBlind_TextBox.Location = new System.Drawing.Point(12, 283);
            this.bigBlind_TextBox.Name = "bigBlind_TextBox";
            this.bigBlind_TextBox.Size = new System.Drawing.Size(75, 20);
            this.bigBlind_TextBox.TabIndex = 19;
            this.bigBlind_TextBox.Text = "500";
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
            // pot_Label
            // 
            this.pot_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pot_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pot_Label.Location = new System.Drawing.Point(654, 188);
            this.pot_Label.Name = "pot_Label";
            this.pot_Label.Size = new System.Drawing.Size(31, 21);
            this.pot_Label.TabIndex = 0;
            this.pot_Label.Text = "Pot";
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
            this.Controls.Add(this.pot_Label);

            this.Controls.Add(this.player.ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[0].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[1].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[2].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[3].ParticipantPanel.StatusButton);
            this.Controls.Add(this.gameBots[4].ParticipantPanel.StatusButton);

            this.Controls.Add(this.bigBlind_TextBox);
            this.Controls.Add(this.smallBlind_Button);
            this.Controls.Add(this.smallBlind_TextBox);
            this.Controls.Add(this.bigBlind_Button);
            this.Controls.Add(this.options_Button);
            this.Controls.Add(this.potTextBox);

            this.Controls.Add(this.player.ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[0].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[1].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[2].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[3].ParticipantPanel.ChipsTextBox);
            this.Controls.Add(this.gameBots[4].ParticipantPanel.ChipsTextBox);

            this.Controls.Add(this.addChips_TextBox);
            this.Controls.Add(this.addChips_Button);
            this.Controls.Add(this.timerProgressBar);
            this.Controls.Add(this.raiseButton);
            this.Controls.Add(this.callButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.foldButton);
            this.DoubleBuffered = true;
            this.Name = "GameEngine";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button foldButton;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button callButton;
        private System.Windows.Forms.Button raiseButton;
        private System.Windows.Forms.ProgressBar timerProgressBar;
        private System.Windows.Forms.Button addChips_Button;
        private System.Windows.Forms.TextBox addChips_TextBox;

        //private System.Windows.Forms.TextBox player_ChipsTextBox;
        //private System.Windows.Forms.TextBox bot5_ChipsTextBox;
        //private System.Windows.Forms.TextBox bot4_ChipsTextBox;
        //private System.Windows.Forms.TextBox bot3_ChipsTextBox;
        //private System.Windows.Forms.TextBox bot2_ChipsTextBox;
        //private System.Windows.Forms.TextBox bot1_ChipsTextBox;
        private System.Windows.Forms.TextBox potTextBox;
        private System.Windows.Forms.Button options_Button;
        private System.Windows.Forms.Button bigBlind_Button;
        private System.Windows.Forms.TextBox smallBlind_TextBox;
        private System.Windows.Forms.Button smallBlind_Button;
        private System.Windows.Forms.TextBox bigBlind_TextBox;
        //private System.Windows.Forms.Label bot5_StatusButton;
        //private System.Windows.Forms.Label bot4_StatusButton;
        //private System.Windows.Forms.Label bot3_StatusButton;
        //private System.Windows.Forms.Label bot1_StatusButton;
        //private System.Windows.Forms.Label playerStatusButton;
        //private System.Windows.Forms.Label bot2_StatusButton;
        private System.Windows.Forms.Label pot_Label;
        private System.Windows.Forms.TextBox raiseTextBox;
    }
}

