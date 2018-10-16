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

        private FlowLayoutPanel FlowLayout = CreatingFlowLayout();
        
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
        private Button Remove = new Button { Font = new Font("San Serif", 15f), Text = "Remove from cart", AutoSize = true, Anchor = AnchorStyles.Bottom, Dock = DockStyle.Top };
        private Button Buy = new Button { Font = new Font("San Serif", 15f), Text = "Buy", AutoSize = true, Anchor = AnchorStyles.Top, Dock = DockStyle.Top};
        private int SelectedRow;
        private List<Products> listProd = Products.GetProducts();
        private Dictionary<Products, int> cartItems = new Dictionary<Products, int>();
        private TableLayoutPanel ButtonLayout = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true };

        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

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

            ShoppingCartGridView.Columns[0].Name = "Product";
            ShoppingCartGridView.Columns[1].Name = "Price";
            ShoppingCartGridView.Columns[2].Name = "Quantity";
            ShoppingCartGridView.Columns[0].Width = 150;


            ShoppingCartGridView.CellClick += DataGridCellClick;
            //Add.Click += AddToCartClick;
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

        /*
        private void AddToCartClick(object sender, EventArgs e)
        {
            foreach (Products p in listProd)
            {
                if (p.Name)
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
        */
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

        public static FlowLayoutPanel CreatingFlowLayout()
        {
            FlowLayoutPanel flow = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoSize = true, AutoScroll = true };

            List<Products> prodInfo = Products.GetProducts();
            PictureBox prodImage;
            Label prodName;
            Label prodPrice;
            Button infoButton;
            Button buyButton;
            Label description;

            foreach (Products item in prodInfo)
            {
                TableLayoutPanel newItem = new TableLayoutPanel()
                {
                    ColumnCount = 2,
                    Size = new Size(210, 350),

                    BackColor = Color.White

                };
                flow.Controls.Add(newItem);
                prodImage = new PictureBox { ImageLocation = item.Image, Size = new Size(200, 200), SizeMode = PictureBoxSizeMode.Zoom };
                newItem.Controls.Add(prodImage);
                newItem.SetColumnSpan(prodImage, 2);
                prodName = new Label { Text = item.Name, Font = new Font("San serif", 10F) };
                newItem.Controls.Add(prodName);
                prodPrice = new Label { Text = item.Price.ToString() + " kr", Font = new Font("San serif", 10F), ForeColor = Color.Red, TextAlign = ContentAlignment.TopRight };
                newItem.Controls.Add(prodPrice);
                description = new Label { Text = item.Description, Dock = DockStyle.Fill, Size = new Size(200, 65) };
                newItem.Controls.Add(description);
                newItem.SetColumnSpan(description, 2);
                //infoButton = new Button { Text = "Info", AutoSize = true };
                //newItem.Controls.Add(infoButton);
                buyButton = new Button { Text = "Add to cart", AutoSize = true, Dock = DockStyle.Top, Font = new Font("San serif", 12F) };
                newItem.SetColumnSpan(buyButton, 2);

                newItem.Controls.Add(buyButton);

            }
            return flow;
        }
    }
}
