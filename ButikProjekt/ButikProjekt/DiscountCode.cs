using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ButikProjekt
{
    class DiscountCode
    {
        public static List<string[]> DiscountCodeList = new List<string[]>();
        
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
    }

}
