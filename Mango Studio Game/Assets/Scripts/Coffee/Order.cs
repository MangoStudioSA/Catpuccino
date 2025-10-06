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

public enum CoffeeAmount { Poco, Medio, Mucho } // Se crean 3 cantidades para los cafes
public enum SugarAmount { Poco, Medio, Mucho } // Se crean 3 cantidades para los cafes
