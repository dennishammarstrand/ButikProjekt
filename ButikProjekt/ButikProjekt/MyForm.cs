using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ButikProjekt
{
    class MyForm : Form
    {
        private TableLayoutPanel MainLayout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };
        private DataGridView ShopGridView = new DataGridView
        {
            Font = new Font("San Serif", 9f),
            ReadOnly = true,
            ColumnCount = 2,
            CellBorderStyle = DataGridViewCellBorderStyle.None,
            AutoSize = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            BackgroundColor = SystemColors.Control,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false
        };
        private DataGridView ShoppingCartGridView = new DataGridView
        {
            Font = new Font("San Serif", 9f),
            ReadOnly = true,
            ColumnCount = 3,
            CellBorderStyle = DataGridViewCellBorderStyle.None,
            AutoSize = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            BackgroundColor = SystemColors.Control,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false
        };
        private Button Add = new Button { Font = new Font("San Serif", 15f), Text = "Add", AutoSize = true, Dock = DockStyle.Top };
        private Button Remove = new Button { Font = new Font("San Serif", 15f), Text = "Remove", AutoSize = true, Dock = DockStyle.Top };
        private Button Buy = new Button { Font = new Font("San Serif", 15f), Text = "Buy", AutoSize = true, Dock = DockStyle.Top };
        private int SelectedRow;
        private List<Products> listProd = Products.GetProducts();
        private Dictionary<Products, int> cartItems = new Dictionary<Products, int>();


        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            Controls.Add(MainLayout);
            MainLayout.Controls.Add(ShopGridView);
            MainLayout.Controls.Add(ShoppingCartGridView, 2, 0);
            MainLayout.Controls.Add(Add, 1, 4);
            MainLayout.Controls.Add(Remove, 1, 5);
            MainLayout.Controls.Add(Buy, 1, 6);

            ShopGridView.Columns[0].Name = "Product";
            ShopGridView.Columns[1].Name = "Price";
            ShopGridView.Columns[0].Width = 150;

            ShoppingCartGridView.Columns[0].Name = "Product";
            ShoppingCartGridView.Columns[1].Name = "Price";
            ShoppingCartGridView.Columns[2].Name = "Quantity";
            ShoppingCartGridView.Columns[0].Width = 150;


            foreach (Products item in listProd)
            {
                object[] prodNameAndPrice = new object[2];
                prodNameAndPrice[0] = item.Name;
                prodNameAndPrice[1] = item.Price;
                ShopGridView.Rows.Add(prodNameAndPrice);
            }

            ShopGridView.CellClick += DataGridCellClick;
            ShoppingCartGridView.CellClick += DataGridCellClick;
            Add.Click += AddToCartClick;
            Remove.Click += RemoveFromCartClick;
        }
        private void RemoveFromCartClick(object sender, EventArgs e)
        {
            if (SelectedRow >= 0 && cartItems.Count > 0 && SelectedRow < cartItems.Count)
            {   
                DataGridViewRow removedItem = ShoppingCartGridView.Rows[SelectedRow];
                string name = removedItem.Cells[0].Value.ToString();
                var remove = cartItems.First(x => x.Key.Name == name);
                if (remove.Value > 1)
                {
                    cartItems[remove.Key]--;
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
                }
                else
                {
                    cartItems.Remove(remove.Key);
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
                }
            }
        }
        private void DataGridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedRow = e.RowIndex;
            }
        }
        private void AddToCartClick(object sender, EventArgs e)
        {
            DataGridViewRow addedItem = ShopGridView.Rows[SelectedRow];
            string name = addedItem.Cells[0].Value.ToString();
            foreach (Products p in listProd)
            {
                if (p.Name == name)
                {
                    if (cartItems.ContainsKey(p))
                    {
                        cartItems[p] = cartItems[p] + 1;
                    }
                    else
                    {
                        cartItems.Add(p, 1);
                    }
                }
            }
            ShoppingCartGridView.Rows.Clear();
            PrintToCartDataGrid();
        }
        private void PrintToCartDataGrid()
        {
            foreach (KeyValuePair<Products, int> pair in cartItems)
            {
                object[] selectedValues = new object[3];
                selectedValues[0] = pair.Key.Name;
                selectedValues[1] = pair.Key.Price;
                selectedValues[2] = pair.Value;
                ShoppingCartGridView.Rows.Add(selectedValues);
            }
        }
    }
}
