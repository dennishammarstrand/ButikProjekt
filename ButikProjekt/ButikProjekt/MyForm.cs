using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ButikProjekt
{
    class MyForm : Form
    {
        private TableLayoutPanel MainLayout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2
        };
        private FlowLayoutPanel FlowLayout = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoSize = true, AutoScroll = true };
        private List<Products> ListProd = Products.GetProducts();
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
            AllowUserToResizeRows = false,
            Dock = DockStyle.Fill
        };
        private Button Remove = new Button { Font = new Font("San Serif", 15f), Text = "Remove from cart", AutoSize = true, Anchor = AnchorStyles.Right };
        private Button Buy = new Button { Font = new Font("San Serif", 15f), Text = "Buy", AutoSize = true, Anchor = AnchorStyles.Left };
        private int SelectedRow;
        private Dictionary<Products, int> cartItems = new Dictionary<Products, int>();
        private TableLayoutPanel ButtonLayout = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true };

        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            Icon = new Icon("MainFormIcon.ico");

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));

            Controls.Add(MainLayout);
            MainLayout.Controls.Add(FlowLayout);
            MainLayout.SetRowSpan(FlowLayout, 2);
            MainLayout.Controls.Add(ShoppingCartGridView);
            MainLayout.Controls.Add(ButtonLayout, 1, 1);
            ButtonLayout.Controls.Add(Remove);
            ButtonLayout.Controls.Add(Buy);

            foreach (Products item in ListProd)
            {
                TableLayoutPanel newItem = new TableLayoutPanel()
                {
                    ColumnCount = 2,
                    Size = new Size(210, 350),
                    BackColor = Color.White

                };
                FlowLayout.Controls.Add(newItem);
                PictureBox ProdImage = new PictureBox { ImageLocation = item.Image, Size = new Size(200, 200), SizeMode = PictureBoxSizeMode.Zoom };
                newItem.Controls.Add(ProdImage);
                newItem.SetColumnSpan(ProdImage, 2);
                Label ProdName = new Label { Text = item.Name, Font = new Font("San serif", 10F) };
                newItem.Controls.Add(ProdName);
                Label prodPrice = new Label { Text = item.Price.ToString() + " kr", Font = new Font("San serif", 10F), ForeColor = Color.Red, TextAlign = ContentAlignment.TopRight };
                newItem.Controls.Add(prodPrice);
                Label description = new Label { Text = item.Description, Dock = DockStyle.Fill, Size = new Size(200, 65) };
                newItem.Controls.Add(description);
                newItem.SetColumnSpan(description, 2);
                Button addButton = new Button { Text = "Add to cart", AutoSize = true, Dock = DockStyle.Top, Font = new Font("San serif", 12F), Tag = item };
                newItem.SetColumnSpan(addButton, 2);
                newItem.Controls.Add(addButton);
                addButton.Click += AddToCartClick;
            }

            ShoppingCartGridView.Columns[0].Name = "Product";
            ShoppingCartGridView.Columns[1].Name = "Price";
            ShoppingCartGridView.Columns[2].Name = "Quantity";
            ShoppingCartGridView.Columns[0].Width = 150;


            ShoppingCartGridView.CellClick += DataGridCellClick;
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
            var add = (Products)((Button)sender).Tag;
            
            if (cartItems.ContainsKey(add))
            {
                cartItems[add] = cartItems[add] + 1;
            }
            else
            {
                cartItems.Add(add, 1);
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
