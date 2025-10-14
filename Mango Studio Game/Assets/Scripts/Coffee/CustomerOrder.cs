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
        CoffeeAmount coffee = (CoffeeAmount)Random.Range(0, 2); // Se genera una cantidad de cafe al azar entre los 3
        SugarAmount sugar = (SugarAmount)Random.Range(0, 3); // Se genera una cantidad de azucar al azar entre los 4
        IceAmount ice = (IceAmount)Random.Range(0, 2); // Se genera una cantidad de hielo al azar entre los 3
        OrderType type = (OrderType)Random.Range(0, 1); // Se genera un tipo de pedido entre los 2
        currentOrder = new Order(coffee, sugar, ice, type); // Se genera el nuevo pedido con las cantidades generadas

        if (orderTxt != null )
        {
            if (sugar == SugarAmount.ninguna || sugar == SugarAmount.una && ice == IceAmount.ningun || ice == IceAmount.un) // Si el pedido tiene 1 o ninguna cucharada de azucar y 1 o ningun hielo
            {
                orderTxt.text = $"Quiero un café {coffee} con {sugar} cucharada de azúcar y {ice} hielo. Lo quiero para {type}."; // Se muestra el pedido por texto
            }
            else // Si el pedido tiene +1 cucharadas de azucar y +1 hielo
            {
                orderTxt.text = $"Quiero un café {coffee} con {sugar} cucharadas de azúcar y {ice} hielos. Lo quiero para {type}."; // Se muestra el pedido por texto
            }
        }

        // Informacion del pedido actual 
        if (playerPreparationTxt != null)
        {
            if (sugar == SugarAmount.ninguna || sugar == SugarAmount.una && ice == IceAmount.ningun || ice == IceAmount.un) // Si el pedido tiene 1 o ninguna cucharada de azucar y 1 o ningun hielo
            {
                playerPreparationTxt.text = $"Tienes que preparar: Café {currentOrder.coffeeAm} con {currentOrder.sugarAm} cucharada de azúcar y {currentOrder.iceAm} hielo. Es un pedido para {type}.";
            }
            else // Si el pedido tiene +1 cucharadas de azucar y +1 hielo
            {
                playerPreparationTxt.text = $"Tienes que preparar: Café {currentOrder.coffeeAm} con {currentOrder.sugarAm} cucharadas de azúcar y {currentOrder.iceAm} hielo. Es un pedido para {type}.";
            }
        }
    }
}
