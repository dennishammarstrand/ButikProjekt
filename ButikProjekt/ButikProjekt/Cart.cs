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
        public Product Product;
        public int Amount;

        public static List<Cart> CartItems = new List<Cart>();
        
        public static void GetSavedCartItems()
        {
            string[] content;
            if (File.Exists(@"C:\Windows\Temp\SavedCart.csv"))
            {
                content = File.ReadAllLines(@"C:\Windows\Temp\SavedCart.csv");
                foreach (string s in content)
                {
                    string[] split = s.Split(';');

                    Product newProd = new Product()
                    {
                        Name = split[0],
                        Description = split[1],
                        Price = int.Parse(split[2]),
                        Image = split[3]
                    };
                    Cart savedCart = new Cart()
                    {
                        Product = newProd,
                        Amount = int.Parse(split[4])
                    };
                    CartItems.Add(savedCart);
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
