using UnityEngine;
using System.Collections.Generic;

public class FoodUnlockerManager : MonoBehaviour
{
    // Se crea un diccionario relacionando el dia del juego con las comidas que se desbloquean 
    private Dictionary<int, FoodCategory[]> foodUnlocks = new()
    {
        {1, new FoodCategory[] {  } },
        {2, new FoodCategory[] { FoodCategory.bizcocho } },
        {3, new FoodCategory[] {  } },
        {4, new FoodCategory[] { FoodCategory.galleta } },
        {5, new FoodCategory[] {  } },
        {6, new FoodCategory[] { FoodCategory.mufflin } },
        {7, new FoodCategory[] {  } },
    };

    // Se genera un tipo aleatorio de comida en funcion de las que se encuentren disponibles en el dia actual
    public FoodCategory GetRandomAvailableFood(int currentDay)
    {
        FoodCategory[] available = GetAvailableFood(currentDay);
        if (available.Length == 0)
        {
            Debug.Log("No hay comida disponible.");
            return FoodCategory.no;
        }
        return available[Random.Range(0, available.Length)];
    }

    // Se crea un array con todos los tipos de comida disponibles
    public FoodCategory[] GetAvailableFood(int currentDay)
    {
        List<FoodCategory> available = new List<FoodCategory>();

        for (int d = 1; d <= currentDay; d++)
        {
            if (foodUnlocks.ContainsKey(d))
            {
                foreach (FoodCategory ct in foodUnlocks[d])
                {
                    if (!available.Contains(ct))
                        available.Add(ct);
                }
            }
        }

        return available.ToArray();
    }

    // Se crea un array con los tipos de comida desbloqueados ese día
    public FoodCategory[] GetUnlockedFood(int currentDay)
    {
        int day = Mathf.Clamp(currentDay, 1, foodUnlocks.Count);
        return foodUnlocks[day];
    }

}
