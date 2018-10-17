using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ButikProjekt
{
    class DiscountCodes
    {
        private static List<string> DiscountCodeList = new List<string>();

        public static List<string> ReadCodes()
        {
            try
            {
                string[] content;
                content = File.ReadAllLines("discountCodes.csv");

                foreach (string s in content)
                {
                    DiscountCodeList.Add(s);
                }
                return DiscountCodeList;
            }
            catch(FileNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
