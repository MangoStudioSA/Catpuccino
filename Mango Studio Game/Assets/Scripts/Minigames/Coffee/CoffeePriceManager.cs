using System.Collections.Generic;
using UnityEngine;

// Clase encargada de vincular los precios y los tipos de cafes
public class CoffeePriceManager : MonoBehaviour
{
    public static CoffeePriceManager Instance;

    // Se crea un diccionario con los tipos de cafes y los precios asociados
    private Dictionary<CoffeeType, float> coffeePrices = new Dictionary<CoffeeType, float>()
    {
        { CoffeeType.espresso, 1.5f },
        { CoffeeType.lungo, 1.7f },
        { CoffeeType.americano, 2.0f },
        { CoffeeType.macchiatto, 2.5f },
        { CoffeeType.latte, 3.0f },
        { CoffeeType.capuccino, 3.0f },
        { CoffeeType.bombon, 3.5f },
        { CoffeeType.vienes, 3.5f },
        { CoffeeType.frappe, 4.0f },
        { CoffeeType.mocca, 4.5f },
        { CoffeeType.irish, 5.0f }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Funcion para obtener el precio del cafe introducido
    public float GetBaseCoffeePrice(CoffeeType coffeeType)
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
