using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Clase para mostrar los mensajes para abrir sobres o comprar monedas
public class MessagePopUp : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot; // Raiz del panel
    [SerializeField] private TextMeshProUGUI messageText; // Mensaje
    [SerializeField] private Button closeButton; // Boton para cerrar

    private void Start()
    {
        popupRoot.SetActive(false);
        closeButton.onClick.AddListener(Hide);
    }

    // Mostrar mensaje
    public void Show(string message)
    {
        messageText.text = message;
        popupRoot.SetActive(true);
    }

    // Desactivar panel
    public void Hide()
    {
        popupRoot.SetActive(false);
    }
}
