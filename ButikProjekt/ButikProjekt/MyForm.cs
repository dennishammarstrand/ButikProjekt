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

        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            Controls.Add(MainLayout);
            MainLayout.Controls.Add(ShopGridView);



        }
    }
}
