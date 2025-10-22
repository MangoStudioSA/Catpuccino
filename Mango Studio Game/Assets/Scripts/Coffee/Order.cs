using UnityEngine;

[System.Serializable]
public class Order
{
    public CoffeeType coffeeType; 
    //public CoffeeAmount coffeeAm;
    public SugarAmount sugarAm;
    public IceAmount iceAm;
    public OrderType orderType;

    // valor de precisión que el jugador debe alcanzar (1.0, 2.0, o 3.0)
    public float coffeeTarget;
    // guarda el valor del slider (0.0 a 4.0)
    public float coffeePrecision;

    // valor exacto de cucharadas de azucar que el jugador debe echar (0-nada, 1-poco o 2-mucho)
    public int sugarTarget;
    // guarda el valor de las cucharadas del jugador (0 a 2)
    public int sugarPrecision;

    // valor exacto de hielos que el jugador debe echar (0-sin o 1-con)
    public int iceTarget;
    // guarda el valor de las cucharadas del jugador (0 a 1)
    public int icePrecision;

    // especifica el tipo de pedido que quiere el cliente (0-tomar o 1-llevar )
    public int typeTarget;
    // guarda el valor del si el jugador ha colocado o no la tapa (0-tomar o 1-llevar)
    public int typePrecision;



    public Order(CoffeeType coffeeType, SugarAmount sugar, IceAmount ice, OrderType type) // Constructor de los pedidos 
    {
        this.coffeeType= coffeeType;
        this.sugarAm = sugar;
        this.iceAm = ice;
        this.orderType = type;

        //inicializamos la precision del cafe a 0
        this.coffeePrecision = 0f;
        this.coffeeTarget = GetTargetFromAmount(coffeeType);

        //inicializamos la precision del azucar a 0
        this.sugarPrecision = 0;
        this.sugarTarget = GetSugarTargetFromAmount(sugar);

        //inicializamos la precision del hielo a 0
        this.icePrecision = 0;
        this.iceTarget = GetIceTargetFromAmount(ice);

        //inicializamos la precision del hielo a 0
        this.typePrecision = 0;
        this.typeTarget = GetOrderTypeTargetFromAmount(type);
    }

    // Funciones utilizadas para declarar las cantidades requeridas en formato float o int
    private float GetTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 1.0f;
            case CoffeeType.americano:
                return 1.0f;
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
            default:
                return 2; //por si algo falla
        }
    }

    private int GetOrderTypeTargetFromAmount(OrderType ordertype)
    {
        switch (ordertype)
        {
            case OrderType.tomar:
                return 0;
            case OrderType.llevar:
                return 1;
            default:
                return 2; //por si algo falla
        }
    }

}

public enum CoffeeType { espresso, americano } // Se crean los tipos de cafe
public enum CoffeeAmount { corto, medio, largo } // Se crean 3 cantidades para los cafes
public enum SugarAmount { ninguna, una, dos, tres } // Se crean 4 cantidades para el azucar
public enum IceAmount { ningun, un, dos, tres } // Se crean 3 cantidades para los hielos
public enum OrderType { tomar, llevar } // Se crean 2 tipos de pedidos 
