namespace EveMarketOrderBrowser
{
    partial class Main
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.dgvSellOrders = new System.Windows.Forms.DataGridView();
            this.dgvBuyOrders = new System.Windows.Forms.DataGridView();
            this.ddlRegion = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSellOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(720, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Location = new System.Drawing.Point(10, 11);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(509, 20);
            this.txtSearchTerm.TabIndex = 1;
            // 
            // dgvSellOrders
            // 
            this.dgvSellOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSellOrders.Location = new System.Drawing.Point(7, 56);
            this.dgvSellOrders.Name = "dgvSellOrders";
            this.dgvSellOrders.Size = new System.Drawing.Size(782, 153);
            this.dgvSellOrders.TabIndex = 2;
            // 
            // dgvBuyOrders
            // 
            this.dgvBuyOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuyOrders.Location = new System.Drawing.Point(6, 235);
            this.dgvBuyOrders.Name = "dgvBuyOrders";
            this.dgvBuyOrders.Size = new System.Drawing.Size(782, 161);
            this.dgvBuyOrders.TabIndex = 3;
            // 
            // ddlRegion
            // 
            this.ddlRegion.FormattingEnabled = true;
            this.ddlRegion.Location = new System.Drawing.Point(529, 10);
            this.ddlRegion.Name = "ddlRegion";
            this.ddlRegion.Size = new System.Drawing.Size(183, 21);
            this.ddlRegion.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sell Orders";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Buy Orders";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 410);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlRegion);
            this.Controls.Add(this.dgvBuyOrders);
            this.Controls.Add(this.dgvSellOrders);
            this.Controls.Add(this.txtSearchTerm);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Eve Market Order Browser";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSellOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.DataGridView dgvSellOrders;
        private System.Windows.Forms.DataGridView dgvBuyOrders;
        private System.Windows.Forms.ComboBox ddlRegion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

