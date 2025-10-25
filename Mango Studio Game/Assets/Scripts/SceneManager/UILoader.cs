using UnityEngine;

// Script encargado de activar/desactivar los paneles de los menus
public class UILoader : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject optionsGamePanel;
    public GameObject roomPanel;
    public GameObject dialoguePanel;

    GameUIManager gameUIManager;


    // Referencia al script que genera los pedidos del cliente
    [SerializeField] private CustomerOrder customerOrderGenerator;

    private void Start()
    {
        gameUIManager = GameObject.FindFirstObjectByType<GameUIManager>();
    }

    public void OpenGameOptions()
    {
        optionsGamePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void CloseGameOptions()
    {
        optionsGamePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenDialogue()
    {
        
        // Le decimos que genere un nuevo pedido aleatorio AHORA
        if (customerOrderGenerator != null)
        {
            customerOrderGenerator.GenRandomOrder();
        }
        // --------------------

        roomPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        gameUIManager.orderScreen = true;
    }
}