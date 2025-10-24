using UnityEngine;
using UnityEngine.UI;

public class ButtonUnlockManager : MonoBehaviour
{
    [Header("Botones de las mecanicas")]
    [SerializeField] public Button coffeeButton;
    [SerializeField] public Button milkButton;
    [SerializeField] public Button waterButton;
    [SerializeField] public Button sugarButton;
    [SerializeField] public Button iceButton;
    [SerializeField] public Button coverButton;

    [Header("Botones del minijuego")]
    [SerializeField] public Button cogerTazaInicioButton;
    [SerializeField] public Button molerButton;
    [SerializeField] public Button filtroButton;
    [SerializeField] public Button filtroCafeteraButton;
    [SerializeField] public Button echarCafeButton;
    [SerializeField] public Button calentarButton;
    [SerializeField] public Button espumadorButton;
    [SerializeField] public Button submitOrderButton;


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


        // Inicialmente se desactiva la interactividad de los botones
        filtroButton.interactable = false;
        filtroCafeteraButton.interactable = false;
        echarCafeButton.interactable = false;
        calentarButton.interactable = false;
        espumadorButton.interactable = false;
        submitOrderButton.interactable = false;

        sugarButton.interactable = false;
        iceButton.interactable = false;
        coverButton.interactable = false;
        waterButton.interactable = false;
        milkButton.interactable = false;

    }

    public void EnableButton(Button button)
    {
        if (button != null)
        {
            button.interactable = true;
        }
    }

    public void DisableButton(Button button)
    {
        if (button != null)
        {
            button.interactable = false;
        }
    }
}
