using UnityEngine;
using UnityEngine.UI;

public class CoffeeContainerManager : MonoBehaviour
{
    public static CoffeeContainerManager instance;

    [Header("Referencia minigame")]
    public MinigameInput minigame;

    [Header("Objetos fisicos")]
    public GameObject Taza;
    public GameObject Vaso;
    public GameObject PlatoTaza;
    public GameObject Estante;

    [Header("GameObjects estante")]
    public GameObject estanteBase;
    public GameObject estanteTazaPremium;
    public GameObject estanteVasoPremium;
    public GameObject estantePremium;

    [Header("Puntos transform")]
    public Transform puntoCafeteraTaza;
    public Transform puntoCafeteraVaso;
    public Transform puntoMesa;
    public Transform puntoTazaPlato;
    public Transform puntoMesaVaso;

    [Header("Configuracion skins")]
    public CoffeeSkin skinBase;
    public CoffeeSkin skinPremium;
    public CoffeeSkin skinB_PP;
    public CoffeeSkin skinP_PB;
    public bool cupIsPremium;
    public bool plateIsPremium;
    public bool vasoIsPremium;
    public bool coverIsPremium;
    public CoffeeSkin currentSkin;
    bool hasPremiumCupCard;
    bool hasPremiumVaseCard;

    [Header("Estado final skins")]
    public bool finalCupIsPremium;
    public bool finalPlateIsPremium;
    public bool finalVasoIsPremium;
    public bool finalCoverIsPremium;

    [Header("Materiales UI glow")]
    public Material defaultMaterial;
    public Material glowMaterial;
    public Image tazasImage;
    public Image tazaBImage;
    public Image tazaPImage;
    public Image vasoImage;
    public Image vasoPImage;
    public Image platoTazaImage;
    public Image platoTazaPImage;
    public Image coverImage;
    public Image coverPImage;

    [Header("Variables interaccion")]
    public bool tazaInHand, vasoInHand, platoTazaInHand, coverInHand;

    public bool tazaIsInCafetera, tazaIsInPlato;
    public bool vasoIsInCafetera, vasoIsInTable;
    public bool platoTazaIsInTable;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Booleano que marca si el jugador tiene la skin de la taza
        hasPremiumCupCard = PlayerDataManager.instance.HasCard("CartaSkinTaza");
        // Booleano que marca si el jugador tiene la skin del vaso
        hasPremiumVaseCard = (PlayerDataManager.instance.HasCard("CartaSkinVaso1") && PlayerDataManager.instance.HasCard("CartaSkinVaso2"));
    }

    public void Start()
    {
        ActualizarBotonCogerEnvase();
        UpdateEstanteSprite();
        UpdateStartSprites();
    }

    public void Update()
    {
        CheckBloomEffect();
    }

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
    }

    public bool cupNotEmpty()
    {
        bool notEmpty = minigame.coffeeServed || minigame.countChocolate != 0 || minigame.countCondensedMilk != 0 || minigame.countCream != 0 || minigame.countMilk != 0 || minigame.countWhiskey != 0 || minigame.countWater != 0;
        return notEmpty;
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

    public void CheckBloomEffect()
    {
        if (tazaIsInCafetera || minigame.coffeeContainerManager.tazaIsInPlato)
        {
            tazasImage.material = defaultMaterial;
            tazaBImage.material = defaultMaterial;
            tazaPImage.material = defaultMaterial;
        }
        else if (vasoIsInCafetera || minigame.coffeeContainerManager.vasoIsInTable)
        {
            vasoImage.material = defaultMaterial;
            vasoPImage.material = defaultMaterial;
        }
        else if (platoTazaIsInTable)
        {
            platoTazaImage.material = defaultMaterial;
            platoTazaPImage.material = defaultMaterial;
        }
        else if (minigame.countCover > 0 && !coverInHand)
        {
            coverImage.material = defaultMaterial;
            coverPImage.material = defaultMaterial;
        }
    }

    // Funcion para gestionar el sprite del estante de los envases segun las skins desbloqueadas
    public void UpdateEstanteSprite()
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
            minigame.buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(true);
            minigame.buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerVasoPInicioButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerTazaPInicioButton.gameObject.SetActive(true);
            minigame.buttonManager.coverPButton.gameObject.SetActive(true);

            minigame.buttonManager.cogerPlatoTazaButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerTazaInicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerVasoInicioButton.gameObject.SetActive(false);
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
            minigame.buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(true);
            minigame.buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerTazaPInicioButton.gameObject.SetActive(true);

            minigame.buttonManager.cogerPlatoTazaButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerTazaInicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerVasoPInicioButton.gameObject.SetActive(false);
            minigame.buttonManager.coverPButton.gameObject.SetActive(false);

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
            minigame.buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(true);
            minigame.buttonManager.cogerVasoPInicioButton.gameObject.SetActive(true);
            minigame.buttonManager.coverPButton.gameObject.SetActive(true);

            minigame.buttonManager.cogerVasoInicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(false);
            minigame.buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerTazaPInicioButton.gameObject.SetActive(false);
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
            minigame.buttonManager.cogerVasoB2InicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerVasoPInicioButton.gameObject.SetActive(false);
            minigame.buttonManager.coverPButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerPlatoTazaB2Button.gameObject.SetActive(false);
            minigame.buttonManager.cogerPlatoTazaPButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerTazaB2InicioButton.gameObject.SetActive(false);
            minigame.buttonManager.cogerTazaPInicioButton.gameObject.SetActive(false);
        }
    }

    public void ActualizarBotonCogerEnvase()
    {
        bool cupNotEmpty = minigame.coffeeServed || minigame.countChocolate != 0 || minigame.countCondensedMilk != 0 ||
                           minigame.countCream != 0 || minigame.countMilk != 0 || minigame.countWhiskey != 0 || minigame.countWater != 0;
        bool vasoTaken = vasoIsInCafetera || vasoIsInTable;

        // Botones taza y vaso
        if (cupNotEmpty)
        {
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerTazaInicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerTazaB2InicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerTazaPInicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerVasoInicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerVasoB2InicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerVasoPInicioButton);
        }
        else
        {
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerTazaInicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerTazaB2InicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerTazaPInicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerVasoInicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerVasoB2InicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerVasoPInicioButton);
        }

        // Botones plato taza
        if (vasoIsInCafetera || vasoIsInTable)
        {
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaB2Button);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaPButton);
        }
        else if (platoTazaIsInTable)
        {
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaB2Button);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaPButton);
        }
        else
        {
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerPlatoTazaButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerPlatoTazaB2Button);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerPlatoTazaPButton);
        }

        // Botones tapa
        if (vasoTaken && minigame.coffeeServed && minigame.countCover == 0)
        {
            minigame.buttonManager.EnableButton(minigame.buttonManager.coverButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.coverPButton);
        }
        else
        {
            minigame.buttonManager.DisableButton(minigame.buttonManager.coverButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.coverPButton);
        }
    }

    public void CogerTaza(bool isPrem)
    {
        if (cupNotEmpty()) return;
        if (tazaIsInCafetera || tazaIsInPlato || vasoIsInCafetera || vasoIsInTable) return;

        if (!minigame.TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !minigame.filtroInHand && !platoTazaInHand && !minigame.tazaMilkInHand)
        {
            cupIsPremium = isPrem;
            currentSkin = cupIsPremium ? skinPremium : skinBase;
            tazaInHand = true;

            if (isPrem) tazaPImage.material = glowMaterial;
            else if (!isPrem && hasPremiumCupCard) tazaBImage.material = glowMaterial;
            else tazasImage.material = glowMaterial;

            MiniGameSoundManager.instance.PlayTakeCup();
            DragController.Instance.StartDragging(currentSkin.tazaSinCafe);
        }
        else if (tazaInHand == true)
        {
            cupIsPremium = false;
            tazaInHand = false;
            currentSkin = null;
            minigame.currentSprite = null;

            if (isPrem) tazaPImage.material = defaultMaterial;
            else if (!isPrem && hasPremiumCupCard) tazaBImage.material = defaultMaterial;
            else tazasImage.material = defaultMaterial;

            MiniGameSoundManager.instance.PlayTakeCup();
            DragController.Instance.StopDragging();
        }
    }

    public void CogerPlatoTaza(bool isPrem)
    {
        if (platoTazaIsInTable) return;
        if (!minigame.TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !minigame.filtroInHand && !platoTazaInHand && !minigame.tazaMilkInHand)
        {
            plateIsPremium = isPrem;
            currentSkin = plateIsPremium ? skinPremium : skinBase;
            platoTazaInHand = true;

            if (isPrem) platoTazaPImage.material = glowMaterial;
            else platoTazaImage.material = glowMaterial;

            MiniGameSoundManager.instance.PlayTakePlate();
            DragController.Instance.StartDragging(currentSkin.platoTaza);
        }
        else if (platoTazaInHand == true)
        {
            plateIsPremium = false;
            platoTazaInHand = false;
            currentSkin = null;
            minigame.currentSprite = null;

            if (isPrem) platoTazaPImage.material = defaultMaterial;
            else platoTazaImage.material = defaultMaterial;

            MiniGameSoundManager.instance.PlayTakePlate();
            DragController.Instance.StopDragging();
        }
    }

    public void CogerVaso(bool isPrem)
    {
        if (vasoIsInCafetera || vasoIsInTable || tazaIsInCafetera || tazaIsInPlato) return;
        if (cupNotEmpty()) return;

        if (!minigame.TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !minigame.filtroInHand && !platoTazaInHand && !minigame.tazaMilkInHand)
        {
            vasoIsPremium = isPrem;
            currentSkin = vasoIsPremium ? skinPremium : skinBase;
            vasoInHand = true;

            if (isPrem) vasoPImage.material = glowMaterial;
            else vasoImage.material = glowMaterial;

            MiniGameSoundManager.instance.PlayTakeVase();
            DragController.Instance.StartDragging(currentSkin.vasoSinTapa);
        }
        else if (vasoInHand == true)
        {
            vasoIsPremium = false;
            vasoInHand = false;
            currentSkin = null;
            minigame.currentSprite = null;

            if (isPrem) vasoPImage.material = defaultMaterial;
            else vasoImage.material = defaultMaterial;

            MiniGameSoundManager.instance.PlayTakeVase();
            DragController.Instance.StopDragging();
        }
    }

    public void ToggleTazaCafetera()
    {
        if (minigame.TengoOtroObjetoEnLaMano() || minigame.tazaMilkInHand || minigame.filtroInHand || platoTazaInHand) return;
        if (!tazaInHand && !tazaIsInCafetera) return;

        if (!tazaIsInCafetera && tazaInHand)
        {
            MiniGameSoundManager.instance.PlayTakeCup();
            currentSkin = cupIsPremium ? skinPremium : skinBase;

            // Poner en la cafetera
            Taza.SetActive(true);
            Taza.transform.position = puntoCafeteraTaza.position;

            tazaInHand = false;
            tazaIsInCafetera = true;

            DragController.Instance.StopDragging();

            if (minigame.coffeeServed)
                minigame.UpdateCupSprite(false);
            else if (minigame.countWater == 0 && minigame.countMilk == 0 && minigame.countCream == 0 && minigame.countCondensedMilk == 0 && minigame.countChocolate == 0 && minigame.countWhiskey == 0)
                minigame.UpdateCupSprite(false);

            if (!minigame.tutorialManager.isRunningT1)
                minigame.buttonManager.EnableButton(minigame.buttonManager.coffeeButton);

            minigame.EnableMechanics();

            if (minigame.tutorialManager.isRunningT1 && minigame.tutorialManager.currentStep == 3)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
        else if (tazaIsInCafetera && !tazaInHand)
        {
            MiniGameSoundManager.instance.PlayTakeCup();
            currentSkin = cupIsPremium ? skinPremium : skinBase;

            //Recoger de la cafetera
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInCafetera = false;

            DragController.Instance.StartDragging(minigame.currentSprite != null ? minigame.currentSprite : currentSkin.tazaSinCafe);
            minigame.DisableMechanics();
        }
        if (minigame.filtroIsInCafetera == true && minigame.coffeeServed == false)
        {
            minigame.buttonManager.EnableButton(minigame.buttonManager.echarCafeButton);
        }
        ActualizarBotonCogerEnvase();
    }
    public void ToggleTazaPlato()
    {
        if (minigame.TengoOtroObjetoEnLaMano() || minigame.tazaMilkInHand) return;
        if (!tazaInHand && !tazaIsInPlato) return;
        if (!platoTazaIsInTable) return;

        if (!tazaIsInPlato && tazaInHand)
        {
            MiniGameSoundManager.instance.PlayTakeCup();
            currentSkin = GetCombinedSkinCup();

            // Poner en el plato
            Taza.SetActive(true);
            Taza.transform.position = puntoTazaPlato.position;

            tazaInHand = false;
            tazaIsInPlato = true;
            minigame.cupServed = true;

            minigame.UpdateCupSprite(true);
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
            MiniGameSoundManager.instance.PlayTakeCup();
            currentSkin = cupIsPremium ? skinPremium : skinBase;

            //Recoger del plato
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInPlato = false;
            minigame.cupServed = false;

            finalCupIsPremium = false;
            finalPlateIsPremium = false;

            minigame.UpdateCupSprite(false);
            PlatoTaza.SetActive(true);
            DragController.Instance.StartDragging(minigame.currentSprite != null ? minigame.currentSprite : currentSkin.tazaSinCafe);

            // Se quita de la bandeja
            CoffeeFoodManager.Instance.ToggleCafe(false, null, null);
        }
        ActualizarBotonCogerEnvase();
    }
    public void ToggleVasoCafetera()
    {
        if (minigame.TengoOtroObjetoEnLaMano() || minigame.tazaMilkInHand || minigame.filtroInHand) return;
        if (!vasoInHand && !vasoIsInCafetera) return;

        if (!vasoIsInCafetera && vasoInHand)
        {
            MiniGameSoundManager.instance.PlayTakeVase();

            // Poner en la cafetera
            Vaso.SetActive(true);
            Sprite spritePoner = minigame.currentSprite != null ? minigame.currentSprite : currentSkin.vasoSinTapa;
            Vaso.GetComponent<Image>().sprite = spritePoner;
            minigame.currentSprite = spritePoner;

            Vaso.transform.position = puntoCafeteraVaso.position;
            Vaso.transform.position = puntoCafeteraVaso.position;

            vasoInHand = false;
            vasoIsInCafetera = true;

            DragController.Instance.StopDragging();

            if (!minigame.tutorialManager.isRunningT1)
                minigame.buttonManager.EnableButton(minigame.buttonManager.coffeeButton);

            minigame.EnableMechanics();

            if (minigame.tutorialManager.isRunningT1 && minigame.tutorialManager.currentStep == 3)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
        else if (vasoIsInCafetera && !vasoInHand)
        {
            MiniGameSoundManager.instance.PlayTakeVase();
            currentSkin = vasoIsPremium ? skinPremium : skinBase;

            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInCafetera = false;

            DragController.Instance.StartDragging(minigame.currentSprite != null ? minigame.currentSprite : currentSkin.vasoSinTapa);
            minigame.DisableMechanics();
        }
        if (minigame.filtroIsInCafetera == true && minigame.coffeeServed == false)
        {
            minigame.buttonManager.EnableButton(minigame.buttonManager.echarCafeButton);
        }
        ActualizarBotonCogerEnvase();
    }

    // Funcion para colocar/quitar el vaso de la mesa
    public void ToggleVasoMesa()
    {
        if (minigame.TengoOtroObjetoEnLaMano() || minigame.tazaMilkInHand) return;
        if (!vasoInHand && !vasoIsInTable) return;

        if (!vasoIsInTable && vasoInHand) // Poner en la mesa
        {
            MiniGameSoundManager.instance.PlayTakeVase(); // Se reproduce el sonido

            // Se activa el vaso con el sprite y la posicion indicados y se actualizan los booleanos 
            Vaso.SetActive(true);
            Sprite spriteVaso = minigame.currentSprite != null ? minigame.currentSprite : currentSkin.vasoSinTapa;
            Vaso.GetComponent<Image>().sprite = spriteVaso;
            Vaso.transform.position = puntoMesaVaso.position;

            vasoInHand = false;
            vasoIsInTable = true;
            minigame.cupServed = true;

            finalVasoIsPremium = vasoIsPremium;
            finalCupIsPremium = false;
            finalPlateIsPremium = false;

            DragController.Instance.StopDragging(); // Se suelta del cursor
            CoffeeFoodManager.Instance.ToggleCafe(true, Vaso.GetComponent<Image>(), Vaso.GetComponent<Image>().sprite); // Se deja en la bandeja
        }
        else if (vasoIsInTable && !vasoInHand) // Quitar de la mesa
        {
            MiniGameSoundManager.instance.PlayTakeVase(); // Se reproduce el sonido

            // Se desactiva el vaso con el sprite y se actualizan los booleanos
            Vaso.SetActive(false);

            vasoInHand = true;
            vasoIsInTable = false;
            minigame.cupServed = false;

            finalVasoIsPremium = false;

            DragController.Instance.StartDragging(minigame.currentSprite != null ? minigame.currentSprite : currentSkin.vasoSinTapa); // Se coge con el cursor
            CoffeeFoodManager.Instance.ToggleCafe(false, null, null); // Se quita de la bandeja
        }
        ActualizarBotonCogerEnvase();
    }
    public void TogglePlatoTazaMesa()
    {
        if (minigame.TengoOtroObjetoEnLaMano() || minigame.tazaMilkInHand || minigame.filtroInHand) return;
        if (!platoTazaInHand && !platoTazaIsInTable) return;

        if (platoTazaInHand)
        {
            MiniGameSoundManager.instance.PlayTakePlate();

            // Poner en la mesa
            PlatoTaza.SetActive(true);
            CoffeeSkin skinPlatoReal = plateIsPremium ? skinPremium : skinBase;
            PlatoTaza.GetComponent<Image>().sprite = skinPlatoReal.platoTaza;
            PlatoTaza.transform.position = puntoMesa.position;

            platoTazaInHand = false;
            platoTazaIsInTable = true;

            DragController.Instance.StopDragging();
            platoTazaImage.material = defaultMaterial;
            platoTazaPImage.material = defaultMaterial;

            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaB2Button);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerPlatoTazaPButton);

            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerVasoInicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerVasoB2InicioButton);
            minigame.buttonManager.DisableButton(minigame.buttonManager.cogerVasoPInicioButton);
        }
        else if (!platoTazaInHand && !minigame.cupServed && !tazaInHand)
        {
            MiniGameSoundManager.instance.PlayTakePlate();
            CoffeeSkin skinPlatoTemp = plateIsPremium ? skinPremium : skinBase;

            // Recoger de la mesa
            PlatoTaza.SetActive(false);

            platoTazaInHand = true;
            platoTazaIsInTable = false;

            DragController.Instance.StartDragging(skinPlatoTemp.platoTaza);

            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerPlatoTazaButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerPlatoTazaB2Button);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerPlatoTazaPButton);

            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerVasoInicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerVasoB2InicioButton);
            minigame.buttonManager.EnableButton(minigame.buttonManager.cogerVasoPInicioButton);
        }
        ActualizarBotonCogerEnvase();
    }

    #region Mecanica tipo de pedido
    public void CogerTapa(bool isPrem)
    {
        if (!minigame.coffeeServed) return;
        if (minigame.countCover != 0) return;
        if (tazaIsInCafetera || tazaIsInPlato || platoTazaIsInTable) return;

        if (!minigame.TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !minigame.tazaMilkInHand)
        {
            coverIsPremium = isPrem;
            CoffeeSkin skinTapaTemp = coverIsPremium ? skinPremium : skinBase; coverInHand = true;

            if (isPrem) coverPImage.material = glowMaterial;
            else coverImage.material = glowMaterial;

            MiniGameSoundManager.instance.PlayTakeVase();
            DragController.Instance.StartDragging(skinTapaTemp.tapaVaso);
        }
        else if (coverInHand == true)
        {
            coverIsPremium = false;
            coverInHand = false;

            if (isPrem) coverPImage.material = defaultMaterial;
            else coverImage.material = defaultMaterial;

            MiniGameSoundManager.instance.PlayTakeVase();
            DragController.Instance.StopDragging();
        }
    }
    public void PonerTapa()
    {
        //Si se tiene la tapa en la mano y el cafe esta servido entonces se puede poner
        if (coverInHand == true && minigame.coffeeServed == true && (vasoIsInCafetera || vasoIsInTable))
        {
            if (minigame.countCover < 1)
            {
                minigame.countCover += 1; //Se incrementa el contador de hielo
                minigame.order.currentOrder.typePrecision = minigame.countCover; // Se guarda el resultado obtenido en la precision del jugador
                minigame.order.currentOrder.stepsPerformed.Add(OrderStep.PutCover); // Se añade a la lista de pasos
            }

            MiniGameSoundManager.instance.PlayTakeVase();
            Image vaso = Vaso.GetComponent<Image>();
            currentSkin = GetCombinedSkinVase();
            vaso.sprite = currentSkin.vasoConTapa;
            minigame.currentSprite = currentSkin.vasoConTapa;
            DragController.Instance.StopDragging();

            finalCoverIsPremium = coverIsPremium;

            if (vasoIsInTable)
            {
                // Se actualiza en la bandeja
                CoffeeFoodManager.Instance.ToggleCafe(true, Vaso.GetComponent<Image>(), Vaso.GetComponent<Image>().sprite);
            }

            coverImage.material = defaultMaterial;
            coverPImage.material = defaultMaterial;

            coverInHand = false;
        }
    }
    #endregion
}
