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
        
        //Adds discount codes from csv to discount code list
        public static void AddDiscountCodesToList()
        {
            try
            {
                string[] fileContent;
                fileContent = File.ReadAllLines("discountCodes.csv");

                foreach (string s in fileContent)
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
        //Adds the discount code and it's discounted value to the datagridview with formatting and 
        //updates the new total price accordingly.
        public static void AddDiscountCodeToReceipt()
        {
            if (!Cart.IsCartListEmpty())
            {
                string[] discountCodeEntered = DiscountCodeList.First(x => x[0] == ReceiptWindow.DiscountCodeTextBox.Text);
                if (discountCodeEntered != null)
                {
                    object[] discountRow = new object[3];
                    double discountValue = GetValueDiscount(discountCodeEntered);
                        
                    discountRow[0] = ReceiptWindow.DiscountCodeTextBox.Text;
                    discountRow[2] = "-" + discountValue;

                    ReceiptWindow.ReceiptSummaryValue -= discountValue;
                    ReceiptWindow.TotalPriceLabel.Text = String.Format("Total Cost {0:C0}", ReceiptWindow.ReceiptSummaryValue);

                    ReceiptWindow.ReceiptDataGridView.Rows.Add(discountRow);

                    int rowCount = ReceiptWindow.ReceiptDataGridView.Rows.Count;
                    ReceiptWindow.ReceiptDataGridView.Rows[rowCount - 1].Cells[2].Style.ForeColor = Color.Red;
                    ReceiptWindow.ActivateDiscountButton.Enabled = false;
                    ReceiptWindow.DiscountCodeTextBox.Enabled = false;
                }
                else
                {
                    throw new Exception("Discount not valid!");
                }
            }
        }
        //Get each discount codes specific discount reduction
        private static double GetValueDiscount(string[] discountCode)
        {
            if (discountCode[1] == "Value")
            {
                return int.Parse(discountCode[2]);
            }
            else if (discountCode[1] == "2for1")
            {
                List<Cart> canonOrderedByPrice = Cart.CartItems.Where(x => x.Product.Name.Contains("Canon")).OrderBy(z => z.Product.Price).ToList();

                if (canonOrderedByPrice.Count() > 1)
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
                return ReceiptWindow.ReceiptSummaryValue * (int.Parse(discountCode[2]) / 100.0);
            }
            throw new Exception("Discount not valid");
        }
    }

}
