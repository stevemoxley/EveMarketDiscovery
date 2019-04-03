using EveAccountant.Common;
using EveSSO.Market.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveMarketOrderBrowser
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchTerm.Text.TrimEnd().ToLower();
            var region = (KeyValuePair<long, string>)ddlRegion.SelectedItem;

            var items = ItemProvider.Items().Where(i => i.Value.ToLower().Equals(searchTerm));

            if(items.Count() > 1)
            {
                MessageBox.Show($"Found more than one result for { searchTerm }");
                return;
            }
            else if(items.Count() == 0)
            {
                MessageBox.Show($"No results found for { searchTerm }");
                return;
            }

            var item = items.FirstOrDefault();

            var orders = await MarketOrderProvider.GetItemMarketOrdersAsync(region.Key, item.Key);

            var buyOrders = orders.MarketOrders.Where(o => o.is_buy_order).OrderByDescending(o => o.price).ToList();
            var sellOrders = orders.MarketOrders.Where(o => !o.is_buy_order).OrderBy(o => o.price).ToList();

            BindingList<MarketOrder> buyOrderBindingList = new BindingList<MarketOrder>(buyOrders);
            BindingList<MarketOrder> sellOrderBindingList = new BindingList<MarketOrder>(sellOrders);

            dgvBuyOrders.DataSource = buyOrderBindingList;
            dgvSellOrders.DataSource = sellOrderBindingList;

            dgvBuyOrders.Columns[6].DefaultCellStyle.Format = "c";
            dgvSellOrders.Columns[6].DefaultCellStyle.Format = "c";

        }

        private void Main_Load(object sender, EventArgs e)
        {
            ddlRegion.DataSource = new RegionProvider().RegionNames.ToList();
        }

    }
}
