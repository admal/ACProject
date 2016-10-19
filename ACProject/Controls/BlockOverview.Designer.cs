namespace ACProject.Controls
{
    partial class BlockOverview
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.blockView = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBlockCount = new System.Windows.Forms.Label();
            this.tbBlocksCount = new System.Windows.Forms.TextBox();
            this.mainContainer.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainContainer
            // 
            this.mainContainer.ColumnCount = 2;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.mainContainer.Controls.Add(this.blockView, 0, 0);
            this.mainContainer.Controls.Add(this.panelButtons, 1, 0);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.RowCount = 1;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainContainer.Size = new System.Drawing.Size(1132, 391);
            this.mainContainer.TabIndex = 0;
            // 
            // blockView
            // 
            this.blockView.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.blockView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blockView.Location = new System.Drawing.Point(3, 3);
            this.blockView.Name = "blockView";
            this.blockView.Size = new System.Drawing.Size(786, 385);
            this.blockView.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.lblBlockCount);
            this.panelButtons.Controls.Add(this.tbBlocksCount);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(795, 3);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(334, 385);
            this.panelButtons.TabIndex = 1;
            // 
            // lblBlockCount
            // 
            this.lblBlockCount.AutoSize = true;
            this.lblBlockCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblBlockCount.Location = new System.Drawing.Point(3, 0);
            this.lblBlockCount.Name = "lblBlockCount";
            this.lblBlockCount.Size = new System.Drawing.Size(29, 31);
            this.lblBlockCount.TabIndex = 0;
            this.lblBlockCount.Text = "#";
            // 
            // tbBlocksCount
            // 
            this.tbBlocksCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbBlocksCount.Location = new System.Drawing.Point(38, 3);
            this.tbBlocksCount.Name = "tbBlocksCount";
            this.tbBlocksCount.Size = new System.Drawing.Size(52, 38);
            this.tbBlocksCount.TabIndex = 1;
            // 
            // BlockOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.mainContainer);
            this.Name = "BlockOverview";
            this.Size = new System.Drawing.Size(1132, 391);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainContainer.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.Panel blockView;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Label lblBlockCount;
        private System.Windows.Forms.TextBox tbBlocksCount;
    }
}
