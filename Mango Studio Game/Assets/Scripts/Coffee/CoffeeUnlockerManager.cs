using UnityEngine;
using System.Collections.Generic;

public class CoffeeUnlockerManager : MonoBehaviour
{
    // Se crea un diccionario relacionando el dia del juego con los cafes y comidas que se desbloquean 
    private Dictionary<int, CoffeeType[]> coffeUnlocks = new Dictionary<int, CoffeeType[]>()
    {
        {1, new CoffeeType[] { CoffeeType.espresso, CoffeeType.americano } },
        {2, new CoffeeType[] { CoffeeType.macchiatto } },
        {3, new CoffeeType[] { CoffeeType.latte, CoffeeType.capuccino } },
        {4, new CoffeeType[] { } },
        {5, new CoffeeType[] { CoffeeType.bombón, CoffeeType.vienés, CoffeeType.frappé } },
        {6, new CoffeeType[] { } },
        {7, new CoffeeType[] { CoffeeType.mocca, CoffeeType.irish } },
    };

    // Se genera un tipo aleatorio de cafe en funcion de los que se encuentren disponibles en el dia actual
    public CoffeeType GetRandomAvailableCoffee(int currentDay)
    {
        int day = Mathf.Clamp(currentDay, 1, coffeUnlocks.Count);
        CoffeeType[] available = coffeUnlocks[day];

        return available[Random.Range(0, available.Length)];
    }

    // Se crea un array con todos los tipos de cafes disponibles
    public CoffeeType[] GetAvailableCoffees(int currentDay)
    {
        List<CoffeeType> available = new List<CoffeeType>();

        for (int d = 1; d <= currentDay; d++)
        {
            if (coffeUnlocks.ContainsKey(d))
            {
                foreach (CoffeeType ct in coffeUnlocks[d])
                {
                    if (!available.Contains(ct))
                        available.Add(ct);
                }
            }
        }

        return available.ToArray();
    }

    // Se crea un array con los tipos de cafes desbloqueados ese día
    public CoffeeType[] GetUnlockedCoffees(int currentDay)
    {
        int day = Mathf.Clamp(currentDay, 1, coffeUnlocks.Count);
        return coffeUnlocks[day];
    }

}
