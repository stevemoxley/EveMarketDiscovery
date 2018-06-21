using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EveSSO.Wallet.Journal
{
    public static class JournalProvider
    {

        public static JournalEntry[] GetCharacterJournal(string characterId, string accessToken)
        {
            try
            {
                Console.WriteLine($"Downloading journal {characterId}");
                var webClient = new WebClient();
                string journalEntryJson = webClient.DownloadString($"https://esi.evetech.net/latest/characters/{ characterId }/wallet/journal/?token={ accessToken }");
                return JsonConvert.DeserializeObject<JournalEntry[]>(journalEntryJson);
            }
            catch (Exception ex)
            {
                return new JournalEntry[0];
            }
        }
    }
}
