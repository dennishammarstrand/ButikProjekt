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
            ColumnCount = 3,
            RowCount = 15,
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
            RowCount = 15,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
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



        }
    }
}
