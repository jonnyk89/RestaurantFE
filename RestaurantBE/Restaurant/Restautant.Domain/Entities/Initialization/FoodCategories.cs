using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities.Initialization
{
    public static class FoodCategories
    {
        private static List<Category> Categories = new List<Category>();

        public static ICollection<Category> GetCategories()
        {
            Categories.AddRange(Main());
            Categories.AddRange(Salads());
            Categories.AddRange(Sandwiches());
            Categories.AddRange(Desserts());
            Categories.AddRange(Beverages());
            Categories.AddRange(Alcoholic());
            Categories.AddRange(NonAlcoholic());

            var result = Categories.ToList();

            return result;
        }

        public static ICollection<Category> Main() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Salads"
                },
                new Category
                {
                    Name = "Sandwiches"
                },
                new Category
                {
                    Name = "Desserts"
                },
                new Category
                {
                    Name = "Beverages"
                },
            };

        public static ICollection<Category> Salads() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Vegetarian Salads",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Salads").Id
                },
                new Category
                {
                    Name = "Non-Vegetarian Salads",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Salads").Id
                },
            };

        public static ICollection<Category> Sandwiches() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Vegetarian Sandwiches",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Sandwiches").Id
                },
                new Category
                {
                    Name = "Non-Vegetarian Sandwiches",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Sandwiches").Id
                },
            };

        public static ICollection<Category> Desserts() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Cake",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Desserts").Id,
                },
                new Category
                {
                    Name = "Pastries",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Desserts").Id,
                },
                new Category
                {
                    Name = "Frozen",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Desserts").Id,
                },
            };

        public static ICollection<Category> Beverages() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Alcoholic",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Beverages").Id,
                },
                new Category
                {
                    Name = "Non-alcoholic",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Beverages").Id,
                },
            };

        public static ICollection<Category> Alcoholic() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Beer",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Alcoholic").Id,
                },
                new Category
                {
                    Name = "Whiskey",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Alcoholic").Id,
                },
                new Category
                {
                    Name = "Vodka",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Alcoholic").Id,
                },
            };

        public static ICollection<Category> NonAlcoholic() =>
            new List<Category>
            {
                new Category
                {
                    Name = "Coffee",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Non-alcoholic").Id,
                },
                new Category
                {
                    Name = "Tea",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Non-alcoholic").Id,
                },
                new Category
                {
                    Name = "Juice",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Non-alcoholic").Id,
                },
                new Category
                {
                    Name = "Carbonated drink",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Non-alcoholic").Id,
                },
                new Category
                {
                    Name = "Water",
                    ParentId = Categories.FirstOrDefault(x => x.Name == "Non-alcoholic").Id,
                },
            };

    }
}
