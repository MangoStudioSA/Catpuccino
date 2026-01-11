using UnityEngine;
using UnityEngine.UI;

// Clase encargada de gestionar el menu de dialogo
public class UIDialogue : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject dialoguePanel;
    public GameObject preparationPanel;
    public GameObject deliveryPanel;
    public GameObject roomPanel;
    CustomerManager manager;

    public CustomerOrder npcOrder;
    public PlayerOrder playerOrder;

    public MinigameInput minigameInput;
    public FoodMinigameInput foodMinigameInput;

    public Button acceptButton;

    GameUIManager gameUI;
    public TutorialManager tutorialManager;
    public PopUpMechanicsMsg popUpMechanicsMsg;

    void Start()
    {
        preparationPanel.SetActive(false);
        acceptButton.onClick.AddListener(StartPreparation);
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
        gameUI = GameObject.FindFirstObjectByType<GameUIManager>();
    }

    // Funcion encargada de activar el menu del minijuego
    public void StartPreparation()
    {
        //inicializacion del pedido
        if (npcOrder !=null && npcOrder.currentOrder !=null && playerOrder != null)
        {
            //creamos nuevo order en playerorder copiando las cantidades del NPC
            playerOrder.NewOrder(npcOrder.currentOrder);
            manager.orderingCustomer.GetComponent<CustomerController>().pedidoLlevar = playerOrder.currentOrder.orderType == OrderType.llevar;
        }
        else
        {
            Debug.LogError("Falta la referencia de npcOrder o playerOrder, o el NPC aún no ha generado un pedido.");
        }

        if (minigameInput != null)
        {
            minigameInput.ResetCafe();
        }
        else
        {
            Debug.LogError("Falta la referencia a minigame");
        }
        if (foodMinigameInput != null)
        {
            foodMinigameInput.ResetFoodState();
        }
        else
        {
            Debug.LogError("Falta la referencia a foodminigame");
        }

        //transicion de la ui
        dialoguePanel.SetActive(false);
        preparationPanel.SetActive(true);

        SoundsMaster.Instance.PlaySound_TakeOrderNote();

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 2)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();

        if (tutorialManager.isRunningT2)
            tutorialManager.StartTutorial2();

        if (tutorialManager.isRunningT3)
            tutorialManager.StartTutorial3();

    }

    // Funcion encargada de cerrar el panel de feedback del cliente
    public void EndDelivery()
    {
        SoundsMaster.Instance.PlaySound_Entregar();

        deliveryPanel.SetActive(false);
        roomPanel.SetActive(true);
        manager.orderButton.SetActive(false);
        manager.customers.Dequeue();
        manager.orderingCustomer.GetComponent<CustomerController>().leaving = true;
        manager.orderingCustomer.GetComponent<CapsuleCollider>().enabled = false;
        manager.orderingCustomer.transform.Translate(0.0f, 0.0f, 0.5f);
        manager.orderingCustomer.transform.Rotate(0.0f, 180.0f, 0.0f);
        manager.orderingCustomer = null;
        gameUI.orderScreen = false;
        popUpMechanicsMsg.DestroyAllPopUps();

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 20)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
    }
}
