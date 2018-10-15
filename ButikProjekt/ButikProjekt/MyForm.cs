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
        private TableLayoutPanel Layout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };

        public MyForm()
        {
            ClientSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

            
            Controls.Add(Layout);



        }
    }
}
