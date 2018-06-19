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
        }
    }
}
