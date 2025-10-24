using UnityEngine;
using TMPro;

public class CustomerOrder : MonoBehaviour
{
    public TextMeshProUGUI orderTxt;
    public TextMeshProUGUI playerPreparationTxt;
    public CoffeeUnlockerManager unlockManager;
    public TimeManager timeManager;
    public Order currentOrder;

    public void Start()
    {
        GenRandomOrder(); // Se genera el nuevo pedido
    }
    public void GenRandomOrder()
    {
        int day = timeManager.currentDay;

        CoffeeType coffeeType = unlockManager.GetRandomAvailableCoffee(day); // Se genera el tipo de cafe entre los disponibles
        SugarAmount sugar = (SugarAmount)Random.Range(0, 3); // Se genera una cantidad de azucar al azar entre los 3 tipos
        IceAmount ice = (IceAmount)Random.Range(0, 2); // Se genera una cantidad de hielo al azar entre los 2 tipos
        OrderType type = (OrderType)Random.Range(0, 2); // Se genera un tipo de pedido entre los 2 tipos

        currentOrder = new Order(coffeeType, sugar, ice, type); // Se genera el nuevo pedido con las cantidades generadas

        if (orderTxt != null)
        {
            string sugarTxt;
            string iceTxt;

            // Azúcar
            switch(sugar)
            {
                case SugarAmount.nada:
                    sugarTxt = "sin azúcar";
                    break;
                case SugarAmount.poco:
                    sugarTxt = "con poco azúcar";
                    break;
                default:
                    sugarTxt = "con mucho azúcar";
                    break;
            }

            // Hielo
            switch (ice)
            {
                case IceAmount.no:
                    iceTxt = "sin hielo";
                    break;
                default:
                    iceTxt = "con hielo";
                    break;
            }

            orderTxt.text = $"Quiero un {coffeeType} {sugarTxt} y {iceTxt}. Lo quiero para {type}.";
            playerPreparationTxt.text = $"Tienes que preparar: {coffeeType} {sugarTxt} y {iceTxt}. Es un pedido para {type}.";
        }
    }
}
