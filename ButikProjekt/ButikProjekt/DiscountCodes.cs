using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace ButikProjekt
{
    class DiscountCode
    {
        public static List<string[]> DiscountCodeList = new List<string[]>();
        
        public static void ReadCodes()
        {
            try
            {
                string[] content;
                content = File.ReadAllLines("discountCodes.csv");

                foreach (string s in content)
                {
                    string[] split = s.Split(';');
                    DiscountCodeList.Add(split);
                }
            }
            catch(FileNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void AddDiscountCodeToCart()
        {

            int count = MyForm.ShoppingCartGridView.Rows.Count;
            object[] discountRow = new object[2];
            if (!Cart.IsCartListEmpty())
            {
                foreach (string[] code in DiscountCodeList)
                {
                    if (code[0] == MyForm.DiscountCodeTextBox.Text)
                    {
                        double discountValue = GetValueDiscount(code);
                        
                        discountRow[0] = MyForm.DiscountCodeTextBox.Text;
                        discountRow[1] = "-" + discountValue;

                        MyForm.CartSummary -= discountValue;
                        MyForm.GetSetSummary = MyForm.CartPriceSummary.Text;

                        MyForm.ShoppingCartGridView.Rows.Add(discountRow);

                        count = MyForm.ShoppingCartGridView.Rows.Count;
                        MyForm.ShoppingCartGridView.Rows[count - 1].Cells[1].Style.ForeColor = Color.Red;
                        MyForm.DiscountCodeTextBox.Enabled = false;
                    }
                    else
                    {
                        throw new Exception("Discount not valid!");
                    }
                }
            }
        }
        private static double GetValueDiscount(string[] code)
        {
            if (code[1] == "Value")
            {
                return int.Parse(code[2]);
            }
            else if (code[1] == "2for1")
            {
                var canons = Cart.CartItems.Where(x => x.Product.Name.Contains("Canon")).OrderBy(z => z.Product.Price).ToList();

                if (canons.Count() > 1)
                {
                    return canons[0].Product.Price;
                }
                else
                {
                    throw new Exception("Discount not valid! Add another canon!");
                }
            }
            else if (code[1] == "Percent")
            {
                double x = MyForm.CartSummary * (int.Parse(code[2]) / 100.0);
                return x;
            }
            throw new Exception("Discount not valid");
        }
    }

}
