using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    public PlayerOrder order;
    public OrderNoteUI orderNoteUI;
    public TutorialManager tutorialManager;
    public GameObject coffeBar; //panel que contiene la barra
    public ButtonUnlockManager buttonManager;
    public CursorManager cursorManager;
    public GameProgressManager progressManager;
    public FoodMinigameInput foodMinigameInput;

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
    [SerializeField] public bool heatedMilk = false;


    [Header("Variables café")]
    int countSugar = 0; 
    public int countIce = 0;
    int countCover = 0;
    int countWater = 0;
    int countMilk = 0;
    int countCondensedMilk = 0;
    int countCream = 0;
    int countChocolate = 0;
    int countWhiskey = 0;
    public bool milkServed = false;
    public bool cMilkServed = false;
    public bool cupServed = false;

    public bool filtroInHand = false, cucharaInHand = false, tazaInHand = false, vasoInHand = false, platoTazaInHand = false, tazaMilkInHand = false, iceInHand = false, coverInHand = false, 
        waterInHand = false, milkInHand = false, condensedMilkInHand = false, creamInHand = false, chocolateInHand = false, whiskeyInHand = false;
    
    public bool tazaIsInCafetera = false, tazaIsInPlato = false, vasoIsInCafetera = false, vasoIsInTable = false, platoTazaIsInTable = false, tazaMilkIsInEspumador = false,
        vasoTapaPuesta = false, filtroIsInCafetera = false;
    #endregion
    #region Sprites
    [Header("Objetos fisicos")]
    public GameObject Taza;
    public GameObject Vaso;
    public GameObject TazaLeche; 
    public GameObject PlatoTaza;
    public GameObject Filtro;
    public GameObject Espumador;
    public GameObject Balda;

    public Transform puntoCafetera;
    public Transform puntoEspumador;
    public Transform puntoMesa;
    public Transform puntoTazaPlato;
    public Transform puntoFiltroCafetera;

    [Header("Sprites mecánicas")]
    public Sprite creamWithSpoon;
    public Sprite creamWithoutSpoon;
    public Sprite filtroImg;
    public Sprite filtroCafeteraImg;

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

    [Header("Sprites tazas y vasos")]
    public Sprite vasoConTapa;
    public Sprite vasoSinTapa;
    public Sprite tazaSinCafe;
    public Sprite tazaSinCafeP;

    [Header("Sprites mecánicas tazas")]
    public Sprite tazaNWater;
    public Sprite tazaNMilk;
    public Sprite tazaNWhiskey;
    public Sprite tazaNChocolate;
    public Sprite tazaNWaterP;
    public Sprite tazaNMilkP;
    public Sprite tazaNWhiskeyP;
    public Sprite tazaNChocolateP;

    [Header("Sprites cafés tazas")]
    public Sprite tazaNEspresso;
    public Sprite tazaNAmericanoLungo;
    public Sprite tazaNMocaIrishLatte;
    public Sprite tazaNCappuccino;
    public Sprite tazaNMachiatoBombon;
    public Sprite tazaNVienes;
    public Sprite tazaNFrappe;

    [Header("Sprites cafés tazas + dibujo")]
    public Sprite tazaNMocaIrishLatteD;
    public Sprite tazaNMachiatoBombonD;
    public Sprite tazaNVienesD;

    [Header("Sprites cafés tazas + plato")]
    public Sprite tazaNEspressoP;
    public Sprite tazaNAmericanoLungoP;
    public Sprite tazaNMocaIrishLatteP;
    public Sprite tazaNCappuccinoP;
    public Sprite tazaNMachiatoBombonP;
    public Sprite tazaNVienesP;
    public Sprite tazaNFrappeP;

    [Header("Sprites cafés tazas + dibujo + plato")]
    public Sprite tazaNMocaIrishLatteDP;
    public Sprite tazaNMachiatoBombonDP;
    public Sprite tazaNVienesDP;

    [Header("Sprites cafés vasos")]
    public Sprite vasoNEspresso;
    public Sprite vasoNAmericanoLungo;
    public Sprite vasoNMocaIrishLatte;
    public Sprite vasoNCappuccino;
    public Sprite vasoNMachiatoBombon;
    public Sprite vasoNVienes;
    public Sprite vasoNFrappe;

    [Header("Sprites cafés vasos + dibujo")]
    public Sprite vasoNMocaIrishLatteD;
    public Sprite vasoNMachiatoBombonD;
    public Sprite vasoNVienesD;

    public Sprite currentSprite;
    public Sprite baseCupSprite;
    #endregion

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        orderNoteUI.ResetNote();
        ResetCafe();

        CoffeeFoodManager.Instance.ResetPanels();
        ActualizarBotonCogerEnvase();
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
    public void ResetCafe()
    {
        buttonManager.filtroCafeteraButton.gameObject.SetActive(false);
        buttonManager.filtroButton.gameObject.SetActive(false);

        buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
        buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerPlatoTazaButton);

        buttonManager.DisableButton(buttonManager.coffeeButton);
        buttonManager.DisableButton(buttonManager.submitOrderButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);
        buttonManager.DisableButton(buttonManager.calentarButton);
        buttonManager.DisableButton(buttonManager.echarCafeButton);
        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);
        DisableMechanics();

        heatPanel.SetActive(false);
        molerPanel.SetActive(false);
        CoffeeFoodManager.Instance.ResetCoffeePanel();

        Taza.SetActive(false);
        Vaso.SetActive(false);
        PlatoTaza.SetActive(false);
        TazaLeche.SetActive(false);
        Filtro.SetActive(false);
        UpdateStartSprites();

        buttonManager.molerButton.gameObject.SetActive(true);
        palancaDown.SetActive(false);

        currentSprite = baseCupSprite = null;
        currentSlideTime = currentHeat = currentMolido = currentAngle = 0f;
        isSliding = isServing = movingRight = coffeeDone = coffeeServed = cupServed = milkServed = cMilkServed = heatedMilk = isHeating = isMoliendo = false;
        tazaIsInCafetera = tazaIsInPlato = vasoIsInCafetera = vasoIsInTable = platoTazaIsInTable = tazaMilkIsInEspumador = filtroIsInCafetera = false;
        countSugar = countIce = countCover = countWater = countMilk = countCondensedMilk = countCream = countChocolate = countWhiskey = 0;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }
    }
    private void UpdateStartSprites()
    {
        Image taza = Taza.GetComponent<Image>();
        taza.sprite = tazaSinCafe;

        Image vaso = Vaso.GetComponent<Image>();
        vaso.sprite = vasoSinTapa;

        Image filtro = Filtro.GetComponent<Image>();
        filtro.sprite = filtroImg;

        Image cantidadCafeBut = buttonManager.coffeeButton.GetComponent<Image>();
        cantidadCafeBut.sprite = botonCantidadCafe_N;

        Image echarCafeBut = buttonManager.echarCafeButton.GetComponent<Image>();
        echarCafeBut.sprite = boton1_N;

        Image pararEcharCafeBut = buttonManager.pararEcharCafeButton.GetComponent<Image>();
        pararEcharCafeBut.sprite = boton2_N;

        Image pararCalentarLecheBut = buttonManager.calentarButton.GetComponent<Image>();
        pararCalentarLecheBut.sprite = boton3_N;
    }

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
                Debug.Log("La barrita llego al limite");
            }
        }
    }
    private void HandleMoler()
    {
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

    public bool TengoOtroObjetoEnLaMano()
    {
        return  cucharaInHand || waterInHand || milkInHand || condensedMilkInHand || creamInHand || chocolateInHand || whiskeyInHand || iceInHand || coverInHand;
    }
    public void DisableMechanics()
    {
        buttonManager.DisableButton(buttonManager.sugarButton);
        buttonManager.DisableButton(buttonManager.iceButton);
        buttonManager.DisableButton(buttonManager.whiskeyButton);
        buttonManager.DisableButton(buttonManager.coverButton);
    }
    public void EnableMechanics()
    {
        buttonManager.EnableButton(buttonManager.sugarButton);
        buttonManager.EnableButton(buttonManager.iceButton);
        buttonManager.EnableButton(buttonManager.whiskeyButton);
        buttonManager.EnableButton(buttonManager.coverButton);
    }
    public void CheckButtons()
    {
        Balda.SetActive(progressManager.condensedMilkEnabled);


        if (tutorialManager.isRunningT1)
        {
            buttonManager.DisableButton(buttonManager.shopButton);

            int step = tutorialManager.currentStep;

            if (step == 12) buttonManager.EnableButton(buttonManager.echarCafeButton);
            if (step == 15) buttonManager.EnableButton(buttonManager.papeleraButton);
            if (step == 17 && cupServed) buttonManager.EnableButton(buttonManager.submitOrderButton);
            else    buttonManager.DisableButton(buttonManager.submitOrderButton);
            if (step == 20) buttonManager.EnableButton(buttonManager.endDeliveryButton);
            else    buttonManager.DisableButton(buttonManager.endDeliveryButton);
            if (step >= 21 && step <= 24) buttonManager.DisableButton(buttonManager.gameButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.endDeliveryButton);
            buttonManager.EnableButton(buttonManager.shopButton);
            buttonManager.EnableButton(buttonManager.gameButton);

            if (tazaIsInCafetera || vasoIsInCafetera)
                buttonManager.EnableButton(buttonManager.papeleraButton);
        }

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 0)
            buttonManager.DisableButton(buttonManager.bakeryButton);
        else
            buttonManager.EnableButton(buttonManager.bakeryButton);

        if (!tutorialManager.isRunningT1 && !tutorialManager.isRunningT2)
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
                buttonManager.EnableButton(buttonManager.bakeryButton);
            }
            else
            {
                buttonManager.EnableButton(buttonManager.recipesBookButton);
                buttonManager.EnableButton(buttonManager.orderNoteButton);
                buttonManager.EnableButton(buttonManager.bakeryButton);
            }
        }

        if (cMilkServed)
            buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);

        if (milkServed)
            buttonManager.DisableButton(buttonManager.calentarButton);
        

        /*if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 20)
            buttonManager.EnableButton(buttonManager.endDeliveryButton);
        else if (tutorialManager.isRunningT1)
            buttonManager.DisableButton(buttonManager.endDeliveryButton);
        else
            buttonManager.EnableButton(buttonManager.endDeliveryButton);

        if (tutorialManager.isRunningT1)
            buttonManager.DisableButton(buttonManager.shopButton);
        else
            buttonManager.EnableButton(buttonManager.shopButton);

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 17 && cupServed)
            buttonManager.EnableButton(buttonManager.submitOrderButton);
        else if (tutorialManager.isRunningT1)
            buttonManager.DisableButton(buttonManager.submitOrderButton);

        if (tutorialManager.isRunningT1 && (tutorialManager.currentStep == 21 || tutorialManager.currentStep == 22 
            || tutorialManager.currentStep == 23 || tutorialManager.currentStep == 24))
            buttonManager.DisableButton(buttonManager.gameButton);
        else
            buttonManager.EnableButton(buttonManager.gameButton);

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 8)
            buttonManager.EnableButton(buttonManager.coffeeButton);
        else if (tutorialManager.isRunningT1 && tutorialManager.currentStep < 8)
            buttonManager.DisableButton(buttonManager.coffeeButton);

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 12)
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        else if (tutorialManager.isRunningT1 && tutorialManager.currentStep < 12)
            buttonManager.DisableButton(buttonManager.echarCafeButton);
            
        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 0)
            buttonManager.DisableButton(buttonManager.bakeryButton);
        else if (tutorialManager.isRunningT2 && tutorialManager.currentStep != 0)
            buttonManager.EnableButton(buttonManager.bakeryButton);
        
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 15)
            buttonManager.EnableButton(buttonManager.papeleraButton);
        else if ((tazaIsInCafetera || vasoIsInCafetera) && !tutorialManager.isRunningT1)
            buttonManager.EnableButton(buttonManager.papeleraButton);

        if (!tutorialManager.isRunningT2 && !tutorialManager.isRunningT1)
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
                buttonManager.EnableButton(buttonManager.bakeryButton);
            }
            else
            {
                buttonManager.EnableButton(buttonManager.recipesBookButton);
                buttonManager.EnableButton(buttonManager.orderNoteButton);
                buttonManager.EnableButton(buttonManager.bakeryButton);
            }
        }*/

        
    }
    #endregion

    #region Envases
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
   
    public void ToggleTazaCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand || filtroInHand || platoTazaInHand)
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

            if (coffeeServed)
                UpdateCupSprite(false);

            if (!tutorialManager.isRunningT1)
                buttonManager.EnableButton(buttonManager.coffeeButton);

            if (!coffeeServed)
            {
                buttonManager.EnableButton(buttonManager.waterButton);
                buttonManager.EnableButton(buttonManager.milkButton);
                buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
                buttonManager.EnableButton(buttonManager.condensedMilkButton);
                buttonManager.EnableButton(buttonManager.creamButton);
                buttonManager.EnableButton(buttonManager.chocolateButton);
            }

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
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }

        ActualizarBotonCogerEnvase();
    }
    public void ToggleTazaPlato()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand)
            return;

        if (!tazaInHand && !tazaIsInPlato)
            return;

        if (!platoTazaIsInTable)
            return;

        if (!tazaIsInPlato && tazaInHand)
        {
            // Poner en el plato
            Taza.SetActive(true);
            Taza.transform.position = puntoTazaPlato.position;

            tazaInHand = false;
            tazaIsInPlato = true;
            cupServed = true;

            UpdateCupSprite(true);
            PlatoTaza.SetActive(false);

            // Se asocia a la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(true, Taza.GetComponent<Image>(), Taza.GetComponent<Image>().sprite);

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

            UpdateCupSprite(false);
            PlatoTaza.SetActive(true);
            cursorManager.UpdateCursorTaza(false);

            // Se quita de la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(false, null, null);
            EnableMechanics();
        }
    }
    public void ToggleVasoCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand || filtroInHand)
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

            buttonManager.DisableButton(buttonManager.cogerPlatoTazaButton);

            if (!tutorialManager.isRunningT1)
                buttonManager.EnableButton(buttonManager.coffeeButton);

            if (!coffeeServed)
            {
                buttonManager.EnableButton(buttonManager.waterButton);
                buttonManager.EnableButton(buttonManager.milkButton);
                buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
                buttonManager.EnableButton(buttonManager.condensedMilkButton);
                buttonManager.EnableButton(buttonManager.creamButton);
                buttonManager.EnableButton(buttonManager.chocolateButton);
            }

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
            cupServed = true;

            // Se deja en la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(true, Vaso.GetComponent<Image>(), Vaso.GetComponent<Image>().sprite);
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
            cupServed = false;

            // Se quita de la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(false, null, null);

            EnableMechanics();
            cursorManager.UpdateCursorVaso(false);
        }
    }
    public void PlacePlatoTazaMesa()
    {
        if (platoTazaIsInTable)
            return;

        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand)
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
            buttonManager.DisableButton(buttonManager.cogerVasoInicioButton);
            Debug.Log($"Plato colocado: {platoTazaIsInTable}");
        }
    }
#endregion

    #region Mecanicas cafe
    public void StartCoffee()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 8) return;

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

            Image cantidadCafeBut = buttonManager.coffeeButton.GetComponent<Image>();
            cantidadCafeBut.sprite = botonCantidadCafe_P;

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 8)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
    public void StartMoler()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 9) return;

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
        if (isMoliendo && currentMolido == maxFillMoler)
        {
            isMoliendo = false;
            molerPanel.SetActive(false);

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Cafe molido");
            buttonManager.DisableButton(buttonManager.molerButton);
            buttonManager.filtroButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.filtroButton);

            buttonManager.molerButton.gameObject.SetActive(false);
            palancaDown.SetActive(true);

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 9)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
    public void TakeFiltro()
    {
        if (TengoOtroObjetoEnLaMano() || tazaInHand || vasoInHand || tazaMilkInHand) return;

        if (!filtroIsInCafetera)
        {
            filtroInHand = true;
            buttonManager.DisableButton(buttonManager.filtroButton);
            buttonManager.EnableButton(buttonManager.filtroCafeteraButton);
            buttonManager.filtroButton.gameObject.SetActive(false);
            buttonManager.filtroCafeteraButton.gameObject.SetActive(true);
        }
    }
    public void PutFiltro()
    {
        if (filtroIsInCafetera == false)
        {
            filtroIsInCafetera = true;
            filtroInHand = false;

            // Poner en la mesa
            Filtro.SetActive(true);
            Filtro.transform.position = puntoFiltroCafetera.position;
            Image filtro = Filtro.GetComponent<Image>();
            filtro.sprite = filtroCafeteraImg;

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 10)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }

        if (tazaIsInCafetera == true || vasoIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }
    public void StartServingCoffee()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 12) return;
        if (coffeeServed) return;

        bool recipienteEnCafetera = tazaIsInCafetera || vasoIsInCafetera;
        if (!recipienteEnCafetera || !filtroIsInCafetera) return;

        Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Echando cafe...");

        isServing = true;
        movingRight = true;

        Image echarCafeBut = buttonManager.echarCafeButton.GetComponent<Image>();
        echarCafeBut.sprite = boton1_P;

        buttonManager.DisableButton(buttonManager.echarCafeButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);

        buttonManager.EnableButton(buttonManager.pararEcharCafeButton);
    }

    public void MoveNeedle()
    {
        float step = rotationSpeed * Time.deltaTime;

        if (movingRight)
        {
            currentAngle += step;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                movingRight = false;
            }
        }
        else
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

    public void StopServingCoffee()
    {
        if (!isServing || coffeeServed) return;

        isServing = false;
        coffeeServed = true;

        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);

        if (order != null && order.currentOrder != null)
        {
            order.currentOrder.coffeeServedPrecision = normalizedPrecision;
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Echar cafe detenido en: {normalizedPrecision}");
        }

        buttonManager.EnableButton(buttonManager.sugarButton);
        buttonManager.EnableButton(buttonManager.iceButton);
        buttonManager.EnableButton(buttonManager.coverButton);
        buttonManager.EnableButton(buttonManager.whiskeyButton);

        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);

        Image pararEcharCafeBut = buttonManager.pararEcharCafeButton.GetComponent<Image>();
        pararEcharCafeBut.sprite = boton2_P;

        if (tazaIsInCafetera)
        {
            CoffeeType currentType = DetermineCoffeeType();
            bool isCup = true;
            baseCupSprite = GetBaseCoffeeSprite(currentType, isCup);
            currentSprite = baseCupSprite;

            Image taza = Taza.GetComponent<Image>();
            taza.sprite = currentSprite;
        }
        else if (vasoIsInCafetera)
        {
            CoffeeType currentType = DetermineCoffeeType();
            bool isCup = false;
            currentSprite = GetBaseCoffeeSprite(currentType, isCup);
            Image vaso = Vaso.GetComponent<Image>();
            vaso.sprite = currentSprite;
        }

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 12)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
    }

    // Actualizar el sprite de cafe/ingrediente actual
    private void UpdateCupSprite(bool inPlato)
    {
        Image taza = Taza.GetComponent<Image>();
        bool cupEmpty = !coffeeServed && !milkServed && countWater == 0 && countMilk == 0
            && countCream == 0 && countWhiskey == 0 && countChocolate == 0 && countCondensedMilk == 0;

        Sprite newBaseSprite = null;

        if (cupEmpty)
        {
            newBaseSprite = inPlato ? tazaSinCafeP : tazaSinCafe;
        }
        // Agua sin cafe
        else if (countWater != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? tazaNWaterP : tazaNWater;
        }
        // Leche / crema / leche condensada sin cafe
        else if ((countMilk != 0 || countCondensedMilk != 0 || countCream != 0) && !coffeeServed)
        {
            newBaseSprite = inPlato ? tazaNMilkP : tazaNMilk;
        }
        // Chocolate
        else if (countChocolate != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? tazaNChocolateP : tazaNChocolate;
        }
        // Whiskey
        else if (countWhiskey != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? tazaNWhiskeyP : tazaNWhiskey;
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
            case CoffeeType.espresso: return isCup ? tazaNEspresso : vasoNEspresso;
            case CoffeeType.lungo: return isCup ? tazaNAmericanoLungo : vasoNAmericanoLungo;
            case CoffeeType.americano: return isCup ? tazaNAmericanoLungo : vasoNAmericanoLungo;
            case CoffeeType.latte: 
                return isCup ? (hasLatteArtCard ? tazaNMocaIrishLatteD : tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? vasoNMocaIrishLatteD : vasoNMocaIrishLatte);
            case CoffeeType.capuccino: return isCup ? tazaNCappuccino : vasoNCappuccino;
            case CoffeeType.irish:
                return isCup ? (hasLatteArtCard ? tazaNMocaIrishLatteD : tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? vasoNMocaIrishLatteD : vasoNMocaIrishLatte);
            case CoffeeType.mocca:
                return isCup ? (hasLatteArtCard ? tazaNMocaIrishLatteD : tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? vasoNMocaIrishLatteD : vasoNMocaIrishLatte);
            case CoffeeType.macchiatto:
                return isCup ? (hasLatteArtCard ? tazaNMachiatoBombonD : tazaNMachiatoBombon)
                             : (hasLatteArtCard ? vasoNMachiatoBombonD : vasoNMachiatoBombon);
            case CoffeeType.bombon:
                return isCup ? (hasLatteArtCard ? tazaNMachiatoBombonD : tazaNMachiatoBombon)
                             : (hasLatteArtCard ? vasoNMachiatoBombonD : vasoNMachiatoBombon);
            case CoffeeType.vienes:
                return isCup ? (hasLatteArtCard ? tazaNVienesD : tazaNVienes)
                             : (hasLatteArtCard ? vasoNVienesD : vasoNVienes);
            case CoffeeType.frappe: return isCup ? tazaNFrappe : vasoNFrappe;
        }
        return isCup ? tazaSinCafe : vasoSinTapa;
    }

    // Vincular el sprite actual de la taza con su plato
    private Sprite CheckFinalCupPlate(Sprite sprite, bool hasLatteArtCard)
    {
        if (currentSprite == tazaNEspresso)
            return tazaNEspressoP;
        if (currentSprite == tazaNAmericanoLungo)
            return tazaNAmericanoLungoP;
        if (currentSprite == tazaNMachiatoBombon || currentSprite == tazaNMachiatoBombonD)
            return hasLatteArtCard ? tazaNMachiatoBombonDP : tazaNMachiatoBombonP;
        if (currentSprite == tazaNMocaIrishLatte || currentSprite == tazaNMocaIrishLatteD)
            return hasLatteArtCard ? tazaNMocaIrishLatteDP : tazaNMocaIrishLatteP;
        if (currentSprite == tazaNCappuccino)
            return tazaNCappuccinoP;
        if (currentSprite == tazaNVienes || currentSprite == tazaNVienesD)
            return hasLatteArtCard ? tazaNVienesDP : tazaNVienesP;
        if (currentSprite == tazaNFrappe)
            return tazaNFrappeP;
        
        return tazaSinCafeP;
    }
    #endregion

    #region Mecanicas leche
    public void CogerLeche()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            milkInHand = true;
            buttonManager.milkButton.image.enabled = false;
        }
        else if (milkInHand == true)
        {
            milkInHand = false;
            buttonManager.milkButton.image.enabled = true;
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
                PopUpMechanicsMsg.Instance.ShowMessage($"+{countMilk} Leche");
            }
            milkServed = true;
            cMilkServed = true;

            if (tazaIsInCafetera)
            {
                Image taza = Taza.GetComponent<Image>();
                taza.sprite = tazaNMilk;
            }
        }
    }
    public void ToggleTazaLecheEspumador()
    {
        if (TengoOtroObjetoEnLaMano() | vasoInHand | tazaInHand)
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

            buttonManager.EnableButton(buttonManager.calentarButton);

            Espumador.SetActive(true);
            Image espumador = Espumador.GetComponent<Image>();
            espumador.sprite = espumadorShort;

            cursorManager.UpdateCursorTazaMilk(true);
            Debug.Log($"Taza con leche colocada en espumador: {tazaMilkIsInEspumador}");

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 1)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();

        }
        else if (tazaMilkIsInEspumador && !tazaMilkInHand)
        {
            //Recoger del espumador
            TazaLeche.SetActive(false);

            tazaMilkInHand = true;
            tazaMilkIsInEspumador = false;

            Image espumador = Espumador.GetComponent<Image>();
            espumador.sprite = espumadorNormal;

            cursorManager.UpdateCursorTazaMilk(false);
        }
    }
    public void StartHeating()
    {
        if (!tazaMilkIsInEspumador || TengoOtroObjetoEnLaMano()) return;

        if (!isHeating && !heatedMilk)
        {
            currentHeat = 0f;
            isHeating = true;

            heatPanel.SetActive(true);
            curvedFillImage.fillAmount = 0f;
            curvedFillImage.color = Color.blue;

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Calentando la leche...");

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 2)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();
        }
    }
    public void StopHeating()
    {
        if (isHeating)
        {
            isHeating = false;

            heatPanel.SetActive(false);
            Espumador.SetActive(false);
            buttonManager.DisableButton(buttonManager.calentarButton);

            Image pararCalentarLecheBut = buttonManager.calentarButton.GetComponent<Image>();
            pararCalentarLecheBut.sprite = boton3_P;

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
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 0;
                    Debug.Log("Leche fria echada");
                }
                else if (currentHeat < 0.8f)
                {
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 1;
                    Debug.Log("Leche caliente echada");
                }
                else
                {
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 2;
                    Debug.Log("Leche quemada echada");
                }

                milkServed = true;
                heatedMilk = true;

                PopUpMechanicsMsg.Instance.ShowMessage($"+{countMilk} Leche");
                buttonManager.DisableButton(buttonManager.waterButton);
                buttonManager.DisableButton(buttonManager.milkButton);

                if (tazaIsInCafetera)
                {
                    Image taza = Taza.GetComponent<Image>();
                    taza.sprite = tazaNMilk;
                }
            }
        }
    }
    #endregion

    #region Mecanica agua
    public void CogerAgua()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            waterInHand = true;
            buttonManager.waterButton.image.enabled = false;
        }
        else if (waterInHand == true)
        {
            waterInHand = false;
            buttonManager.waterButton.image.enabled = true;
        }
    }
    public void EcharAgua()
    {
        //Si se tiene el agua en la mano y el cafe no esta servido entonces se puede echar 
        if (waterInHand == true && coffeeServed == false)
        {
            if (countWater < 1)
            {
                countWater += 1; //Se incrementa el contador de agua
                order.currentOrder.waterPrecision = countWater; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado agua.");
                PopUpMechanicsMsg.Instance.ShowMessage("+ Agua");
            }

            if (tazaIsInCafetera)
            {
                Image taza = Taza.GetComponent<Image>();
                taza.sprite = tazaNWater;
            }
        }
    }
    #endregion

    #region Mecanica leche condensada
    public void CogerLecheCondensada()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            condensedMilkInHand = true;
            buttonManager.condensedMilkButton.image.enabled = false;
        }
        else if (condensedMilkInHand == true)
        {
            condensedMilkInHand = false;
            buttonManager.condensedMilkButton.image.enabled = true;
        }
    }
    public void EcharLecheCondensada()
    {
        //Si se tiene la leche condensada en la mano y el cafe no esta servido entonces se puede echar 
        if (condensedMilkInHand == true && coffeeServed == false)
        {
            if (countCondensedMilk < 1)
            {
                countCondensedMilk += 1; //Se incrementa el contador de leche condensada
                order.currentOrder.condensedMilkPrecision = countCondensedMilk; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado leche condensada.");
                PopUpMechanicsMsg.Instance.ShowMessage("+Leche Condensada");
            }
            if (tazaIsInCafetera)
            {
                Image taza = Taza.GetComponent<Image>();
                taza.sprite = tazaNMilk;
            }
        }
    }
    #endregion

    #region Mecanica crema
    public void CogerCrema()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            creamInHand = true;
            buttonManager.creamButton.image.sprite = creamWithoutSpoon;
        }
        else if (creamInHand == true)
        {
            creamInHand = false;
            buttonManager.creamButton.image.sprite = creamWithSpoon; 
        }
    }
    public void EcharCrema()
    {
        //Si se tiene la crema en la mano y el cafe no esta servido entonces se puede echar
        if (creamInHand == true && coffeeServed == false)
        {
            if (countCream < 1)
            {
                countCream += 1; //Se incrementa el contador de crema
                order.currentOrder.creamPrecision = countCream; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado crema.");
                PopUpMechanicsMsg.Instance.ShowMessage("+ Crema");
            }
            if (tazaIsInCafetera)
            {
                Image taza = Taza.GetComponent<Image>();
                taza.sprite = tazaNMilk;
            }
        }
    }
    #endregion

    #region Mecanica chocolate
    public void CogerChocolate()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            chocolateInHand = true;
            buttonManager.chocolateButton.image.enabled = false;
        }
        else if (chocolateInHand == true)
        {
            chocolateInHand = false;
            buttonManager.chocolateButton.image.enabled = true;
        }
    }
    public void EcharChocolate()
    {
        //Si se tiene el chocolate en la mano y el cafe no esta servido entonces se puede echar
        if (chocolateInHand == true && coffeeServed == false)
        {
            if (countChocolate < 1)
            {
                countChocolate += 1; //Se incrementa el contador de chocolate
                order.currentOrder.chocolatePrecision = countChocolate; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado chocolate.");
                PopUpMechanicsMsg.Instance.ShowMessage("+ Chocolate");
            }
            if (tazaIsInCafetera)
            {
                Image taza = Taza.GetComponent<Image>();
                taza.sprite = tazaNChocolate;
            }
        }
    }
    #endregion

    #region Mecanica whiskey
    public void CogerWhiskey()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            whiskeyInHand = true;
            buttonManager.whiskeyButton.image.enabled = false;
        }
        else if (whiskeyInHand == true)
        {
            whiskeyInHand = false;
            buttonManager.whiskeyButton.image.enabled = true;
        }
    }
    public void EcharWhiskey()
    {
        //Si se tiene el whiskey en la mano y el cafe esta servido entonces se puede echar
        if (whiskeyInHand == true && coffeeServed == true)
        {
            if (countWhiskey < 1)
            {
                countWhiskey += 1; //Se incrementa el contador de hielo
                order.currentOrder.whiskeyPrecision = countWhiskey; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado whiskey.");
                PopUpMechanicsMsg.Instance.ShowMessage("+ Whiskey");
            }
            if (tazaIsInCafetera)
            {
                Image taza = Taza.GetComponent<Image>();
                taza.sprite = tazaNWhiskey;
            }
        }
    }
    #endregion

    #region Mecanica azucar
    public void CogerAzucar()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && countCover <= 0)
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
                PopUpMechanicsMsg.Instance.ShowMessage($"+{countSugar} Azúcar");
            }
        }
    }
    #endregion

    #region Mecanica hielo
    public void CogerHielo()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && countCover <= 0)
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
            if (countIce < 1)
            {
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado hielo.");
                PopUpMechanicsMsg.Instance.ShowMessage("+Hielo");
                cursorManager.ChangeHieloSpoon();

                if (countCream > 0 && coffeeServed)
                {
                    CoffeeType currentType = CoffeeType.frappe;
                    if (tazaIsInCafetera)
                    {
                        bool isCup = true;
                        baseCupSprite = GetBaseCoffeeSprite(currentType, isCup);
                        currentSprite = baseCupSprite;

                        Image taza = Taza.GetComponent<Image>();
                        taza.sprite = currentSprite;
                    }
                    else if (vasoIsInCafetera)
                    {
                        bool isCup = false;
                        currentSprite = GetBaseCoffeeSprite(currentType, isCup);

                        Image vaso = Vaso.GetComponent<Image>();
                        vaso.sprite = currentSprite;
                    }
                }    
            }
        }
    }
    #endregion

    #region Mecanica tipo de pedido
    public void CogerTapa()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
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
            if (countCover < 1)
            {
                countCover += 1; //Se incrementa el contador de hielo
                order.currentOrder.typePrecision = countCover; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Tapa puesta.");
            }
            Image vaso = Vaso.GetComponent<Image>();
            vaso.sprite = vasoConTapa;

            buttonManager.DisableButton(buttonManager.coverButton);
            cursorManager.SetDefaultCursor();
            coverInHand = false;
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
