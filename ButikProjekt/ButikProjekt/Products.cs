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
    public class Products
    {
        public string Name;
        public string Description;
        public int Price;
        public string Image;

        public static List<Products> listProd = new List<Products>();

        public static List<Products> GetProducts()
        {
            string[] content;

            content = File.ReadAllLines("products.csv");

            foreach (string s in content)
            {
                string[] split = s.Split(';');

                Products newProd = new Products()
                {
                    Name = split[0],
                    Description = split[1],
                    Price = int.Parse(split[2]),
                    Image = split[3]
                };

                listProd.Add(newProd);
            }
            return listProd;
        }
    }

}
