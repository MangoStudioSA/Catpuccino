using UnityEngine;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    [SerializeField] GameObject coffeBar; //panel que contiene la barra
    [SerializeField] UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve

    [SerializeField] Button coffeeButton;
    [SerializeField] Button molerButton;
    [SerializeField] Button filtroButton;
    [SerializeField] Button filtroCafeteraButton;

    [SerializeField] float slideSpeed = 0.8f;
    [SerializeField] float maxAmount = 4.0f;

    float currentSlideTime = 0f;
    bool isSliding = false;
    bool coffeeDone = false;

    public bool tazaIsThere = false;


    public GameObject Taza;
    //public GameObject Filtro;
    //public GameObject FiltroCafetera;

    PlayerOrder order;

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        currentSlideTime = 0f;
        isSliding = false;
        coffeeDone = false;

        Taza.SetActive(false);
        //Filtro.SetActive(false);
        //FiltroCafetera.SetActive(false);

        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = false;

        tazaIsThere = false;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }

        if(coffeeButton != null)
        {
            coffeeButton.interactable = true;
        }
        if (molerButton != null)
        {
            molerButton.interactable = false;
        }
    }

    public void Update()
    {
        //movimiento
        if (isSliding)
        {
            currentSlideTime += Time.unscaledDeltaTime * slideSpeed; //no se por que pero solo me funciona si uso unscaled (no lo entiendo)

            //el slider se actualiza con el tiempo de deslizamiento
            coffeeSlider.value = currentSlideTime;


            if (currentSlideTime > maxAmount)
            {
                currentSlideTime = maxAmount;
                StopCoffee();
                Debug.Log("La barrita llego al limite");
            }
        }
    }

    
    public void StartCoffee()
    {
        if  (!isSliding && !coffeeDone)
        {
            //reiniciamos la pos de la barra
            currentSlideTime = 0f;

            isSliding = true;
            Debug.Log("Preparacion: Carga de cafe iniciada.");
        }
    }
    
    public void StopCoffee()
    {
        if (isSliding)
        {
            // detenemos el movimiento
            isSliding = false;
            coffeeDone = true;

            if(coffeeButton != null)
            {
                coffeeButton.interactable = false;
            }

            if(molerButton != null)
            {
                molerButton.interactable = true;
            }

            // guarda la pos del slider
            if (order != null && order.currentOrder != null)
            {
                order.currentOrder.coffeePrecision = currentSlideTime;
                Debug.Log($"Preparacion: Cafe detenido en: {currentSlideTime:F2}. Valor guardado.");
            }
            else
            {
                Debug.LogWarning($"Preparacion: Cafe detenido en: {currentSlideTime:F2}, pero no se pudo guardar porque no hay un pedido activo.");
            }
        }
    }
    
    public void Moler()
    {
        if (molerButton != null && molerButton.interactable)
        {
            Debug.Log("Preparacion: Moliendo cafe");

            molerButton.interactable = false;
            filtroButton.interactable = true;
            //Filtro.SetActive(true);
        }
    }

    public void PutTaza()
    {
        if (tazaIsThere == false)
        {
            Taza.SetActive(true);
            tazaIsThere = true;
        }
    }
    public void TakeFiltro()
    {
        //Filtro.SetActive(false);
        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = true;
    }
    public void putFiltro()
    {
        //FiltroCafetera.SetActive(true);
    }

    public void BotonDownMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeMachine();
    }

    public void BotonUpMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeReady();
    }
}
