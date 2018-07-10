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
            dt.Columns[3].DataType = typeof(string);
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
                if (journalEntry.ref_type == "transaction_tax")
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
            float totalUnsoldInventory = 0;

            foreach (var itemGroup in itemGroups)
            {
                var item = ItemProvider.Items()[itemGroup.FirstOrDefault().type_id];
                var buyOrders = itemGroup.Where(i => i.is_buy).ToList();
                var sellOrders = itemGroup.Where(i => !i.is_buy).ToList();

                var quantitySold = sellOrders.Sum(s => s.quantity);
                var quantityBought = buyOrders.Sum(s => s.quantity);

                float revenue = sellOrders.Sum(s => s.quantity * s.unit_price);
                float cost = 0;
                float profit = 0; //Profit would be the number I actually sold times the price of those things sold
                float unsold = 0;

                var totalLeft = quantitySold;
                foreach (var buyOrder in buyOrders)
                {
                    if (totalLeft >= buyOrder.quantity)
                    {
                        totalLeft -= buyOrder.quantity;
                        cost += buyOrder.unit_price * buyOrder.quantity;
                    }
                    else
                    {
                        cost += totalLeft * buyOrder.unit_price;
                        totalLeft = 0;
                        break;
                    }
                }

                var unsoldQuantity = quantityBought - quantitySold;
                if (unsoldQuantity > 0)
                {
                    foreach (var buyOrder in buyOrders)
                    {
                        if (unsoldQuantity >= buyOrder.quantity)
                        {
                            unsoldQuantity -= buyOrder.quantity;
                            unsold += buyOrder.quantity * buyOrder.unit_price;
                        }
                        else
                        {
                            unsoldQuantity = 0;
                            unsold += unsoldQuantity * buyOrder.unit_price;
                        }
                    }
                }

                totalUnsoldInventory += unsold;

                profit = revenue - cost;

                var profitMargin = Math.Round((profit / cost) * 100, 2);

                totalProfit += profit;
                var row = dataTable.NewRow();
                row[0] = item;
                row[1] = quantityBought;
                row[2] = quantitySold;
                row[3] = profit.ToString("C");
                row[4] = profitMargin;

                dataTable.Rows.Add(row);
            }

            dataGridView1.DataSource = dataTable;
            lblTotalProfit.Text = $"Total Profit: { totalProfit.ToString("C") }";
            lblUnsoldInventory.Text = $"Unsold Inventory: { totalUnsoldInventory.ToString("C") }";
        }

    }
}
