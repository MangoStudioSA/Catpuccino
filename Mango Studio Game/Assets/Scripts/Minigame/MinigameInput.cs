using UnityEngine;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    #region Variables
    /*[SerializeField] Button coffeeButton;
    [SerializeField] Button molerButton;
    [SerializeField] Button filtroButton;
    [SerializeField] Button filtroCafeteraButton;
    [SerializeField] Button echarCafeButton;
    [SerializeField] Button submitOrderButton;
    [SerializeField] Button sugarButton;
    [SerializeField] Button iceButton;
    [SerializeField] Button coverButton;
    [SerializeField] Button waterButton;*/

    [SerializeField] GameObject coffeBar; //panel que contiene la barra
    [SerializeField] ButtonUnlockManager buttonManager;
    [SerializeField] UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve

    [SerializeField] float slideSpeed = 0.8f;
    [SerializeField] float maxAmount = 4.0f;

    float currentSlideTime = 0f;
    bool isSliding = false;
    bool coffeeDone = false;
    bool molerDone =false;

    int countSugar = 0;
    int countIce = 0;
    int countCover = 0;
    int countWater = 0;
    int countMilk = 0;

    public bool cucharaInHand = false;
    public bool tazaInHand = false;
    public bool iceInHand = false;
    public bool coverInHand = false;
    public bool waterInHand = false;
    public bool milkInHand = false;

    public bool tazaIsThere = false;
    bool filtroIsInCafetera = false;
    public bool coffeeServed = false;

    PlayerOrder order;
    public GameObject Taza;
    //public GameObject Filtro;
    //public GameObject FiltroCafetera;
    #endregion

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
        countWater = 0;
        countMilk = 0;

        Taza.SetActive(false);
        //Filtro.SetActive(false);
        //FiltroCafetera.SetActive(false);

        /*filtroButton.interactable = false;
        filtroCafeteraButton.interactable = false;
        echarCafeButton.interactable = false;
        submitOrderButton.interactable = false;
        sugarButton.interactable = false;
        iceButton.interactable = false;
        coverButton.interactable= false;
        waterButton.interactable = false;*/

        tazaIsThere = false;
        filtroIsInCafetera = false;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }

        /*if(coffeeButton != null)
        {
            coffeeButton.interactable = true;
        }
        if (molerButton != null)
        {
            molerButton.interactable = false;
        }*/

        buttonManager.EnableButton(buttonManager.coffeeButton);
        buttonManager.DisableButton(buttonManager.molerButton);
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

    #region Mecanicas minijuego
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

            /*if(coffeeButton != null)
            {
                coffeeButton.interactable = false;
            }

            if(molerButton != null)
            {
                molerButton.interactable = true;
            }*/
            buttonManager.DisableButton(buttonManager.coffeeButton);
            buttonManager.EnableButton(buttonManager.molerButton);


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
        Debug.Log("Preparacion: Moliendo cafe");
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.EnableButton(buttonManager.filtroButton);
        molerDone = true;

        /*if (molerButton != null && molerButton.interactable)
        {
            Debug.Log("Preparacion: Moliendo cafe");

            molerButton.interactable = false;
            filtroButton.interactable = true;
            //Filtro.SetActive(true);
            molerDone = true;
        }*/
    }

    public void PutTaza()
    {
        if (tazaIsThere == false && tazaInHand)
        {
            Taza.SetActive(true);
            tazaIsThere = true;
            tazaInHand = false;
            //waterButton.interactable = true;
            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            //echarCafeButton.interactable = true;
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }

    }

    public void TakeFiltro()
    {
        if (filtroIsInCafetera == false)
        {
        ////Filtro.SetActive(false);
        //filtroButton.interactable = false;
        buttonManager.DisableButton(buttonManager.filtroButton);
        //filtroCafeteraButton.interactable = true;
        buttonManager.EnableButton(buttonManager.filtroCafeteraButton);

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
            //echarCafeButton.interactable = true;
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }

    public void EcharCafe()
    {
        if(tazaIsThere != false && filtroIsInCafetera != false && coffeeServed ==false)
        {
            Debug.Log("Preparacion: Echando cafe");
            coffeeServed = true;
            buttonManager.DisableButton(buttonManager.echarCafeButton);
            buttonManager.EnableButton(buttonManager.submitOrderButton);
            buttonManager.EnableButton(buttonManager.sugarButton);
            buttonManager.EnableButton(buttonManager.iceButton);
            buttonManager.EnableButton(buttonManager.coverButton);
            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.DisableButton(buttonManager.milkButton);

            /*echarCafeButton.interactable = false;
            submitOrderButton.interactable = true;
            sugarButton.interactable = true;
            iceButton.interactable = true;
            coverButton.interactable = true;
            waterButton.interactable = false;*/
        }
    }
    #endregion

    #region Mecanicas 
    public void CogerLeche()
    {
        if ((milkInHand == false && coverInHand == false && cucharaInHand == false && iceInHand == false && waterInHand == false))
        {
            milkInHand = true;
            Debug.Log("tengo la leche");
        }
        else if (milkInHand == true)
        {
            milkInHand = false;
            Debug.Log("no tengo la leche");
        }
    }

    public void CogerAgua()
    {
        if ((coverInHand == false && cucharaInHand == false && iceInHand == false && waterInHand == false && milkInHand == false))
        {
            waterInHand = true;
            Debug.Log("tengo el agua");
        }
        else if (waterInHand == true)
        {
            waterInHand = false;
            Debug.Log("no tengo el agua");
        }
    }

    public void CogerAzucar()
    {
        if ((coverInHand == false && cucharaInHand == false && iceInHand == false && waterInHand == false && milkInHand == false))
        {
            cucharaInHand = true;
            Debug.Log("tengo el azucar");
        }
        else if (cucharaInHand == true)
        {
            cucharaInHand = false;
            Debug.Log("no tengo el azucar");
        }
    }

    public void CogerHielo()
    {
        if ((coverInHand == false && iceInHand == false && cucharaInHand == false && waterInHand == false && milkInHand == false))
        {
            iceInHand = true;
            Debug.Log("tengo el hielo");
        }
        else if (iceInHand == true)
        {
            iceInHand = false;
            Debug.Log("no tengo el hielo");
        }
    }

    public void CogerTapa()
    {
        if (coverInHand == false && iceInHand == false && cucharaInHand == false && waterInHand == false && milkInHand == false)
        {
            coverInHand = true;
            Debug.Log("tengo la tapa");
        }
        else if (coverInHand == true)
        {
            coverInHand = false;
            Debug.Log("no tengo la tapa");
        }
    }
    public void EcharLeche()
    {
        if (milkInHand == true && coffeeServed == false)
        {
            if (countMilk <= 1)
            {
                countMilk += 1;
                order.currentOrder.milkPrecision = countMilk;
                Debug.Log("Cantidad de leche: " + countMilk);
            }
        }
    }

    public void EcharAgua()
    {
        if (waterInHand == true && coffeeServed == false)
        {
            if (countWater <= 1)
            {
                countWater += 1;
                order.currentOrder.waterPrecision = countWater;
                Debug.Log("Has echado agua.");
            }
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
            if (countIce <= 1)
            {
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce;
                Debug.Log("Has echado hielo.");
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

    #endregion

    #region Sonidos
    public void BotonDownMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeMachine();
    }

    public void BotonUpMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeReady();
    }
    #endregion
}
