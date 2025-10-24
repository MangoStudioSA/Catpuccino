using System.Collections.Generic;
using UnityEngine;

public class CoffeePriceManager : MonoBehaviour
{
    public static CoffeePriceManager Instance;

    private Dictionary<CoffeeType, float> coffeePrices = new Dictionary<CoffeeType, float>()
    {
        { CoffeeType.espresso, 1.5f },
        { CoffeeType.americano, 2.0f },
        { CoffeeType.macchiatto, 2.5f },
        { CoffeeType.latte, 3.0f },
        { CoffeeType.capuccino, 3.0f },
        { CoffeeType.bombón, 3.5f },
        { CoffeeType.vienés, 3.5f },
        { CoffeeType.frappé, 4.0f },
        { CoffeeType.mocca, 3.5f },
        { CoffeeType.irish, 5.0f }
    };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public float GetBasePrice(CoffeeType coffeeType)
    {
        if (coffeePrices.ContainsKey(coffeeType))
        {
            return coffeePrices[coffeeType];
        }
        else
        {
            Debug.LogWarning($"No hay precio asignado para el cafe {coffeeType}");
            return 0f;
        }
            
    }
}
