using UnityEngine;

// Definimos que tipo de contenido tiene el slot
public enum RecipeCategory
{
    Coffee,
    Food
}

// Clase auxiliar para las recetas del recetario
public class RecipeSlot : MonoBehaviour
{
    public RecipeCategory category;
    public CoffeeType coffeeType;
    public FoodCategory foodType;
    public Sprite recipeSprite;
}
