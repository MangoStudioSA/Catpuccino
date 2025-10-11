using UnityEngine;
using TMPro;

public class CustomerOrder : MonoBehaviour
{
    public TextMeshProUGUI orderTxt;
    public Order currentOrder;

    public void Start()
    {
        GenRandomOrder(); // Se genera el nuevo pedido
    }
    public void GenRandomOrder()
    {
        CoffeeAmount coffee = (CoffeeAmount)Random.Range(0, 2); // Se genera una cantidad de cafe al azar entre los 3
        SugarAmount sugar = (SugarAmount)Random.Range(0, 3); // Se genera una cantidad de azucar al azar entre los 3
        currentOrder = new Order(coffee, sugar); // Se genera el nuevo pedido con las cantidades generadas

        if (orderTxt != null )
        {
            if (sugar == SugarAmount.ninguna || sugar == SugarAmount.una) // Si el pedido tiene 1 o ninguna cucharada de azucar
            {
                orderTxt.text = $"Quiero un caf� {coffee} con {sugar} cucharada de az�car."; // Se muestra el pedido por texto
            }
            else // Si el pedido tiene +1 cucharadas de azucar
            {
                orderTxt.text = $"Quiero un caf� {coffee} con {sugar} cucharadas de az�car."; // Se muestra el pedido por texto
            }
        }
    }

}
