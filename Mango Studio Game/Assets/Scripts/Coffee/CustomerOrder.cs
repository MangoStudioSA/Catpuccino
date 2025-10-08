using UnityEngine;
using TMPro;

public class CustomerOrder : MonoBehaviour
{
    public TextMeshProUGUI orderTxt;
    public Order currentOrder;

    public void Start()
    {
        GenRandomOrder();
    }
    public void GenRandomOrder()
    {
        CoffeeAmount coffee = (CoffeeAmount)Random.Range(0, 2); // Se genera una cantidad de cafe al azar entre los 3
        SugarAmount sugar = (SugarAmount)Random.Range(0, 3); // Se genera una cantidad de azucar al azar entre los 3
        currentOrder = new Order(coffee, sugar); // Se genera el nuevo pedido con las cantidades generadas

        if (orderTxt != null )
        {
            if (sugar == SugarAmount.ninguna || sugar == SugarAmount.una)
            {
                orderTxt.text = $"Quiero un café {coffee} con {sugar} cucharada de azúcar."; // Se muestra el pedido por texto
            }
            else
            {
                orderTxt.text = $"Quiero un café {coffee} con {sugar} cucharadas de azúcar."; // Se muestra el pedido por texto
            }
        }
    }

}
