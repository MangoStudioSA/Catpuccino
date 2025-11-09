using System.Collections.Generic;
using UnityEngine;

// Clase utilizada para mostrar las recetas en el recetario 
public class CoffeeRecipesManager : MonoBehaviour
{
    [Header("Cafes + recetas")]
    public CoffeeType[] coffeeTypes;
    public GameObject[] coffeeRecipesPanels;

    private Dictionary<CoffeeType, GameObject> coffeeUIDict;

    // Se inicializan las recetas para cada cafe
    private void Awake()
    {
        coffeeUIDict = new Dictionary<CoffeeType, GameObject>();

        for (int i = 0; i < coffeeTypes.Length; i++)
        {
            coffeeUIDict[coffeeTypes[i]] = coffeeRecipesPanels[i];
            coffeeRecipesPanels[i].SetActive(false);
        }
    }

    // Se comprueba que cafes estan disponibles para mostrar su receta
    public void UnlockCoffeRecipePanel(CoffeeType coffeeType)
    {
        if (coffeeUIDict.ContainsKey(coffeeType))
            coffeeUIDict[coffeeType].SetActive(true);
    }

    // Se accede a las recetas que se han desbloqueado en el dia actual en CoffeUnlockerManager
    public void UnlockRecipesForDay(int currentDay, CoffeeUnlockerManager coffeeUnlockerManager)
    {
        CoffeeType[] newRecipeUnlocked = coffeeUnlockerManager.GetAvailableCoffees(currentDay);
        foreach (var coffee in newRecipeUnlocked)
        {
            UnlockCoffeRecipePanel (coffee);
        }
    }
}
