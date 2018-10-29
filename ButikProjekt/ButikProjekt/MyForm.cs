﻿using System;
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
        //Main form variable declarations
        public static TableLayoutPanel MainLayout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2
        };
        public static DataGridView ShoppingCartGridView = new DataGridView
        {
            Font = new Font("San Serif", 9f),
            ReadOnly = true,
            ColumnCount = 3,
            CellBorderStyle = DataGridViewCellBorderStyle.None,
            AutoSize = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false,
            Dock = DockStyle.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        private Button RemoveButton = new Button { Font = new Font("San Serif", 15f), Text = "Remove from cart", AutoSize = true, Dock = DockStyle.Top };
        private static Button BuyButton = new Button { Enabled = false, Font = new Font("San Serif", 15f), Text = "Buy", AutoSize = true, Dock = DockStyle.Top };
        private Button ClearCartButton = new Button { Font = new Font("San Serif", 15f), Text = "Clear cart", AutoSize = true, Dock = DockStyle.Top };
        private int SelectedRow { get; set; }
        private TableLayoutPanel ButtonLayout = new TableLayoutPanel { ColumnCount = 1, Dock = DockStyle.Fill, AutoSize = true };
        public static TextBox DiscountCodeTextBox = new TextBox { Text = "Discount Code, press Enter", Font = new Font("San Serif", 15f), Dock = DockStyle.Bottom, ForeColor = SystemColors.InactiveCaption };
        public static double CartSummaryValue { get; set; }
        public static Label CartPriceSummaryLabel = new Label { Text = String.Format("Total Cost {0:C0}", CartSummaryValue), Font = new Font("San serif", 10F, FontStyle.Bold), ForeColor = Color.Red, Anchor = AnchorStyles.Bottom, Dock = DockStyle.Bottom };
        public static string PriceSummaryTextFormatting
        {
            get
            {
                return CartPriceSummaryLabel.Text;
            }
            set
            {
                CartPriceSummaryLabel.Text = String.Format("Total Cost {0:C0}", CartSummaryValue);
            }
        }

        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Camera store";
            Icon = new Icon("MainFormIcon.ico");
            Product.AddProductsToList();
            Product.ProductPanelCreation();
            Cart.GetSavedCartItems();
            DiscountCode.AddDiscountCodesToList();
            ShoppingCartGridView.Rows.Clear();
            PrintToCartDataGrid();
            //Updates total cost if you have saved products in your cart since last visit
            foreach (Cart item in Cart.CartItems)
            {
                if (item.Amount > 1)
                {
                    for (int i = 0; i < item.Amount; i++)
                    {
                        CartSummaryValue += item.Product.Price;
                    }
                }
                else
                {
                    CartSummaryValue += item.Product.Price;
                }
            }
            PriceSummaryTextFormatting = CartPriceSummaryLabel.Text;

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            ButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Controls.Add(MainLayout);
            MainLayout.Controls.Add(ShoppingCartGridView);
            MainLayout.Controls.Add(CartPriceSummaryLabel, 1, 1);
            MainLayout.Controls.Add(ButtonLayout, 1, 2);
            ButtonLayout.Controls.Add(RemoveButton);
            ButtonLayout.Controls.Add(ClearCartButton);
            ButtonLayout.Controls.Add(BuyButton);
            ButtonLayout.Controls.Add(DiscountCodeTextBox);

            ShoppingCartGridView.Columns[0].Name = "Product";
            ShoppingCartGridView.Columns[1].Name = "Price";
            ShoppingCartGridView.Columns[2].Name = "Quantity";
            ShoppingCartGridView.Columns[0].Width = 150;
            
            ShoppingCartGridView.CellClick += DataGridCellClick;
            RemoveButton.Click += RemoveFromCartClick;
            BuyButton.Click += BuyButtonClick;
            DiscountCodeTextBox.Enter += DiscountCodeTextBoxEnter;
            DiscountCodeTextBox.Leave += DiscountCodeTextBoxLeave;
            DiscountCodeTextBox.KeyDown += DiscountCodeKeyDown;
            ClearCartButton.Click += ClearCartClick;
            FormClosing += SaveCartWhenExit;
            Load += FormOpenEventHandler;
        }
        //Disable buy button on form load if cart is empty
        private void FormOpenEventHandler(object sender, EventArgs e)
        {
            if (!Cart.IsCartListEmpty())
            {
                BuyButton.Enabled = true;
            }
        }
        //Saves our cart to a csv file
        private void SaveCartWhenExit(object sender, FormClosingEventArgs e)
        {
            if (!Cart.IsCartListEmpty())
            {
                using (TextWriter tw = new StreamWriter(@"C:\Windows\Temp\SavedCart.csv"))
                {
                    foreach (Cart item in Cart.CartItems)
                    {
                        tw.WriteLine(string.Format("{0};{1};{2};{3};{4}", item.Product.Name, item.Product.Description, item.Product.Price, item.Product.Image, item.Amount.ToString()));
                    }
                }
            }
        }
        //Clears the cart
        private void ClearCartClick(object sender, EventArgs e)
        {
            ClearCart();
        }
        //Clears cart a resets the values of everything to default
        private void ClearCart()
        {
            ShoppingCartGridView.Rows.Clear();
            Cart.CartItems.Clear();
            DiscountCodeTextBox.Enabled = true;
            DiscountCodeTextBox.Text = "Discount Code, press Enter";
            DiscountCodeTextBox.ForeColor = SystemColors.InactiveCaption;
            CartSummaryValue = 0;
            PriceSummaryTextFormatting = CartPriceSummaryLabel.Text;
            BuyButton.Enabled = false;
            File.Delete(@"C:\Windows\Temp\SavedCart.csv");
        }
        //Checks if you've pressed enter after entering a discount code and calls discount code methods
        private void DiscountCodeKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    DiscountCode.AddDiscountCodeToCart();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //
        private void BuyButtonClick(object sender, EventArgs e)
        {
            if (ShoppingCartGridView.RowCount > 0)
            {
                ShoppingCartGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                ShoppingCartGridView.SelectAll();
                Clipboard.SetDataObject(ShoppingCartGridView.GetClipboardContent());
                string receipt = String.Format("This is your receipt \r\n\n{0}\r\n\n{1}", Clipboard.GetText(), CartPriceSummaryLabel.Text);
                string caption = "Receipt";

                MessageBox.Show(receipt, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                ClearCart();
            }
        }
        //Changes the description text of the Discount textbox when leaving the textbox
        private void DiscountCodeTextBoxLeave(object sender, EventArgs e)
        {
            if (DiscountCodeTextBox.Text == "")
            {
                DiscountCodeTextBox.Text = "Discount Code, press Enter";
                DiscountCodeTextBox.ForeColor = SystemColors.InactiveCaption;
            }
        }
        //Changes the description text of the Discount textbox when entering the textbox 
        private void DiscountCodeTextBoxEnter(object sender, EventArgs e)
        {
            if (DiscountCodeTextBox.Text == "Discount Code, press Enter")
            {
                DiscountCodeTextBox.Text = "";
                DiscountCodeTextBox.ForeColor = Color.Black;
            }
        }
        //Handles the remove button and checks if the product exists in the cart and at what
        //index in the cart. Given what item you've clicked in the datagridview.
        private void RemoveFromCartClick(object sender, EventArgs e)
        {
            if (SelectedRow >= 0 && !Cart.IsCartListEmpty() && SelectedRow < Cart.CartItems.Count)
            {   
                DataGridViewRow selectedItem = ShoppingCartGridView.Rows[SelectedRow];
                string productName = selectedItem.Cells[0].Value.ToString();
                var itemToRemove = Cart.CartItems.First(x => x.Product.Name == productName);
                var index = Cart.CartItems.IndexOf(itemToRemove);
                if (itemToRemove.Amount > 1)
                {
                    Cart.CartItems[index].Amount--;
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
                    CartSummaryValue -= Cart.CartItems[index].Product.Price;
                    PriceSummaryTextFormatting = CartPriceSummaryLabel.Text;
                }
                else
                {
                    CartSummaryValue -= Cart.CartItems[index].Product.Price;
                    Cart.CartItems.Remove(itemToRemove);
                    ShoppingCartGridView.Rows.Clear();
                    PrintToCartDataGrid();
                    PriceSummaryTextFormatting = CartPriceSummaryLabel.Text;
                }
            }
            if(Cart.IsCartListEmpty())
            {
                BuyButton.Enabled = false;
            }
        }
        //Sets a varible to a index value given what row in the datagridview you've clicked
        private void DataGridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedRow = e.RowIndex;
            }
        }
        //Adds a product to cart and checks whether that product allready exist in the 
        //cart and updates the datagridview and total cost.
        public static void AddToCartClick(object sender, EventArgs e)
        {
            var productToAdd = ((AddButton)sender).Product;
            Cart cartItem = new Cart
            {
                Product = productToAdd,
                Amount = 1
            };
            CartSummaryValue += productToAdd.Price;
            var objectInCart = Cart.CartItems.FirstOrDefault(x => x.Product == productToAdd);
            if (objectInCart != null)
            {
                objectInCart.Amount++;
                var index = Cart.CartItems.IndexOf(objectInCart);
                Cart.CartItems[index] = objectInCart;
            }
            else
            {
                Cart.CartItems.Add(cartItem);
            }
            ShoppingCartGridView.Rows.Clear();
            PrintToCartDataGrid();
            PriceSummaryTextFormatting = CartPriceSummaryLabel.Text;
            BuyButton.Enabled = true;
        }
        //Populats our datagridview with our list of cart products
        private static void PrintToCartDataGrid()
        {
            foreach (Cart item in Cart.CartItems)
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
