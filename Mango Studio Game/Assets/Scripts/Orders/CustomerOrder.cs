using UnityEngine;
using TMPro;

// Clase encargada de generar el pedido del cliente
public class CustomerOrder : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI orderTxt;
    [SerializeField] private CoffeeUnlockerManager coffeeUnlocker;
    [SerializeField] private FoodUnlockerManager foodUnlocker;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private OrderNoteUI orderNoteUI;

    [Header("Datos pedido actual")]
    public Order currentOrder { get; private set; }
    private FoodOrder foodOrder = null;

    [Range(0f, 1f), Tooltip("Probabilidad de que el cliente pida comida")]
    [SerializeField] private float foodRequestChance = 0.8f;

    // Funcion para generar un pedido aleatorio
    public void GenRandomOrder()
    {
        if (timeManager == null || coffeeUnlocker == null)
        {
            Debug.LogError("[CustomerOrder] Faltan referencias (TimeManager o CoffeeUnlockerManager).");
            return;
        }

        int currentDay = timeManager.currentDay;

        foodOrder = null;
        // Si se han desbloqueado las comidas, se genera una al azar del tipo desbloqueado
        if (foodUnlocker != null && Random.value < foodRequestChance)
        {
            FoodCategory randomCategory = foodUnlocker.GetRandomAvailableFood(currentDay);
            if (randomCategory != FoodCategory.no)
                foodOrder = new FoodOrder(randomCategory);
        }

        CoffeeType coffeeType = coffeeUnlocker.GetRandomAvailableCoffee(currentDay); // Se genera el tipo de cafe entre los disponibles
        SugarAmount sugar = (SugarAmount)Random.Range(0, System.Enum.GetValues(typeof(SugarAmount)).Length); // Se genera una cantidad de azucar al azar entre los 3 tipos
        IceAmount ice = (IceAmount)Random.Range(0, System.Enum.GetValues(typeof(IceAmount)).Length); // Se genera una cantidad de hielo al azar entre los 2 tipos
        OrderType type = (OrderType)Random.Range(0, System.Enum.GetValues(typeof(OrderType)).Length); // Se genera un tipo de pedido entre los 2 tipos

        currentOrder = new Order(coffeeType, sugar, ice, type, foodOrder); // Se genera el nuevo pedido con las cantidades generadas
        orderNoteUI.SetCurrentOrder(currentOrder);

        if (orderTxt != null) orderTxt.text = BuildOrderText(currentOrder);
    }

    // Funcion encargada de generar el texto mostrado del cliente para el pedido
    private string BuildOrderText(Order order)
    {
        if (order == null) return "Error: pedido no generado";

        string sugarText = order.sugarAm switch
        {
            SugarAmount.nada => "sin azúcar",
            SugarAmount.poco => "con poco azúcar",
            _ => "con mucho azúcar"
        };
        string iceText = order.iceAm switch
        {
            IceAmount.no => "sin hielo",
            _ => "con hielo"
        };
        string foodText = order.foodOrder != null
            ? order.foodOrder.GetFoodDescription()
            : " No quiero comida.";
        string baseText = order.coffeeType == CoffeeType.frappe
            ? $"Quiero un {order.coffeeType} {sugarText}.{foodText} Lo quiero para {order.orderType}."
            : $"Quiero un {order.coffeeType} {sugarText} y {iceText}.{foodText} Lo quiero para {order.orderType}.";

        return baseText;
    }
}
