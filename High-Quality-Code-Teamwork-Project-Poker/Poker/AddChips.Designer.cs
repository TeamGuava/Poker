namespace Poker
{
    public partial class AddChips
    {
        private System.Windows.Forms.Label labelMainMessage;
        private System.Windows.Forms.Button buttonAddChips;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.TextBox textBoxAddNewChips;

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
            this.labelMainMessage = new System.Windows.Forms.Label();
            this.buttonAddChips = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.textBoxAddNewChips = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelMainMessage
            // 
            this.labelMainMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMainMessage.Location = new System.Drawing.Point(48, 49);
            this.labelMainMessage.Name = "labelMainMessage";
            this.labelMainMessage.Size = new System.Drawing.Size(176, 23);
            this.labelMainMessage.TabIndex = 0;
            this.labelMainMessage.Text = "You ran out of chips !";
            this.labelMainMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAddChips
            // 
            this.buttonAddChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddChips.Location = new System.Drawing.Point(12, 226);
            this.buttonAddChips.Name = "buttonAddChips";
            this.buttonAddChips.Size = new System.Drawing.Size(75, 23);
            this.buttonAddChips.TabIndex = 1;
            this.buttonAddChips.Text = "Add Chips";
            this.buttonAddChips.UseVisualStyleBackColor = true;
            this.buttonAddChips.Click += new System.EventHandler(this.ClickButtonAddChips);
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.Location = new System.Drawing.Point(197, 226);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.ClickButtonExit);
            // 
            // textBoxAddNewChips
            // 
            this.textBoxAddNewChips.Location = new System.Drawing.Point(91, 229);
            this.textBoxAddNewChips.Name = "textBoxAddNewChips";
            this.textBoxAddNewChips.Size = new System.Drawing.Size(100, 20);
            this.textBoxAddNewChips.TabIndex = 3;
            // 
            // AddChips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBoxAddNewChips);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonAddChips);
            this.Controls.Add(this.labelMainMessage);
            this.Name = "AddChips";
            this.Text = "You Ran Out Of Chips";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}