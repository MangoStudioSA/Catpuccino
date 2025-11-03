using UnityEngine;
using UnityEngine.UI;

public class ButtonUnlockManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonConfig
    {
        public Button button;
        public bool ignoreSpriteChange = false;
    }

    [SerializeField] public ButtonConfig[] allButtons;


    [Header("Sprites botones")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite activeSprite;

    [Header("Botones de interacciones")]
    [SerializeField] public Button endDeliveryButton;
    [SerializeField] public Button gameButton;
    [SerializeField] public Button submitOrderButton;
    [SerializeField] public Button recipesBookButton;
    [SerializeField] public Button orderNoteButton;

    [Header("Botones de las mecanicas")]
    [SerializeField] public Button coffeeButton;
    [SerializeField] public Button milkButton;
    [SerializeField] public Button waterButton;
    [SerializeField] public Button sugarButton;
    [SerializeField] public Button iceButton;
    [SerializeField] public Button coverButton;
    [SerializeField] public Button condensedMilkButton;
    [SerializeField] public Button creamButton;
    [SerializeField] public Button chocolateButton;
    [SerializeField] public Button whiskeyButton;

    [Header("Botones del minijuego de cafe")]
    [SerializeField] public Button cogerTazaInicioButton;
    [SerializeField] public Button cogerVasoInicioButton;
    [SerializeField] public Button cogerPlatoTazaButton;
    [SerializeField] public Button molerButton;
    [SerializeField] public Button filtroButton;
    [SerializeField] public Button filtroCafeteraButton;
    [SerializeField] public Button echarCafeButton;
    [SerializeField] public Button pararEcharCafeButton;
    [SerializeField] public Button calentarButton;
    [SerializeField] public Button espumadorButton;
    [SerializeField] public Button cogerTazaLecheButton;
    [SerializeField] public Button papeleraButton;

    [Header("Botones del minijuego de pastelería")]
    [SerializeField] public Button bakeryButton;
    [SerializeField] public Button returnBakeryButton;
    [SerializeField] public Button cogerPlatoInicioButton;
    [SerializeField] public Button cogerBolsaLlevarInicioButton;
    [SerializeField] public Button cogerBZanahoriaButton;
    [SerializeField] public Button cogerBMantequillaButton;
    [SerializeField] public Button cogerBChocolateButton;
    [SerializeField] public Button cogerBRedVelvetButton;
    [SerializeField] public Button cogerGChocolateButton;
    [SerializeField] public Button cogerGChocolateBlButton;
    [SerializeField] public Button cogerGMantequillaButton;
    [SerializeField] public Button cogerMArandanosButton;
    [SerializeField] public Button cogerMCerezaButton;
    [SerializeField] public Button cogerMPistachoButton;
    [SerializeField] public Button cogerMDulceLecheButton;
    [SerializeField] public Button hornoButton;
    [SerializeField] public Button hornearButton;
    [SerializeField] public Button stopHorneadoButton;

    private void Awake()
    {
        allButtons = new ButtonConfig[]
        {
            new ButtonConfig { button = endDeliveryButton, ignoreSpriteChange = false },
            new ButtonConfig { button = gameButton, ignoreSpriteChange = true },
            new ButtonConfig { button = submitOrderButton, ignoreSpriteChange = false },
            new ButtonConfig { button = recipesBookButton, ignoreSpriteChange = false },
            new ButtonConfig { button = orderNoteButton, ignoreSpriteChange = false },

            new ButtonConfig { button = coffeeButton, ignoreSpriteChange = false },
            new ButtonConfig { button = milkButton, ignoreSpriteChange = true },
            new ButtonConfig { button = waterButton, ignoreSpriteChange = true },
            new ButtonConfig { button = sugarButton, ignoreSpriteChange = true },
            new ButtonConfig { button = iceButton, ignoreSpriteChange = true },
            new ButtonConfig { button = coverButton, ignoreSpriteChange = true },
            new ButtonConfig { button = condensedMilkButton, ignoreSpriteChange = true },
            new ButtonConfig { button = creamButton, ignoreSpriteChange = true },
            new ButtonConfig { button = chocolateButton, ignoreSpriteChange = true },
            new ButtonConfig { button = whiskeyButton, ignoreSpriteChange = true },

            new ButtonConfig { button = cogerTazaInicioButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerVasoInicioButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerPlatoTazaButton, ignoreSpriteChange = true },
            new ButtonConfig { button = molerButton, ignoreSpriteChange = false },
            new ButtonConfig { button = filtroButton, ignoreSpriteChange = true },
            new ButtonConfig { button = filtroCafeteraButton, ignoreSpriteChange = true },
            new ButtonConfig { button = echarCafeButton, ignoreSpriteChange = false },
            new ButtonConfig { button = pararEcharCafeButton, ignoreSpriteChange = false },
            new ButtonConfig { button = calentarButton, ignoreSpriteChange = false },
            new ButtonConfig { button = espumadorButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerTazaLecheButton, ignoreSpriteChange = true },
            new ButtonConfig { button = papeleraButton, ignoreSpriteChange = true },

            new ButtonConfig { button = bakeryButton, ignoreSpriteChange = false },
            new ButtonConfig { button = returnBakeryButton, ignoreSpriteChange = false },
            new ButtonConfig { button = cogerPlatoInicioButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerBolsaLlevarInicioButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerBChocolateButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerBZanahoriaButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerBMantequillaButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerBRedVelvetButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerGChocolateButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerGChocolateBlButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerGMantequillaButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerMArandanosButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerMCerezaButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerMPistachoButton, ignoreSpriteChange = true },
            new ButtonConfig { button = cogerMDulceLecheButton, ignoreSpriteChange = true },
            new ButtonConfig { button = hornoButton, ignoreSpriteChange = true },
            new ButtonConfig { button = hornearButton, ignoreSpriteChange = false },
            new ButtonConfig { button = stopHorneadoButton, ignoreSpriteChange = true },
        };
    }
    private void Start()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        var progress = GameProgressManager.Instance;

        // Se muestran u ocultan los botones segun las mecanicas desbloqueadas
        milkButton.gameObject.SetActive(progress.milkEnabled);
        calentarButton.gameObject.SetActive(progress.heatedMilkEnabled);
        espumadorButton.gameObject.SetActive(progress.heatedMilkEnabled);
        cogerTazaLecheButton.gameObject.SetActive(progress.heatedMilkEnabled);
        condensedMilkButton.gameObject.SetActive(progress.condensedMilkEnabled);
        creamButton.gameObject.SetActive(progress.creamEnabled);
        chocolateButton.gameObject.SetActive(progress.chocolateEnabled);
        whiskeyButton.gameObject.SetActive(progress.whiskeyEnabled);

        bakeryButton.gameObject.SetActive(progress.cakesEnabled);
        cogerBChocolateButton.gameObject.SetActive(progress.cakesEnabled);
        cogerBMantequillaButton.gameObject.SetActive(progress.cakesEnabled);
        cogerBRedVelvetButton.gameObject.SetActive(progress.cakesEnabled);
        cogerBZanahoriaButton.gameObject.SetActive(progress.cakesEnabled);
        cogerGChocolateBlButton.gameObject.SetActive(progress.cookiesEnabled);
        cogerGMantequillaButton.gameObject.SetActive(progress.cookiesEnabled);
        cogerGChocolateButton.gameObject.SetActive(progress.cookiesEnabled);
        cogerMArandanosButton.gameObject.SetActive(progress.mufflinsEnabled);
        cogerMCerezaButton.gameObject.SetActive(progress.mufflinsEnabled);
        cogerMPistachoButton.gameObject.SetActive(progress.mufflinsEnabled);
        cogerMDulceLecheButton.gameObject.SetActive(progress.mufflinsEnabled);

        // Inicialmente se desactiva la interactividad de los botones
        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = false;
        echarCafeButton.interactable = false;
        pararEcharCafeButton.interactable = false;
        calentarButton.interactable = false;
        cogerTazaLecheButton.interactable = false;
        submitOrderButton.interactable = false;

        sugarButton.interactable = false;
        iceButton.interactable = false;
        coverButton.interactable = false;
        waterButton.interactable = false;
        milkButton.interactable = false;
        condensedMilkButton.interactable = false;
        creamButton.interactable = false;
        chocolateButton.interactable = false;
        whiskeyButton.interactable = false;

        cogerBChocolateButton.interactable = false;
        cogerBMantequillaButton.interactable = false;
        cogerBRedVelvetButton.interactable = false;
        cogerBZanahoriaButton.interactable = false;
        cogerGChocolateBlButton.interactable = false;
        cogerGMantequillaButton.interactable = false;
        cogerGChocolateButton.interactable = false;
        cogerMArandanosButton.interactable = false;
        cogerMCerezaButton.interactable = false;
        cogerMPistachoButton.interactable = false;
        cogerMDulceLecheButton.interactable = false;
        hornearButton.interactable = false;
    }
    public void EnableButton(Button button)
    {
        if (button == null) return;
        else button.interactable = true;

        var config = System.Array.Find(allButtons, b => b.button == button);
        if (config != null && !config.ignoreSpriteChange && activeSprite != null)
        {
            button.image.sprite = activeSprite;
        }
    }

    public void DisableButton(Button button)
    {
        if (button == null) return;
        else button.interactable = false;

        var config = System.Array.Find(allButtons, b => b.button == button);
        if (config != null && !config.ignoreSpriteChange && defaultSprite != null)
        {
            button.image.sprite = defaultSprite;
        }
    }
}
