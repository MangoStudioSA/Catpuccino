using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopUp : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        popupRoot.SetActive(false);
        closeButton.onClick.AddListener(Hide);
    }

    public void Show(string message)
    {
        messageText.text = message;
        popupRoot.SetActive(true);
    }

    public void Hide()
    {
        popupRoot.SetActive(false);
    }
}
