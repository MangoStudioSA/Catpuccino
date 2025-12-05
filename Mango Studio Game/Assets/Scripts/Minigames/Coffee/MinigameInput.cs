using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

// Clase principal del minijuego de cafes - Gestiona los ingredientes, el resultado del jugador etc
public class MinigameInput : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    public PlayerOrder order;
    public OrderNoteUI orderNoteUI;
    public TutorialManager tutorialManager;
    public ButtonUnlockManager buttonManager;
    public CursorManager cursorManager;
    public GameProgressManager progressManager;
    public FoodMinigameInput foodMinigameInput;
    public CoffeeContainerManager coffeeContainerManager;

    [Header("Mecánica cantidad café")]
    public UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve
    public Image coffeeSliderFillImage;
    public float slideSpeed = 0.8f;
    public float maxAmount = 4.0f;
    float currentSlideTime = 0f;
    bool isSliding = false;
    public bool coffeeDone = false;
    public Color coffeeFillColor;

    [Header("Mecánica echar café")]
    public RectTransform needle;
    public float rotationSpeed = 150f;
    public float maxAngle = 180f;
    public float currentAngle = 0f;
    public bool isServing = false;
    public bool movingRight = true;
    public bool coffeeServed = false;
    private float normalizedPrecision = 0f;

    [Header("Mecánica moler café")]
    [SerializeField] private GameObject molerPanel;
    [SerializeField] private GameObject palancaDown;
    [SerializeField] private Image molerFillImage;
    [SerializeField] private float molerFillSpeed = 0.4f;
    [SerializeField] private float maxFillMoler = 1f;
    [SerializeField] private float currentMolido = 0f;
    [SerializeField] private bool isMoliendo = false;

    [Header("Mecánica espumador")]
    [SerializeField] private GameObject heatPanel;
    [SerializeField] private Image curvedFillImage;
    [SerializeField] private float fillSpeed = 0.05f;
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private bool isHeating = false;
    [SerializeField] public bool heatedMilk = false;

    [Header("Variables café")]
    public int countSugar = 0; 
    public int countIce = 0;
    public int countCover = 0;
    public int countWater = 0;
    public int countMilk = 0;
    public int countCondensedMilk = 0;
    public int countCream = 0;
    public int countChocolate = 0;
    public int countWhiskey = 0;
    public bool milkServed = false;
    public bool cMilkServed = false;
    public bool cupServed = false;

    public bool filtroInHand = false, cucharaInHand = false, tazaMilkInHand = false, iceInHand = false,
        waterInHand = false, milkInHand = false, condensedMilkInHand = false, creamInHand = false, chocolateInHand = false, whiskeyInHand = false;

    public bool tazaMilkIsInEspumador = false;
    public bool filtroIsInCafetera = false;
    #endregion

    #region Sprites
    [Header("Objetos fisicos")]
    public GameObject TazaLeche; 
    public GameObject Filtro;
    public GameObject Espumador;
    public GameObject Balda;

    [Header("Puntos transform")]
    public Transform puntoEspumador;
    public Transform puntoFiltroCafetera;

    [Header("Objetos ingredientes")]
    public Material defaultMaterial;
    public Material glowMaterial;
    public Image waterImage;
    public Image milkImage;
    public Image milkCupImage;
    public Image sugarImage;
    public Sprite sugarSpoonImage;
    public Image creamImage;
    public Sprite creamSpoonImage;
    public Image condensedMilkImage;
    public Image chocolateImage;
    public Image whiskeyImage;
    public Image iceImage;
    public Sprite iceSpoonWithIceImage;
    public Sprite iceSpoonWithoutIceImage;

    [Header("Sprites mecánicas")]
    public Sprite creamWithSpoon;
    public Sprite creamWithoutSpoon;
    public Sprite filtroImg;
    public Sprite filtroCafeteraImg;
    public Sprite coverImg;

    [Header("Sprites objetos")]
    public Sprite espumadorNormal;
    public Sprite espumadorShort;
    public Sprite boton1_N;
    public Sprite boton1_P;
    public Sprite boton2_N;
    public Sprite boton2_P;
    public Sprite boton3_N;
    public Sprite boton3_P;
    public Sprite botonCantidadCafe_N;
    public Sprite botonCantidadCafe_P;

    [Header("Configuracion sprites")]
    public Sprite currentSprite;
    public Sprite baseCupSprite;
    #endregion

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        orderNoteUI.ResetNote();
        ResetCafe();
        order.currentOrder.ResetSteps();

        CoffeeFoodManager.Instance.ResetPanels();
        UpdateStartSprites();
    }

    public void Update()
    {
        CheckButtons();
        HandleCoffeeSlider();
        HandleMoler();
        HandleHeating();

        if (isServing && !coffeeServed) MoveNeedle();
    }

    #region Funciones auxiliares

    // Funcion encargada de resetear las variables del minijuego del cafe 
    public void ResetCafe()
    {
        if (TengoOtroObjetoEnLaMano() || filtroInHand || coffeeContainerManager.tazaInHand || tazaMilkInHand || coffeeContainerManager.vasoInHand || coffeeContainerManager.platoTazaInHand) return;

        buttonManager.filtroCafeteraButton.gameObject.SetActive(false);
        buttonManager.filtroButton.gameObject.SetActive(false);

        buttonManager.EnableButton(buttonManager.cogerTazaPInicioButton);
        buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
        buttonManager.EnableButton(buttonManager.cogerTazaB2InicioButton);

        buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerVasoPInicioButton);
        buttonManager.EnableButton(buttonManager.cogerVasoB2InicioButton);

        buttonManager.EnableButton(buttonManager.cogerPlatoTazaButton);
        buttonManager.EnableButton(buttonManager.cogerPlatoTazaPButton);
        buttonManager.EnableButton(buttonManager.cogerPlatoTazaB2Button);

        buttonManager.EnableButton(buttonManager.coffeeButton);

        buttonManager.DisableButton(buttonManager.sugarButton);
        buttonManager.DisableButton(buttonManager.iceButton);
        buttonManager.DisableButton(buttonManager.submitOrderButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
        buttonManager.DisableButton(buttonManager.calentarButton);
        buttonManager.DisableButton(buttonManager.echarCafeButton);
        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);
        DisableMechanics();

        Espumador.SetActive(progressManager.heatedMilkEnabled);
        heatPanel.SetActive(false);
        molerPanel.SetActive(false);
        palancaDown.SetActive(false);

        coffeeContainerManager.Taza.SetActive(false);
        coffeeContainerManager.Vaso.SetActive(false);
        coffeeContainerManager.PlatoTaza.SetActive(false);
        TazaLeche.SetActive(false);
        Filtro.SetActive(false);
        buttonManager.molerButton.gameObject.SetActive(true);

        UpdateStartSprites();
        CoffeeFoodManager.Instance.ResetCoffeePanel();

        currentSprite = baseCupSprite = null;
        currentSlideTime = currentHeat = currentMolido = currentAngle = 0f;
        isSliding = isServing = movingRight = coffeeDone = coffeeServed = cupServed = milkServed = cMilkServed = heatedMilk = isHeating = isMoliendo = false;
        coffeeContainerManager.tazaIsInCafetera = coffeeContainerManager.tazaIsInPlato = coffeeContainerManager.vasoIsInCafetera = coffeeContainerManager.vasoIsInTable = coffeeContainerManager.platoTazaIsInTable = tazaMilkIsInEspumador = filtroIsInCafetera = false;
        countSugar = countIce = countCover = countWater = countMilk = countCondensedMilk = countCream = countChocolate = countWhiskey = 0;
        coffeeContainerManager.finalCoverIsPremium = coffeeContainerManager.finalCupIsPremium = coffeeContainerManager.finalPlateIsPremium = coffeeContainerManager.finalVasoIsPremium = false;
        coffeeContainerManager.cupIsPremium = coffeeContainerManager.plateIsPremium = coffeeContainerManager.vasoIsPremium = coffeeContainerManager.coverIsPremium = false;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }
    }

    // Funcion encargada de actualizar los sprites iniciales de los objetos interactuables y botones visuales
    private void UpdateStartSprites()
    {
        Image filtro = Filtro.GetComponent<Image>(); // Filtro
        filtro.sprite = filtroImg;

        Image cantidadCafeBut = buttonManager.coffeeButton.GetComponent<Image>(); // Boton cantidad cafe
        cantidadCafeBut.sprite = botonCantidadCafe_N;

        Image echarCafeBut = buttonManager.echarCafeButton.GetComponent<Image>(); // Boton echar cafe
        echarCafeBut.sprite = boton1_N;

        Image pararEcharCafeBut = buttonManager.pararEcharCafeButton.GetComponent<Image>(); // Boton parar echar cafe
        pararEcharCafeBut.sprite = boton2_N;

        Image pararCalentarLecheBut = buttonManager.calentarButton.GetComponent<Image>(); // Boton espumador
        pararCalentarLecheBut.sprite = boton3_N;
    }

    // Funcion encargada de la gestion del slider de la cantidad de cafe
    private void HandleCoffeeSlider()
    {
        // Movimiento slider cantidad cafe
        if (isSliding)
        {
            currentSlideTime += Time.unscaledDeltaTime * slideSpeed;

            // El slider se actualiza con el tiempo de deslizamiento
            coffeeSlider.value = currentSlideTime;
            coffeeSliderFillImage.color = coffeeFillColor;

            if (currentSlideTime > maxAmount)
            {
                currentSlideTime = maxAmount;
                StopCoffee();
                //Debug.Log("La barrita llego al limite");
            }
        }
    }

    // Funcion encargada de gestionar la interaccion con la palanca para moler el cafe
    private void HandleMoler()
    {
        if (!coffeeDone) return; 

        if (isMoliendo && Input.GetMouseButton(0))
        {
            currentMolido += molerFillSpeed * Time.unscaledDeltaTime;
            currentMolido = Mathf.Clamp01(currentMolido);

            molerFillImage.fillAmount = currentMolido * 0.5f;
            molerFillImage.color = Color.Lerp(Color.yellow, Color.red, currentMolido);

            if (currentMolido == maxFillMoler)
            {
                StopMoler();
            }
        }
    }

    // Funcion encargada de gestionar la interaccion para calentar la leche
    private void HandleHeating()
    {
        if (!tazaMilkIsInEspumador) return;

        // Movimiento circunferencia calentar leche
        if (isHeating && Input.GetMouseButton(0))
        {
            currentHeat += fillSpeed * Time.unscaledDeltaTime;
            currentHeat = Mathf.Clamp01(currentHeat);

            curvedFillImage.fillAmount = currentHeat;
            curvedFillImage.color = Color.Lerp(Color.blue, Color.red, currentHeat);
        }

        if (isHeating && Input.GetMouseButtonUp(0))
        {
            StopHeating();
        }

        // Espumador
        if (isHeating)
        {
            buttonManager.espumadorButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.espumadorButton);
        }
        else
        {
            buttonManager.espumadorButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.espumadorButton);
        }
    }

    // Funcion para comprobar si el jugador tiene algo en la mano
    public bool TengoOtroObjetoEnLaMano()
    {
        return  cucharaInHand || waterInHand || milkInHand || condensedMilkInHand || creamInHand || chocolateInHand || whiskeyInHand || iceInHand || coffeeContainerManager.coverInHand;
    }

    // Funcion para deshabilitar los botones de los ingredientes
    public void DisableMechanics()
    {
        buttonManager.DisableButton(buttonManager.whiskeyButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);
    }

    // Funcion para habilitar los botones de los ingredientes
    public void EnableMechanics()
    {
        buttonManager.EnableButton(buttonManager.whiskeyButton);
        buttonManager.EnableButton(buttonManager.waterButton);
        buttonManager.EnableButton(buttonManager.milkButton);
        buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.EnableButton(buttonManager.condensedMilkButton);
        buttonManager.EnableButton(buttonManager.creamButton);
        buttonManager.EnableButton(buttonManager.chocolateButton);
    }

    // Funcion para comprobar los botones segun el estado del minijuego o tutorial
    public void CheckButtons()
    {
        Balda.SetActive(progressManager.condensedMilkEnabled);

        if (tutorialManager.isRunningT1)
        {
            buttonManager.DisableButton(buttonManager.shopButton);

            int step = tutorialManager.currentStep;

            if (step == 12) buttonManager.EnableButton(buttonManager.echarCafeButton);
            if (step == 15 || step == 17) buttonManager.EnableButton(buttonManager.papeleraButton);
            else buttonManager.DisableButton(buttonManager.papeleraButton);
            if (step == 17 && cupServed) buttonManager.EnableButton(buttonManager.submitOrderButton);
            else buttonManager.DisableButton(buttonManager.submitOrderButton);
            if (step == 20) buttonManager.EnableButton(buttonManager.endDeliveryButton);
            else    buttonManager.DisableButton(buttonManager.endDeliveryButton);
            if (step >= 21 && step <= 24) buttonManager.DisableButton(buttonManager.gameButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.endDeliveryButton);
            buttonManager.EnableButton(buttonManager.shopButton);
            buttonManager.EnableButton(buttonManager.gameButton);
        }

        if (tutorialManager.isRunningT2)
        {
            int step = tutorialManager.currentStep;

            if (step == 0) buttonManager.DisableButton(buttonManager.bakeryButton);
            if (step == 1) buttonManager.EnableButton(buttonManager.bakeryButton);
        }
        else if (tutorialManager.isRunningT3)
            buttonManager.DisableButton(buttonManager.bakeryButton);
        else
            buttonManager.EnableButton(buttonManager.bakeryButton);

        if (!tutorialManager.isRunningT1 && !tutorialManager.isRunningT2 && !tutorialManager.isRunningT3)
        {
            if (coffeeContainerManager.tazaInHand || coffeeContainerManager.vasoInHand || TengoOtroObjetoEnLaMano() || coffeeContainerManager.platoTazaInHand || filtroInHand)
            {
                buttonManager.DisableButton(buttonManager.submitOrderButton);
                buttonManager.DisableButton(buttonManager.bakeryButton);
                buttonManager.DisableButton(buttonManager.recipesBookButton);
                buttonManager.DisableButton(buttonManager.orderNoteButton);
                buttonManager.DisableButton(buttonManager.bakeryButton);
                buttonManager.DisableButton(buttonManager.papeleraButton);
            }
            else
            {
                buttonManager.EnableButton(buttonManager.submitOrderButton);
                buttonManager.EnableButton(buttonManager.bakeryButton);
                buttonManager.EnableButton(buttonManager.recipesBookButton);
                buttonManager.EnableButton(buttonManager.orderNoteButton);
                buttonManager.EnableButton(buttonManager.bakeryButton);
                buttonManager.EnableButton(buttonManager.papeleraButton);
            }
        }

        if (cMilkServed)
            buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);

        if (heatedMilk)
            buttonManager.DisableButton(buttonManager.calentarButton);
    }
    #endregion

    #region Mecanicas cafe

    // Funcion que activa el slider de la cantidad de cafe
    public void StartCoffee()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 8) return;
        if (TengoOtroObjetoEnLaMano() || coffeeContainerManager.vasoInHand || coffeeContainerManager.tazaInHand || coffeeContainerManager.platoTazaInHand || tazaMilkInHand) return;

        if  (!isSliding && !coffeeDone)
        {
            MiniGameSoundManager.instance.PlayButtonDown();
            // Se reinicia la posicion del slider
            currentSlideTime = 0f;

            isSliding = true;
            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Carga de cafe iniciada.");
        }
    }

    // Funcion que desactiva el slider de la cantidad de cafe
    public void StopCoffee()
    {
        if (isSliding)
        {
            MiniGameSoundManager.instance.PlayButtonUp();

            // Detenemos el movimiento
            isSliding = false;
            coffeeDone = true;

            buttonManager.DisableButton(buttonManager.coffeeButton);
            buttonManager.EnableButton(buttonManager.molerButton);

            // Guarda la pos del slider
            if (order != null && order.currentOrder != null)
            {
                order.currentOrder.coffeePrecision = currentSlideTime;
                //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Cafe detenido en: {currentSlideTime:F2}. Valor guardado.");
            } 
            else
            {
                //Debug.LogWarning($"[Cliente {order.currentOrder.orderId}] Preparacion: Cafe detenido en: {currentSlideTime:F2}, pero no se pudo guardar porque no hay un pedido activo.");
            }

            Image cantidadCafeBut = buttonManager.coffeeButton.GetComponent<Image>();
            cantidadCafeBut.sprite = botonCantidadCafe_P;

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 8)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }

    // Funcion que activa la interaccion de moler el cafe
    public void StartMoler()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 9) return;
        if (TengoOtroObjetoEnLaMano() || coffeeContainerManager.vasoInHand || coffeeContainerManager.tazaInHand || coffeeContainerManager.platoTazaInHand || tazaMilkInHand) return;
        if (!coffeeDone) return;

        if (!isMoliendo)
        {
            MiniGameSoundManager.instance.PlayMolerCafe();

            currentMolido = 0f;
            isMoliendo = true;

            molerPanel.SetActive(true);
            molerFillImage.fillAmount = 0f;
            molerFillImage.color = Color.yellow;
            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Moliendo cafe...");
        }
    }

    // Funcion que desactiva la interaccion de moler el cafe
    public void StopMoler()
    {
        if (isMoliendo && currentMolido == maxFillMoler)
        {
            isMoliendo = false;
            molerPanel.SetActive(false);

            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Cafe molido");
            buttonManager.DisableButton(buttonManager.molerButton);
            buttonManager.filtroButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.filtroButton);

            buttonManager.molerButton.gameObject.SetActive(false);
            palancaDown.SetActive(true);

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 9)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }

    // Funcion para coger el filtro
    public void TakeFiltro()
    {
        if (TengoOtroObjetoEnLaMano() || coffeeContainerManager.tazaInHand || coffeeContainerManager.vasoInHand || coffeeContainerManager.platoTazaInHand || tazaMilkInHand) return;

        if (!filtroIsInCafetera)
        {
            filtroInHand = true;

            DragController.Instance.StartDragging(filtroImg); // Coger con el cursor

            buttonManager.DisableButton(buttonManager.filtroButton);
            buttonManager.EnableButton(buttonManager.filtroCafeteraButton);
            buttonManager.filtroButton.gameObject.SetActive(false);
            buttonManager.filtroCafeteraButton.gameObject.SetActive(true);
        }
    }

    // Funcion para colocar el filtro
    public void PutFiltro()
    {
        if (filtroIsInCafetera == false)
        {
            filtroIsInCafetera = true;
            filtroInHand = false;

            // Poner en la cafetera
            Filtro.SetActive(true);
            Filtro.transform.position = puntoFiltroCafetera.position;
            Image filtro = Filtro.GetComponent<Image>();
            filtro.sprite = filtroCafeteraImg;

            DragController.Instance.StopDragging(); // Soltar del cursor
            buttonManager.DisableButton(buttonManager.filtroCafeteraButton);

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 10)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }

        if (coffeeContainerManager.tazaIsInCafetera == true || coffeeContainerManager.vasoIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }

    // Funcion para activar la interaccion de echar el cafe
    public void StartServingCoffee()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 12) return;
        if (coffeeServed) return;
        if (TengoOtroObjetoEnLaMano() || coffeeContainerManager.tazaInHand || coffeeContainerManager.vasoInHand || coffeeContainerManager.platoTazaInHand || tazaMilkInHand) return;

        bool recipienteEnCafetera = coffeeContainerManager.tazaIsInCafetera || coffeeContainerManager.vasoIsInCafetera;
        if (!recipienteEnCafetera || !filtroIsInCafetera) return;

        //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Echando cafe...");

        MiniGameSoundManager.instance.PlayButtonDown();
        MiniGameSoundManager.instance.PlayMachinePour();
        isServing = true;
        movingRight = true;

        Image echarCafeBut = buttonManager.echarCafeButton.GetComponent<Image>();
        echarCafeBut.sprite = boton1_P;

        buttonManager.DisableButton(buttonManager.echarCafeButton);
        buttonManager.EnableButton(buttonManager.pararEcharCafeButton);
    }

    // Funcion para mover la aguja de echar el cafe de izquierda a derecha
    public void MoveNeedle()
    {
        float step = rotationSpeed * Time.deltaTime;

        if (movingRight) // Movimiento derecha
        {
            currentAngle += step;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                movingRight = false;
            }
        }
        else // Movimiento izquierda
        {
            currentAngle -= step;
            if (currentAngle <= 0f)
            {
                currentAngle = 0f;
                movingRight = true;
            }
        }

        needle.localEulerAngles = new Vector3(0,0, -currentAngle);
        normalizedPrecision = currentAngle / maxAngle;
    }

    // Funcion para desactivar la interaccion de echar el cafe
    public void StopServingCoffee()
    {
        if (!isServing || coffeeServed) return;

        isServing = false;
        coffeeServed = true;

        MiniGameSoundManager.instance.StopMachinePour();
        MiniGameSoundManager.instance.PlayButtonDown();
        MiniGameSoundManager.instance.PlayEcharCafe();

        if (order != null && order.currentOrder != null)
        {
            order.currentOrder.coffeeServedPrecision = normalizedPrecision;
            order.currentOrder.stepsPerformed.Add(OrderStep.AddCoffee);
            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Echar cafe detenido en: {normalizedPrecision}");
        }

        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);
        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);

        buttonManager.EnableButton(buttonManager.sugarButton);
        buttonManager.EnableButton(buttonManager.iceButton);

        Image pararEcharCafeBut = buttonManager.pararEcharCafeButton.GetComponent<Image>();
        pararEcharCafeBut.sprite = boton2_P;

        // Actualizar sprites taza/vaso
        if (coffeeContainerManager.tazaIsInCafetera)
        {
            CoffeeType currentType = DetermineCoffeeType();
            bool isCup = true;
            baseCupSprite = GetBaseCoffeeSprite(currentType, isCup);
            currentSprite = baseCupSprite;

            Image taza = coffeeContainerManager.Taza.GetComponent<Image>();
            taza.sprite = currentSprite;
        }
        else if (coffeeContainerManager.vasoIsInCafetera)
        {
            CoffeeType currentType = DetermineCoffeeType();
            bool isCup = false;
            currentSprite = GetBaseCoffeeSprite(currentType, isCup);
            Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
            vaso.sprite = currentSprite;
        }

        coffeeContainerManager.ActualizarBotonCogerEnvase();

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 12)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
    }

    // Actualizar el sprite de cafe/ingrediente actual
    public void UpdateCupSprite(bool inPlato)
    {
        Image taza = coffeeContainerManager.Taza.GetComponent<Image>();
        bool cupEmpty = !coffeeServed && !milkServed && countWater == 0 && countMilk == 0 && countCream == 0 && countWhiskey == 0 && countChocolate == 0 && countCondensedMilk == 0;
        Sprite newBaseSprite = null;

        // Taza vacia
        if (cupEmpty)
        {
            newBaseSprite = inPlato ? coffeeContainerManager.currentSkin.tazaSinCafeP : coffeeContainerManager.currentSkin.tazaSinCafe;
        }
        // Agua sin cafe
        else if (countWater != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? coffeeContainerManager.currentSkin.tazaNWaterP : coffeeContainerManager.currentSkin.tazaNWater;
        }
        // Leche / crema / leche condensada sin cafe
        else if ((countMilk != 0 || countCondensedMilk != 0 || countCream != 0) && !coffeeServed)
        {
            newBaseSprite = inPlato ? coffeeContainerManager.currentSkin.tazaNMilkP : coffeeContainerManager.currentSkin.tazaNMilk;
        }
        // Chocolate sin cafe
        else if (countChocolate != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? coffeeContainerManager.currentSkin.tazaNChocolateP : coffeeContainerManager.currentSkin.tazaNChocolate;
        }
        // Whiskey sin cafe
        else if (countWhiskey != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? coffeeContainerManager.currentSkin.tazaNWhiskeyP : coffeeContainerManager.currentSkin.tazaNWhiskey;
        }
        // Cafe preparado
        else
        {
            CoffeeType type = DetermineCoffeeType();
            bool hasLatteArtCard = PlayerDataManager.instance.HasCard("CartaLatteArt");

            newBaseSprite = GetBaseCoffeeSprite(type, true);
            if (inPlato)
                newBaseSprite = CheckFinalCupPlate(newBaseSprite, hasLatteArtCard);
        }

        taza.sprite = newBaseSprite;
        currentSprite = newBaseSprite;

        if (!inPlato)
            baseCupSprite = newBaseSprite;
    }

    // Vincular la preparacion realizada con el tipo de cafe asociado
    private CoffeeType DetermineCoffeeType()
    {
        if (!milkServed && countWater == 0 && countWhiskey == 0 && countCondensedMilk == 0 && countCream == 0 && countChocolate == 0)
        {
            if (order.currentOrder.coffeePrecision <= 1.5f) return CoffeeType.espresso;
            if (order.currentOrder.coffeePrecision >= 3f) return CoffeeType.lungo;
        }

        if (countWater != 0 && !milkServed) return CoffeeType.americano;
        if (countCondensedMilk > 0 && !milkServed) return CoffeeType.bombon;

        if (milkServed)
        {
            if (!heatedMilk) return CoffeeType.macchiatto;    

            if (countChocolate > 0) return CoffeeType.mocca;

            if (countWhiskey > 0) return CoffeeType.irish;

            if (countMilk > 1) return CoffeeType.latte;

            return CoffeeType.capuccino;
        }

        if (countCream > 0 && countIce <= 0) return CoffeeType.vienes;
        if (countIce > 0 && countCream > 0) return CoffeeType.frappe;

        return CoffeeType.lungo;
    } 
    
    // Devolver el sprite segun el cafe y si es taza o vaso
    private Sprite GetBaseCoffeeSprite(CoffeeType type, bool isCup)
    {
        bool hasLatteArtCard = PlayerDataManager.instance.HasCard("CartaLatteArt");
        switch(type)
        {
            case CoffeeType.espresso: return isCup ? coffeeContainerManager.currentSkin.tazaNEspresso : coffeeContainerManager.currentSkin.vasoNEspresso;
            case CoffeeType.lungo: return isCup ? coffeeContainerManager.currentSkin.tazaNAmericanoLungo : coffeeContainerManager.currentSkin.vasoNAmericanoLungo;
            case CoffeeType.americano: return isCup ? coffeeContainerManager.currentSkin.tazaNAmericanoLungo : coffeeContainerManager.currentSkin.vasoNAmericanoLungo;
            case CoffeeType.latte: 
                return isCup ? (hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMocaIrishLatteD : coffeeContainerManager.currentSkin.tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? coffeeContainerManager.currentSkin.vasoNMocaIrishLatteD : coffeeContainerManager.currentSkin.vasoNMocaIrishLatte);
            case CoffeeType.capuccino: return isCup ? coffeeContainerManager.currentSkin.tazaNCappuccino : coffeeContainerManager.currentSkin.vasoNCappuccino;
            case CoffeeType.irish:
                return isCup ? (hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMocaIrishLatteD : coffeeContainerManager.currentSkin.tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? coffeeContainerManager.currentSkin.vasoNMocaIrishLatteD : coffeeContainerManager.currentSkin.vasoNMocaIrishLatte);
            case CoffeeType.mocca:
                return isCup ? (hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMocaIrishLatteD : coffeeContainerManager.currentSkin.tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? coffeeContainerManager.currentSkin.vasoNMocaIrishLatteD : coffeeContainerManager.currentSkin.vasoNMocaIrishLatte);
            case CoffeeType.macchiatto:
                return isCup ? (hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMachiatoBombonD : coffeeContainerManager.currentSkin.tazaNMachiatoBombon)
                             : (hasLatteArtCard ? coffeeContainerManager.currentSkin.vasoNMachiatoBombonD : coffeeContainerManager.currentSkin.vasoNMachiatoBombon);
            case CoffeeType.bombon:
                return isCup ? (hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMachiatoBombonD : coffeeContainerManager.currentSkin.tazaNMachiatoBombon)
                             : (hasLatteArtCard ? coffeeContainerManager.currentSkin.vasoNMachiatoBombonD : coffeeContainerManager.currentSkin.vasoNMachiatoBombon);
            case CoffeeType.vienes:
                return isCup ? (hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNVienesD : coffeeContainerManager.currentSkin.tazaNVienes)
                             : (hasLatteArtCard ? coffeeContainerManager.currentSkin.vasoNVienesD : coffeeContainerManager.currentSkin.vasoNVienes);
            case CoffeeType.frappe: return isCup ? coffeeContainerManager.currentSkin.tazaNFrappe : coffeeContainerManager.currentSkin.vasoNFrappe;
        }
        return isCup ? coffeeContainerManager.currentSkin.tazaSinCafe : coffeeContainerManager.currentSkin.vasoSinTapa;
    }

    // Vincular el sprite actual de la taza con su plato
    private Sprite CheckFinalCupPlate(Sprite sprite, bool hasLatteArtCard)
    {
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNEspresso)
            return coffeeContainerManager.currentSkin.tazaNEspressoP;
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNAmericanoLungo)
            return coffeeContainerManager.currentSkin.tazaNAmericanoLungoP;
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNMachiatoBombon || currentSprite == coffeeContainerManager.currentSkin.tazaNMachiatoBombonD)
            return hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMachiatoBombonDP : coffeeContainerManager.currentSkin.tazaNMachiatoBombonP;
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNMocaIrishLatte || currentSprite == coffeeContainerManager.currentSkin.tazaNMocaIrishLatteD)
            return hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNMocaIrishLatteDP : coffeeContainerManager.currentSkin.tazaNMocaIrishLatteP;
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNCappuccino)
            return coffeeContainerManager.currentSkin.tazaNCappuccinoP;
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNVienes || currentSprite == coffeeContainerManager.currentSkin.tazaNVienesD)
            return hasLatteArtCard ? coffeeContainerManager.currentSkin.tazaNVienesDP : coffeeContainerManager.currentSkin.tazaNVienesP;
        if (currentSprite == coffeeContainerManager.currentSkin.tazaNFrappe)
            return coffeeContainerManager.currentSkin.tazaNFrappeP;
        
        return coffeeContainerManager.currentSkin.tazaSinCafeP;
    }
    #endregion

    #region Mecanicas leche

    // Funcion para coger/dejar el brick de leche
    public void CogerLeche()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger leche
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            milkInHand = true;
            milkImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StartDragging(milkImage.sprite); // Coger con el cursor
        }
        else if (milkInHand == true) // Dejar leche
        {
            milkInHand = false;
            milkImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar la leche
    public void EcharLecheFria()
    {
        //Si se tiene la leche en la mano y el cafe no esta servido entonces se puede echar
        if (milkInHand == true)
        {
            if (countMilk <= 1)
            {
                MiniGameSoundManager.instance.PlayEcharLiquido();
                countMilk += 1; //Se incrementa el contador de leche
                order.currentOrder.milkPrecision = countMilk; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage($"+{countMilk} Leche");
            }
            milkServed = true;
            cMilkServed = true;
            order.currentOrder.stepsPerformed.Add(OrderStep.AddMilk); // Añadir a la lista de pasos

            // Actualizar sprites taza/vaso
            if (coffeeContainerManager.tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
            }
            else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            coffeeContainerManager.ActualizarBotonCogerEnvase();
        }
    }

    // Funcion para coger/dejar la jarra de leche
    public void CogerJarraLeche()
    {
        // Coger jarra de leche
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand && !tazaMilkInHand)
        {
            tazaMilkInHand = true;
            milkCupImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StartDragging(TazaLeche.GetComponent<Image>().sprite); // Coger con el cursor
        }
        else if (tazaMilkInHand == true) // Dejar jarra de leche
        {
            tazaMilkInHand = false;
            milkCupImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para activar/desactivar la jarra de leche en el espumador
    public void ToggleTazaLecheEspumador()
    {
        if (TengoOtroObjetoEnLaMano() || coffeeContainerManager.vasoInHand || coffeeContainerManager.tazaInHand)
            return;
        if (!tazaMilkInHand && !tazaMilkIsInEspumador)
            return;

        if (!tazaMilkIsInEspumador && tazaMilkInHand) // Colocar en el espumador
        {
            MiniGameSoundManager.instance.PlayIntObjeto();
            DragController.Instance.StopDragging(); // Soltar del cursor
            TazaLeche.SetActive(true);
            TazaLeche.transform.position = puntoEspumador.position;

            tazaMilkInHand = false;
            tazaMilkIsInEspumador = true;

            if (!heatedMilk)
            {
                buttonManager.EnableButton(buttonManager.calentarButton);
            }

            Image espumador = Espumador.GetComponent<Image>();
            espumador.sprite = espumadorShort;

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 1)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();

        }
        else if (tazaMilkIsInEspumador && !tazaMilkInHand) // Recoger del espumador
        {
            MiniGameSoundManager.instance.PlayIntObjeto();
            DragController.Instance.StartDragging(TazaLeche.GetComponent<Image>().sprite); // Coger con el cursor
            TazaLeche.SetActive(false);

            tazaMilkInHand = true;
            tazaMilkIsInEspumador = false;

            buttonManager.DisableButton(buttonManager.calentarButton);
            Image espumador = Espumador.GetComponent<Image>();
            espumador.sprite = espumadorNormal;
        }
    }

    // Funcion para comenzar a calentar la jarra de leche
    public void StartHeating()
    {
        if (!tazaMilkIsInEspumador || TengoOtroObjetoEnLaMano()) return;
        if (heatedMilk) return;

        if (!isHeating && !heatedMilk)
        {
            MiniGameSoundManager.instance.PlayButtonDown();
            MiniGameSoundManager.instance.PlayEspumadorPour();

            currentHeat = 0f;
            isHeating = true;

            heatPanel.SetActive(true);
            curvedFillImage.fillAmount = 0f;
            curvedFillImage.color = Color.blue;

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 2)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();
        }
    }

    // Funcion para parar de calentar la jarra de leche
    public void StopHeating()
    {
        if (isHeating)
        {
            MiniGameSoundManager.instance.StopEspumadorPour();

            heatedMilk = true;
            isHeating = false;
            heatPanel.SetActive(false);
            buttonManager.DisableButton(buttonManager.calentarButton);

            Image pararCalentarLecheBut = buttonManager.calentarButton.GetComponent<Image>();
            pararCalentarLecheBut.sprite = boton3_P;
        }
    }

    // Funcion para echar leche de la jarra de leche
    public void EcharLecheCaliente()
    {
        //Si se tiene la leche caliente en la mano y el cafe no esta servido entonces se puede echar
        if (tazaMilkInHand == true)
        {
            if (countMilk <= 1)
            {
                if (currentHeat < 0.2f)
                {
                    MiniGameSoundManager.instance.PlayEcharLiquido();
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 0;
                }
                else if (currentHeat < 0.6f)
                {
                    MiniGameSoundManager.instance.PlayEcharLiquido();
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 1;
                }
                else
                {
                    MiniGameSoundManager.instance.PlayEcharLiquido();
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 2;
                }

                milkServed = true;
                order.currentOrder.stepsPerformed.Add(OrderStep.AddMilk);
                order.currentOrder.stepsPerformed.Add(OrderStep.HeatMilk);

                PopUpMechanicsMsg.Instance.ShowMessage($"+{countMilk} Leche");

                // Actualizar sprite taza/vaso
                if (coffeeContainerManager.tazaIsInCafetera)
                {
                    UpdateCupSprite(false);
                    currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
                }
                else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
                {
                    CoffeeType currentType = DetermineCoffeeType();
                    bool isCup = false;
                    currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                    Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                    vaso.sprite = currentSprite;
                }
                coffeeContainerManager.ActualizarBotonCogerEnvase();
            }
        }
    }
    #endregion

    #region Mecanica agua

    // Funcion para coger/dejar el agua
    public void CogerAgua()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger agua
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !tazaMilkInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            waterInHand = true;
            waterImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StartDragging(waterImage.sprite); // Coger con el cursor
        }
        else if (waterInHand == true) // Dejar agua
        {
            waterInHand = false;
            waterImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar agua
    public void EcharAgua()
    {
        //Si se tiene el agua en la mano y el cafe no esta servido entonces se puede echar 
        if (waterInHand == true)
        {
            if (countWater < 1)
            {
                MiniGameSoundManager.instance.PlayEcharLiquido();
                countWater += 1; //Se incrementa el contador de agua
                order.currentOrder.waterPrecision = countWater; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+ Agua");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddWater); // Añadir paso a la lista
            }

            // Actualizar sprite taza/vaso
            if (coffeeContainerManager.tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
            }
            else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            coffeeContainerManager.ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica leche condensada

    // Funcion para coger/dejar la leche condensada
    public void CogerLecheCondensada()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger leche condensada
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !tazaMilkInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            condensedMilkInHand = true;
            condensedMilkImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StartDragging(condensedMilkImage.sprite); // Coger con el cursor
        }
        else if (condensedMilkInHand == true) // Dejar leche condensada
        {
            condensedMilkInHand = false;
            condensedMilkImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar leche condensada
    public void EcharLecheCondensada()
    {
        //Si se tiene la leche condensada en la mano y el cafe no esta servido entonces se puede echar 
        if (condensedMilkInHand == true)
        {
            if (countCondensedMilk < 1)
            {
                countCondensedMilk += 1; //Se incrementa el contador de leche condensada
                order.currentOrder.condensedMilkPrecision = countCondensedMilk; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+Leche Condensada");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddCondensedMilk); // Añadir a la lista de pasos
            }

            // Actualizar sprite taza/vaso
            if (coffeeContainerManager.tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
            }
            else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            coffeeContainerManager.ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica crema

    // Funcion para coger/dejar la crema
    public void CogerCrema()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger cuchara crema
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !tazaMilkInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            creamInHand = true;
            buttonManager.creamButton.image.sprite = creamWithoutSpoon;
            creamImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayCuchara();

            DragController.Instance.StartDragging(creamSpoonImage); // Coger con el cursor
        }
        else if (creamInHand == true) // Dejar cuchara crema
        {
            creamInHand = false;
            buttonManager.creamButton.image.sprite = creamWithSpoon;
            creamImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayCuchara();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar crema
    public void EcharCrema()
    {
        //Si se tiene la crema en la mano y el cafe no esta servido entonces se puede echar
        if (creamInHand == true)
        {
            if (countCream < 1)
            {
                countCream += 1; //Se incrementa el contador de crema
                order.currentOrder.creamPrecision = countCream; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+ Crema");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddCream); // Añadir paso a la lista
            }

            // Actualizar sprite taza/vaso
            if (coffeeContainerManager.tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
            }
            else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            coffeeContainerManager.ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica chocolate

    // Funcion para coger/dejar el chocolate
    public void CogerChocolate()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger chocolate
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !tazaMilkInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            chocolateInHand = true;
            chocolateImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StartDragging(chocolateImage.sprite); // Coger con el cursor
        }
        else if (chocolateInHand == true) // Dejar chocolate
        {
            chocolateInHand = false;
            chocolateImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar el chocolate
    public void EcharChocolate()
    {
        //Si se tiene el chocolate en la mano y el cafe no esta servido entonces se puede echar
        if (chocolateInHand == true)
        {
            if (countChocolate < 1)
            {
                countChocolate += 1; //Se incrementa el contador de chocolate
                order.currentOrder.chocolatePrecision = countChocolate; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+ Chocolate");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddChocolate); // Añadir a la lista de pasos
            }

            // Actualizar sprite taza/vaso
            if (coffeeContainerManager.tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
            }
            else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            coffeeContainerManager.ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica whiskey

    // Funcion para coger/dejar el whiskey
    public void CogerWhiskey()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger el whiskey
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && !tazaMilkInHand && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            whiskeyInHand = true;
            whiskeyImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StartDragging(whiskeyImage.sprite); // Coger con el cursor
        }
        else if (whiskeyInHand == true) // Dejar el whiskey
        {
            whiskeyInHand = false;
            whiskeyImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayIntObjeto();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar el whiskey
    public void EcharWhiskey()
    {
        //Si se tiene el whiskey en la mano y el cafe esta servido entonces se puede echar
        if (whiskeyInHand == true)
        {
            if (countWhiskey < 1)
            {
                MiniGameSoundManager.instance.PlayEcharLiquido();
                countWhiskey += 1; //Se incrementa el contador de hielo
                order.currentOrder.whiskeyPrecision = countWhiskey; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+ Whiskey");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddWhiskey); // Se añade el paso a la lista
            }

            // Actualizar sprites taza/vaso
            if (coffeeContainerManager.tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = coffeeContainerManager.Taza.GetComponent<Image>().sprite;
            }
            else if (coffeeContainerManager.vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            coffeeContainerManager.ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica azucar

    // Funcion para coger/dejar la cuchara de azucar
    public void CogerAzucar()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger cuchara de azucar
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && countCover <= 0 && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            cucharaInHand = true;
            sugarImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayCuchara();

            DragController.Instance.StartDragging(sugarSpoonImage); // Coger con el cursor
        }
        else if (cucharaInHand == true) // Dejar cuchara de azucar
        {
            cucharaInHand = false;
            sugarImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayCuchara();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar azucar
    public void EcharAzucar()
    {
        //Si se tiene la cuchara de azucar en la mano y el cafe esta servido entonces se puede echar
        if (cucharaInHand == true && coffeeServed == true)
        {
            if (countSugar <= 1)
            {
                MiniGameSoundManager.instance.PlaySugar();
                countSugar += 1; //Se incrementa el contador de hielo
                order.currentOrder.sugarPrecision = countSugar; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage($"+{countSugar} Azúcar");
            }
        }
    }
    #endregion

    #region Mecanica hielo

    // Funcion para coger/dejar cuchara de hielo
    public void CogerHielo()
    {
        if (tazaMilkInHand || filtroInHand) return;

        // Coger cuchara de hielo
        if (!TengoOtroObjetoEnLaMano() && !coffeeContainerManager.tazaInHand && !coffeeContainerManager.vasoInHand && countCover <= 0 && !filtroInHand && !coffeeContainerManager.platoTazaInHand)
        {
            iceInHand = true;
            iceImage.material = glowMaterial;
            MiniGameSoundManager.instance.PlayCogerHielo();

            DragController.Instance.StartDragging(iceSpoonWithIceImage); // Coger con el cursor
        }
        else if (iceInHand == true) // Dejar cuchara de hielo
        {
            iceInHand = false;
            iceImage.material = defaultMaterial;
            MiniGameSoundManager.instance.PlayDejarHielo();

            DragController.Instance.StopDragging(); // Soltar del cursor
        }
    }

    // Funcion para echar hielo
    public void EcharHielo()
    {
        //Si se tiene la cuchara de hielo en la mano y el cafe esta servido entonces se puede echar
        if (iceInHand == true && coffeeServed == true)
        {
            if (countIce < 1)
            {
                MiniGameSoundManager.instance.PlayEcharHielo();
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+Hielo");
                DragController.Instance.StartDragging(iceSpoonWithoutIceImage); // Coger con el cursor

                if (countCream > 0 && coffeeServed)
                {
                    CoffeeType currentType = CoffeeType.frappe;

                    // Actualizar sprite taza/vaso si es un cafe frappe
                    if (coffeeContainerManager.tazaIsInCafetera)
                    {
                        bool isCup = true;
                        baseCupSprite = GetBaseCoffeeSprite(currentType, isCup);
                        currentSprite = baseCupSprite;

                        Image taza = coffeeContainerManager.Taza.GetComponent<Image>();
                        taza.sprite = currentSprite;
                    }
                    else if (coffeeContainerManager.vasoIsInCafetera)
                    {
                        bool isCup = false;
                        currentSprite = GetBaseCoffeeSprite(currentType, isCup);

                        Image vaso = coffeeContainerManager.Vaso.GetComponent<Image>();
                        vaso.sprite = currentSprite;
                    }
                }    
            }
        }
    }
    #endregion    
}
