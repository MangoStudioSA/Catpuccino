using UnityEngine;
using TMPro; 

public class HUDManager : MonoBehaviour
{
    public CoffeeUnlockerManager unlockManager;
    public TimeManager timeManager;
    public static HUDManager Instance { get; private set; }

    public TextMeshProUGUI textoSatisfaccion;
    public TextMeshProUGUI availableCoffeesTxt;
    public TextMeshProUGUI unlockedCoffeesTxt;
    public TextMeshProUGUI textoMonedas;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        ShowAvailableCoffees();
    }

    public void UpdateMonedas(int cantidad)
    {
        textoMonedas.text = cantidad + " $";
    }

    public void UpdateSatisfaccion(float cantidad)
    {
        // "F0" formatea el n�mero para que no tenga decimales
        textoSatisfaccion.text = $"Satisfacci�n: {cantidad:F0}%";
    }

    // Funcion encargada de mostrar por pantalla los cafes disponibles del dia actual
    public void ShowAvailableCoffees()
    {
        int day = timeManager.currentDay;
        CoffeeType[] availableCoffees = unlockManager.GetAvailableCoffees(day);

        if (availableCoffees.Length == 0)
        {
            availableCoffeesTxt.text = "No hay cafes disponibles.";
            return;
        }

        string coffeesList = string.Join(", ", availableCoffees); // Se separa cada cafe por ","
        availableCoffeesTxt.text = $"Caf�s disponibles hoy: {coffeesList}.";
    }

    // Funcion encargada de mostrar por pantalla los cafes desbloqueados en el dia actual
    public void ShowUnlockedCoffees()
    {
        int day = timeManager.currentDay;
        CoffeeType[] unlockedCoffees = unlockManager.GetUnlockedCoffees(day);

        if (unlockedCoffees.Length == 0)
        {
            unlockedCoffeesTxt.text = "No has desbloqueado ning�n caf�.";
            return;
        }

        string unlockedCoffeesList = string.Join(", ", unlockedCoffees); // Se separa cada cafe por ","
        unlockedCoffeesTxt.text = $"�Has desbloqueado los siguientes cafes: {unlockedCoffeesList}!";
    }


}