using UnityEngine;

[System.Serializable]
public class Order
{
    public CoffeeAmount coffeeAm;
    public SugarAmount sugarAm;

    // valor de precisión que el jugador debe alcanzar (1.0, 2.0, o 3.0)
    public float coffeeTarget;

    // guarda el valor del slider (0.0 a 4.0)
    public float coffeePrecision;

    public Order(CoffeeAmount coffee, SugarAmount sugar) // Constructor de los pedidos 
    {
        this.coffeeAm = coffee;
        this.sugarAm = sugar;

        //inicializamos la precision a 0
        this.coffeePrecision = 0f;
        this.coffeeTarget = GetTargetFromAmount(coffee);
    }

    private float GetTargetFromAmount(CoffeeAmount amount)
    {
        switch (amount)
        {
            case CoffeeAmount.corto:
                return 1.0f;
            case CoffeeAmount.medio:
                return 2.0f;
            case CoffeeAmount.largo:
                return 3.0f;
            default:
                return 2.0f; //por si algo falla
        }
    }

}

public enum CoffeeAmount { corto, medio, largo } // Se crean 3 cantidades para los cafes
public enum SugarAmount { ninguna, una, dos, tres } // Se crean 3 cantidades para los cafes
