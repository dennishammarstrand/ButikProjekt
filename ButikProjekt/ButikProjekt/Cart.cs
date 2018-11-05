using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButikProjekt
{
    class Cart
    {
        public Product Product { get; set; }
        public int Amount { get; set; }

        public static List<Cart> CartItems = new List<Cart>();
        
        public static void GetSavedCartItems()
        {
            string[] fileContent;
            if (File.Exists(@"C:\Windows\Temp\SavedCart.csv"))
            {
                fileContent = File.ReadAllLines(@"C:\Windows\Temp\SavedCart.csv");
                foreach (string s in fileContent)
                {
                    string[] split = s.Split(';');

                    Product newProduct = new Product()
                    {
                        Name = split[0],
                        Description = split[1],
                        Price = int.Parse(split[2]),
                        Image = split[3]
                    };
                    Cart newCartItem = new Cart()
                    {
                        Product = newProduct,
                        Amount = int.Parse(split[4])
                    };
                    CartItems.Add(newCartItem);
                }
            }
            else
            {
                File.Create(@"C:\Windows\Temp\SavedCart.csv");
            }
        }
        public static bool IsCartListEmpty()
        {
            return Cart.CartItems.Count == 0;
        }
        
    }
}
