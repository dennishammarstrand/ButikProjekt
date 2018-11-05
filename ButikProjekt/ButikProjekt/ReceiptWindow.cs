using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ButikProjekt
{
    class ReceiptWindow : Form
    {
        private TableLayoutPanel MainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        public static DataGridView ReceiptDataGridView = new DataGridView
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            Font = new Font("San Serif", 9f),
            ReadOnly = true,
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
        };
        public static double ReceiptSummaryValue;
        private static DateTime TodaysDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        private Label ThankYouLabel = new Label { Text = "", Font = new Font("San serif", 15F, FontStyle.Bold), Anchor = AnchorStyles.Top, AutoSize = true };
        public static TextBox DiscountCodeTextBox = new TextBox { Anchor = AnchorStyles.Right, Width = 250, Text = "Discount Code", Font = new Font("San Serif", 10f), ForeColor = SystemColors.InactiveCaption };
        public static Button ActivateDiscountButton = new Button { Anchor = AnchorStyles.Left, Font = new Font("San Serif", 10f), Text = "Activate Discount", AutoSize = true, Dock = DockStyle.Top };
        public Button BuyButton = new Button { Font = new Font("San Serif", 13f), Text = "Buy", Dock = DockStyle.Top, AutoSize = true };
        private Label PurchaseDateLabel = new Label { Visible = false, Text = TodaysDate.ToString(), Dock = DockStyle.Top, TextAlign = ContentAlignment.TopLeft };
        public static Label TotalPriceLabel = new Label
        {
            Font = new Font("San serif", 10F, FontStyle.Bold),
            ForeColor = Color.Red,
            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.TopLeft,
        };

        public ReceiptWindow()
        {
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon("MainFormIcon.ico");
            Text = "Checkout";
            Size = new Size(500, 350);
            Controls.Add(MainLayout);

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));

            MainLayout.Controls.Add(ThankYouLabel);
            MainLayout.SetColumnSpan(ThankYouLabel, 2);
            MainLayout.Controls.Add(ReceiptDataGridView);
            MainLayout.SetColumnSpan(ReceiptDataGridView, 2);
            MainLayout.Controls.Add(DiscountCodeTextBox);
            MainLayout.Controls.Add(ActivateDiscountButton);
            MainLayout.Controls.Add(BuyButton, 1, 3);
            MainLayout.Controls.Add(PurchaseDateLabel, 0, 4);
            MainLayout.Controls.Add(TotalPriceLabel, 1, 4);

            ReceiptDataGridView.DefaultCellStyle.SelectionBackColor = ReceiptDataGridView.DefaultCellStyle.BackColor;
            ReceiptDataGridView.DefaultCellStyle.SelectionForeColor = ReceiptDataGridView.DefaultCellStyle.ForeColor;

            ReceiptDataGridView.Columns[0].Name = "Name";
            ReceiptDataGridView.Columns[1].Name = "Quantity";
            ReceiptDataGridView.Columns[2].Name = "Price";
            ReceiptDataGridView.Columns[0].Width = 250;

            AddCartToReceipt();
            SetReceiptSummaryValue();

            FormClosing += ClosingReceipt;
            DiscountCodeTextBox.Enter += DiscountCodeTextBoxEnter;
            DiscountCodeTextBox.Leave += DiscountCodeTextBoxLeave;
            ActivateDiscountButton.Click += ActivateDiscountButtonClickHandler;
            BuyButton.Click += BuyButton_Click;
        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            if (BuyButton.Text == "Buy")
            {
                DiscountCodeTextBox.Visible = false;
                ActivateDiscountButton.Visible = false;
                ThankYouLabel.Text = "Thank you for your order!";
                PurchaseDateLabel.Visible = true;
                Text = "Your receipt";
                BuyButton.Text = "Close";
            }
            else if(BuyButton.Text == "Close")
            {
                ShopWindow.ClearCart();
                Application.Exit();
            }
        }

        //Adds the discount code and it's discounted value to the datagridview with formatting and 
        //updates the new total price accordingly.
        public static void AddDiscountCodeToReceipt()
        {
            try
            {
                string[] discountCodeEntered = DiscountCode.DiscountCodeList.First(x => x[0] == DiscountCodeTextBox.Text);
                if (discountCodeEntered != null)
                {
                    object[] discountRow = new object[3];
                    double discountValue = GetValueDiscount(discountCodeEntered);

                    discountRow[0] = DiscountCodeTextBox.Text;
                    discountRow[2] = String.Format(" -{0:C0}", discountValue);

                    ReceiptSummaryValue -= discountValue;
                    TotalPriceLabel.Text = String.Format("Total Cost {0:C0}", ReceiptSummaryValue);

                    ReceiptDataGridView.Rows.Add(discountRow);

                    int rowCount = ReceiptDataGridView.Rows.Count;
                    ReceiptDataGridView.Rows[rowCount - 1].Cells[0].Style.ForeColor = Color.Red;
                    ReceiptDataGridView.Rows[rowCount - 1].Cells[2].Style.ForeColor = Color.Red;
                    ActivateDiscountButton.Enabled = false;
                    DiscountCodeTextBox.Enabled = false;
                }
            }
            catch
            {
                throw new Exception("Discount code not valid");
            }

        }

        //Get each discount codes specific discount reduction
        public static double GetValueDiscount(string[] discountCode)
        {
            if (discountCode[1] == "Value")
            {
                return int.Parse(discountCode[2]);
            }
            else if (discountCode[1] == "2for1")
            {
                List<Cart> canonOrderedByPrice = Cart.CartItems.Where(x => x.Product.Name.Contains("Canon")).OrderBy(z => z.Product.Price).ToList();

                if (canonOrderedByPrice.Count() > 1 || canonOrderedByPrice[0].Amount > 1)
                {
                    return canonOrderedByPrice[0].Product.Price;
                }
                else
                {
                    throw new Exception("Discount not valid! Add another canon!");
                }
            }
            else if (discountCode[1] == "Percent")
            {
                return ReceiptSummaryValue * (int.Parse(discountCode[2]) / 100.0);
            }
            else
            {
                throw new Exception("Discount not valid");
            }
        }

        private void ActivateDiscountButtonClickHandler(object sender, EventArgs e)
        {
            try
            {
                AddDiscountCodeToReceipt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClosingReceipt(object sender, FormClosingEventArgs e)
        {
            if (BuyButton.Visible == false)
            {
                ShopWindow.ClearCart();
                Application.Exit();
            }
            else
            {
                ReceiptDataGridView.Rows.Clear();
                ReceiptDataGridView.Refresh();
                ActivateDiscountButton.Enabled = true;
                DiscountCodeTextBox.Enabled = true;
                DiscountCodeTextBox.Text = "Discount Code";
                DiscountCodeTextBox.ForeColor = SystemColors.InactiveCaption;
                ActivateDiscountButton.Click -= ActivateDiscountButtonClickHandler;
            }
        }
        public void AddCartToReceipt()
        {
            foreach (Cart cartItem in Cart.CartItems)
            {
                object[] row = new object[3];
                row[0] = cartItem.Product.Name;
                row[1] = cartItem.Amount;
                row[2] = String.Format("{0:C0}",cartItem.Product.Price);
                ReceiptDataGridView.Rows.Add(row);
            }
        }
        //Changes the description text of the Discount textbox when leaving the textbox
        private void DiscountCodeTextBoxLeave(object sender, EventArgs e)
        {
            if (DiscountCodeTextBox.Text == "")
            {
                DiscountCodeTextBox.Text = "Discount Code";
                DiscountCodeTextBox.ForeColor = SystemColors.InactiveCaption;
            }
        }
        //Changes the description text of the Discount textbox when entering the textbox 
        private void DiscountCodeTextBoxEnter(object sender, EventArgs e)
        {
            if (DiscountCodeTextBox.Text == "Discount Code")
            {
                DiscountCodeTextBox.Text = "";
                DiscountCodeTextBox.ForeColor = Color.Black;
            }
        }
        public static void SetReceiptSummaryValue()
        {
            var summary = Cart.CartItems.Sum(x => x.Product.Price * x.Amount);
            ReceiptSummaryValue = summary;
            TotalPriceLabel.Text = String.Format("Total Cost {0:C0}", summary);

        }

    }
}
