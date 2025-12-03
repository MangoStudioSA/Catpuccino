using System.Collections.Generic;
using UnityEngine;

// Clase encargada de vincular los precios con las comidas
public class FoodPriceManager : MonoBehaviour
{
    public static FoodPriceManager Instance;

    // Se crea un diccionario con la categoria de comida y su precio
    private Dictionary<FoodCategory, float> foodPrices = new Dictionary<FoodCategory, float>()
    {
        { FoodCategory.no, 0f },
        { FoodCategory.bizcocho, 4f },
        { FoodCategory.galleta, 3.5f },
        { FoodCategory.mufflin, 5f },
    };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Funcion para obtener el precio de la categoria de comida indicada
    public float GetBaseFoodPrice(FoodCategory foodCategory)
    {
        if (foodPrices.ContainsKey(foodCategory))
        {
            return foodPrices[foodCategory];
        }
        else
        {
            Debug.LogWarning($"No hay precio asignado para la comida {foodCategory}");
            return 0f;
        }

    }
}
