using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Globalization;

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
        private List<string> DiscountList = DiscountCodes.ReadCodes();
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
        private Button Remove = new Button { Font = new Font("San Serif", 15f), Text = "Remove from cart", AutoSize = true, Dock = DockStyle.Top };
        private Button Buy = new Button { Font = new Font("San Serif", 15f), Text = "Buy", AutoSize = true, Dock = DockStyle.Top };
        private Button ClearCart = new Button { Font = new Font("San Serif", 15f), Text = "Clear cart", AutoSize = true, Dock = DockStyle.Top };
        private int SelectedRow;
        private Dictionary<Products, int> cartItems = new Dictionary<Products, int>();
        private TableLayoutPanel ButtonLayout = new TableLayoutPanel { ColumnCount = 1, Dock = DockStyle.Fill, AutoSize = true };
        private TextBox DiscountCode = new TextBox { Text = "Discount Code", Font = new Font("San Serif", 15f), Dock = DockStyle.Bottom, ForeColor = SystemColors.InactiveCaption };
        private static int CartSummary;
        private Label CartPriceSummary = new Label { Text = String.Format("Total Cost {0:C0}", CartSummary), Font = new Font("San serif", 10F, FontStyle.Bold), ForeColor = Color.Red, Anchor = AnchorStyles.Bottom, Dock = DockStyle.Bottom };
        public string GetSetSummary
        {
            get
            {
                return CartPriceSummary.Text;
            }
            set
            {
                CartPriceSummary.Text = String.Format("Total Cost {0:C0}", CartSummary);
            }
        }

        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            Icon = new Icon("MainFormIcon.ico");

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            ButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Controls.Add(MainLayout);
            MainLayout.Controls.Add(FlowLayout);
            MainLayout.SetRowSpan(FlowLayout, 3);
            MainLayout.Controls.Add(ShoppingCartGridView);
            MainLayout.Controls.Add(CartPriceSummary, 1, 1);
            MainLayout.Controls.Add(ButtonLayout, 1, 2);
            ButtonLayout.Controls.Add(Remove);
            ButtonLayout.Controls.Add(ClearCart);
            ButtonLayout.Controls.Add(Buy);
            ButtonLayout.Controls.Add(DiscountCode);

            foreach (Products item in ListProd)
            {
                TableLayoutPanel newItem = new TableLayoutPanel()
                {
                    ColumnCount = 2,
                    Size = new Size(210, 340),
                    BackColor = Color.White
                };
                FlowLayout.Controls.Add(newItem);
                PictureBox ProdImage = new PictureBox { ImageLocation = item.Image, Size = new Size(200, 200), SizeMode = PictureBoxSizeMode.Zoom };
                newItem.Controls.Add(ProdImage);
                newItem.SetColumnSpan(ProdImage, 2);
                Label ProdName = new Label { Text = item.Name, Font = new Font("San serif", 10F) };
                newItem.Controls.Add(ProdName);
                Label prodPrice = new Label { Text = String.Format("{0:C0}", item.Price), Font = new Font("San serif", 10F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.TopRight };
                newItem.Controls.Add(prodPrice);
                Label description = new Label { Text = item.Description, Dock = DockStyle.Fill, Size = new Size(200, 65), AutoEllipsis = true };
                newItem.Controls.Add(description);
                newItem.SetColumnSpan(description, 2);
                Button addButton = new Button { Text = "Add to cart", AutoSize = true, Dock = DockStyle.Top, Font = new Font("San serif", 12F), BackColor = Color.ForestGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Tag = item, Margin = new Padding(10,0,10,0) };
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
            Buy.Click += BuyButtonClickEvent;
            DiscountCode.Enter += TextBoxEnter;
            DiscountCode.Leave += TextBoxLeave;
            DiscountCode.KeyDown += DiscountCode_KeyDown;
            ClearCart.Click += ClearCartClick;
        }
        private void ClearCartClick(object sender, EventArgs e)
        {
            ShoppingCartGridView.Rows.Clear();
            cartItems.Clear();
            CartSummary = 0;
            GetSetSummary = CartPriceSummary.Text;
            PrintToCartDataGrid();
        }
        private void DiscountCode_KeyDown(object sender, KeyEventArgs e)
        {
            int count = ShoppingCartGridView.Rows.Count;
            object[] discountRow = new object[2];
            if (e.KeyCode == Keys.Enter)
            {
                if (DiscountList.Contains(DiscountCode.Text))
                {
                    if (ShoppingCartGridView.Rows[count - 1].Cells[0].Value.ToString() != DiscountCode.Text)
                    {
                        discountRow[0] = DiscountCode.Text;
                        discountRow[1] = 1000;

                        CartSummary -= (int)discountRow[1];
                        GetSetSummary = CartPriceSummary.Text;

                        ShoppingCartGridView.Rows.Add(discountRow);

                        count = ShoppingCartGridView.Rows.Count;
                        ShoppingCartGridView.Rows[count - 1].Cells[1].Style.ForeColor = Color.Red;
                    }
                }
            }
        }
        private void BuyButtonClickEvent(object sender, EventArgs e)
        {
            MessageBox.Show("Hej");
        }
        private void TextBoxLeave(object sender, EventArgs e)
        {
            if (DiscountCode.Text == "")
            {
                DiscountCode.Text = "Discount Code";
                DiscountCode.ForeColor = SystemColors.InactiveCaption;
            }
        }
        private void TextBoxEnter(object sender, EventArgs e)
        {
            if (DiscountCode.Text == "Discount Code")
            {
                DiscountCode.Text = "";
                DiscountCode.ForeColor = Color.Black;
            }
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
                    CartSummary -= remove.Key.Price;
                    GetSetSummary = CartPriceSummary.Text;
                }
                else
                {
                    cartItems.Remove(remove.Key);
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
                    CartSummary -= remove.Key.Price;
                    GetSetSummary = CartPriceSummary.Text;
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
            CartSummary += add.Price;
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
            GetSetSummary = CartPriceSummary.Text;
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
