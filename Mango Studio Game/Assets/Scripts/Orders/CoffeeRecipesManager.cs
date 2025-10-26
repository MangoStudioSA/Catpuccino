using System.Collections.Generic;
using UnityEngine;

public class CoffeeRecipesManager : MonoBehaviour
{
    [Header("Cafes + recetas")]
    public CoffeeType[] coffeeTypes;
    public GameObject[] coffeeRecipesPanels;

    private Dictionary<CoffeeType, GameObject> coffeeUIDict;

    private void Awake()
    {
        coffeeUIDict = new Dictionary<CoffeeType, GameObject>();

        for (int i = 0; i < coffeeTypes.Length; i++)
        {
            coffeeUIDict[coffeeTypes[i]] = coffeeRecipesPanels[i];
            coffeeRecipesPanels[i].SetActive(false);
        }
    }

    public void UnlockCoffeRecipePanel(CoffeeType coffeeType)
    {
        if (coffeeUIDict.ContainsKey(coffeeType))
            coffeeUIDict[coffeeType].SetActive(true);
    }

    public void UnlockRecipesForDay(int currentDay, CoffeeUnlockerManager coffeeUnlockerManager)
    {
        CoffeeType[] newRecipeUnlocked = coffeeUnlockerManager.GetUnlockedCoffees(currentDay);
        foreach (var coffee in newRecipeUnlocked)
        {
            UnlockCoffeRecipePanel (coffee);
        }
    }
}
