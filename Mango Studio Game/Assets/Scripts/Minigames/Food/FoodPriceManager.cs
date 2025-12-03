using System.Collections.Generic;
using UnityEngine;

public class FoodPriceManager : MonoBehaviour
{
    public static FoodPriceManager Instance;

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
