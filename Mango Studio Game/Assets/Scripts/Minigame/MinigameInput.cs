using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    #region Variables

    [SerializeField] GameObject coffeBar; //panel que contiene la barra
    [SerializeField] ButtonUnlockManager buttonManager;
    [SerializeField] CursorManager cursorManager;
    [SerializeField] UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve
    [SerializeField] UnityEngine.UI.Slider bakeSlider;
    [SerializeField] private Image fillBakeImage;

    [SerializeField] float slideSpeed = 0.8f;
    [SerializeField] float maxAmount = 4.0f;

    [Header("Mecánica molar café")]
    [SerializeField] private GameObject molerPanel;
    [SerializeField] private Image molerFillImage;
    [SerializeField] private float molerFillSpeed = 0.5f;
    [SerializeField] private float maxFillMoler = 1f;
    [SerializeField] private float currentMolido = 0f;
    [SerializeField] private bool isMoliendo = false;

    [Header("Mecánica espumador")]
    [SerializeField] private GameObject heatPanel;
    [SerializeField] private Image curvedFillImage;
    [SerializeField] private float fillSpeed = 0.5f;
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private bool isHeating = false;

    [Header("Horneados")]
    public float tiempoHorneado = 15f;

    private Coroutine horneadoCoroutine;
    private CookState currentCookState;

    float currentSlideTime = 0f;
    bool isSliding = false, coffeeDone = false;
    
    int countSugar = 0, countIce = 0, countCover = 0, countFoodCover = 0, countWater = 0, countMilk = 0, countCondensedMilk = 0, countCream = 0, countChocolate = 0, countWhiskey = 0;

    public bool cucharaInHand = false, tazaInHand = false, vasoInHand = false, platoTazaInHand = false, tazaMilkInHand = false, iceInHand = false, coverInHand = false, 
        waterInHand = false, milkInHand = false, condensedMilkInHand = false, creamInHand = false, chocolateInHand = false, whiskeyInHand = false;

    public bool platoInHand = false, carryBagInHand = false, foodInHand = false, platoIsInEncimera = false, carryBagIsInEncimera = false, foodIsInBolsaLlevar = false, foodIsInPlato = false, foodIsInHorno = false;

    public bool tazaIsInCafetera = false, tazaIsInPlato = false, vasoIsInCafetera = false, vasoIsInTable = false, platoTazaIsInTable = false, tazaMilkIsInEspumador = false, 
        vasoTapaPuesta = false, filtroIsInCafetera = false;

    public bool coffeeServed = false, milkServed = false, heatedMilk = false, foodServed = false, foodBaked = false, cupServed = false;

    public FoodCategory foodCategoryInHand, foodCategoryInPlato, foodCategoryInCarryBag, foodCategoryInHorno;
    public object foodTypeInHand, foodTypeInPlato, foodTypeInCarryBag, foodTypeInHorno;

    PlayerOrder order;
    public FoodManager foodManager;
    public FoodOrder foodOrder;
    public OrderNoteUI orderNoteUI;

    public GameObject Taza, Vaso, TazaLeche, Plato, PlatoTaza, BolsaLlevar;
    GameObject foodInPlatoObj = null, foodInHornoObj = null, foodInBolsaLlevarObj = null;

    public Sprite vasoConTapa, vasoConCafe, vasoSinCafe, tazaSinCafe, tazaConCafe;
    public Transform puntoCafetera, puntoEspumador, puntoEncimera, puntoComida, puntoHorno, puntoMesa, puntoTazaPlato;

    #endregion

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        orderNoteUI.ResetNote();
        ResetCafe();
        ResetFoodState();
        
        ActualizarBotonCogerComida();
        ActualizarBotonCogerEnvase();
    }

    public void Update()
    {
        // Movimiento slider cantidad cafe
        if (isSliding)
        {
            currentSlideTime += Time.unscaledDeltaTime * slideSpeed; 

            // El slider se actualiza con el tiempo de deslizamiento
            coffeeSlider.value = currentSlideTime;


            if (currentSlideTime > maxAmount)
            {
                currentSlideTime = maxAmount;
                StopCoffee();
                Debug.Log("La barrita llego al limite");
            }
        }

        // Movimiento circunferencia moler cafe
        if (isMoliendo && Input.GetMouseButton(0))
        {
            currentMolido += molerFillSpeed * Time.unscaledDeltaTime;
            currentMolido = Mathf.Clamp01(currentMolido);

            molerFillImage.fillAmount = currentMolido * 0.5f;
            molerFillImage.color = Color.Lerp(Color.yellow, Color.red, currentMolido);

            if (currentMolido >= maxFillMoler)
            {
                StopMoler();
            }
        }

        // Movimiento circunferencia calentar leche
        if (isHeating && Input.GetMouseButton(0))
        {
            currentHeat += fillSpeed * Time.unscaledDeltaTime;
            currentHeat = Mathf.Clamp01(currentHeat);

            curvedFillImage.fillAmount = currentHeat;
            curvedFillImage.color = Color.Lerp(Color.blue, Color.red, currentHeat);
        }

        if ( isHeating && Input.GetMouseButtonUp(0))
        {
            StopHeating();
        }

        CheckButtons();
    }
    public void ResetCafe()
    {
        buttonManager.coffeeButton.gameObject.SetActive(true);
        buttonManager.filtroCafeteraButton.gameObject.SetActive(true);
        buttonManager.filtroButton.gameObject.SetActive(true);

        buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
        buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerPlatoTazaButton);
        buttonManager.EnableButton(buttonManager.coffeeButton);

        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);
        DisableMechanics();

        heatPanel.SetActive(false);
        molerPanel.SetActive(false);

        Taza.SetActive(false);
        Vaso.SetActive(false);
        PlatoTaza.SetActive(false);
        TazaLeche.SetActive(false);

        Image taza = Taza.GetComponent<Image>();
        taza.sprite = tazaSinCafe;

        Image vaso = Vaso.GetComponent<Image>();
        vaso.sprite = vasoSinCafe;

        currentSlideTime = currentHeat = currentMolido = 0f;
        isSliding = coffeeDone = coffeeServed = cupServed = milkServed = heatedMilk = isHeating = isMoliendo = false;
        tazaIsInCafetera = tazaIsInPlato = vasoIsInCafetera = vasoIsInTable = platoTazaIsInTable = tazaMilkIsInEspumador = filtroIsInCafetera = false;
        countSugar = countIce = countCover = countFoodCover = countWater = countMilk = countCondensedMilk = countCream = countChocolate = countWhiskey = 0;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }
    }
    public void ResetFoodState()
    {
        buttonManager.EnableButton(buttonManager.cogerPlatoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerBolsaLlevarInicioButton);
        buttonManager.EnableButton(buttonManager.bakeryButton);
        buttonManager.EnableButton(buttonManager.returnBakeryButton);

        buttonManager.DisableButton(buttonManager.hornoButton);

        buttonManager.stopHorneadoButton.gameObject.SetActive(false);
        bakeSlider.gameObject.SetActive(false);

        //platoInHand = false;
        platoIsInEncimera = foodIsInHorno = foodIsInPlato = foodIsInBolsaLlevar = carryBagIsInEncimera = false;
        foodServed = foodBaked = false;
        countFoodCover = 0;
        //foodInHand = false;

        foodCategoryInHand = FoodCategory.no;
        foodTypeInHand = null;
        foodCategoryInPlato = FoodCategory.no;
        foodTypeInHorno = null;
        foodCategoryInCarryBag = FoodCategory.no;
        foodTypeInCarryBag = null;
        foodCategoryInHorno = FoodCategory.no;
        foodTypeInHorno = null;

        Plato.SetActive(false);
        BolsaLlevar.SetActive(false);

        if (foodInPlatoObj != null)
        {
            foodInPlatoObj.SetActive(false);
            foodInPlatoObj = null;
        }

        if (foodInHornoObj != null)
        {
            foodInHornoObj.SetActive(false);
            foodInHornoObj = null;
        }

        if (foodInBolsaLlevarObj != null)
        {
            foodInBolsaLlevarObj = null;
        }
    }
    public bool TengoOtroObjetoEnLaMano()
    {
        return cucharaInHand || waterInHand || milkInHand || condensedMilkInHand || creamInHand || chocolateInHand || whiskeyInHand || iceInHand || coverInHand || foodInHand;
    }
    public void DisableMechanics()
    {
        buttonManager.DisableButton(buttonManager.sugarButton);
        buttonManager.DisableButton(buttonManager.iceButton);
        buttonManager.DisableButton(buttonManager.whiskeyButton);
        buttonManager.DisableButton(buttonManager.coverButton);
    }
    public void CheckButtons()
    {
        if (tazaInHand || vasoInHand || TengoOtroObjetoEnLaMano())
        {
            buttonManager.DisableButton(buttonManager.submitOrderButton);
            buttonManager.DisableButton(buttonManager.bakeryButton);
            buttonManager.DisableButton(buttonManager.recipesBookButton);
            buttonManager.DisableButton(buttonManager.orderNoteButton);
            buttonManager.DisableButton(buttonManager.papeleraButton);
        }
        else if (cupServed || vasoIsInTable)
        {
            buttonManager.EnableButton(buttonManager.submitOrderButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.bakeryButton);
            buttonManager.EnableButton(buttonManager.recipesBookButton);
            buttonManager.EnableButton(buttonManager.orderNoteButton);
        }

        if (heatedMilk && milkServed)
        {
            buttonManager.DisableButton(buttonManager.calentarButton);
        }
        else if (tazaMilkIsInEspumador)
        {
            buttonManager.EnableButton(buttonManager.calentarButton);
        }

        if (platoInHand || foodInHand)
        {
            buttonManager.DisableButton(buttonManager.returnBakeryButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.returnBakeryButton);
        }
        if (foodBaked)
        {
            buttonManager.DisableButton(buttonManager.hornearButton);
        }
    }
    public void ResetMinigame()
    {
        if (TengoOtroObjetoEnLaMano()) return;

        ResetCafe();
        ResetFoodState();
        ActualizarBotonCogerComida();
    }
    #region Mecanicas cafe
    public void ActualizarBotonCogerEnvase()
    {
        if (tazaInHand || tazaIsInCafetera || vasoInHand || vasoIsInCafetera)
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
    public void ActualizarBotonCogerComida()
    {
        bool canTakeFood = carryBagIsInEncimera || platoIsInEncimera && !foodInHand && !foodIsInPlato && !foodServed && !foodIsInHorno;

        Button[] botonesComida =
        {
            buttonManager.cogerBChocolateButton,
            buttonManager.cogerBZanahoriaButton,
            buttonManager.cogerBMantequillaButton,
            buttonManager.cogerBRedVelvetButton,
            buttonManager.cogerGChocolateBlButton,
            buttonManager.cogerGChocolateButton,
            buttonManager.cogerGMantequillaButton,
            buttonManager.cogerMArandanosButton,
            buttonManager.cogerMCerezaButton,
            buttonManager.cogerMPistachoButton,
            buttonManager.cogerMDulceLecheButton
        };

        foreach (Button boton in botonesComida)
        {
            if (boton == null) continue;

            if (canTakeFood)
            {
                buttonManager.EnableButton(boton);
            }
            else
            {
                buttonManager.DisableButton(boton);
            }
        }
    }
    public void ToggleTazaCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand)
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

            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
            buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
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
    public void ToggleTazaPlato()
    {
        if (TengoOtroObjetoEnLaMano())
            return;

        if (!tazaInHand && !tazaIsInPlato)
            return;

        if (!tazaIsInPlato && tazaInHand)
        {
            // Poner en el plato
            Taza.SetActive(true);
            Taza.transform.position = puntoTazaPlato.position;

            tazaInHand = false;
            tazaIsInPlato = true;
            cupServed = true;

            cursorManager.UpdateCursorTaza(true);
            DisableMechanics();
            Debug.Log($"Taza colocada: {tazaIsInPlato}");

        }
        else if (tazaIsInPlato && !tazaInHand)
        {
            //Recoger de la cafetera
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInPlato = false;
            cupServed = false;

            cursorManager.UpdateCursorTaza(false);
        }
        else
        {
            Debug.Log("No hay ninguna taza en el plato");
        }
    }
    public void ToggleVasoCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand)
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

            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
            buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
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
    public void ToggleVasoMesa()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand)
            return;

        if (!vasoInHand && !vasoIsInTable)
            return;

        if (!vasoIsInTable && vasoInHand)
        {
            // Poner en la cafetera
            Vaso.SetActive(true);
            Vaso.transform.position = puntoMesa.position;

            vasoInHand = false;
            vasoIsInTable = true;

            cursorManager.UpdateCursorVaso(true);

            DisableMechanics();
            Debug.Log($"Vaso colocado: {vasoIsInTable}");
        }
        else if (vasoIsInTable && !vasoInHand)
        {
            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInTable = false;

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
    }
    public void PlacePlatoTazaMesa()
    {
        if (platoTazaIsInTable)
            return;

        if (TengoOtroObjetoEnLaMano())
            return;

        if (!platoTazaInHand && !platoTazaIsInTable)
            return;

        if (platoTazaInHand)
        {
            // Poner en la mesa
            PlatoTaza.SetActive(true);
            PlatoTaza.transform.position = puntoMesa.position;

            platoTazaInHand = false;
            platoTazaIsInTable = true;

            cursorManager.UpdateCursorPlato(true);
            buttonManager.DisableButton(buttonManager.cogerPlatoTazaButton);
            Debug.Log($"Plato colocado: {platoTazaIsInTable}");
        }
    }
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

            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }

    }
    public void StartMoler()
    {
        if (!isMoliendo)
        {
            currentMolido = 0f;
            isMoliendo = true;

            molerPanel.SetActive(true);
            molerFillImage.fillAmount = 0f;
            molerFillImage.color = Color.yellow;
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Moliendo cafe...");
        }
    }
    public void StopMoler()
    {
        if (isMoliendo || currentMolido == maxFillMoler)
        {
            isMoliendo = false;
            molerPanel.SetActive(false);

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Cafe molido");
            buttonManager.DisableButton(buttonManager.molerButton);
            buttonManager.EnableButton(buttonManager.filtroButton);

            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
    public void TakeFiltro()
    {
        if (filtroIsInCafetera == false)
        {
            buttonManager.DisableButton(buttonManager.filtroButton);
            buttonManager.filtroButton.gameObject.SetActive(false);
            buttonManager.EnableButton(buttonManager.filtroCafeteraButton);
        }
    }
    public void PutFiltro()
    {
        if (filtroIsInCafetera == false)
        {
            filtroIsInCafetera = true;
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }

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

            buttonManager.filtroCafeteraButton.gameObject.SetActive(false);

            buttonManager.DisableButton(buttonManager.echarCafeButton);
            buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
            buttonManager.DisableButton(buttonManager.calentarButton);
            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.DisableButton(buttonManager.milkButton);
            buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
            buttonManager.DisableButton(buttonManager.condensedMilkButton);
            buttonManager.DisableButton(buttonManager.creamButton);
            buttonManager.DisableButton(buttonManager.chocolateButton);

            buttonManager.EnableButton(buttonManager.sugarButton);
            buttonManager.EnableButton(buttonManager.iceButton);
            buttonManager.EnableButton(buttonManager.coverButton);
            buttonManager.EnableButton(buttonManager.whiskeyButton);

            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }

        if (tazaIsInCafetera)
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
    #endregion

    #region Mecanicas leche
    public void CogerLeche()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            milkInHand = true;
        }
        else if (milkInHand == true)
        {
            milkInHand = false;
        }
    }
    public void EcharLecheFria()
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
        }
    }
    public void ToggleTazaLecheEspumador()
    {
        if (TengoOtroObjetoEnLaMano())
            return;
        if (!tazaMilkInHand && !tazaMilkIsInEspumador)
            return;

        if (!tazaMilkIsInEspumador && tazaMilkInHand)
        {
            // Poner en el espumador
            TazaLeche.SetActive(true);
            TazaLeche.transform.position = puntoEspumador.position;

            tazaMilkInHand = false;
            tazaMilkIsInEspumador = true;

            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.DisableButton(buttonManager.milkButton);
            buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);

            cursorManager.UpdateCursorTazaMilk(true);
            Debug.Log($"Taza con leche colocada en espumador: {tazaMilkIsInEspumador}");

        }
        else if (tazaMilkIsInEspumador && !tazaMilkInHand)
        {
            //Recoger del espumador
            TazaLeche.SetActive(false);

            tazaMilkInHand = true;
            tazaMilkIsInEspumador = false;

            cursorManager.UpdateCursorTazaMilk(false);
        }
        else
        {
            Debug.Log("No hay ninguna taza en el espumador");
        }
        if (milkServed)
        {
            buttonManager.EnableButton(buttonManager.calentarButton);
        }
    }
    public void StartHeating()
    {
        if (!isHeating && !heatedMilk)
        {
            currentHeat = 0f;
            isHeating = true;

            heatPanel.SetActive(true);
            curvedFillImage.fillAmount = 0f;
            curvedFillImage.color = Color.blue;

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Calentando la leche...");
        }
    }
    public void StopHeating()
    {
        if (isHeating)
        {
            isHeating = false;
            heatedMilk = true;

            heatPanel.SetActive(false);
            buttonManager.calentarButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.calentarButton);

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Leche calentada");
        }
    }
    public void EcharLecheCaliente()
    {
        //Si se tiene la leche caliente en la mano y el cafe no esta servido entonces se puede echar
        if (tazaMilkInHand == true && coffeeServed == false)
        {
            if (countMilk <= 1)
            {
                if (currentHeat < 0.4f)
                {
                    order.currentOrder.heatedMilkPrecision = 0;
                    Debug.Log("Leche fria echada");
                }
                else if (currentHeat < 0.8f)
                {
                    order.currentOrder.heatedMilkPrecision = 1;
                    Debug.Log("Leche caliente echada");
                }
                else
                {
                    order.currentOrder.heatedMilkPrecision = 2;
                    Debug.Log("Leche quemada echada");
                }
            }
            milkServed = true;
            TazaLeche.SetActive(false);
            tazaMilkInHand = false;
            cursorManager.UpdateCursorTazaMilk(true);
        }
    }
    #endregion

    #region Mecanica agua
    public void CogerAgua()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            waterInHand = true;
        }
        else if (waterInHand == true)
        {
            waterInHand = false;
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
    #endregion

    #region Mecanica leche condensada
    public void CogerLecheCondensada()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            condensedMilkInHand = true;
        }
        else if (condensedMilkInHand == true)
        {
            condensedMilkInHand = false;
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
    #endregion

    #region Mecanica crema
    public void CogerCrema()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            creamInHand = true;
        }
        else if (creamInHand == true)
        {
            creamInHand = false;
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
    #endregion

    #region Mecanica chocolate
    public void CogerChocolate()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            chocolateInHand = true;
        }
        else if (chocolateInHand == true)
        {
            chocolateInHand = false;
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
    #endregion

    #region Mecanica whiskey
    public void CogerWhiskey()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            whiskeyInHand = true;
        }
        else if (whiskeyInHand == true)
        {
            whiskeyInHand = false;
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
    #endregion

    #region Mecanica azucar
    public void CogerAzucar()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            cucharaInHand = true;
        }
        else if (cucharaInHand == true)
        {
            cucharaInHand = false;
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
    #endregion

    #region Mecanica hielo
    public void CogerHielo()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            iceInHand = true;
        }
        else if (iceInHand == true)
        {
            iceInHand = false;
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
    #endregion

    #region Mecanica tipo de pedido
    public void CogerTapa()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            coverInHand = true;
        }
        else if (coverInHand == true)
        {
            coverInHand = false;
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
    #endregion

    #region Mecanicas comida
    public void PlacePlatoEncimera()
    {
        if (platoIsInEncimera || carryBagIsInEncimera)
            return;

        if (TengoOtroObjetoEnLaMano())
            return;

        if (!platoInHand && !platoIsInEncimera)
            return;

        if (platoInHand)
        {
            // Poner en la encimera
            Plato.SetActive(true);
            Plato.transform.position = puntoEncimera.position;

            platoInHand = false;
            platoIsInEncimera = true;

            cursorManager.UpdateCursorPlato(true);
            buttonManager.EnableButton(buttonManager.hornoButton);
            buttonManager.DisableButton(buttonManager.cogerBolsaLlevarInicioButton);
            buttonManager.DisableButton(buttonManager.cogerPlatoInicioButton);
            Debug.Log($"Plato colocado: {platoIsInEncimera}");
        }
        ActualizarBotonCogerComida();
    }
    public void PlaceCarryBagEncimera()
    {
        if (carryBagIsInEncimera || platoIsInEncimera)
            return;

        if (TengoOtroObjetoEnLaMano())
            return;

        if (!carryBagInHand && !carryBagIsInEncimera)
            return;

        if (carryBagInHand)
        {
            // Poner en la encimera
            BolsaLlevar.SetActive(true);
            BolsaLlevar.transform.position = puntoEncimera.position;

            carryBagInHand = false;
            carryBagIsInEncimera = true;

            cursorManager.UpdateCursorCarryBag(true);
            buttonManager.EnableButton(buttonManager.hornoButton);
            buttonManager.DisableButton(buttonManager.cogerBolsaLlevarInicioButton);
            buttonManager.DisableButton(buttonManager.cogerPlatoInicioButton);
            Debug.Log($"Bolsa para llevar colocada: {carryBagIsInEncimera}");
        }
        ActualizarBotonCogerComida();
    }
    public void ToggleFoodPlato()
    {
        if (carryBagIsInEncimera) return;

        if (!foodInHand && !foodIsInPlato && !platoIsInEncimera) return;

        if (foodInHand)
        {
            GameObject foodObj = foodManager.GetFoodObject(foodCategoryInHand, foodTypeInHand);
            if (foodObj == null)
            {
                Debug.Log("No se encontro el objeto de comida correspondiente");
                return;
            }

            foodObj.SetActive(true);
            foodObj.transform.position = puntoComida.position;

            foodInPlatoObj = foodObj;
            //  Se asocia la categoria y el tipo de comida de la mano al plato
            foodCategoryInPlato = foodCategoryInHand;
            foodTypeInPlato = foodTypeInHand;

            foodIsInPlato = true;
            foodServed = true;
            foodInHand = false;

            cursorManager.UpdateCursorFood(true, foodCategoryInHand, foodTypeInHand);

            if (order.currentOrder.foodOrder != null)
            {
                order.currentOrder.foodOrder.SetFoodPrecision(foodCategoryInHand, (int)foodTypeInHand);
                order.currentOrder.foodOrder.SetCookStatePrecision(currentCookState);
            }
            else
            {
                Debug.Log("No hay comida en el pedido del cliente, no se puede establecer precision");
            }
            //  Se resetea la categoria y el tipo de comida de la mano 
            foodCategoryInHand = FoodCategory.no;
            foodTypeInHand = null;

            Debug.Log("Comida colocada en el plato");

        }
        else if (foodIsInPlato)
        {
            foodInPlatoObj.SetActive(false);
            foodIsInPlato = false;
            foodInHand = true;
            //  Se asocia la categoria y el tipo de comida del plato a la mano
            foodCategoryInHand = foodCategoryInPlato;
            foodTypeInHand = foodTypeInPlato;
            //  Se resetea la categoria y el tipo de comida del plato 
            foodCategoryInPlato = FoodCategory.no;
            foodTypeInPlato = null;

            cursorManager.UpdateCursorFood(false, foodCategoryInHand, foodTypeInHand);
            Debug.Log("Comida recogida del plato");
        }

        ActualizarBotonCogerComida();
    }
    public void ToggleFoodBolsaLlevar()
    {
        if (platoIsInEncimera) return;
        if (!foodInHand && !foodIsInBolsaLlevar && !carryBagIsInEncimera) return;

        if (foodInHand)
        {
            GameObject foodObj = foodManager.GetFoodObject(foodCategoryInHand, foodTypeInHand);
            if (foodObj == null)
            {
                Debug.Log("No se encontro el objeto de comida correspondiente");
                return;
            }

            foodInBolsaLlevarObj = foodObj;
            //  Se asocia la categoria y el tipo de comida de la mano al plato
            foodCategoryInCarryBag = foodCategoryInHand;
            foodTypeInCarryBag = foodTypeInHand;

            countFoodCover += 1;
            order.currentOrder.typeOrderFoodPrecision = countFoodCover; // Se guarda el resultado obtenido en la precision del jugador

            foodIsInBolsaLlevar = true;
            foodServed = true;
            foodInHand = false;

            cursorManager.UpdateCursorFood(true, foodCategoryInHand, foodTypeInHand);

            if (order.currentOrder.foodOrder != null)
            {
                order.currentOrder.foodOrder.SetFoodPrecision(foodCategoryInHand, (int)foodTypeInHand);
                order.currentOrder.foodOrder.SetCookStatePrecision(currentCookState);
            }
            else
            {
                Debug.Log("No hay comida en el pedido del cliente, no se puede establecer precision");
            }
            //  Se resetea la categoria y el tipo de comida de la mano 
            foodCategoryInHand = FoodCategory.no;
            foodTypeInHand = null;

            Debug.Log("Comida colocada en la bolsa para llevar");
        }
        ActualizarBotonCogerComida();
    }
    public void ToggleFoodHorno()
    {
        if (!foodInHand && !platoIsInEncimera && !carryBagIsInEncimera)
            return;

        if (foodInHand)
        {
            GameObject foodObj = foodManager.GetFoodObject(foodCategoryInHand, foodTypeInHand);
            if (foodObj == null)
            {
                Debug.Log("No se encontro el objeto de comida correspondiente");
                return;
            }

            foodObj.SetActive(true);
            foodObj.transform.position = puntoHorno.position;

            foodInHornoObj = foodObj;
            //  Se asocia la categoria y el tipo de comida de la mano al horno
            foodCategoryInHorno = foodCategoryInHand;
            foodTypeInHorno = foodTypeInHand;

            foodIsInHorno = true;
            foodInHand = false;

            cursorManager.UpdateCursorFood(true, foodCategoryInHand, foodTypeInHand);
            buttonManager.EnableButton(buttonManager.hornearButton);

            //  Se resetea la categoria y el tipo de comida de la mano 
            foodCategoryInHand = FoodCategory.no;
            foodTypeInHand = null;

            Debug.Log("Comida colocada en el horno");

        }
        else if (foodIsInHorno)
        {
            foodInHornoObj.SetActive(false);
            foodIsInHorno = false;
            foodInHand = true;
            //  Se asocia la categoria y el tipo de comida del horno a la mano
            foodCategoryInHand = foodCategoryInHorno;
            foodTypeInHand = foodTypeInHorno;
            //  Se resetea la categoria y el tipo de comida del horno 
            foodCategoryInHorno = FoodCategory.no;
            foodTypeInHorno = null;

            cursorManager.UpdateCursorFood(false, foodCategoryInHand, foodTypeInHand);
            Debug.Log($"Comida recogida del horno. Estado: {currentCookState}");
        }

        ActualizarBotonCogerComida();
    }
    public void StartHorneado()
    {
        if (!foodIsInHorno || foodInHornoObj == null || foodBaked)
            return;
        if (horneadoCoroutine != null)
        {
            StopCoroutine(horneadoCoroutine);
        }
        horneadoCoroutine = StartCoroutine(HornearCoroutine());
        foodBaked = true;
        buttonManager.stopHorneadoButton.gameObject.SetActive(true);
    }
    private IEnumerator HornearCoroutine()
    {
        Debug.Log("Comienza el horneado...");
        bakeSlider.gameObject.SetActive(true);
        bakeSlider.value = 0f;

        float elapsed = 0f;
        float tiempoIdealMin = tiempoHorneado * 0.45f;
        float tiempoIdealMax = tiempoHorneado * 0.65f;

        currentCookState = CookState.crudo;

        while (elapsed < tiempoHorneado)
        {
            elapsed += Time.deltaTime;
            bakeSlider.value = elapsed / tiempoHorneado;
            UpdateBakingBarColor(bakeSlider.value);
            yield return null;
        }

        currentCookState = CookState.quemado;
        bakeSlider.gameObject.SetActive(false);
        buttonManager.DisableButton(buttonManager.hornearButton);
        buttonManager.stopHorneadoButton.gameObject.SetActive(false);
        Debug.Log("Se ha pasado el tiempo: comida quemada");
    }
    private void UpdateBakingBarColor(float value)
    {
        Color newColor;

        if (value < 0.4f)
        {
            newColor = Color.Lerp(Color.yellow, Color.green, value * 2.5f);
        }
        else
        {
            newColor = Color.Lerp(Color.green, Color.red, (value - 0.45f) * 0.55f);
        }
        fillBakeImage.color = newColor;
    }
    public void StopHorneado()
    {
        if (horneadoCoroutine != null)
        {
            StopCoroutine(horneadoCoroutine);
            horneadoCoroutine = null;
        }

        float progress = bakeSlider.value;
        bakeSlider.gameObject.SetActive(false);

        if (progress < 0.40f)
        {
            currentCookState = CookState.crudo;
        }
        else if (progress <= 0.70f)
        {
            currentCookState = CookState.horneado;
        }
        else
        {
            currentCookState = CookState.quemado;
        }
        Debug.Log($"Horneado detenido: {currentCookState}");
        buttonManager.DisableButton(buttonManager.hornearButton);
        buttonManager.stopHorneadoButton.gameObject.SetActive(false);
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
