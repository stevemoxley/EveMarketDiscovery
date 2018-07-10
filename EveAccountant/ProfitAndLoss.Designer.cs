namespace EveAccountant
{
    partial class ProfitAndLoss
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblTotalProfit = new System.Windows.Forms.Label();
            this.lblFees = new System.Windows.Forms.Label();
            this.lblUnsoldInventory = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 221);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(836, 353);
            this.dataGridView1.TabIndex = 0;
            // 
            // lblTotalProfit
            // 
            this.lblTotalProfit.AutoSize = true;
            this.lblTotalProfit.Location = new System.Drawing.Point(12, 29);
            this.lblTotalProfit.Name = "lblTotalProfit";
            this.lblTotalProfit.Size = new System.Drawing.Size(64, 13);
            this.lblTotalProfit.TabIndex = 1;
            this.lblTotalProfit.Text = "Total Profit: ";
            // 
            // lblFees
            // 
            this.lblFees.AutoSize = true;
            this.lblFees.Location = new System.Drawing.Point(12, 60);
            this.lblFees.Name = "lblFees";
            this.lblFees.Size = new System.Drawing.Size(63, 13);
            this.lblFees.TabIndex = 2;
            this.lblFees.Text = "Total Fees: ";
            // 
            // lblUnsoldInventory
            // 
            this.lblUnsoldInventory.AutoSize = true;
            this.lblUnsoldInventory.Location = new System.Drawing.Point(12, 87);
            this.lblUnsoldInventory.Name = "lblUnsoldInventory";
            this.lblUnsoldInventory.Size = new System.Drawing.Size(90, 13);
            this.lblUnsoldInventory.TabIndex = 3;
            this.lblUnsoldInventory.Text = "Unsold Inventory:";
            // 
            // ProfitAndLoss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 586);
            this.Controls.Add(this.lblUnsoldInventory);
            this.Controls.Add(this.lblFees);
            this.Controls.Add(this.lblTotalProfit);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ProfitAndLoss";
            this.Text = "Profit And Loss";
            this.Load += new System.EventHandler(this.ProfitAndLoss_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblTotalProfit;
        private System.Windows.Forms.Label lblFees;
        private System.Windows.Forms.Label lblUnsoldInventory;
    }
}