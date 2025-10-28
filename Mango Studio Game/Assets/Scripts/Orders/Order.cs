using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderId;
    public static int nextId = 1;

    public CoffeeType coffeeType; 
    public SugarAmount sugarAm;
    public IceAmount iceAm;
    public OrderType orderType;

    public FoodOrder foodOrder;

    // valor de precisión que el jugador debe alcanzar (1.0, 2.0, o 3.0)
    public float coffeeTarget;
    // guarda el valor del slider (0.0 a 4.0)
    public float coffeePrecision;

    // valor exacto de leche que el jugador debe echar (0-nada, 1-poco, 2-mucha)
    public int milkTarget;
    // guarda el valor de la leche echada por el jugador (0 a 1)
    public int milkPrecision;

    // booleano de cómo se necesita la leche (false-fria, true-caliente)
    public int heatedMilkTarget;
    // guarda el valor de como ha preparado la leche el jugador
    public int heatedMilkPrecision;

    // valor exacto de agua que el jugador debe echar (0-nada, 1-bastante)
    public int waterTarget;
    // guarda el valor del agua echada por el jugador (0 a 1)
    public int waterPrecision;

    // valor de leche condensada que el jugador debe echar (0-no, 1-si)
    public int condensedMilkTarget;
    // guarda el valor de leche condensada echada por el jugador (0 a 1)
    public int condensedMilkPrecision;

    // valor de crema que el jugador debe echar (0-no, 1-si)
    public int creamTarget;
    // guarda el valor de crema echada por el jugador (0 a 1)
    public int creamPrecision;

    // valor de chocolate que el jugador debe echar (0-no, 1-si)
    public int chocolateTarget;
    // guarda el valor de chocolate echado por el jugador (0 a 1)
    public int chocolatePrecision;

    // valor de whiskey que el jugador debe echar (0-no, 1-si)
    public int whiskeyTarget;
    // guarda el valor del whiskey echado por el jugador (0 a 1)
    public int whiskeyPrecision;

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


    public Order(CoffeeType coffeeType, SugarAmount sugar, IceAmount ice, OrderType type, FoodOrder foodOrder = null) // Constructor de los pedidos 
    {
        orderId = nextId;
        nextId++;

        this.coffeeType= coffeeType;
        this.sugarAm = sugar;
        this.iceAm = ice;
        this.orderType = type;
        this.foodOrder = foodOrder ?? new FoodOrder(FoodCategory.no);

        //inicializamos la precision del cafe a 0
        this.coffeePrecision = 0f;
        this.coffeeTarget = GetCoffeeTargetFromAmount(coffeeType);

        //inicializamos la precision de la leche a 0
        this.milkPrecision = 0;
        this.milkTarget = GetMilkTargetFromAmount(coffeeType);

        //inicializamos la precision de la leche caliente en false
        this.heatedMilkPrecision = 0;
        this.heatedMilkTarget = GetHeatMilkTargetFromAmount(coffeeType);

        //inicializamos la precision del agua a 0
        this.waterPrecision = 0;
        this.waterTarget = GetWaterTargetFromAmount(coffeeType);

        //inicializamos la precision de la leche condensada a 0
        this.condensedMilkPrecision = 0;
        this.condensedMilkTarget = GetCondensedMilkTargetFromAmount(coffeeType);

        //inicializamos la precision de la crema a 0
        this.creamPrecision = 0;
        this.creamTarget = GetCreamTargetFromAmount(coffeeType);

        //inicializamos la precision del chocolate a 0
        this.chocolatePrecision = 0;
        this.chocolateTarget = GetChocolateTargetFromAmount(coffeeType);

        //inicializamos la precision del whiskey a 0
        this.whiskeyPrecision = 0;
        this.whiskeyTarget = GetWhiskeyTargetFromAmount(coffeeType);

        //inicializamos la precision del azucar a 0
        this.sugarPrecision = 0;
        this.sugarTarget = GetSugarTargetFromAmount(sugar);

        //inicializamos la precision del hielo a 0
        this.icePrecision = 0;
        this.iceTarget = GetIceTargetFromAmount(ice, coffeeType);

        //inicializamos la precision del tipo de pedido a 0
        this.typePrecision = 0;
        this.typeTarget = GetOrderTypeTargetFromAmount(type);
    }

    // Funciones utilizadas para declarar las cantidades requeridas en formato float o int
    private float GetCoffeeTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 1.0f;
            case CoffeeType.lungo:
                return 3.0f;
            case CoffeeType.americano:
                return 1.0f;
            case CoffeeType.macchiatto:
                return 1.0f;
            case CoffeeType.latte:
                return 2.0f;
            case CoffeeType.capuccino:
                return 1.0f;
            case CoffeeType.bombon:
                return 2.0f;
            case CoffeeType.vienes:
                return 1.0f;
            case CoffeeType.frappe:
                return 2.0f;
            case CoffeeType.mocca:
                return 1.0f;
            case CoffeeType.irish:
                return 2.0f;
            default:
                return 2.0f; //por si algo falla
        }
    }
    
    private int GetMilkTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 0;
            case CoffeeType.macchiatto:
                return 1;
            case CoffeeType.latte:
                return 2;
            case CoffeeType.capuccino:
                return 1;
            case CoffeeType.bombon:
                return 0;
            case CoffeeType.vienes:
                return 0;
            case CoffeeType.frappe:
                return 0;
            case CoffeeType.mocca:
                return 1;
            case CoffeeType.irish:
                return 1;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetHeatMilkTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 0;
            case CoffeeType.macchiatto:
                return 0;
            case CoffeeType.latte:
                return 1;
            case CoffeeType.capuccino:
                return 1;
            case CoffeeType.bombon:
                return 0;
            case CoffeeType.vienes:
                return 0;
            case CoffeeType.frappe:
                return 0;
            case CoffeeType.mocca:
                return 1;
            case CoffeeType.irish:
                return 1;
            default:
                return 0; //por si algo falla
        }
    }

    private int GetWaterTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 1;
            case CoffeeType.macchiatto:
                return 0;
            case CoffeeType.latte:
                return 0;
            case CoffeeType.capuccino:
                return 0;
            case CoffeeType.bombon:
                return 0;
            case CoffeeType.vienes:
                return 0;
            case CoffeeType.frappe:
                return 0;
            case CoffeeType.mocca:
                return 0;
            case CoffeeType.irish:
                return 0;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetCondensedMilkTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 0;
            case CoffeeType.macchiatto:
                return 0;
            case CoffeeType.latte:
                return 0;
            case CoffeeType.capuccino:
                return 0;
            case CoffeeType.bombon:
                return 1;
            case CoffeeType.vienes:
                return 0;
            case CoffeeType.frappe:
                return 0;
            case CoffeeType.mocca:
                return 0;
            case CoffeeType.irish:
                return 0;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetCreamTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 0;
            case CoffeeType.macchiatto:
                return 0;
            case CoffeeType.latte:
                return 0;
            case CoffeeType.capuccino:
                return 0;
            case CoffeeType.bombon:
                return 0;
            case CoffeeType.vienes:
                return 1;
            case CoffeeType.frappe:
                return 1;
            case CoffeeType.mocca:
                return 0;
            case CoffeeType.irish:
                return 0;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetChocolateTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 0;
            case CoffeeType.macchiatto:
                return 0;
            case CoffeeType.latte:
                return 0;
            case CoffeeType.capuccino:
                return 0;
            case CoffeeType.bombon:
                return 0;
            case CoffeeType.vienes:
                return 0;
            case CoffeeType.frappe:
                return 0;
            case CoffeeType.mocca:
                return 1;
            case CoffeeType.irish:
                return 0;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetWhiskeyTargetFromAmount(CoffeeType coffeetype)
    {
        switch (coffeetype)
        {
            case CoffeeType.espresso:
                return 0;
            case CoffeeType.lungo:
                return 0;
            case CoffeeType.americano:
                return 0;
            case CoffeeType.macchiatto:
                return 0;
            case CoffeeType.latte:
                return 0;
            case CoffeeType.capuccino:
                return 0;
            case CoffeeType.bombon:
                return 0;
            case CoffeeType.vienes:
                return 0;
            case CoffeeType.frappe:
                return 0;
            case CoffeeType.mocca:
                return 0;
            case CoffeeType.irish:
                return 1;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetSugarTargetFromAmount(SugarAmount Samount)
    {
        switch (Samount)
        {
            case SugarAmount.nada:
                return 0;
            case SugarAmount.poco:
                return 1;
            case SugarAmount.mucho:
                return 2;
            default:
                return 2; //por si algo falla
        }
    }

    private int GetIceTargetFromAmount(IceAmount Iamount, CoffeeType coffeeType)
    {
        if (coffeeType == CoffeeType.frappe)
        {
            return 1;
        }
        switch (Iamount)
        {
            case IceAmount.no:
                return 0;
            case IceAmount.si:
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

public enum CoffeeType { espresso, lungo, americano, macchiatto, latte, capuccino, bombon, vienes, frappe, mocca, irish } // Se crean los tipos de cafe
//public enum CoffeeAmount { corto, medio, largo } // Se crean 3 cantidades para los cafes
public enum MilkAmount { nada, poco, mucha } // Se crean 3 cantidades de leche
public enum HeatMilk { fria, caliente, quemada } // Se crean 2 tipos de leche
public enum WaterAmount { no, si } // Se crean 2 cantidades de agua
public enum CondensedMilkAmount { no, si } // Se crean 2 cantidades de leche condensada
public enum CreamAmount { no, si } // Se crean 2 cantidades de crema
public enum ChocolateAmount { no, si } // Se crean 2 cantidades de chocolate
public enum WhiskeyAmount { no, si } // Se crean 2 cantidades de whiskey
public enum SugarAmount { nada, poco, mucho } // Se crean 3 cantidades de azucar
public enum IceAmount { no, si } // Se crean 2 cantidades de hielo
public enum OrderType { tomar, llevar } // Se crean 2 tipos de pedidos 

