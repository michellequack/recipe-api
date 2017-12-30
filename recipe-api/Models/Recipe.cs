using System;
using System.Collections.Generic;

namespace RecipeApi.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            Ingredient = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Directions { get; set; }
        public string NumberOfServings { get; set; }
        public string Author { get; set; }

        public ICollection<Ingredient> Ingredient { get; set; }
    }
}
