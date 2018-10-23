using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
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
        private List<Products> ListProd = Products.GetProducts();
        private FlowLayoutPanel FlowLayout = Products.ProductPanelCreation();
        private List<string> DiscountList = DiscountCodes.ReadCodes();
        private static DataGridView ShoppingCartGridView = new DataGridView
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
        private TableLayoutPanel ButtonLayout = new TableLayoutPanel { ColumnCount = 1, Dock = DockStyle.Fill, AutoSize = true };
        private TextBox DiscountCode = new TextBox { Text = "Discount Code", Font = new Font("San Serif", 15f), Dock = DockStyle.Bottom, ForeColor = SystemColors.InactiveCaption };
        private static int CartSummary;
        private static Label CartPriceSummary = new Label { Text = String.Format("Total Cost {0:C0}", CartSummary), Font = new Font("San serif", 10F, FontStyle.Bold), ForeColor = Color.Red, Anchor = AnchorStyles.Bottom, Dock = DockStyle.Bottom };
        public static string GetSetSummary
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
            Cart.GetSavedCartItems();
            ShoppingCartGridView.Rows.Clear();
            PrintToCartDataGrid();
            foreach (var item in Cart.CartItems)
            {
                if (item.Amount > 1)
                {
                    for (int i = 0; i < item.Amount; i++)
                    {
                        CartSummary += item.Product.Price;
                    }
                }
                else
                {
                    CartSummary += item.Product.Price;
                }
            }
            GetSetSummary = CartPriceSummary.Text;

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
            FormClosing += SaveCartWhenExit;
        }
        private void SaveCartWhenExit(object sender, FormClosingEventArgs e)
        {
            using (TextWriter tw = new StreamWriter(@"C:\Windows\Temp\SavedCart.csv"))
            {
                foreach (var item in Cart.CartItems)
                {
                    tw.WriteLine(string.Format("{0};{1};{2};{3};{4}", item.Product.Name, item.Product.Description, item.Product.Price, item.Product.Image, item.Amount.ToString()));
                }
            }
        }
        private void ClearCartClick(object sender, EventArgs e)
        {
            ShoppingCartGridView.Rows.Clear();
            Cart.CartItems.Clear();
            CartSummary = 0;
            GetSetSummary = CartPriceSummary.Text;
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
            if (SelectedRow >= 0 && Cart.CartItems.Count > 0 && SelectedRow < Cart.CartItems.Count)
            {   
                DataGridViewRow removedItem = ShoppingCartGridView.Rows[SelectedRow];
                string name = removedItem.Cells[0].Value.ToString();
                var remove = Cart.CartItems.First(x => x.Product.Name == name);
                var index = Cart.CartItems.IndexOf(remove);
                if (remove.Amount > 1)
                {
                    Cart.CartItems[index].Amount--;
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
                    CartSummary -= Cart.CartItems[index].Product.Price;
                    GetSetSummary = CartPriceSummary.Text;
                }
                else
                {
                    CartSummary -= Cart.CartItems[index].Product.Price;
                    Cart.CartItems.Remove(remove);
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
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
        public static void AddToCartClick(object sender, EventArgs e)
        {
            var add = (Products)((Button)sender).Tag;
            Cart cartItem = new Cart
            {
                Product = add,
                Amount = 1
            };
            CartSummary += add.Price;
            var obj = Cart.CartItems.FirstOrDefault(x => x.Product.Name == add.Name);
            if (obj != null)
            {
                obj.Amount++;
                var index = Cart.CartItems.IndexOf(obj);
                Cart.CartItems[index] = obj;
            }
            else
            {
                Cart.CartItems.Add(cartItem);
            }
            ShoppingCartGridView.Rows.Clear();
            PrintToCartDataGrid();
            GetSetSummary = CartPriceSummary.Text;
        }
        private static void PrintToCartDataGrid()
        {
            foreach (var item in Cart.CartItems)
            {
                object[] row = new object[3];
                row[0] = item.Product.Name;
                row[1] = item.Product.Price;
                row[2] = item.Amount;
                ShoppingCartGridView.Rows.Add(row);
            }
        }
    }
}
