using UnityEngine;
using UnityEngine.UI;

public class ButtonUnlockManager : MonoBehaviour
{
    [Header("Botones de interacciones")]
    [SerializeField] public Button endDeliveryButton;
    [SerializeField] public Button gameButton;
    [SerializeField] public Button acceptOrderButton;
    [SerializeField] public Button shopButton;
    [SerializeField] public Button submitOrderButton;
    [SerializeField] public Button recipesBookButton;
    [SerializeField] public Button recipesBookBButton;
    [SerializeField] public Button orderNoteButton;
    [SerializeField] public Button orderNoteBButton;

    [Header("Botones de las mecanicas")]
    [SerializeField] public Button coffeeButton;
    [SerializeField] public Button milkButton;
    [SerializeField] public Button waterButton;
    [SerializeField] public Button sugarButton;
    [SerializeField] public Button iceButton;
    [SerializeField] public Button coverButton;
    [SerializeField] public Button coverPButton;
    [SerializeField] public Button condensedMilkButton;
    [SerializeField] public Button creamButton;
    [SerializeField] public Button chocolateButton;
    [SerializeField] public Button whiskeyButton;

    [Header("Botones de los envases de cafe")]
    [SerializeField] public Button cogerTazaInicioButton;
    [SerializeField] public Button cogerVasoInicioButton;
    [SerializeField] public Button cogerPlatoTazaButton;
    [SerializeField] public Button cogerTazaB2InicioButton;
    [SerializeField] public Button cogerVasoB2InicioButton;
    [SerializeField] public Button cogerPlatoTazaB2Button;
    [SerializeField] public Button cogerTazaPInicioButton;
    [SerializeField] public Button cogerVasoPInicioButton;
    [SerializeField] public Button cogerPlatoTazaPButton;

    [Header("Botones del minijuego de cafe")]
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
    [SerializeField] public Button papeleraRButton;

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
        submitOrderButton.interactable = false;

        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = false;
        coffeeButton.interactable = false;
        echarCafeButton.interactable = false;
        pararEcharCafeButton.interactable = false;
        calentarButton.interactable = false;
        cogerTazaLecheButton.interactable = false;

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
    }

    public void DisableButton(Button button)
    {
        if (button == null) return;
        else button.interactable = false;
    }
}
