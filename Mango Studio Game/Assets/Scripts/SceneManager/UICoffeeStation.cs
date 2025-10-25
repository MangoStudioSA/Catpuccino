using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject preparationPanel;
    public GameObject deliveryPanel;
    public GameObject bakeryPanel;

    public CoffeeGameManager gameManager;

    public MinigameInput miniGameInput;

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<CoffeeGameManager>();
        }
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
