using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EveSSO.Wallet.Transactions
{
    public class TransactionsProvider
    {
        public static Transaction[] GetCharacterTransactions(string characterId, string accessToken)
        {
            Console.WriteLine($"Downloading journal {characterId}");
            var webClient = new WebClient();
            string transactionJson = webClient.DownloadString($"https://esi.evetech.net/latest/characters/{ characterId }/wallet/transactions/?token={ accessToken }");
            return JsonConvert.DeserializeObject<Transaction[]>(transactionJson);
        }
    }
}
