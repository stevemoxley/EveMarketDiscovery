using Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveAccountant
{
    public partial class ProfitAndLoss : Form
    {
        public ProfitAndLoss()
        {
            InitializeComponent();
            SetUpDataGridView();
        }

        private void SetUpDataGridView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Item");
            dt.Columns.Add("Total Purchased");
            dt.Columns.Add("Total Sold");
            dt.Columns.Add("Total Profit");
            dt.Columns.Add("Profit Margin");

            dt.Columns[0].DataType = typeof(string);
            dt.Columns[1].DataType = typeof(int);
            dt.Columns[2].DataType = typeof(int);
            dt.Columns[3].DataType = typeof(float);
            dt.Columns[4].DataType = typeof(float);
            dataGridView1.DataSource = dt;
        }

        private void ProfitAndLoss_Load(object sender, EventArgs e)
        {
            LoadTransactions();
            LoadJournalEntries();
        }
        
        private void LoadJournalEntries()
        {
            var journalEntryDao = new JournalEntryDAO();
            var journalEntries = journalEntryDao.GetJournalEntries();

            float totalFees = 0;

            foreach (var journalEntry in journalEntries)
            {
                if(journalEntry.ref_type == "transaction_tax")
                {
                    totalFees += journalEntry.amount;
                }
            }

            lblFees.Text = $"Total Fees: { totalFees.ToString("C") }";
        }
            
        private void LoadTransactions()
        {
            var transactionDao = new TransactionDAO();
            var transactions = transactionDao.GetTransactions();

            var itemGroups = transactions.GroupBy(t => t.type_id);

            var dataTable = dataGridView1.DataSource as DataTable;

            float totalProfit = 0;

            foreach (var itemGroup in itemGroups)
            {
                var item = ItemProvider.Items()[itemGroup.FirstOrDefault().type_id];
                var buyOrders = itemGroup.Where(i => i.is_buy).ToList();
                var sellOrders = itemGroup.Where(i => !i.is_buy).ToList();

                var sumOfBuyOrders = buyOrders.Sum(s => s.quantity * s.unit_price);
                var sumOfSellorders = sellOrders.Sum(s => s.quantity * s.unit_price);

                var totalSold = sellOrders.Sum(s => s.quantity);
                var totalBought = buyOrders.Sum(s => s.quantity);

                var profit = sumOfSellorders - sumOfBuyOrders;
                totalProfit += profit;

                var profitMargin = Math.Round((((sumOfSellorders - sumOfBuyOrders) / sumOfSellorders) * 100), 2);

                var row = dataTable.NewRow();
                row[0] = item;
                row[1] = totalBought;
                row[2] = totalSold;
                row[3] = profit.ToString("#,##0.00");
                row[4] = profitMargin;

                dataTable.Rows.Add(row);
            }

            dataGridView1.DataSource = dataTable;
            lblTotalProfit.Text = $"Total Profit: { totalProfit.ToString("C") }";
        }

    }
}
