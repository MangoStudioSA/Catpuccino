using UnityEngine;

[System.Serializable]
public class Order
{
    public CoffeeAmount coffeeAm;
    public SugarAmount sugarAm;

    public Order(CoffeeAmount coffee, SugarAmount sugar) // Constructor de los pedidos 
    {
        this.coffeeAm = coffee;
        this.sugarAm = sugar;
    }

}

public enum CoffeeAmount { solo, cortado, latte } // Se crean 3 cantidades para los cafes
public enum SugarAmount { ninguna, una, dos, tres } // Se crean 3 cantidades para los cafes
