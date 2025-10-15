using UnityEngine;
using TMPro;

public class CustomerOrder : MonoBehaviour
{
    public TextMeshProUGUI orderTxt;
    public TextMeshProUGUI playerPreparationTxt;
    public Order currentOrder;

    public void Start()
    {
        GenRandomOrder(); // Se genera el nuevo pedido
    }
    public void GenRandomOrder()
    {
        CoffeeAmount coffee = (CoffeeAmount)Random.Range(0, 2); // Se genera una cantidad de cafe al azar entre los 3 tipos
        SugarAmount sugar = (SugarAmount)Random.Range(0, 3); // Se genera una cantidad de azucar al azar entre los 4 tipos
        IceAmount ice = (IceAmount)Random.Range(0, 3); // Se genera una cantidad de hielo al azar entre los 4 tipos
        OrderType type = (OrderType)Random.Range(0, 1); // Se genera un tipo de pedido entre los 2 tipos

        currentOrder = new Order(coffee, sugar, ice, type); // Se genera el nuevo pedido con las cantidades generadas

        if(orderTxt != null)
        {
            string sugarTxt;
            string iceTxt;

            // Azúcar
            switch(sugar)
            {
                case SugarAmount.ninguna:
                    sugarTxt = "sin azúcar";
                    break;
                case SugarAmount.una:
                    sugarTxt = "con 1 cucharada de azúcar";
                    break;
                default:
                    sugarTxt = $"con {(int)sugar} cucharadas de azúcar";
                    break;
            }

            // Hielo
            switch (ice)
            {
                case IceAmount.ningun:
                    iceTxt = "sin hielo";
                    break;
                case IceAmount.un:
                    iceTxt = "con 1 hielo";
                    break;
                default:
                    iceTxt = $"con {(int)ice} hielos";
                    break;
            }

            orderTxt.text = $"Quiero un café {coffee} {sugarTxt} y {iceTxt}. Lo quiero para {type}.";
            playerPreparationTxt.text = $"Tienes que preparar: Café {coffee} {sugarTxt} y {iceTxt}. Es un pedido para {type}.";
        }
    }
}
