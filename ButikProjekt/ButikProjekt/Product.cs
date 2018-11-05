using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ButikProjekt
{
    class AddButton : Button
    {
        public Product Product { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }

        private static List<Product> ListOfProducts = new List<Product>();

        public static void AddProductsToList()
        {
            try
            {
                string[] fileContent;

                fileContent = File.ReadAllLines("products.csv");

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

                    ListOfProducts.Add(newProduct);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void ProductPanelCreation()
        {

            FlowLayoutPanel FlowLayout = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoSize = true, AutoScroll = true };

            foreach (Product item in ListOfProducts)
            {
                TableLayoutPanel newProductTemplate = new TableLayoutPanel()
                {
                    ColumnCount = 2,
                    Size = new Size(210, 340),
                    BackColor = Color.White
                };
                FlowLayout.Controls.Add(newProductTemplate);
                PictureBox productImagePictureBox = new PictureBox { ImageLocation = item.Image, Size = new Size(200, 200), SizeMode = PictureBoxSizeMode.Zoom };
                newProductTemplate.Controls.Add(productImagePictureBox);
                newProductTemplate.SetColumnSpan(productImagePictureBox, 2);
                Label produtNameLabel = new Label { Text = item.Name, Font = new Font("San serif", 10F) };
                newProductTemplate.Controls.Add(produtNameLabel);
                Label productPriceLabel = new Label { Text = String.Format("{0:C0}", item.Price), Font = new Font("San serif", 10F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.TopRight };
                newProductTemplate.Controls.Add(productPriceLabel);
                Label productDescriptionLabel = new Label { Text = item.Description, Dock = DockStyle.Fill, Size = new Size(200, 65), AutoEllipsis = true };
                newProductTemplate.Controls.Add(productDescriptionLabel);
                newProductTemplate.SetColumnSpan(productDescriptionLabel, 2);
                AddButton productAddButton = new AddButton { Product = item, Text = "Add to cart", AutoSize = true, Dock = DockStyle.Top, Font = new Font("San serif", 12F), BackColor = Color.ForestGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Margin = new Padding(10, 0, 10, 0) };

                newProductTemplate.SetColumnSpan(productAddButton, 2);
                newProductTemplate.Controls.Add(productAddButton);
                productAddButton.Click += ShopWindow.AddToCartClick;
            }
            ShopWindow.MainLayout.Controls.Add(FlowLayout);
            ShopWindow.MainLayout.SetRowSpan(FlowLayout, 3);
        }
        public static bool operator ==(Product a, Product b)
        {
            return a.Name == b.Name;
        }
        public static bool operator !=(Product a, Product b)
        {
            return a.Name != b.Name;
        }
    }
}
