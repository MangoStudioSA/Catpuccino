using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject preparationPanel;
    private CanvasGroup gameCanvasGroup;

    public void Start()
    {
        gameCanvasGroup = gamePanel.GetComponent<CanvasGroup>();
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenPauseMenu()
    {
        pausePanel.SetActive(true); // Activar UI menu opciones desde el juego
        gameCanvasGroup.interactable = false;
        gameCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 0.0f;
    }
    public void OpenGameOptions()
    {
        optionsPanel.SetActive(true); // Activar UI menu opciones desde el juego
        pausePanel.SetActive(false);
    }

    public void ClosePauseMenu()
    {
        pausePanel.SetActive(false);
        gameCanvasGroup.interactable = true;
        gameCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 1.0f;
    }

    public void CloseGameOptions()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OpenDialogue()
    {
        dialoguePanel.SetActive(true); // Activar UI menu opciones
        gameCanvasGroup.interactable = false;
        gameCanvasGroup.blocksRaycasts = false;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false); // Desactivar UI menu opciones
        gameCanvasGroup.interactable = true;
        gameCanvasGroup.blocksRaycasts = true;
    }

}
