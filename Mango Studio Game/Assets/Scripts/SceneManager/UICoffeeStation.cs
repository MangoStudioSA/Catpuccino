using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject preparationPanel;
    public GameObject deliveryPanel;
    public GameObject bakeryPanel;
    public GameObject recipesPanel;

    public CoffeeGameManager gameManager;

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
    }
    public void CloseRecipesPanel()
    {
        recipesPanel.SetActive(false);
        preparationPanel.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void ShowBakeryPanel()
    {
        preparationPanel.SetActive(false);
        bakeryPanel.SetActive(true);
    }

    public void ReturnBakeryPanel()
    {
        bakeryPanel.SetActive(false);
        preparationPanel.SetActive(true);
    }

    public void SubmitOrderUI()
    {

        if (gameManager != null && miniGameInput.coffeeServed == true && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand && !miniGameInput.coverInHand)
        {
            gameManager.SubmitOrder();
            preparationPanel.SetActive(false);
            deliveryPanel.SetActive(true);
        }
    }
}
