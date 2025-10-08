using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject preparationPanel;

    public Button acceptButton;

    void Start()
    {
        preparationPanel.SetActive(false);
        acceptButton.onClick.AddListener(StartPreparation);
    }

    public void StartPreparation()
    {
        dialoguePanel.SetActive(false);
        preparationPanel.SetActive(true);
    }
}
