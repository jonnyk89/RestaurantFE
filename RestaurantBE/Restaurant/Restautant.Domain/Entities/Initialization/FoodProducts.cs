using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities.Initialization
{
    public static class FoodProducts
    {
        public static ICollection<Product> GetProducts(Dictionary<string, string> categoryDictionary) =>
            new List<Product>
            {
                new Product
                {
                    CategoryId = categoryDictionary["Vegetarian Salads"],
                    Name = "Cool Beans Salad",
                    Description = "Description placeholder",
                    Price = 5.99m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Vegetarian Salads"],
                    Name = "Hearty Asian Lettuce Salad",
                    Description = "Description placeholder",
                    Price = 6.65m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Vegetarian Salads"],
                    Name = "Mediterranean Bulgur Bowl",
                    Description = "Description placeholder",
                    Price = 4.20m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Non-Vegetarian Salads"],
                    Name = "Caesar Salad",
                    Description = "Description placeholder",
                    Price = 2.50m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Non-Vegetarian Salads"],
                    Name = "Chicken Caesar Salad",
                    Description = "Description placeholder",
                    Price = 2.50m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Non-Vegetarian Salads"],
                    Name = "Grilled Squid Salad",
                    Description = "Description placeholder",
                    Price = 3.50m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Vegetarian Sandwiches"],
                    Name = "Veggie & Hummus Sandwich",
                    Description = "Description placeholder",
                    Price = 5
                },
                new Product
                {
                    CategoryId = categoryDictionary["Vegetarian Sandwiches"],
                    Name = "Avocado Egg Salad Sandwiches",
                    Description = "Description placeholder",
                    Price = 4.10m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Vegetarian Sandwiches"],
                    Name = "Veggie Burger",
                    Description = "Description placeholder",
                    Price = 5.35m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Non-Vegetarian Sandwiches"],
                    Name = "Pork Burger",
                    Description = "Description placeholder",
                    Price = 4.35m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Non-Vegetarian Sandwiches"],
                    Name = "Chicken Burger",
                    Description = "Description placeholder",
                    Price = 3.10m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Cake"],
                    Name = "Chocolate Cake",
                    Description = "Description placeholder",
                    Price = 2.85m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Cake"],
                    Name = "Red Velvet Cake",
                    Description = "Description placeholder",
                    Price = 2.90m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Cake"],
                    Name = "Cheesecake",
                    Description = "Description placeholder",
                    Price = 2.15m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Cake"],
                    Name = "Molten lava cakes",
                    Description = "Description placeholder",
                    Price = 3.90m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Cake"],
                    Name = "Cheesecake",
                    Description = "Description placeholder",
                    Price = 1.99m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Pastries"],
                    Name = "Apple pie",
                    Description = "Description placeholder",
                    Price = 1.35m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Pastries"],
                    Name = "Pumpkin pie",
                    Description = "Description placeholder",
                    Price = 1.60m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Pastries"],
                    Name = "Giant chocolate chip cookies",
                    Description = "Description placeholder",
                    Price = 2.05m,
                },
                new Product
                {
                    CategoryId = categoryDictionary["Pastries"],
                    Name = "Banana split",
                    Description = "Description placeholder",
                    Price = 3.85m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Pastries"],
                    Name = "Cinnamon rolls",
                    Description = "Description placeholder",
                    Price = 2.15m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Pastries"],
                    Name = "Baklava",
                    Description = "Description placeholder",
                    Price = 1.55m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Carbonated drink"],
                    Name = "Coke",
                    Description = "Description placeholder",
                    Price = 1.50m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Carbonated drink"],
                    Name = "Coke light",
                    Description = "Description placeholder",
                    Price = 1.55m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Carbonated drink"],
                    Name = "Fanta",
                    Description = "Description placeholder",
                    Price = 1.55m,
                },
                new Product
                {
                    CategoryId = categoryDictionary["Beer"],
                    Name = "Bud Light",
                    Description = "Description placeholder",
                    Price = 2
                },
                new Product
                {
                    CategoryId = categoryDictionary["Beer"],
                    Name = "Coors Light",
                    Description = "Description placeholder",
                    Price = 1.90m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Beer"],
                    Name = "Corona Extra",
                    Description = "Description placeholder",
                    Price = 1.10m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Whiskey"],
                    Name = "Jack Daniels",
                    Description = "Description placeholder",
                    Price = 3m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Whiskey"],
                    Name = "Poper 12",
                    Description = "Description placeholder",
                    Price = 3.10m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Vodka"],
                    Name = "Smirnoff",
                    Description = "white wine",
                    Price = 1.85m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Juice"],
                    Name = "Apple Juice",
                    Description = "Description placeholder",
                    Price = 1.90m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Juice"],
                    Name = "Orange Juice",
                    Description = "Description placeholder",
                    Price = 2.50m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Juice"],
                    Name = "Peach Juice",
                    Description = "Description placeholder",
                    Price = 1.10m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Coffee"],
                    Name = "Expresso",
                    Description = "Description placeholder",
                    Price = 2.79m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Coffee"],
                    Name = "Decaf",
                    Description = "Description placeholder",
                    Price = 2.89m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Coffee"],
                    Name = "Arabica",
                    Description = "Description placeholder",
                    Price = 2.89m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Tea"],
                    Name = "Black Tea",
                    Description = "Description placeholder",
                    Price = 1.89m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Tea"],
                    Name = "Green Tea",
                    Description = "Description placeholder",
                    Price = 1.89m
                },
                new Product
                {
                    CategoryId = categoryDictionary["Water"],
                    Name = "Bottled Spring Water",
                    Description = "Description placeholder",
                    Price = 0.89m
                },
            };
    }
}
