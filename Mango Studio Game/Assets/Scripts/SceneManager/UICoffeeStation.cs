using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject preparationPanel;
    public GameObject deliveryPanel;
    public GameObject bakeryPanel;
    public GameObject recipesPanel;

    public CoffeeGameManager gameManager;
    public TutorialManager tutorialManager;

    public MinigameInput miniGameInput;

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<CoffeeGameManager>();
        }
        bakeryPanel.SetActive(false);   
        recipesPanel.SetActive(false);
    }

    public void ShowRecipesPanel()
    {
        preparationPanel.SetActive(false);
        recipesPanel.SetActive(true);
        Time.timeScale = 0.0f;
        
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 7)
            tutorialManager.tutorialPanel.gameObject.SetActive(false);
    }
    public void CloseRecipesPanel()
    {
        recipesPanel.SetActive(false);
        preparationPanel.SetActive(true);
        Time.timeScale = 1.0f;

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 7)
        {
            tutorialManager.tutorialPanel.gameObject.SetActive(true);
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }

    public void ShowBakeryPanel()
    {
        preparationPanel.SetActive(false);
        bakeryPanel.SetActive(true);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 1)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }

    public void ReturnBakeryPanel()
    {
        bakeryPanel.SetActive(false);
        preparationPanel.SetActive(true);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 9)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }

    public void SubmitOrderUI()
    {

        if (gameManager != null && miniGameInput.coffeeServed == true && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand && !miniGameInput.coverInHand)
        {
            gameManager.SubmitOrder();
            preparationPanel.SetActive(false);
            deliveryPanel.SetActive(true);

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 17)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
}
