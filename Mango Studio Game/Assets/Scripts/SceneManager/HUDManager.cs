using UnityEngine;
using TMPro; 

public class HUDManager : MonoBehaviour
{
    public CoffeeUnlockerManager coffeeUnlockManager;
    public FoodUnlockerManager foodUnlockerManager;
    public TimeManager timeManager;
    public static HUDManager Instance { get; private set; }

    public TextMeshProUGUI textoSatisfaccion;
    public TextMeshProUGUI availableElementsTxt;
    public TextMeshProUGUI unlockedElementsTxt;
    public TextMeshProUGUI textoMonedas;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        ShowAvailableElements();
    }

    public void UpdateMonedas(int cantidad)
    {
        textoMonedas.text = cantidad + " $";
    }

    public void UpdateSatisfaccion(float cantidad)
    {
        // "F0" formatea el número para que no tenga decimales
        textoSatisfaccion.text = $"Satisfacción: {cantidad:F0}%";
    }

    // Funcion encargada de mostrar por pantalla los cafes disponibles del dia actual
    public void ShowAvailableElements()
    {
        int day = timeManager.currentDay;
        CoffeeType[] availableCoffees = coffeeUnlockManager.GetAvailableCoffees(day);
        FoodCategory[] availableFood = foodUnlockerManager.GetAvailableFood(day);

        string message = "Disponible en la carta: \n";

        if (availableCoffees.Length > 0)
        {
            string coffeesList = string.Join(", ", availableCoffees); // Se separa cada cafe por ","
            message += $"Cafés: {coffeesList}. \n";
        } else
        {
            message += "No hay cafes disponibles. \n";
        }

        if (availableFood.Length > 0)
        {
            string foodList = string.Join(", ", availableFood); // Se separa cada comida por ","
            message += $"Comidas: {foodList}.";
        }
        else
        {
            message += "No hay comida disponible.";
        }
        availableElementsTxt.text = message;
    }

    // Funcion encargada de mostrar por pantalla los cafes desbloqueados en el dia actual
    public void ShowUnlockedElements()
    {
        int day = timeManager.currentDay;
        CoffeeType[] unlockedCoffees = coffeeUnlockManager.GetUnlockedCoffees(day);
        FoodCategory[] unlockedFood = foodUnlockerManager.GetUnlockedFood(day);

        string message = "Hoy has desbloqueado: \n";

        if (unlockedCoffees.Length > 0)
        {
            string unlockedCoffeesList = string.Join(", ", unlockedCoffees); // Se separa cada cafe por ","
            message += $"Cafés: {unlockedCoffeesList}. \n";
        }
        else
        {
            message += "";
        }

        if (unlockedFood.Length > 0)
        {
            string unlockedFoodList = string.Join(", ", unlockedFood); // Se separa cada comida por ","
            message += $"Comidas: {unlockedFoodList}. \n";
        }
        else
        {
            message += "";
        }
        unlockedElementsTxt.text = message;
    }


}