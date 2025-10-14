using UnityEngine;

[System.Serializable]
public class Order
{
    public CoffeeAmount coffeeAm;
    public SugarAmount sugarAm;
    public IceAmount iceAm;

    // valor de precisión que el jugador debe alcanzar (1.0, 2.0, o 3.0)
    public float coffeeTarget;

    // guarda el valor del slider (0.0 a 4.0)
    public float coffeePrecision;

    // valor exacto de cucharadas de azucar que el jugador debe echar (0, 1, 2 o 3)
    public int sugarTarget;

    // guarda el valor de las cucharadas del jugador (0 a 3)
    public int sugarPrecision;

    // valor exacto de hielos que el jugador debe echar (0, 1 o 2)
    public int iceTarget;

    // guarda el valor de las cucharadas del jugador (0 a 2)
    public int icePrecision;

    public Order(CoffeeAmount coffee, SugarAmount sugar, IceAmount ice) // Constructor de los pedidos 
    {
        this.coffeeAm = coffee;
        this.sugarAm = sugar;
        this.iceAm = ice;

        //inicializamos la precision del cafe a 0
        this.coffeePrecision = 0f;
        this.coffeeTarget = GetTargetFromAmount(coffee);

        //inicializamos la precision del azucar a 0
        this.sugarPrecision = 0;
        this.sugarTarget = GetSugarTargetFromAmount(sugar);

        //inicializamos la precision del hielo a 0
        this.icePrecision = 0;
        this.iceTarget = GetIceTargetFromAmount(ice);
    }

    // Funciones utilizadas para declarar las cantidades requeridas en formato float o int
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

    private int GetSugarTargetFromAmount(SugarAmount Samount)
    {
        switch (Samount)
        {
            case SugarAmount.ninguna:
                return 0;
            case SugarAmount.una:
                return 1;
            case SugarAmount.dos:
                return 2;
            case SugarAmount.tres:
                return 3;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetIceTargetFromAmount(IceAmount Iamount)
    {
        switch (Iamount)
        {
            case IceAmount.ningun:
                return 0;
            case IceAmount.un:
                return 1;
            case IceAmount.dos:
                return 2;
            default:
                return 2; //por si algo falla
        }
    }

}

public enum CoffeeAmount { corto, medio, largo } // Se crean 3 cantidades para los cafes
public enum SugarAmount { ninguna, una, dos, tres } // Se crean 4 cantidades para el azucar
public enum IceAmount { ningun, un, dos } // Se crean 3 cantidades para los hielos
