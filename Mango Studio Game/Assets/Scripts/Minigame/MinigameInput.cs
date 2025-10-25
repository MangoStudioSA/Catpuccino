using UnityEngine;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    #region Variables

    [SerializeField] GameObject coffeBar; //panel que contiene la barra
    [SerializeField] ButtonUnlockManager buttonManager;
    [SerializeField] CursorManager cursorManager;
    [SerializeField] UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve

    [SerializeField] float slideSpeed = 0.8f;
    [SerializeField] float maxAmount = 4.0f;

    float currentSlideTime = 0f;
    bool isSliding = false;
    bool coffeeDone = false;
    //bool molerDone =false;

    int countSugar = 0;
    int countIce = 0;
    int countCover = 0;
    int countWater = 0;
    int countMilk = 0;
    int countCondensedMilk = 0;
    int countCream = 0;
    int countChocolate = 0;
    int countWhiskey = 0;

    public bool cucharaInHand = false;
    public bool tazaInHand = false;
    public bool vasoInHand = false;
    public bool iceInHand = false;
    public bool coverInHand = false;
    public bool waterInHand = false;
    public bool milkInHand = false;
    public bool condensedMilkInHand = false;
    public bool creamInHand = false;
    public bool chocolateInHand = false;
    public bool whiskeyInHand = false;

    bool filtroIsInCafetera = false;
    public bool coffeeServed = false;
    public bool milkServed = false;
    public bool heatMilk = false;

    PlayerOrder order;
    public GameObject Taza;
    public GameObject Vaso;
    public bool tazaIsInCafetera = false;
    public bool tazaIsInEspumador = false;
    public bool vasoIsInCafetera = false;
    public bool vasoIsInEspumador = false;
    public bool vasoTapaPuesta = false;

    public Sprite vasoConTapa;
    public Sprite vasoConCafe;
    public Sprite vasoSinCafe;
    public Sprite tazaSinCafe;
    public Sprite tazaConCafe;

    public Transform puntoCafetera;
    public Transform puntoEspumador;

    //public GameObject Filtro;
    //public GameObject FiltroCafetera;
    #endregion

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();
        buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
        buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);

        currentSlideTime = 0f;
        isSliding = false;
        coffeeDone = false;
        coffeeServed = false;

        countSugar = 0;
        countIce = 0;
        countCover = 0;
        countWater = 0;
        countMilk = 0;
        countCondensedMilk = 0;
        countCream = 0;
        countChocolate = 0;
        countWhiskey = 0;

        Taza.SetActive(false);
        Vaso.SetActive(false);
        //Filtro.SetActive(false);
        //FiltroCafetera.SetActive(false);
        Image taza = Taza.GetComponent<Image>();
        taza.sprite = tazaSinCafe;

        Image vaso = Vaso.GetComponent<Image>();
        vaso.sprite = vasoSinCafe;

        tazaIsInCafetera = false;
        tazaIsInEspumador= false;
        vasoIsInCafetera = false;
        vasoIsInEspumador = false;

        filtroIsInCafetera = false;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }

        buttonManager.EnableButton(buttonManager.coffeeButton);
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
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

        if (tazaInHand || vasoInHand)
        {
            buttonManager.DisableButton(buttonManager.submitOrderButton);
        } else if (coffeeServed)
        {
            buttonManager.EnableButton(buttonManager.submitOrderButton);
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
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Carga de cafe iniciada.");
        }
    }
    
    public void StopCoffee()
    {
        if (isSliding)
        {
            // detenemos el movimiento
            isSliding = false;
            coffeeDone = true;

            buttonManager.DisableButton(buttonManager.coffeeButton);
            buttonManager.EnableButton(buttonManager.molerButton);


            // guarda la pos del slider
            if (order != null && order.currentOrder != null)
            {
                order.currentOrder.coffeePrecision = currentSlideTime;
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Cafe detenido en: {currentSlideTime:F2}. Valor guardado.");
            } 
            else
            {
                Debug.LogWarning($"[Cliente {order.currentOrder.orderId}] Preparacion: Cafe detenido en: {currentSlideTime:F2}, pero no se pudo guardar porque no hay un pedido activo.");
            }
        }
    }
    
    public void Moler()
    {
        Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Moliendo cafe");
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.EnableButton(buttonManager.filtroButton);
        //molerDone = true;
    }

    public void ActualizarBotonCogerEnvase()
    {
        if (tazaInHand || tazaIsInCafetera || tazaIsInEspumador || vasoInHand || vasoIsInCafetera || vasoIsInEspumador)
        {
            buttonManager.DisableButton(buttonManager.cogerTazaInicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoInicioButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
        }
    }

    public void ToggleTazaCafetera()
    {
        if (TengoOtroObjetoEnLaMano())
            return;

        if (!tazaInHand && !tazaIsInCafetera)
            return;

        if (!tazaIsInCafetera && tazaInHand)
        {
            // Poner en la cafetera
            Taza.SetActive(true);
            Taza.transform.position = puntoCafetera.position;

            tazaInHand = false;
            tazaIsInCafetera = true;
            tazaIsInEspumador = false;

            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
            buttonManager.EnableButton(buttonManager.condensedMilkButton);
            buttonManager.EnableButton(buttonManager.creamButton);
            buttonManager.EnableButton(buttonManager.chocolateButton);

            cursorManager.UpdateCursorTaza(true);
            Debug.Log($"Taza colocada: {tazaIsInCafetera}");

        }
        else if (tazaIsInCafetera && !tazaInHand)
        {
            //Recoger de la cafetera
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInCafetera = false;

            cursorManager.UpdateCursorTaza(false);
        }
        else
        {
            Debug.Log("No hay ninguna taza en la cafetera");
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }

        ActualizarBotonCogerEnvase();
    }

    public void ToggleTazaEspumador()
    {
        if (TengoOtroObjetoEnLaMano())
            return;
        if (!tazaInHand && !tazaIsInEspumador)
            return;

        if (!tazaIsInEspumador && tazaInHand)
        {
            // Poner en el espumador
            Taza.SetActive(true);
            Taza.transform.position = puntoEspumador.position;

            tazaInHand = false;
            tazaIsInCafetera = false;
            tazaIsInEspumador = true;

            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.DisableButton(buttonManager.milkButton);

            cursorManager.UpdateCursorTaza(true);
            Debug.Log($"Taza colocada: {tazaIsInEspumador}");

        }
        else if (tazaIsInEspumador && !tazaInHand)
        {
            //Recoger del espumador
            Taza.SetActive(false);

            tazaInHand = true;
            tazaIsInEspumador = false;

            cursorManager.UpdateCursorTaza(false);
        }
        else
        {
            Debug.Log("No hay ninguna taza en el espumador");
        }
        if (milkServed)
        {
            buttonManager.EnableButton(buttonManager.calentarButton);
        }
        ActualizarBotonCogerEnvase();
    }

    public void ToggleVasoCafetera()
    {
        if (TengoOtroObjetoEnLaMano())
            return;

        if (!vasoInHand && !vasoIsInCafetera)
            return;

        if (!vasoIsInCafetera && vasoInHand)
        {
            // Poner en la cafetera
            Vaso.SetActive(true);
            Vaso.transform.position = puntoCafetera.position;

            vasoInHand = false;
            vasoIsInCafetera = true;
            vasoIsInEspumador = false;

            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
            buttonManager.EnableButton(buttonManager.condensedMilkButton);
            buttonManager.EnableButton(buttonManager.creamButton);
            buttonManager.EnableButton(buttonManager.chocolateButton);

            cursorManager.UpdateCursorVaso(true);
            Debug.Log($"Vaso colocado: {vasoIsInCafetera}");
        }
        else if (vasoIsInCafetera && !vasoInHand)
        {
            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInCafetera = false;

            cursorManager.UpdateCursorVaso(false);
        }
        else
        {
            Debug.Log("No hay ningun vaso en la cafetera");
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }

        ActualizarBotonCogerEnvase();
    }

    public void ToggleVasoEspumador()
    {
        if (TengoOtroObjetoEnLaMano())
            return;

        if (!vasoInHand && !vasoIsInEspumador)
            return;

        if (!vasoIsInEspumador && vasoInHand)
        {
            // Poner en el espumador
            Vaso.SetActive(true);
            Vaso.transform.position = puntoEspumador.position;

            vasoInHand = false;
            vasoIsInCafetera = false;
            vasoIsInEspumador = true;

            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.DisableButton(buttonManager.milkButton);

            cursorManager.UpdateCursorVaso(true);
            Debug.Log($"Vaso colocado: {vasoIsInEspumador}");
        }
        else if (vasoIsInEspumador && !vasoInHand)
        {
            //Recoger del espumador
            Vaso.SetActive(false);

            vasoInHand = true;
            vasoIsInEspumador = false;

            cursorManager.UpdateCursorVaso(false);
        }
        else
        {
            Debug.Log("No hay ningun vaso en el espumador");
        }
        if (milkServed)
        {
            buttonManager.EnableButton(buttonManager.calentarButton);
        }
        ActualizarBotonCogerEnvase();
    }

    public void TakeFiltro()
    {
        if (filtroIsInCafetera == false)
        {
        ////Filtro.SetActive(false);

        buttonManager.DisableButton(buttonManager.filtroButton);
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
        if (tazaIsInCafetera == true || vasoIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }

    public void EcharCafe()
    {
        bool recipienteEnCafetera = tazaIsInCafetera || vasoIsInCafetera;

        if(recipienteEnCafetera && filtroIsInCafetera != false && coffeeServed == false)
        {
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Echando cafe");
            coffeeServed = true;

            buttonManager.DisableButton(buttonManager.echarCafeButton);
            buttonManager.DisableButton(buttonManager.calentarButton);
            buttonManager.DisableButton(buttonManager.espumadorButton);
            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.DisableButton(buttonManager.milkButton);
            buttonManager.DisableButton(buttonManager.condensedMilkButton);
            buttonManager.DisableButton(buttonManager.creamButton);
            buttonManager.DisableButton(buttonManager.chocolateButton);

            //buttonManager.EnableButton(buttonManager.submitOrderButton);
            buttonManager.EnableButton(buttonManager.sugarButton);
            buttonManager.EnableButton(buttonManager.iceButton);
            buttonManager.EnableButton(buttonManager.coverButton);
            buttonManager.EnableButton(buttonManager.whiskeyButton);
        }

        if(tazaIsInCafetera)
        {
            Image taza = Taza.GetComponent<Image>();
            taza.sprite = tazaConCafe;
        }
        else if (vasoIsInCafetera)
        {
            Image vaso = Vaso.GetComponent<Image>();
            vaso.sprite = vasoConCafe;
        }
    }

    public void CalentarLeche()
    {
        if ((tazaIsInEspumador == true || vasoIsInEspumador == true) && milkServed == true && coffeeServed == false)
        {
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Calentando la leche");
            heatMilk = true;
            order.currentOrder.heatedMilkPrecision = heatMilk;
            buttonManager.DisableButton(buttonManager.calentarButton);
        }
    }
    #endregion

    #region Mecanicas 
    public void CogerLeche()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
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
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
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
    public void CogerLecheCondensada()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            condensedMilkInHand = true;
            Debug.Log("tengo la leche condensada");
        }
        else if (condensedMilkInHand == true)
        {
            condensedMilkInHand = false;
            Debug.Log("no tengo la leche condensada");
        }
    }
    public void CogerCrema()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            creamInHand = true;
            Debug.Log("tengo la crema");
        }
        else if (creamInHand == true)
        {
            creamInHand = false;
            Debug.Log("no tengo la crema");
        }
    }
    public void CogerChocolate()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            chocolateInHand = true;
            Debug.Log("tengo el chocolate");
        }
        else if (chocolateInHand == true)
        {
            chocolateInHand = false;
            Debug.Log("no tengo el chocolate");
        }
    }
    public void CogerWhiskey()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            whiskeyInHand = true;
            Debug.Log("tengo el whiskey");
        }
        else if (whiskeyInHand == true)
        {
            whiskeyInHand = false;
            Debug.Log("no tengo el whiskey");
        }
    }
    public void CogerAzucar()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
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
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
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
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
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
        //Si se tiene la leche en la mano y el cafe no esta servido entonces se puede echar
        if (milkInHand == true && coffeeServed == false)
        {
            if (countMilk <= 1)
            {
                countMilk += 1; //Se incrementa el contador de leche
                order.currentOrder.milkPrecision = countMilk; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Cantidad de leche: " + countMilk);
            }
            milkServed = true; 
            buttonManager.EnableButton(buttonManager.espumadorButton);

        }
    }
    public void EcharAgua()
    {
        //Si se tiene el agua en la mano y el cafe no esta servido entonces se puede echar 
        if (waterInHand == true && coffeeServed == false)
        {
            if (countWater <= 1)
            {
                countWater += 1; //Se incrementa el contador de agua
                order.currentOrder.waterPrecision = countWater; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado agua.");
            }
        }
    }
    public void EcharLecheCondensada()
    {
        //Si se tiene la leche condensada en la mano y el cafe no esta servido entonces se puede echar 
        if (condensedMilkInHand == true && coffeeServed == false)
        {
            if (countCondensedMilk <= 1)
            {
                countCondensedMilk += 1; //Se incrementa el contador de leche condensada
                order.currentOrder.condensedMilkPrecision = countCondensedMilk; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado leche condensada.");
            }
        }
    }
    public void EcharCrema()
    {
        //Si se tiene la crema en la mano y el cafe no esta servido entonces se puede echar
        if (creamInHand == true && coffeeServed == false)
        {
            if (countCream <= 1)
            {
                countCream += 1; //Se incrementa el contador de crema
                order.currentOrder.creamPrecision = countCream; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado crema.");
            }
        }
    }
    public void EcharChocolate()
    {
        //Si se tiene el chocolate en la mano y el cafe no esta servido entonces se puede echar
        if (chocolateInHand == true && coffeeServed == false)
        {
            if (countChocolate <= 3)
            {
                countChocolate += 1; //Se incrementa el contador de chocolate
                order.currentOrder.chocolatePrecision = countChocolate; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado chocolate.");
            }
        }
    }
    public void EcharWhiskey()
    {
        //Si se tiene el whiskey en la mano y el cafe esta servido entonces se puede echar
        if (whiskeyInHand == true && coffeeServed == true)
        {
            if (countWhiskey <= 1)
            {
                countWhiskey += 1; //Se incrementa el contador de hielo
                order.currentOrder.whiskeyPrecision = countWhiskey; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado whiskey.");
            }
        }
    }
    public void EcharAzucar()
    {
        //Si se tiene la cuchara de azucar en la mano y el cafe esta servido entonces se puede echar
        if (cucharaInHand == true && coffeeServed == true)
        {
            if (countSugar <= 1)
            {
                countSugar += 1; //Se incrementa el contador de hielo
                order.currentOrder.sugarPrecision = countSugar; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Cantidad de azucar: " + countSugar);
            }
        }
    }
    public void EcharHielo()
    {
        //Si se tiene la cuchara de hielo en la mano y el cafe esta servido entonces se puede echar
        if (iceInHand == true && coffeeServed == true) 
        {
            if (countIce <= 1)
            {
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado hielo.");
            }
        }
    }
    public void PonerTapa()
    {
        //Si se tiene la tapa en la mano y el cafe esta servido entonces se puede poner
        if (coverInHand == true && coffeeServed == true && vasoIsInCafetera)
        {
            if (countCover <= 1)
            {
                countCover += 1; //Se incrementa el contador de hielo
                order.currentOrder.typePrecision = countCover; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Tapa puesta.");
            }
            Image vaso = Vaso.GetComponent<Image>();
            vaso.sprite = vasoConTapa;
        }
    }
    public bool TengoOtroObjetoEnLaMano()
    {
        return cucharaInHand || waterInHand || milkInHand || condensedMilkInHand || creamInHand || chocolateInHand || whiskeyInHand || iceInHand || coverInHand;
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
