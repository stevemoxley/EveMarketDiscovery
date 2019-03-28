using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveAccountant.Common;
using EveSSO.Wallet.Transactions;

namespace EveAccountant.Services
{
    public static class AccountingProvider
    {

        public static AccountingReport LoadReport()
        {
            AccountingReport result = new AccountingReport();
            var transactionDao = new TransactionDAO();
            var transactions = transactionDao.GetTransactions();

            foreach (var transaction in transactions)
            {
                string name = "";
                if (ItemProvider.TryGetItem(transaction.type_id, out name))
                {
                    transaction.type = name;
                }
            }

            result.Transactions = transactions;

            var itemGroups = transactions.GroupBy(t => t.type_id);

            foreach (var itemGroup in itemGroups)
            {

                string item = string.Empty;
                if (!ItemProvider.TryGetItem(itemGroup.FirstOrDefault().type_id, out item))
                {
                    continue;
                }
                var buyOrders = itemGroup.Where(i => i.is_buy).ToList();
                var sellOrders = itemGroup.Where(i => !i.is_buy).ToList();

                var quantitySold = sellOrders.Sum(s => s.quantity);
                var quantityBought = buyOrders.Sum(s => s.quantity);

                decimal revenue = (decimal)sellOrders.Sum(s => s.quantity * s.unit_price);
                decimal cost = 0;
                decimal unsold = 0;

                var totalLeft = quantitySold;
                foreach (var buyOrder in buyOrders)
                {
                    if (totalLeft >= buyOrder.quantity)
                    {
                        totalLeft -= buyOrder.quantity;
                        cost += (decimal)(buyOrder.unit_price * buyOrder.quantity);
                    }
                    else
                    {
                        cost += (decimal)(totalLeft * buyOrder.unit_price);
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
                            unsold += (decimal)(buyOrder.quantity * buyOrder.unit_price);
                        }
                        else
                        {
                            unsoldQuantity = 0;
                            unsold += (decimal)(unsoldQuantity * buyOrder.unit_price);
                        }
                    }
                }

                var transaction = new ItemGroup
                {
                    Item = item,
                    QuantityBought = quantityBought,
                    QuantitySold = quantitySold,
                    Revenue = revenue,
                    Cost = cost,
                    UnsoldValue = unsold,
                };

                result.ItemGroups.Add(transaction);
            }

            return result;
        }

    }


    public class AccountingReport
    {
        public decimal TotalRevenue
        {
            get
            {
                return ItemGroups.Sum(t => t.Revenue);
            }
        }

        public decimal TotalCost
        {
            get
            {
                return ItemGroups.Sum(t => t.Cost);
            }
        }

        public decimal TotalProfit
        {
            get
            {
                return ItemGroups.Sum(t => t.Profit);
            }
        }

        public decimal ProfitMargin
        {
            get
            {
                return Math.Round((TotalProfit / TotalCost) * 100, 2);
            }
        }

        public decimal TotalUnsoldInventory
        {
            get
            {
                return ItemGroups.Sum(t => t.UnsoldValue);
            }
        }

        public List<ItemGroup> ItemGroups { get; set; } = new List<ItemGroup>();

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }


    public class ItemGroup
    {
        public string Item { get; set; }

        public long QuantityBought { get; set; }

        public long QuantitySold { get; set; }

        public decimal UnsoldValue { get; set; }

        public decimal Revenue { get; set; }

        public decimal Cost { get; set; }

        public decimal Profit
        {
            get
            {
                return Revenue - Cost;
            }

        }

        public decimal ProfitMargin
        {
            get
            {
                return Math.Round((Profit / Cost) * 100, 2);
            }
        }

    }
}
