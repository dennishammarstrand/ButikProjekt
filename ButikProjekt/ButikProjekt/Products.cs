﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ButikProjekt
{
    public class Products
    {
        public string Name;
        public string Description;
        public int Price;
        public string Image;

        private static List<Products> ListProd = new List<Products>();

        public static List<Products> GetProducts()
        {
            try
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

                    ListProd.Add(newProd);
                }
                return ListProd;
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static FlowLayoutPanel ProductPanelCreation()
        {
            FlowLayoutPanel FlowLayout = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoSize = true, AutoScroll = true };

            foreach (Products item in ListProd)
            {
                TableLayoutPanel newItem = new TableLayoutPanel()
                {
                    ColumnCount = 2,
                    Size = new Size(210, 340),
                    BackColor = Color.White
                };
                FlowLayout.Controls.Add(newItem);
                PictureBox ProdImage = new PictureBox { ImageLocation = item.Image, Size = new Size(200, 200), SizeMode = PictureBoxSizeMode.Zoom };
                newItem.Controls.Add(ProdImage);
                newItem.SetColumnSpan(ProdImage, 2);
                Label ProdName = new Label { Text = item.Name, Font = new Font("San serif", 10F) };
                newItem.Controls.Add(ProdName);
                Label prodPrice = new Label { Text = String.Format("{0:C0}", item.Price), Font = new Font("San serif", 10F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.TopRight };
                newItem.Controls.Add(prodPrice);
                Label description = new Label { Text = item.Description, Dock = DockStyle.Fill, Size = new Size(200, 65), AutoEllipsis = true };
                newItem.Controls.Add(description);
                newItem.SetColumnSpan(description, 2);
                Button addButton = new Button { Text = "Add to cart", AutoSize = true, Dock = DockStyle.Top, Font = new Font("San serif", 12F), BackColor = Color.ForestGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Tag = item, Margin = new Padding(10, 0, 10, 0) };
                newItem.SetColumnSpan(addButton, 2);
                newItem.Controls.Add(addButton);
                addButton.Click += MyForm.AddToCartClick;
            }
            return FlowLayout;
        }
    }

}
