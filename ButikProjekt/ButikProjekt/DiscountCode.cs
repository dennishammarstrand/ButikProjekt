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
        public string Name { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

        public static List<DiscountCode> DiscountCodeList = new List<DiscountCode>();
        
        public static void AddDiscountCodesToList()
        {
            try
            {
                string[] fileContent;
                fileContent = File.ReadAllLines("discountCodes.csv");

                foreach (string s in fileContent)
                {
                    string[] split = s.Split(';');

                    if (split.Length == 3)
                    {
                        DiscountCode newDiscount = new DiscountCode
                        {
                            Name = split[0],
                            Type = split[1],
                            Value = int.Parse(split[2])
                        };

                        DiscountCodeList.Add(newDiscount);
                    }
                    else
                    {

                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
