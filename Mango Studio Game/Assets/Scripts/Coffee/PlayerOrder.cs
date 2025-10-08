using UnityEngine;
using TMPro;

public class PlayerOrder : MonoBehaviour
{
    public Order currentOrder;
    public TextMeshProUGUI playerPreparationTxt;

    public void NewOrder(Order npcOrder)
    {
        currentOrder = new Order(npcOrder.coffeeAm, npcOrder.sugarAm);
    }

    void updateOrderText()
    {
        playerPreparationTxt.text = $"Tienes que preparar un cafe {currentOrder.coffeeAm} con {currentOrder.sugarAm} de azucar.";
    }
}
