public class Ingredient
{
    // Properties to store ingredient details
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int OriginalQuantity { get; set; }
    public string FoodGroup { get; set; }
    public string Measurement { get; set; }
    public int Calories { get; set; }
    // Constructor to initialize an Ingredient object
    public Ingredient(string name, int quantity, string foodGroup, string measurement, int calories)
    {
        Name = name;
        Quantity = quantity;
        OriginalQuantity = quantity;
        FoodGroup = foodGroup;
        Measurement = measurement;
        Calories = calories;
    }
    // Method to reset the quantity to its original value
    public void ResetQuantity()
    {
        Quantity = OriginalQuantity;
    }
    // Override the ToString method to provide a string representation of the ingredient
    public override string ToString()
    {
        return $"{Quantity} {Measurement} of {Name} ({FoodGroup}), {Calories} calories";
    }
}
