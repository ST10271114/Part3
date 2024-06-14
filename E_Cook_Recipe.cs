using System.Collections.Generic;
using System.Linq;

namespace demo
{
    public class E_Cook_Recipe
    {
        // Lists to store recipe names, steps, and ingredients
        private List<string> name = new List<string>();
        private List<List<string>> steps = new List<List<string>>();
        private List<List<Ingredient>> ingredients = new List<List<Ingredient>>();

        // Method to save a recipe
        public string saves(string names, List<string> stepList, List<Ingredient> ingredientsList)
        {
            // Add the recipe details to the respective lists
            name.Add(names);
            steps.Add(stepList);
            ingredients.Add(ingredientsList);
            return "Capture is done !!"; // Return confirmation message
        }

        // Add Method to display all recipes with an optional scale factor
        public string displays(double scaleFactor)
        {
            string message = "";
            for (int count = 0; count < name.Count; count++)
            {
                message += "Recipe name: " + name[count] + "\nIngredients:\n";
                int totalCalories = 0;
                foreach (var ingredient in ingredients[count])
                {
                    int scaledQuantity = (int)(ingredient.OriginalQuantity * scaleFactor); // Scale the ingredient quantity
                    message += $"{scaledQuantity} {ingredient.Measurement} of {ingredient.Name} ({ingredient.FoodGroup}), {ingredient.Calories} calories\n";
                    totalCalories += ingredient.Calories; // Sum up the calories
                }
                message += $"Total Calories: {totalCalories}\n\nSteps:\n";
                for (int stepCount = 0; stepCount < steps[count].Count; stepCount++)
                {
                    message += (stepCount + 1) + ". " + steps[count][stepCount] + "\n";
                }
                message += "\n";
            }
            return message; // Return the constructed message
        }

        // Method to reset quantities of all ingredients to their original values
        public void ResetAllQuantities()
        {
            foreach (var ingredientList in ingredients)
            {
                foreach (var ingredient in ingredientList)
                {
                    ingredient.ResetQuantity();
                }
            }
        }

        // Method to sort recipes alphabetically by name
        public void SortRecipes()
        {
            // Create a list of indices based on the number of recipes
            List<int> indices = Enumerable.Range(0, name.Count).ToList();
            // Sort the indices based on the recipe names
            indices.Sort((i, j) => string.Compare(name[i], name[j]));
            // Rearrange the recipes according to the sorted indices
            name = indices.Select(i => name[i]).ToList();
            steps = indices.Select(i => steps[i]).ToList();
            ingredients = indices.Select(i => ingredients[i]).ToList();
        }

        // Method to check if there are any recipes stored
        public bool HasRecipes()
        {
            return name.Count > 0;
        }
    }
}
