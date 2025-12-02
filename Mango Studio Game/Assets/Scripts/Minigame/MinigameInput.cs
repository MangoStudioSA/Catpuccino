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
    public GameObject Estante;

    [Header("Puntos transform")]
    public Transform puntoCafeteraTaza;
    public Transform puntoCafeteraVaso;
    public Transform puntoEspumador;
    public Transform puntoMesa;
    public Transform puntoTazaPlato;
    public Transform puntoMesaVaso;
    public Transform puntoFiltroCafetera;

    [Header("GameObjects estante")]
    public GameObject estanteBase;
    public GameObject estanteTazaPremium;
    public GameObject estanteVasoPremium;
    public GameObject estantePremium;

    [Header("Objetos ingredientes")]
    public Material defaultMaterial;
    public Material glowMaterial;
    public Image tazasImage;
    public Image tazaBImage;
    public Image tazaPImage;
    public Image vasoImage;
    public Image vasoPImage;
    public Image platoTazaImage;
    public Image platoTazaPImage;
    public Image waterImage;
    public Image milkImage;
    public Image milkCupImage;
    public Image condensedMilkImage;
    public Image chocolateImage;
    public Image whiskeyImage;
    public Image coverImage;
    public Image coverPImage;

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

    [Header("Comprobaciones skins")]
    bool hasPremiumCupCard;
    bool hasPremiumVaseCard;

    [Header("Configuracion skins")]
    public CoffeeSkin skinBase;
    public CoffeeSkin skinPremium;
    public CoffeeSkin skinB_PP;
    public CoffeeSkin skinP_PB;

    private CoffeeSkin currentSkin;
    private bool cupIsPremium = false;
    private bool plateIsPremium = false;
    private bool vasoIsPremium = false;
    private bool coverIsPremium = false;

    public Sprite currentSprite;
    public Sprite baseCupSprite;

    [Header("Estado final skins")]
    public bool finalCupIsPremium;
    public bool finalPlateIsPremium;
    public bool finalVasoIsPremium;
    public bool finalCoverIsPremium;
    #endregion

    public void Awake()
    {
        // Booleano que marca si el jugador tiene la skin de la taza
        hasPremiumCupCard = PlayerDataManager.instance.HasCard("CartaSkinTaza");
        // Booleano que marca si el jugador tiene la skin del vaso
        hasPremiumVaseCard = (PlayerDataManager.instance.HasCard("CartaSkinVaso1") && PlayerDataManager.instance.HasCard("CartaSkinVaso2"));
    }

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        orderNoteUI.ResetNote();
        ResetCafe();
        order.currentOrder.ResetSteps();

        CoffeeFoodManager.Instance.ResetPanels();
        ActualizarBotonCogerEnvase();
        UpdateEstanteSprite();
        UpdateStartSprites();
    }

    public void Update()
    {
        CheckButtons();
        CheckBloomEffect();
        HandleCoffeeSlider();
        HandleMoler();
        HandleHeating();

        if (isServing && !coffeeServed) MoveNeedle();
    }

    #region Funciones auxiliares
    public void ResetCafe()
    {
        if (TengoOtroObjetoEnLaMano() || filtroInHand || tazaInHand || tazaMilkInHand || vasoInHand || platoTazaInHand) return;

        buttonManager.filtroCafeteraButton.gameObject.SetActive(false);
        buttonManager.filtroButton.gameObject.SetActive(false);

        buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
        buttonManager.EnableButton(buttonManager.cogerTazaPInicioButton);
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

        Taza.SetActive(false);
        Vaso.SetActive(false);
        PlatoTaza.SetActive(false);
        TazaLeche.SetActive(false);
        Filtro.SetActive(false);
        buttonManager.molerButton.gameObject.SetActive(true);

        UpdateStartSprites();
        CoffeeFoodManager.Instance.ResetCoffeePanel();

        currentSprite = baseCupSprite = null;
        currentSlideTime = currentHeat = currentMolido = currentAngle = 0f;
        isSliding = isServing = movingRight = coffeeDone = coffeeServed = cupServed = milkServed = cMilkServed = heatedMilk = isHeating = isMoliendo = false;
        tazaIsInCafetera = tazaIsInPlato = vasoIsInCafetera = vasoIsInTable = platoTazaIsInTable = tazaMilkIsInEspumador = filtroIsInCafetera = false;
        countSugar = countIce = countCover = countWater = countMilk = countCondensedMilk = countCream = countChocolate = countWhiskey = 0;
        finalCoverIsPremium = finalCupIsPremium = finalPlateIsPremium = finalVasoIsPremium = false;
        cupIsPremium = plateIsPremium = vasoIsPremium = coverIsPremium = false;

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
        if (currentSkin == null)
            currentSkin = skinBase;

        Image taza = Taza.GetComponent<Image>(); // Taza
        taza.sprite = currentSkin.tazaSinCafe;

        Image platoTaza = PlatoTaza.GetComponent<Image>(); // Plato taza
        platoTaza.sprite = currentSkin.platoTaza;

        Image vaso = Vaso.GetComponent<Image>(); // Vaso
        vaso.sprite = currentSkin.vasoSinTapa;

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

    // Funcion para gestionar el sprite del estante de los envases segun las skins desbloqueadas
    private void UpdateEstanteSprite()
    {
        // Ambas skins desbloqueadas
        if (hasPremiumCupCard && hasPremiumVaseCard)
        {
            // Imagenes props
            estantePremium.SetActive(true);
            vasoPImage.gameObject.SetActive(true);
            tazaBImage.gameObject.SetActive(true);
            tazaPImage.gameObject.SetActive(true);
            platoTazaPImage.gameObject.SetActive(true);
            coverPImage.gameObject.SetActive(true);

            tazasImage.gameObject.SetActive(false);

            // Botones
            buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(true);
            buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(true);
            buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(true);
            buttonManager.cogerVasoPInicioButton.gameObject.SetActive(true);
            buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(true);
            buttonManager.cogerTazaPInicioButton.gameObject.SetActive(true);
            buttonManager.coverPButton.gameObject.SetActive(true);

            buttonManager.cogerPlatoTazaButton.gameObject.SetActive(false);
            buttonManager.cogerTazaInicioButton.gameObject.SetActive(false);
            buttonManager.cogerVasoInicioButton.gameObject.SetActive(false);
        }
        else if (hasPremiumCupCard && !hasPremiumVaseCard) // Skin taza desbloqueada
        {
            // Imagenes props
            estanteTazaPremium.SetActive(true);
            platoTazaPImage.gameObject.SetActive(true);
            tazaBImage.gameObject.SetActive(true);
            tazaPImage.gameObject.SetActive(true);

            tazasImage.gameObject.SetActive(false);

            // Botones
            buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(true);
            buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(true);
            buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(true);
            buttonManager.cogerTazaPInicioButton.gameObject.SetActive(true);

            buttonManager.cogerPlatoTazaButton.gameObject.SetActive(false);
            buttonManager.cogerTazaInicioButton.gameObject.SetActive(false);
            buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(false);
            buttonManager.cogerVasoPInicioButton.gameObject.SetActive(false);
            buttonManager.coverPButton.gameObject.SetActive(false);

        }
        else if (!hasPremiumCupCard && hasPremiumVaseCard) // Skin vaso desbloqueada
        {
            // Imagenes props
            estanteVasoPremium.SetActive(true);
            coverPImage.gameObject.SetActive(true);
            vasoPImage.gameObject.SetActive(true);
            tazasImage.gameObject.SetActive(true);

            tazaPImage.gameObject.SetActive(false);
            tazaBImage.gameObject.SetActive(false);

            // Botones
            buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(true);
            buttonManager.cogerVasoPInicioButton.gameObject.SetActive(true);
            buttonManager.coverPButton.gameObject.SetActive(true);

            buttonManager.cogerVasoInicioButton.gameObject.SetActive(false);
            buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(false);
            buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(false);
            buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(false);
            buttonManager.cogerTazaPInicioButton.gameObject.SetActive(false);
        }
        else // Ninguna skin desbloqueada
        {
            // Imagenes props
            estanteBase.SetActive(true);
            tazasImage.gameObject.SetActive(true);

            platoTazaPImage.gameObject.SetActive(false);
            coverPImage.gameObject.SetActive(false);
            vasoPImage.gameObject.SetActive(false);
            tazaPImage.gameObject.SetActive(false);
            tazaBImage.gameObject.SetActive(false);

            // Botones
            buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(false);
            buttonManager.cogerVasoPInicioButton.gameObject.SetActive(false);
            buttonManager.coverPButton.gameObject.SetActive(false);
            buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(false);
            buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(false);
            buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(false);
            buttonManager.cogerTazaPInicioButton.gameObject.SetActive(false);
        }
    }

    public void SetSkin(bool isPremium)
    {
        currentSkin = isPremium ? skinPremium : skinBase;
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
                //Debug.Log("La barrita llego al limite");
            }
        }
    }
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
        buttonManager.DisableButton(buttonManager.whiskeyButton);
        buttonManager.DisableButton(buttonManager.coverButton);
        buttonManager.DisableButton(buttonManager.coverPButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);
    }
    public void EnableMechanics()
    {
        buttonManager.EnableButton(buttonManager.whiskeyButton);
        buttonManager.EnableButton(buttonManager.coverButton);
        buttonManager.EnableButton(buttonManager.coverPButton);
        buttonManager.EnableButton(buttonManager.waterButton);
        buttonManager.EnableButton(buttonManager.milkButton);
        buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.EnableButton(buttonManager.condensedMilkButton);
        buttonManager.EnableButton(buttonManager.creamButton);
        buttonManager.EnableButton(buttonManager.chocolateButton);
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
        }

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 0)
            buttonManager.DisableButton(buttonManager.bakeryButton);
        else
            buttonManager.EnableButton(buttonManager.bakeryButton);

        if (!tutorialManager.isRunningT1 && !tutorialManager.isRunningT2)
        {
            if (tazaInHand || vasoInHand || TengoOtroObjetoEnLaMano() || platoTazaInHand || filtroInHand)
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

        if (tutorialManager.isRunningT3)
            buttonManager.DisableButton(buttonManager.bakeryButton);
        else
            buttonManager.EnableButton(buttonManager.bakeryButton);

        if (cMilkServed)
            buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);

        if (heatedMilk)
            buttonManager.DisableButton(buttonManager.calentarButton);
    }
    
    public void CheckBloomEffect()
    {
        if (tazaIsInCafetera || tazaIsInPlato) 
        {
            tazasImage.material = defaultMaterial;
            tazaBImage.material = defaultMaterial;
            tazaPImage.material = defaultMaterial;
        } 
        else if (vasoIsInCafetera || vasoIsInTable) 
        {
            vasoImage.material = defaultMaterial;
            vasoPImage.material = defaultMaterial;
        }
        else if (platoTazaIsInTable) 
        {
            platoTazaImage.material = defaultMaterial;
            platoTazaPImage.material = defaultMaterial;
        }
        else if (countCover > 0 && !coverInHand) 
        {
            coverImage.material = defaultMaterial;
            coverPImage.material = defaultMaterial;
        }
    }

    private CoffeeSkin GetCombinedSkinCup()
    {
        if (!cupIsPremium && !plateIsPremium) return skinBase; // Normal - Normal
        if (cupIsPremium && plateIsPremium) return skinPremium; // Premium - Premium
        if (!cupIsPremium && plateIsPremium) return skinB_PP; // Taza N - Plato P
        if (cupIsPremium && !plateIsPremium) return skinP_PB; // Taza P - Plato N

        return skinBase;
    }

    private CoffeeSkin GetCombinedSkinVase()
    {
        if (!vasoIsPremium && !coverIsPremium) return skinBase; // Normal - Normal
        if (vasoIsPremium && coverIsPremium) return skinPremium; // Premium - Premium
        if (!vasoIsPremium && coverIsPremium) return skinB_PP; // Taza N - Plato P
        if (vasoIsPremium && !coverIsPremium) return skinP_PB; // Taza P - Plato N

        return skinBase;
    }
    #endregion

    #region Envases
    public void CogerTaza(bool isPrem)
    {
        if (coffeeServed || countChocolate != 0 || countCondensedMilk != 0 || countCream != 0 || countMilk != 0 || countWhiskey != 0 || countWater != 0) return; 

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !filtroInHand && !platoTazaInHand && !tazaMilkInHand)
        {
            cupIsPremium = isPrem;
            currentSkin = cupIsPremium ? skinPremium : skinBase;
            tazaInHand = true;

            if (isPrem) tazaPImage.material = glowMaterial; 
            else if (!isPrem && hasPremiumCupCard) tazaBImage.material = glowMaterial;
            else tazasImage.material = glowMaterial;

            TakeCupSound();
            DragController.Instance.StartDragging(currentSkin.tazaSinCafe);
        }
        else if (tazaInHand == true)
        {
            cupIsPremium = false;
            tazaInHand = false;
            currentSkin = null;
            currentSprite = null;

            if (isPrem) tazaPImage.material = defaultMaterial;
            else if (!isPrem && hasPremiumCupCard) tazaBImage.material = defaultMaterial;
            else tazasImage.material = defaultMaterial;

            TakeCupSound();
            DragController.Instance.StopDragging();
        }
    }

    public void CogerPlatoTaza(bool isPrem)
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !filtroInHand && !platoTazaInHand && !tazaMilkInHand)
        {
            plateIsPremium = isPrem;
            currentSkin = plateIsPremium ? skinPremium : skinBase;
            platoTazaInHand = true;
            
            if (isPrem) platoTazaPImage.material = glowMaterial;
            else platoTazaImage.material = glowMaterial; 
            
            TakePlateSound();
            DragController.Instance.StartDragging(currentSkin.platoTaza);
        }
        else if (platoTazaInHand == true)
        {
            plateIsPremium = false;
            platoTazaInHand = false;
            currentSkin = null;
            currentSprite = null;

            if (isPrem) platoTazaPImage.material = defaultMaterial;
            else platoTazaImage.material = defaultMaterial; 
            
            TakePlateSound();
            DragController.Instance.StopDragging();
        }
    }

    public void CogerVaso(bool isPrem)
    {
        if (coffeeServed || countChocolate != 0 || countCondensedMilk != 0 || countCream != 0 || countMilk != 0 || countWhiskey != 0 || countWater != 0) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !filtroInHand && !platoTazaInHand && !tazaMilkInHand)
        {
            vasoIsPremium = isPrem;
            currentSkin = vasoIsPremium ? skinPremium : skinBase;
            vasoInHand = true;

            if (isPrem) vasoPImage.material = glowMaterial;
            else vasoImage.material = glowMaterial; 
            
            TakeVaseSound();
            DragController.Instance.StartDragging(currentSkin.vasoSinTapa);
        }
        else if (vasoInHand == true)
        {
            vasoIsPremium = false;
            vasoInHand = false;
            currentSkin = null;
            currentSprite = null;

            if (isPrem) vasoPImage.material = defaultMaterial;
            else vasoImage.material = defaultMaterial;

            TakeVaseSound();
            DragController.Instance.StopDragging();
        }
    }

    public void ActualizarBotonCogerEnvase()
    {
        if (coffeeServed || countChocolate != 0 || countCondensedMilk != 0 || countCream != 0 || countMilk != 0 || countWhiskey != 0 || countWater != 0)
        {
            buttonManager.DisableButton(buttonManager.cogerTazaInicioButton);
            buttonManager.DisableButton(buttonManager.cogerTazaB2InicioButton);
            buttonManager.DisableButton(buttonManager.cogerTazaPInicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoInicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoB2InicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoPInicioButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
            buttonManager.EnableButton(buttonManager.cogerTazaB2InicioButton);
            buttonManager.EnableButton(buttonManager.cogerTazaPInicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoB2InicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoPInicioButton);
        }
    }
    public void ToggleTazaCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand || filtroInHand || platoTazaInHand) return;
        if (!tazaInHand && !tazaIsInCafetera) return;

        if (!tazaIsInCafetera && tazaInHand)
        {
            TakeCupSound();

            // Poner en la cafetera
            Taza.SetActive(true);
            Taza.transform.position = puntoCafeteraTaza.position;

            tazaInHand = false;
            tazaIsInCafetera = true;

            DragController.Instance.StopDragging();

            if (coffeeServed)
                UpdateCupSprite(false);
            else if (countWater == 0 && countMilk == 0 && countCream == 0 && countCondensedMilk == 0 && countChocolate == 0 && countWhiskey == 0)
                UpdateCupSprite(false);

            if (!tutorialManager.isRunningT1)
                buttonManager.EnableButton(buttonManager.coffeeButton);
            
            EnableMechanics();

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 3)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
        else if (tazaIsInCafetera && !tazaInHand)
        {
            TakeCupSound();

            //Recoger de la cafetera
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInCafetera = false;

            DragController.Instance.StartDragging(currentSprite != null ? currentSprite : currentSkin.tazaSinCafe);
            DisableMechanics();
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }
    public void ToggleTazaPlato()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand) return;
        if (!tazaInHand && !tazaIsInPlato) return;
        if (!platoTazaIsInTable) return;

        if (!tazaIsInPlato && tazaInHand)
        {
            TakeCupSound();
            currentSkin = GetCombinedSkinCup();

            // Poner en el plato
            Taza.SetActive(true);
            Taza.transform.position = puntoTazaPlato.position;

            tazaInHand = false;
            tazaIsInPlato = true;
            cupServed = true;

            UpdateCupSprite(true);
            PlatoTaza.SetActive(false);

            finalCupIsPremium = cupIsPremium;
            finalPlateIsPremium = plateIsPremium;
            finalVasoIsPremium = false;
            finalCoverIsPremium = false;

            DragController.Instance.StopDragging();
            // Se asocia a la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(true, Taza.GetComponent<Image>(), Taza.GetComponent<Image>().sprite);
        }
        else if (tazaIsInPlato && !tazaInHand)
        {
            TakeCupSound();
            currentSkin = cupIsPremium ? skinPremium : skinBase;

            //Recoger del plato
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInPlato = false;
            cupServed = false;

            finalCupIsPremium = false;
            finalPlateIsPremium = false;

            UpdateCupSprite(false);
            PlatoTaza.SetActive(true);
            DragController.Instance.StartDragging(currentSprite != null ? currentSprite : currentSkin.tazaSinCafe);

            // Se quita de la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(false, null, null);
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
            TakeVaseSound();

            // Poner en la cafetera
            Vaso.SetActive(true);
            Sprite spritePoner = currentSprite != null ? currentSprite : currentSkin.vasoSinTapa;
            Vaso.GetComponent<Image>().sprite = spritePoner;
            currentSprite = spritePoner;

            Vaso.transform.position = puntoCafeteraVaso.position;
            Vaso.transform.position = puntoCafeteraVaso.position;

            vasoInHand = false;
            vasoIsInCafetera = true;

            DragController.Instance.StopDragging();

            buttonManager.DisableButton(buttonManager.cogerPlatoTazaButton);
            buttonManager.DisableButton(buttonManager.cogerPlatoTazaB2Button);
            buttonManager.DisableButton(buttonManager.cogerPlatoTazaPButton);

            if (!tutorialManager.isRunningT1)
                buttonManager.EnableButton(buttonManager.coffeeButton);

            EnableMechanics();

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 3)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
        else if (vasoIsInCafetera && !vasoInHand)
        {
            TakeVaseSound();

            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInCafetera = false;

            DragController.Instance.StartDragging(currentSprite != null ? currentSprite : currentSkin.vasoSinTapa);
            DisableMechanics();
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }
    public void ToggleVasoMesa()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand) return;
        if (!vasoInHand && !vasoIsInTable) return;

        if (!vasoIsInTable && vasoInHand)
        {
            TakeVaseSound();

            // Poner en la cafetera
            Vaso.SetActive(true);
            Sprite spriteVaso = currentSprite != null ? currentSprite : currentSkin.vasoSinTapa;
            Vaso.GetComponent<Image>().sprite = spriteVaso;
            Vaso.transform.position = puntoMesaVaso.position;

            vasoInHand = false;
            vasoIsInTable = true;
            cupServed = true;

            finalVasoIsPremium = vasoIsPremium;
            finalCupIsPremium = false;
            finalPlateIsPremium = false;

            DragController.Instance.StopDragging();
            // Se deja en la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(true, Vaso.GetComponent<Image>(), Vaso.GetComponent<Image>().sprite);

            if (coffeeServed)
            {
                buttonManager.EnableButton(buttonManager.coverButton);
                buttonManager.EnableButton(buttonManager.coverPButton);
            }
        }
        else if (vasoIsInTable && !vasoInHand)
        {
            TakeVaseSound();

            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInTable = false;
            cupServed = false;

            finalVasoIsPremium = false;

            buttonManager.DisableButton(buttonManager.coverButton);
            buttonManager.DisableButton(buttonManager.coverPButton);

            DragController.Instance.StartDragging(currentSprite != null ? currentSprite : currentSkin.vasoSinTapa);
            // Se quita de la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(false, null, null);
        }
    }
    public void PlacePlatoTazaMesa()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand || filtroInHand) return;
        if (!platoTazaInHand && !platoTazaIsInTable) return;

        if (platoTazaInHand)
        {
            TakePlateSound();

            // Poner en la mesa
            PlatoTaza.SetActive(true);
            PlatoTaza.GetComponent<Image>().sprite = currentSkin.platoTaza;
            PlatoTaza.transform.position = puntoMesa.position;

            platoTazaInHand = false;
            platoTazaIsInTable = true;

            DragController.Instance.StopDragging();

            buttonManager.DisableButton(buttonManager.cogerPlatoTazaButton);
            buttonManager.DisableButton(buttonManager.cogerPlatoTazaB2Button);
            buttonManager.DisableButton(buttonManager.cogerPlatoTazaPButton);

            buttonManager.DisableButton(buttonManager.cogerVasoInicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoB2InicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoPInicioButton);
        }
        else if (!platoTazaInHand && !cupServed && !tazaInHand)
        {
            TakePlateSound();

            // Poner en la mesa
            PlatoTaza.SetActive(false);

            platoTazaInHand = true;
            platoTazaIsInTable = false;

            DragController.Instance.StartDragging(currentSprite != null ? currentSprite : currentSkin.platoTaza);

            buttonManager.EnableButton(buttonManager.cogerPlatoTazaButton);
            buttonManager.EnableButton(buttonManager.cogerPlatoTazaB2Button);
            buttonManager.EnableButton(buttonManager.cogerPlatoTazaPButton);

            buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoB2InicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoPInicioButton);
        }
    }
#endregion

    #region Mecanicas cafe
    public void StartCoffee()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 8) return;
        if (TengoOtroObjetoEnLaMano() || vasoInHand || tazaInHand || platoTazaInHand || tazaMilkInHand) return;

        if  (!isSliding && !coffeeDone)
        {
            BotonDownMachine();
            //reiniciamos la pos de la barra
            currentSlideTime = 0f;

            isSliding = true;
            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Carga de cafe iniciada.");
        }
    }
    public void StopCoffee()
    {
        if (isSliding)
        {
            BotonUpMachine();

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
    public void StartMoler()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep != 9) return;
        if (TengoOtroObjetoEnLaMano() || vasoInHand || tazaInHand || platoTazaInHand || tazaMilkInHand) return;
        if (!coffeeDone) return;

        if (!isMoliendo)
        {
            MolerCafeSound();

            currentMolido = 0f;
            isMoliendo = true;

            molerPanel.SetActive(true);
            molerFillImage.fillAmount = 0f;
            molerFillImage.color = Color.yellow;
            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Moliendo cafe...");
        }
    }
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
    public void TakeFiltro()
    {
        if (TengoOtroObjetoEnLaMano() || tazaInHand || vasoInHand || platoTazaInHand || tazaMilkInHand) return;

        if (!filtroIsInCafetera)
        {
            filtroInHand = true;

            DragController.Instance.StartDragging(filtroImg);

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

            DragController.Instance.StopDragging();
            buttonManager.DisableButton(buttonManager.filtroCafeteraButton);

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
        if (TengoOtroObjetoEnLaMano() || tazaInHand || vasoInHand || platoTazaInHand || tazaMilkInHand) return;

        bool recipienteEnCafetera = tazaIsInCafetera || vasoIsInCafetera;
        if (!recipienteEnCafetera || !filtroIsInCafetera) return;

        //Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Echando cafe...");

        BotonDownMachine();
        CafeteraSound();
        isServing = true;
        movingRight = true;

        Image echarCafeBut = buttonManager.echarCafeButton.GetComponent<Image>();
        echarCafeBut.sprite = boton1_P;

        buttonManager.DisableButton(buttonManager.echarCafeButton);
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

        SoundsMaster.Instance.StopAudio("CoffeeMachine");

        BotonDownMachine();
        EcharCafeSound();

        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);

        if (order != null && order.currentOrder != null)
        {
            order.currentOrder.coffeeServedPrecision = normalizedPrecision;
            //Debug.Log($"[Cliente {order.currentOrder.orderId}] Echar cafe detenido en: {normalizedPrecision}");
        }

        order.currentOrder.stepsPerformed.Add(OrderStep.AddCoffee);
        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);

        buttonManager.EnableButton(buttonManager.sugarButton);
        buttonManager.EnableButton(buttonManager.iceButton);
        buttonManager.EnableButton(buttonManager.coverButton);
        buttonManager.EnableButton(buttonManager.coverPButton);

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
        ActualizarBotonCogerEnvase();

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
            newBaseSprite = inPlato ? currentSkin.tazaSinCafeP : currentSkin.tazaSinCafe;
        }
        // Agua sin cafe
        else if (countWater != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? currentSkin.tazaNWaterP : currentSkin.tazaNWater;
        }
        // Leche / crema / leche condensada sin cafe
        else if ((countMilk != 0 || countCondensedMilk != 0 || countCream != 0) && !coffeeServed)
        {
            newBaseSprite = inPlato ? currentSkin.tazaNMilkP : currentSkin.tazaNMilk;
        }
        // Chocolate
        else if (countChocolate != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? currentSkin.tazaNChocolateP : currentSkin.tazaNChocolate;
        }
        // Whiskey
        else if (countWhiskey != 0 && !coffeeServed)
        {
            newBaseSprite = inPlato ? currentSkin.tazaNWhiskeyP : currentSkin.tazaNWhiskey;
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
            case CoffeeType.espresso: return isCup ? currentSkin.tazaNEspresso : currentSkin.vasoNEspresso;
            case CoffeeType.lungo: return isCup ? currentSkin.tazaNAmericanoLungo : currentSkin.vasoNAmericanoLungo;
            case CoffeeType.americano: return isCup ? currentSkin.tazaNAmericanoLungo : currentSkin.vasoNAmericanoLungo;
            case CoffeeType.latte: 
                return isCup ? (hasLatteArtCard ? currentSkin.tazaNMocaIrishLatteD : currentSkin.tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? currentSkin.vasoNMocaIrishLatteD : currentSkin.vasoNMocaIrishLatte);
            case CoffeeType.capuccino: return isCup ? currentSkin.tazaNCappuccino : currentSkin.vasoNCappuccino;
            case CoffeeType.irish:
                return isCup ? (hasLatteArtCard ? currentSkin.tazaNMocaIrishLatteD : currentSkin.tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? currentSkin.vasoNMocaIrishLatteD : currentSkin.vasoNMocaIrishLatte);
            case CoffeeType.mocca:
                return isCup ? (hasLatteArtCard ? currentSkin.tazaNMocaIrishLatteD : currentSkin.tazaNMocaIrishLatte)
                             : (hasLatteArtCard ? currentSkin.vasoNMocaIrishLatteD : currentSkin.vasoNMocaIrishLatte);
            case CoffeeType.macchiatto:
                return isCup ? (hasLatteArtCard ? currentSkin.tazaNMachiatoBombonD : currentSkin.tazaNMachiatoBombon)
                             : (hasLatteArtCard ? currentSkin.vasoNMachiatoBombonD : currentSkin.vasoNMachiatoBombon);
            case CoffeeType.bombon:
                return isCup ? (hasLatteArtCard ? currentSkin.tazaNMachiatoBombonD : currentSkin.tazaNMachiatoBombon)
                             : (hasLatteArtCard ? currentSkin.vasoNMachiatoBombonD : currentSkin.vasoNMachiatoBombon);
            case CoffeeType.vienes:
                return isCup ? (hasLatteArtCard ? currentSkin.tazaNVienesD : currentSkin.tazaNVienes)
                             : (hasLatteArtCard ? currentSkin.vasoNVienesD : currentSkin.vasoNVienes);
            case CoffeeType.frappe: return isCup ? currentSkin.tazaNFrappe : currentSkin.vasoNFrappe;
        }
        return isCup ? currentSkin.tazaSinCafe : currentSkin.vasoSinTapa;
    }

    // Vincular el sprite actual de la taza con su plato
    private Sprite CheckFinalCupPlate(Sprite sprite, bool hasLatteArtCard)
    {
        if (currentSprite == currentSkin.tazaNEspresso)
            return currentSkin.tazaNEspressoP;
        if (currentSprite == currentSkin.tazaNAmericanoLungo)
            return currentSkin.tazaNAmericanoLungoP;
        if (currentSprite == currentSkin.tazaNMachiatoBombon || currentSprite == currentSkin.tazaNMachiatoBombonD)
            return hasLatteArtCard ? currentSkin.tazaNMachiatoBombonDP : currentSkin.tazaNMachiatoBombonP;
        if (currentSprite == currentSkin.tazaNMocaIrishLatte || currentSprite == currentSkin.tazaNMocaIrishLatteD)
            return hasLatteArtCard ? currentSkin.tazaNMocaIrishLatteDP : currentSkin.tazaNMocaIrishLatteP;
        if (currentSprite == currentSkin.tazaNCappuccino)
            return currentSkin.tazaNCappuccinoP;
        if (currentSprite == currentSkin.tazaNVienes || currentSprite == currentSkin.tazaNVienesD)
            return hasLatteArtCard ? currentSkin.tazaNVienesDP : currentSkin.tazaNVienesP;
        if (currentSprite == currentSkin.tazaNFrappe)
            return currentSkin.tazaNFrappeP;
        
        return currentSkin.tazaSinCafeP;
    }
    #endregion

    #region Mecanicas leche
    public void CogerLeche()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !filtroInHand && !platoTazaInHand)
        {
            milkInHand = true;
            milkImage.material = glowMaterial;
            CogerDejarObjSound();

            DragController.Instance.StartDragging(milkImage.sprite);
        }
        else if (milkInHand == true)
        {
            milkInHand = false;
            milkImage.material = defaultMaterial;
            CogerDejarObjSound();

            DragController.Instance.StopDragging();
        }
    }
    public void EcharLecheFria()
    {
        //Si se tiene la leche en la mano y el cafe no esta servido entonces se puede echar
        if (milkInHand == true)
        {
            if (countMilk <= 1)
            {
                EcharLiquidoSound();
                countMilk += 1; //Se incrementa el contador de leche
                order.currentOrder.milkPrecision = countMilk; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage($"+{countMilk} Leche");
            }
            milkServed = true;
            cMilkServed = true;
            order.currentOrder.stepsPerformed.Add(OrderStep.AddMilk);

            if (tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = Taza.GetComponent<Image>().sprite;
            }
            else if (vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            ActualizarBotonCogerEnvase();
        }
    }
    public void CogerJarraLeche()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !filtroInHand && !platoTazaInHand && !tazaMilkInHand)
        {
            tazaMilkInHand = true;
            milkCupImage.material = glowMaterial;
            CogerDejarObjSound();

            DragController.Instance.StartDragging(TazaLeche.GetComponent<Image>().sprite);
        }
        else if (tazaMilkInHand == true)
        {
            tazaMilkInHand = false;
            milkCupImage.material = defaultMaterial;
            CogerDejarObjSound();

            DragController.Instance.StopDragging();
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
            CogerDejarObjSound();
            DragController.Instance.StopDragging();

            // Poner en el espumador
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
        else if (tazaMilkIsInEspumador && !tazaMilkInHand)
        {
            CogerDejarObjSound();
            DragController.Instance.StartDragging(TazaLeche.GetComponent<Image>().sprite);

            //Recoger del espumador
            TazaLeche.SetActive(false);

            tazaMilkInHand = true;
            tazaMilkIsInEspumador = false;

            buttonManager.DisableButton(buttonManager.calentarButton);
            Image espumador = Espumador.GetComponent<Image>();
            espumador.sprite = espumadorNormal;
        }
    }
    public void StartHeating()
    {
        if (!tazaMilkIsInEspumador || TengoOtroObjetoEnLaMano()) return;
        if (heatedMilk) return;

        if (!isHeating && !heatedMilk)
        {
            BotonDownMachine();
            EspumadorSound();

            currentHeat = 0f;
            isHeating = true;

            heatPanel.SetActive(true);
            curvedFillImage.fillAmount = 0f;
            curvedFillImage.color = Color.blue;

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 2)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();
        }
    }
    public void StopHeating()
    {
        if (isHeating)
        {
            SoundsMaster.Instance.StopAudio("Espumador");

            isHeating = false;

            heatPanel.SetActive(false);
            buttonManager.DisableButton(buttonManager.calentarButton);

            Image pararCalentarLecheBut = buttonManager.calentarButton.GetComponent<Image>();
            pararCalentarLecheBut.sprite = boton3_P;

            heatedMilk = true;
        }
    }
    public void EcharLecheCaliente()
    {
        //Si se tiene la leche caliente en la mano y el cafe no esta servido entonces se puede echar
        if (tazaMilkInHand == true)
        {
            if (countMilk <= 1)
            {
                if (currentHeat < 0.2f)
                {
                    EcharLiquidoSound();
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 0;
                }
                else if (currentHeat < 0.6f)
                {
                    EcharLiquidoSound();
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 1;
                }
                else
                {
                    EcharLiquidoSound();
                    countMilk += 1;
                    order.currentOrder.milkPrecision = countMilk;
                    order.currentOrder.heatedMilkPrecision = 2;
                }

                milkServed = true;
                order.currentOrder.stepsPerformed.Add(OrderStep.AddMilk);
                order.currentOrder.stepsPerformed.Add(OrderStep.HeatMilk);

                PopUpMechanicsMsg.Instance.ShowMessage($"+{countMilk} Leche");

                if (tazaIsInCafetera)
                {
                    UpdateCupSprite(false);
                    currentSprite = Taza.GetComponent<Image>().sprite;
                }
                else if (vasoIsInCafetera && coffeeServed)
                {
                    CoffeeType currentType = DetermineCoffeeType();
                    bool isCup = false;
                    currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                    Image vaso = Vaso.GetComponent<Image>();
                    vaso.sprite = currentSprite;
                }
                ActualizarBotonCogerEnvase();
            }
        }
    }
    #endregion

    #region Mecanica agua
    public void CogerAgua()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand && !filtroInHand && !platoTazaInHand)
        {
            waterInHand = true;
            waterImage.material = glowMaterial;
            CogerDejarObjSound();

            DragController.Instance.StartDragging(waterImage.sprite);
        }
        else if (waterInHand == true)
        {
            waterInHand = false;
            waterImage.material = defaultMaterial;
            CogerDejarObjSound();

            DragController.Instance.StopDragging();
        }
    }
    public void EcharAgua()
    {
        //Si se tiene el agua en la mano y el cafe no esta servido entonces se puede echar 
        if (waterInHand == true)
        {
            if (countWater < 1)
            {
                EcharLiquidoSound();
                countWater += 1; //Se incrementa el contador de agua
                order.currentOrder.waterPrecision = countWater; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+ Agua");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddWater);
            }

            if (tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = Taza.GetComponent<Image>().sprite;
            }
            else if (vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica leche condensada
    public void CogerLecheCondensada()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand && !filtroInHand && !platoTazaInHand)
        {
            condensedMilkInHand = true;
            condensedMilkImage.material = glowMaterial;
            CogerDejarObjSound();

            DragController.Instance.StartDragging(condensedMilkImage.sprite);
        }
        else if (condensedMilkInHand == true)
        {
            condensedMilkInHand = false;
            condensedMilkImage.material = defaultMaterial;
            CogerDejarObjSound();

            DragController.Instance.StopDragging();
        }
    }
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
                order.currentOrder.stepsPerformed.Add(OrderStep.AddCondensedMilk);
            }

            if (tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = Taza.GetComponent<Image>().sprite;
            }
            else if (vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica crema
    public void CogerCrema()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand && !filtroInHand && !platoTazaInHand)
        {
            creamInHand = true;
            buttonManager.creamButton.image.sprite = creamWithoutSpoon;
            CucharaSound();
        }
        else if (creamInHand == true)
        {
            creamInHand = false;
            buttonManager.creamButton.image.sprite = creamWithSpoon;
            CucharaSound();
        }
    }
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
            }
            order.currentOrder.stepsPerformed.Add(OrderStep.AddCream);
            if (tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = Taza.GetComponent<Image>().sprite;
            }
            else if (vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica chocolate
    public void CogerChocolate()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand && !filtroInHand && !platoTazaInHand)
        {
            chocolateInHand = true;
            chocolateImage.material = glowMaterial;
            CogerDejarObjSound();

            DragController.Instance.StartDragging(chocolateImage.sprite);
        }

        else if (chocolateInHand == true)
        {
            chocolateInHand = false;
            chocolateImage.material = defaultMaterial;
            CogerDejarObjSound();

            DragController.Instance.StopDragging();
        }
    }
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
                order.currentOrder.stepsPerformed.Add(OrderStep.AddChocolate);
            }

            if (tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = Taza.GetComponent<Image>().sprite;
            }
            else if (vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica whiskey
    public void CogerWhiskey()
    {
        if (tazaMilkInHand) return;

        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand && !filtroInHand && !platoTazaInHand)
        {
            whiskeyInHand = true;
            whiskeyImage.material = glowMaterial;
            CogerDejarObjSound();

            DragController.Instance.StartDragging(whiskeyImage.sprite);
        }
        else if (whiskeyInHand == true)
        {
            whiskeyInHand = false;
            whiskeyImage.material = defaultMaterial;
            CogerDejarObjSound();

            DragController.Instance.StopDragging();
        }
    }
    public void EcharWhiskey()
    {
        //Si se tiene el whiskey en la mano y el cafe esta servido entonces se puede echar
        if (whiskeyInHand == true)
        {
            if (countWhiskey < 1)
            {
                EcharLiquidoSound();
                countWhiskey += 1; //Se incrementa el contador de hielo
                order.currentOrder.whiskeyPrecision = countWhiskey; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage("+ Whiskey");
                order.currentOrder.stepsPerformed.Add(OrderStep.AddWhiskey);
            }

            if (tazaIsInCafetera)
            {
                UpdateCupSprite(false);
                currentSprite = Taza.GetComponent<Image>().sprite;
            }
            else if (vasoIsInCafetera && coffeeServed)
            {
                CoffeeType currentType = DetermineCoffeeType();
                bool isCup = false;
                currentSprite = GetBaseCoffeeSprite(currentType, isCup);
                Image vaso = Vaso.GetComponent<Image>();
                vaso.sprite = currentSprite;
            }
            ActualizarBotonCogerEnvase();
        }
    }
    #endregion

    #region Mecanica azucar
    public void CogerAzucar()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && countCover <= 0 && !filtroInHand && !platoTazaInHand)
        {
            cucharaInHand = true;
            CucharaSound();
        }
        else if (cucharaInHand == true)
        {
            cucharaInHand = false;
            CucharaSound();
        }
    }
    public void EcharAzucar()
    {
        //Si se tiene la cuchara de azucar en la mano y el cafe esta servido entonces se puede echar
        if (cucharaInHand == true && coffeeServed == true)
        {
            if (countSugar <= 1)
            {
                AzucarSound();
                countSugar += 1; //Se incrementa el contador de hielo
                order.currentOrder.sugarPrecision = countSugar; // Se guarda el resultado obtenido en la precision del jugador
                PopUpMechanicsMsg.Instance.ShowMessage($"+{countSugar} Azúcar");
            }
        }
    }
    #endregion

    #region Mecanica hielo
    public void CogerHielo()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && countCover <= 0 && !filtroInHand && !platoTazaInHand)
        {
            iceInHand = true;
            CogerHieloSound();
        }
        else if (iceInHand == true)
        {
            iceInHand = false;
            DejarHieloSound();
        }
    }
    public void EcharHielo()
    {
        //Si se tiene la cuchara de hielo en la mano y el cafe esta servido entonces se puede echar
        if (iceInHand == true && coffeeServed == true)
        {
            if (countIce < 1)
            {
                EcharHieloSound();
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce; // Se guarda el resultado obtenido en la precision del jugador
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
    public void CogerTapa(bool isPrem)
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            coverIsPremium = isPrem;
            currentSkin = coverIsPremium ? skinPremium : skinBase;
            coverInHand = true;

            if (isPrem) coverPImage.material = glowMaterial;
            else coverImage.material = glowMaterial;

            TakeVaseSound();
            DragController.Instance.StartDragging(currentSkin.tapaVaso);
        }
        else if (coverInHand == true)
        {
            coverIsPremium = false;
            coverInHand = false;

            if (isPrem) coverPImage.material = defaultMaterial;
            else coverImage.material = defaultMaterial; 
            
            TakeVaseSound();
            DragController.Instance.StopDragging();
        }
    }
    public void PonerTapa()
    {
        //Si se tiene la tapa en la mano y el cafe esta servido entonces se puede poner
        if (coverInHand == true && coffeeServed == true && (vasoIsInCafetera || vasoIsInTable))
        {
            if (countCover < 1)
            {
                countCover += 1; //Se incrementa el contador de hielo
                order.currentOrder.typePrecision = countCover; // Se guarda el resultado obtenido en la precision del jugador
                order.currentOrder.stepsPerformed.Add(OrderStep.PutCover); // Se añade a la lista de pasos
            }

            TakeVaseSound();
            Image vaso = Vaso.GetComponent<Image>();
            currentSkin = GetCombinedSkinVase();
            vaso.sprite = currentSkin.vasoConTapa;
            currentSprite = currentSkin.vasoConTapa;
            DragController.Instance.StopDragging();

            finalCoverIsPremium = coverIsPremium;

            if (vasoIsInTable)
            {
                // Se actualiza en la bandeja
                CoffeeFoodManager.Instance.ToggleCafe(true, Vaso.GetComponent<Image>(), Vaso.GetComponent<Image>().sprite);
            }

            buttonManager.DisableButton(buttonManager.coverButton);
            buttonManager.DisableButton(buttonManager.coverPButton);

            coverImage.material = defaultMaterial;
            coverPImage.material = defaultMaterial;

            coverInHand = false;
        }
    }
    #endregion

    #region Sonidos envases
    public void TakeCupSound()
    {
        SoundsMaster.Instance.PlaySound_TakeCup();
    }
    public void TakeVaseSound()
    {
        SoundsMaster.Instance.PlaySound_TakeVase();
    }

    public void TakePlateSound()
    {
        SoundsMaster.Instance.PlaySound_TakePlate();
    }

    public void BotonDownMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeAmountMachine();
    }

    public void BotonUpMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeAmountReady();
    }

    public void MolerCafeSound()
    {
        SoundsMaster.Instance.PlaySound_MolerCafe();
    }

    public void CafeteraSound()
    {
        SoundsMaster.Instance.PlayAudio("CoffeeMachine");
    }   

    public void EcharCafeSound()
    {
        SoundsMaster.Instance.PlaySound_EcharCafe();
    }

    public void EspumadorSound()
    {
        SoundsMaster.Instance.PlayAudio("Espumador");
    }

    public void EcharLiquidoSound()
    {
        SoundsMaster.Instance.PlaySound_EcharLiquido();
    }

    public void CogerDejarObjSound()
    {
        SoundsMaster.Instance.PlaySound_CogerDejarObj();
    }

    public void CogerHieloSound()
    {
        SoundsMaster.Instance.PlaySound_CogerHielo();
    }

    public void EcharHieloSound()
    {
        SoundsMaster.Instance.PlaySound_EcharHielo();
    }

    public void DejarHieloSound()
    {
        SoundsMaster.Instance.PlaySound_DejarHielo();
    }

    public void CucharaSound()
    {
        SoundsMaster.Instance.PlaySound_Cuchara();
    }

    public void AzucarSound()
    {
        SoundsMaster.Instance.PlaySound_Azucar();
    }

    public void PapeleraSound()
    {
        SoundsMaster.Instance.PlaySound_Papelera();
    }
    #endregion
}
