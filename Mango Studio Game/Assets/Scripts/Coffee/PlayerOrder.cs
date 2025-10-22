using UnityEngine;

public class PlayerOrder : MonoBehaviour
{
    public Order currentOrder;

    public void NewOrder(Order npcOrder)
    {
        currentOrder = new Order(npcOrder.coffeeType, npcOrder.sugarAm, npcOrder.iceAm, npcOrder.orderType);
    }
}
