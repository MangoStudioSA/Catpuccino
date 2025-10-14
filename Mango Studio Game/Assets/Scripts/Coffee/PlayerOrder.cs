using UnityEngine;

public class PlayerOrder : MonoBehaviour
{
    public Order currentOrder;

    public void NewOrder(Order npcOrder)
    {
        currentOrder = new Order(npcOrder.coffeeAm, npcOrder.sugarAm, npcOrder.iceAm);
    }
}
