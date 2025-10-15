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
    [SerializeField] Button echarCafeButton;
    [SerializeField] Button submitOrderButton;
    [SerializeField] Button sugarButton;
    [SerializeField] Button iceButton;
    [SerializeField] Button coverButton;


    [SerializeField] float slideSpeed = 0.8f;
    [SerializeField] float maxAmount = 4.0f;

    float currentSlideTime = 0f;
    bool isSliding = false;
    bool coffeeDone = false;
    bool molerDone =false;

    int countSugar = 0;
    int countIce = 0;
    int countCover = 0;

    public bool cucharaInHand = false;
    public bool tazaInHand = false;
    public bool iceInHand = false;
    public bool coverInHand = false;

    public bool tazaIsThere = false;
    bool filtroIsInCafetera = false;
    public bool coffeeServed = false;


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
        coffeeServed = false;

        countSugar = 0;
        countIce = 0;
        countCover = 0;

        Taza.SetActive(false);
        //Filtro.SetActive(false);
        //FiltroCafetera.SetActive(false);

        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = false;
        echarCafeButton.interactable = false;
        submitOrderButton.interactable = false;
        sugarButton.interactable = false;
        iceButton.interactable = false;
        coverButton.interactable= false;

        tazaIsThere = false;
        filtroIsInCafetera = false;

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
            molerDone = true;
        }
    }

    public void PutTaza()
    {
        if (tazaIsThere == false && tazaInHand)
        {
            Taza.SetActive(true);
            tazaIsThere = true;
            tazaInHand = false;
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            echarCafeButton.interactable = true;
        }
 
    }

    public void TakeFiltro()
    {
        if (filtroIsInCafetera == false)
        { 
        //Filtro.SetActive(false);
        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = true;
        }
    }

    public void putFiltro()
    {
        if (filtroIsInCafetera == false)
        {
            filtroIsInCafetera = true;
        }
        //FiltroCafetera.SetActive(true);
        if (tazaIsThere == true && coffeeServed == false)
        {
            echarCafeButton.interactable = true;
        }
    }

    public void EcharCafe()
    {
        if(tazaIsThere != false && filtroIsInCafetera != false && coffeeServed ==false)
        {
            Debug.Log("Preparacion: Echando cafe");
            coffeeServed = true;
            echarCafeButton.interactable = false;
            submitOrderButton.interactable = true;
            sugarButton.interactable = true;
            iceButton.interactable = true;
            coverButton.interactable = true;
        }
    }

    public void EcharAzucar()
    {
        if (cucharaInHand == true && coffeeServed == true)
        {
            if (countSugar <= 3)
            {
                countSugar += 1;
                order.currentOrder.sugarPrecision = countSugar;
                Debug.Log("Cantidad de azucar: " + countSugar);
            }
        }
    }

    public void EcharHielo()
    {
        //Si se tiene la cuchara de hielo en la mano y el cafe esta servido entonces se puede echar el hielo
        if (iceInHand == true && coffeeServed == true) 
        {
            if (countIce <= 2)
            {
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce;
                Debug.Log("Cantidad de hielo: " + countIce);
            }
        }
    }

    public void PonerTapa()
    {
        //Si se tiene la tapa en la mano y el cafe esta servido entonces se puede poner la tapa
        if (coverInHand == true && coffeeServed == true)
        {
            if (countCover <= 1)
            {
                countCover += 1; //Se incrementa el contador de hielo
                order.currentOrder.typePrecision = countCover;
                Debug.Log("Tapa puesta.");
            }
        }
    }

    public void CogerAzucar()
    {
        if (cucharaInHand == false)
        { 
            cucharaInHand = true; 
        }
        else if (cucharaInHand == true)
        {
            cucharaInHand = false;
        }
    }

    public void CogerHielo()
    {
        if (iceInHand == false)
        {
            iceInHand = true;
        }
        else if (iceInHand == true)
        {
            iceInHand = false;
        }
    }

    public void CogerTapa()
    {
        if (coverInHand == false)
        {
            coverInHand = true;
        }
        else if (coverInHand == true)
        {
            coverInHand = false;
        }
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
