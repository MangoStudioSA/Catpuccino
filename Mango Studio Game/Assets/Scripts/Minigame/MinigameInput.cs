using UnityEngine;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    [SerializeField] GameObject coffeBar; //panel que contiene la barra
    [SerializeField] UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve

    [SerializeField] float slideSpeed = 0.1f;
    [SerializeField] float maxAmount = 4.0f;

    float currentSlideTime = 0f;
    bool isSliding = false;

    PlayerOrder order;

    private void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }

        if (coffeBar != null)
        {
            coffeBar.SetActive(false);
        }
    }

    public void Update()
    {
        if (isSliding)
        {
            currentSlideTime += Time.unscaledDeltaTime * slideSpeed;

            if (currentSlideTime > maxAmount)
            {
                currentSlideTime = maxAmount;
                StopCoffeeSlide();
                Debug.Log("La barrita llego al limite");
            }

            coffeeSlider.value = currentSlideTime;
        }
    }
    public void AddCoffe()
    {
        if (isSliding)
        {
            Debug.Log("Preparacion: Se ha detenido el cafe");
            StopCoffeeSlide();
        }
        else
        {
            Debug.Log("Preparacion: Añadiendo cafe");
            StartCoffeeSlide();
        }
    }
    
    private void StartCoffeeSlide()
    {
        //reiniciamos la pos de la barra
        currentSlideTime = 0f;
        coffeeSlider.value = 0f;

        coffeBar.SetActive (true);
        isSliding = true;
    }
    
    private void StopCoffeeSlide()
    {
        isSliding = false;
        coffeBar.SetActive(false);

        //guarda la pos del slider
        if (order != null && order.currentOrder !=null)
        {
            order.currentOrder.coffeePrecision = currentSlideTime;

            Debug.Log($"Slider de Café detenido en: {currentSlideTime:F2}. Cantidad: {order.currentOrder.coffeeAm}");
        }
        else
        {
            Debug.LogWarning($"Slider de Café detenido en: {currentSlideTime:F2}, pero no se pudo guardar porque no hay un pedido activo (order.currentOrder es null).");
        }

    }
    
    public void Moler()
    {
        Debug.Log("Preparacion: Moliendo cafe");
    }
}
