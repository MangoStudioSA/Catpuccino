using UnityEngine;

// Encargada de gestionar los paneles de los menus de la escena del juego
public class GameUIManager : MonoBehaviour
{
    [Header("Paneles escena Game")]
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject endOfDayPanel;
    private CanvasGroup gameCanvasGroup;
    public TutorialManager tutorialManager;

    public bool orderScreen = false;
    // Referencia al script que genera los pedidos del cliente
    [SerializeField] private CustomerOrder customerOrderGenerator;

    public void Start()
    {
        gameCanvasGroup = gamePanel.GetComponent<CanvasGroup>();
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        shopPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Menu de pausa
    public void OpenPauseMenu()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        pausePanel.SetActive(true); // Activar UI menu pausa desde el juego
        gameCanvasGroup.interactable = false;
        gameCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 0.0f;
    }
    public void ClosePauseMenu()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        pausePanel.SetActive(false);
        gameCanvasGroup.interactable = true;
        gameCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 1.0f;
    }

    // Menu de opciones (desde el juego)
    public void OpenGameOptions()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        optionsPanel.SetActive(true); // Activar UI menu opciones desde el juego
        pausePanel.SetActive(false);
    }
    public void CloseGameOptions()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // Menu de la tienda
    public void OpenShopMenu()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        shopPanel.SetActive(true); // Activar UI menu tienda desde el juego
        gameCanvasGroup.interactable = false;
        gameCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 0.0f;

        ShopManager.Instance.UpdateUI();
    }
    public void CloseShopMenu()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        shopPanel.SetActive(false); // Desactivar UI menu tienda desde el juego
        gameCanvasGroup.interactable = true;
        gameCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 1.0f;

        HUDManager.Instance.UpdateUI();
    }

    // Mostrar panel dialogo con el cliente
    public void OpenDialogue()
    {
        // Se genera un nuevo pedido aleatorio 
        if (customerOrderGenerator != null)
        {
            customerOrderGenerator.GenRandomOrder();
        }

        SoundsMaster.Instance.PlaySound_Entregar();

        gameCanvasGroup.gameObject.SetActive(false);
        dialoguePanel.SetActive(true);
        orderScreen = true;

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 1)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
    }  

    // Menu del final del dia
    public void ShowEndOfDayPanel()
    {
        SoundsMaster.Instance.PlaySound_FinDia();
        endOfDayPanel.SetActive(true);
        // Desactivamos el panel principal del juego para que no se pueda interactuar con él
        gamePanel.SetActive(false);
    }

    // Menu juego
    public void ShowGamePanel()
    {
        endOfDayPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    // Funcion para cambiar de dia 
    public void OnNextDayButtonPressed()
    {
        // Llama al TimeManager para que inicie el nuevo día
        TimeManager.Instance.StartNewDay();
    }

}
