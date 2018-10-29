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
        private DataGridView ReceiptDataGridView = new DataGridView
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
            AllowUserToResizeRows = false,
            Enabled = false
        };
        private Label TotalPriceLabel = new Label
        {
            Text = String.Format("Total Cost {0:C0}", MyForm.CartSummaryValue),
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


        public ReceiptWindow()
        {
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon("MainFormIcon.ico");
            Text = "Your reciept";
            Size = new Size(500, 250);
            Controls.Add(MainLayout);

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));


            MainLayout.Controls.Add(ReceiptDataGridView);
            MainLayout.SetColumnSpan(ReceiptDataGridView, 2);
            MainLayout.Controls.Add(PurchaseDateLabel);
            MainLayout.Controls.Add(TotalPriceLabel);

            ReceiptDataGridView.DefaultCellStyle.SelectionBackColor = ReceiptDataGridView.DefaultCellStyle.BackColor;
            ReceiptDataGridView.DefaultCellStyle.SelectionForeColor = ReceiptDataGridView.DefaultCellStyle.ForeColor;

            ReceiptDataGridView.Columns[0].Name = "Name";
            ReceiptDataGridView.Columns[1].Name = "Quantity";
            ReceiptDataGridView.Columns[2].Name = "Price";
            ReceiptDataGridView.Columns[0].Width = 250;

            AddCartToReceipt();

            FormClosing += ClosingReceipt;
        }

        private void ClosingReceipt(object sender, FormClosingEventArgs e)
        {
            MyForm.ClearCart();
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
    }
}
