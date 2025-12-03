using UnityEngine;

// Clase encargada de gestionar los paneles dentro del minijuego
public class UICoffeeStation : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject preparationPanel;
    public GameObject deliveryPanel;
    public GameObject bakeryPanel;
    public GameObject recipesPanel;

    public CoffeeGameManager gameManager;
    public TutorialManager tutorialManager;
    public OrderNoteUI orderNoteUI;
    public CoffeeContainerManager coffeeContainerManager;
    public MinigameInput miniGameInput;
    public FoodMinigameInput foodMinigameInput;
    public FoodManager foodManager;
    public CoffeeRecipesManager recipesManager;
    public CoffeeUnlockerManager unlockerManager;
    public TimeManager timeManager;

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<CoffeeGameManager>();
        }
        bakeryPanel.SetActive(false);   
        recipesPanel.SetActive(false);
    }

    // Mostrar panel de recetas
    public void ShowRecipesPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        recipesPanel.SetActive(true);
        Time.timeScale = 0.0f;

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 7)
            tutorialManager.tutorialPanel.gameObject.SetActive(false);
        if (tutorialManager.isRunningT1 || tutorialManager.isRunningT2 || tutorialManager.isRunningT3)
            tutorialManager.tutorialPanel.gameObject.SetActive(false);
    }

    // Cerrar panel de recetas
    public void CloseRecipesPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        recipesPanel.SetActive(false);
        Time.timeScale = 1.0f;

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 7)
        {
            tutorialManager.tutorialPanel.gameObject.SetActive(true);
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
        if (tutorialManager.isRunningT1 || tutorialManager.isRunningT2 || tutorialManager.isRunningT3)
            tutorialManager.tutorialPanel.gameObject.SetActive(true);
    }

    // Mostrar panel de reposteria
    public void ShowBakeryPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        preparationPanel.SetActive(false);
        bakeryPanel.SetActive(true);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 1)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }

    // Cerrar panel de reposteria
    public void ReturnBakeryPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();

        bakeryPanel.SetActive(false);
        preparationPanel.SetActive(true);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 9)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }

    // Mostrar panel de feedback del cliente
    public void SubmitOrderUI()
    {
        if (gameManager != null && miniGameInput.coffeeServed == true && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand && !coffeeContainerManager.coverInHand)
        {
            SoundsMaster.Instance.PlaySound_Entregar();

            gameManager.SubmitOrder();
            preparationPanel.SetActive(false);
            deliveryPanel.SetActive(true);

            orderNoteUI.isVisible = false;

            foodManager.ResetDepletedFood();
            foodManager.ResetDepletedCakes();

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 17)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
}
