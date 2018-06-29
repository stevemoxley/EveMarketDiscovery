using EveSSO.Wallet.Journal;
using EveSSO.Wallet.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class JournalEntryDAO
    {
        public void Add(JournalEntry journalEntry)
        {
            try
            {

                using (var db = new DataContext())
                {
                    db.JournalEntries.Add(journalEntry);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(JournalEntry journalEntry)
        {
            try
            {

                using (var db = new DataContext())
                {
                    db.JournalEntries.Remove(journalEntry);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(JournalEntry journalEntry)
        {
            try
            {

                using (var db = new DataContext())
                {
                    db.Entry(journalEntry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JournalEntry> GetJournalEntries()
        {
            List<JournalEntry> result = new List<JournalEntry>();

            try
            {

                using (var db = new DataContext())
                {
                    result = db.JournalEntries.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public JournalEntry GetJournalEntry(long id)
        {
            try
            {

                using (var db = new DataContext())
                {
                    return db.JournalEntries.Where(t => t.id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
