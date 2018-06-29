using EveSSO.Wallet.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionDAO
    {
        public void AddTransaction(Transaction transaction)
        {
            try
            {

                using (var db = new DataContext())
                {
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveTransaction(Transaction transaction)
        {
            try
            {

                using (var db = new DataContext())
                {
                    db.Transactions.Remove(transaction);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTransaction(Transaction transaction)
        {
            try
            {

                using (var db = new DataContext())
                {
                    db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Transaction> GetTransactions()
        {
            List<Transaction> result = new List<Transaction>();

            try
            {

                using (var db = new DataContext())
                {
                    result = db.Transactions.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Transaction GetTransaction(long transactionId)
        {
            try
            {

                using (var db = new DataContext())
                {
                    return db.Transactions.Where(t => t.transaction_id == transactionId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
