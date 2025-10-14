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
        currentOrder = new Order(coffee, sugar, ice); // Se genera el nuevo pedido con las cantidades generadas

        if (orderTxt != null )
        {
            if (sugar == SugarAmount.ninguna || sugar == SugarAmount.una && ice == IceAmount.ningun || ice == IceAmount.un) // Si el pedido tiene 1 o ninguna cucharada de azucar y 1 o ningun hielo
            {
                orderTxt.text = $"Quiero un caf� {coffee} con {sugar} cucharada de az�car y {ice} hielo."; // Se muestra el pedido por texto
            }
            else // Si el pedido tiene +1 cucharadas de azucar y +1 hielo
            {
                orderTxt.text = $"Quiero un caf� {coffee} con {sugar} cucharadas de az�car y {ice} hielos."; // Se muestra el pedido por texto
            }
        }

        // Informacion del pedido actual 
        if (playerPreparationTxt != null)
        {
            if (sugar == SugarAmount.ninguna || sugar == SugarAmount.una && ice == IceAmount.ningun || ice == IceAmount.un) // Si el pedido tiene 1 o ninguna cucharada de azucar y 1 o ningun hielo
            {
                playerPreparationTxt.text = $"Tienes que preparar: Caf� {currentOrder.coffeeAm} con {currentOrder.sugarAm} cucharada de az�car y {currentOrder.iceAm} hielo.";
            }
            else // Si el pedido tiene +1 cucharadas de azucar y +1 hielo
            {
                playerPreparationTxt.text = $"Tienes que preparar: Caf� {currentOrder.coffeeAm} con {currentOrder.sugarAm} cucharadas de az�car y {currentOrder.iceAm} hielo.";
            }
        }
    }
}
