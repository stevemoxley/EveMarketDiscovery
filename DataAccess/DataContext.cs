using EveSSO.Wallet.Transactions;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataContext")
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<DataContext>(modelBuilder);
            Database.SetInitializer(sqliteConectionInitializer);
        }

    }
}
