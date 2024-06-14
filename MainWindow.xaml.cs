using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;

namespace demo
{
    public partial class MainWindow : Window
    {
        // Observable collections to store ingredients and steps
        private ObservableCollection<Ingredient> ingredients = new ObservableCollection<Ingredient>();
        private ObservableCollection<string> steps = new ObservableCollection<string>();
        private E_Cook_Recipe Reci_Cook = new E_Cook_Recipe();

        // Property for Ingredients
        public ObservableCollection<Ingredient> Ingredients
        {
            get { return (ObservableCollection<Ingredient>)GetValue(IngredientsProperty); }
            set { SetValue(IngredientsProperty, value); }
        }

        // DependencyProperty for Ingredients
        public static readonly DependencyProperty IngredientsProperty =
            DependencyProperty.Register("Ingredients", typeof(ObservableCollection<Ingredient>), typeof(MainWindow));

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Ingredients = ingredients;
            recipe_steps.ItemsSource = steps;
            ingredientsList.ItemsSource = Ingredients;
        }

        // Event handler for the Capture button
        private void Capture(object sender, RoutedEventArgs e)
        {
            // Show the capturing grid and hide others
            landing_page.Visibility = Visibility.Collapsed;
            capturing.Visibility = Visibility.Visible;
            display_all.Visibility = Visibility.Collapsed;
        }

        // Event handler for the Display button
        private void Display(object sender, RoutedEventArgs e)
        {
            // Check if there are any recipes
            if (!Reci_Cook.HasRecipes())
            {
                MessageBox.Show("No recipes added.");
                return;
            }

            // Show the display_all grid and hide others
            landing_page.Visibility = Visibility.Collapsed;
            capturing.Visibility = Visibility.Collapsed;
            display_all.Visibility = Visibility.Visible;
            Reci_Cook.SortRecipes();

            // Ask if the user wants to apply a scale factor
            MessageBoxResult result = MessageBox.Show("Would you like to apply a scale factor?", "Scale Factor", MessageBoxButton.YesNo);
            double scaleFactor = 1.0;

            if (result == MessageBoxResult.Yes)
            {
                scaleFactor = GetScaleFactor();
            }

            // Display the recipes with the applied scale factor
            add_all.Items.Clear();
            add_all.Items.Add(Reci_Cook.displays(scaleFactor));
        }

        // Event handler for the Reset Quantities button
        private void ResetQuantities(object sender, RoutedEventArgs e)
        {
            // Reset all quantities in the recipe and show a message
            Reci_Cook.ResetAllQuantities();
            MessageBox.Show("Quantities have been reset to their original values.");

            // Refresh the ingredients list
            ingredientsList.ItemsSource = null;
            ingredientsList.ItemsSource = Ingredients;

            // Refresh the display view if it is visible
            if (display_all.Visibility == Visibility.Visible)
            {
                Display(sender, e);
            }
        }

        // Method to get the scale factor from the user
        private double GetScaleFactor()
        {
            string input = Interaction.InputBox("Enter scale factor (0.5 for half, 2 for double, 3 for triple):", "Scale Factor", "1");
            double scaleFactor;
            while (!double.TryParse(input, out scaleFactor) || (scaleFactor != 0.5 && scaleFactor != 2 && scaleFactor != 3))
            {
                MessageBox.Show("Invalid scale factor. Please enter 0.5, 2, or 3.");
                input = Interaction.InputBox("Enter scale factor (0.5 for half, 2 for double, 3 for triple):", "Scale Factor", "1");
            }
            return scaleFactor;
        }

        // Event handler for text changes in the text box (currently empty)
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        // Event handler for the Submit button
        private void save(object sender, RoutedEventArgs e)
        {
            // Get the recipe name
            string name = recipe_name.Text;

            // Check if all required fields are filled
            if (string.IsNullOrEmpty(name) || steps.Count == 0 || ingredients.Count == 0)
            {
                MessageBox.Show("Error Message !! \n\n All fields are required!! \n\n");
            }
            else
            {
                // Save the recipe and show a message
                string message = Reci_Cook.saves(name, new List<string>(steps), new List<Ingredient>(ingredients));
                MessageBox.Show("Message !! \n\n" + message);

                // Clear the fields and lists
                recipe_name.Clear();
                steps.Clear();
                new_step.Clear();
                ingredients.Clear();
            }
        }

        // Event handler to add a step
        private void AddStep(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(new_step.Text))
            {
                steps.Add(new_step.Text);
                new_step.Clear();
            }
        }

        // Event handler to add an ingredient
        private void AddIngredient(object sender, RoutedEventArgs e)
        {
            // Get ingredient details from the user
            string ingredientName = Interaction.InputBox("Enter ingredient name:", "Add Ingredient");
            if (!string.IsNullOrEmpty(ingredientName))
            {
                int quantity = Convert.ToInt32(Interaction.InputBox("Enter quantity of " + ingredientName + ":", "Add Ingredient"));
                string measurement = Interaction.InputBox("Enter measurement for " + ingredientName + ":", "Add Ingredient");

                string[] foodGroups = { "starch", "fruit & veg", "fats & oil", "dairy", "dry beans", "water", "protein" };
                string foodGroup = Interaction.InputBox("Select food group (starch, fruit & veg, fats & oil, dairy, dry beans, water, protein):", "Add Ingredient");
                while (!foodGroups.Contains(foodGroup))
                {
                    MessageBox.Show("Invalid food group. Please select from: starch, fruit & veg, fats & oil, dairy, dry beans, water, protein.");
                    foodGroup = Interaction.InputBox("Select food group (starch, fruit & veg, fats & oil, dairy, dry beans, water, protein):", "Add Ingredient");
                }

                int calories = Convert.ToInt32(Interaction.InputBox("Enter calories for " + ingredientName + ":", "Add Ingredient"));
                Ingredient ingredient = new Ingredient(ingredientName, quantity, foodGroup, measurement, calories);

                // Check total calories and add ingredient if within limit
                int totalCalories = ingredients.Sum(i => i.Calories) + ingredient.Calories;
                if (totalCalories > 300)
                {
                    MessageBox.Show("Warning: Total calories exceed 300!");
                }
                else
                {
                    ingredients.Add(ingredient);
                }
            }
        }
    }
}
