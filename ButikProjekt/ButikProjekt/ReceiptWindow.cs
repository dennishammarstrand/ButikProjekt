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
            Dock = DockStyle.Top,
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
            AllowUserToResizeRows = false
        };
        public static double ReceiptSummaryValue;
        public static Label TotalPriceLabel = new Label
        {
            Font = new Font("San serif", 10F, FontStyle.Bold),
            ForeColor = Color.Red,
            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.TopLeft,
        };
        private static DateTime TodaysDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        private Label PurchaseDateLabel = new Label { Text = TodaysDate.ToString(), Dock = DockStyle.Top, TextAlign = ContentAlignment.TopLeft };
        private Label ThankYouLabel = new Label { Text = "Thanks for ordering", Font = new Font("San serif", 15F, FontStyle.Bold), Anchor = AnchorStyles.Top, AutoSize = true };
        public static TextBox DiscountCodeTextBox = new TextBox { Anchor = AnchorStyles.Right, Width = 250, Text = "Discount Code", Font = new Font("San Serif", 10f), ForeColor = SystemColors.InactiveCaption };
        public static Button ActivateDiscountButton = new Button { Anchor = AnchorStyles.Left, Font = new Font("San Serif", 10f), Text = "Activate Discount", AutoSize = true };

        public ReceiptWindow()
        {
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon("MainFormIcon.ico");
            Text = "Your reciept";
            Size = new Size(500, 350);
            Controls.Add(MainLayout);

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));

            MainLayout.Controls.Add(ThankYouLabel);
            MainLayout.Controls.Add(ReceiptDataGridView);
            MainLayout.SetColumnSpan(ReceiptDataGridView, 2);
            MainLayout.Controls.Add(DiscountCodeTextBox);
            MainLayout.Controls.Add(ActivateDiscountButton);
            MainLayout.Controls.Add(PurchaseDateLabel, 0, 3);
            MainLayout.Controls.Add(TotalPriceLabel, 1, 3);

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
        }
        private void ActivateDiscountButtonClickHandler(object sender, EventArgs e)
        {
            try
            {
                DiscountCode.AddDiscountCodeToReceipt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClosingReceipt(object sender, FormClosingEventArgs e)
        {
            ReceiptDataGridView.Rows.Clear();
            ReceiptDataGridView.Refresh();
            ActivateDiscountButton.Enabled = true;
            DiscountCodeTextBox.Enabled = true;
            DiscountCodeTextBox.Text = "Discount Code";
            DiscountCodeTextBox.ForeColor = SystemColors.InactiveCaption;
            ActivateDiscountButton.Click -= ActivateDiscountButtonClickHandler;
        }
        public void AddCartToReceipt()
        {
            foreach (Cart cartItem in Cart.CartItems)
            {
                object[] row = new object[3];
                row[0] = cartItem.Product.Name;
                row[1] = cartItem.Amount;
                row[2] = cartItem.Product.Price;
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
