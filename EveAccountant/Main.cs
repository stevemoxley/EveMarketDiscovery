using Common;
using DataAccess;
using EveSSO.Wallet.Journal;
using EveSSO.Wallet.Transactions;
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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void authenticateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var authentication = new Authentication();
            authentication.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var journalEntries = JournalProvider.GetCharacterJournal(AuthenticationManager.CharacterInfo.CharacterID, AuthenticationManager.AccessToken.access_token);
            var transactions = TransactionsProvider.GetCharacterTransactions(AuthenticationManager.CharacterInfo.CharacterID, AuthenticationManager.AccessToken.access_token);
            var sellTransactions = transactions.Where( t=> !t.is_buy).ToArray();
            var buyTransactions = transactions.Where(t => t.is_buy).ToArray();

            SaveAllTransactions(transactions);
            LoadDataTable(sellTransactions);
        }

        private void SaveAllTransactions(Transaction[] transactions)
        {
            var transactionDAO = new TransactionDAO();
            foreach (var transaction in transactions)
            {
                //See if it exists
                var existingTransaction = transactionDAO.GetTransaction(transaction.transaction_id);
                if(existingTransaction == null)
                {
                    transactionDAO.AddTransaction(transaction);
                }
            }
        }

        private void LoadDataTable(Transaction[] transactions)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Time");
            dt.Columns.Add("Item");
            dt.Columns.Add("Sale Price");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Total");
            dt.Columns.Add("Profit Margin");

            foreach (var transaction in transactions)
            {
                var row = dt.NewRow();
                row[0] = transaction.date;
                row[1] = ItemProvider.Items()[transaction.type_id];
                row[2] = transaction.unit_price.ToString("N0");
                row[3] = transaction.quantity.ToString("N0");
                row[4] = (transaction.unit_price * transaction.quantity).ToString("N0");
                dt.Rows.Add(row);
            }

            dataGridView1.DataSource = dt;
        }
    }
}
