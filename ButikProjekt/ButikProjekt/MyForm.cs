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
        public MyForm()
        {
            Label newLabel = new Label() { };
            Controls.Add(newLabel);
        }
    }
}
