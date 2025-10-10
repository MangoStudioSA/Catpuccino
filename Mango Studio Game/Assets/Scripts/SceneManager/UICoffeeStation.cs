using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject preparationPanel;
    public GameObject deliveryPanel;

    public CoffeeGameManager gameManager;

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<CoffeeGameManager>();
        }
    }

    public void SubmitOrderUI()
    {

        if (gameManager != null)
        {
            gameManager.SubmitOrder();
        }
        else
        {
            Debug.LogError("falta la referencia a CoffeGameMaager");
        }

        preparationPanel.SetActive(false);
        deliveryPanel.SetActive(true);

    }
}
