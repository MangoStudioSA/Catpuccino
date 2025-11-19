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

    public Button acceptButton;

    GameUIManager gameUI;
    public TutorialManager tutorialManager;

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
        }
        else
        {
            Debug.LogError("Falta la referencia de npcOrder o playerOrder, o el NPC aún no ha generado un pedido.");
        }

        //transicion de la ui
        dialoguePanel.SetActive(false);
        preparationPanel.SetActive(true);

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
        deliveryPanel.SetActive(false);
        roomPanel.SetActive(true);
        gameUI.orderScreen = false;

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 20)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
    }
}
