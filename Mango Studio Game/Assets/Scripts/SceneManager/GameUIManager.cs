using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject endOfDayPanel;
    private CanvasGroup gameCanvasGroup;

    public bool orderScreen = false;

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
    public void ShowEndOfDayPanel()
    {
        endOfDayPanel.SetActive(true);
        // Desactivamos el panel principal del juego para que no se pueda interactuar con él
        gamePanel.SetActive(false);
    }

    public void ShowGamePanel()
    {
        endOfDayPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void OnNextDayButtonPressed()
    {
        // Llama al TimeManager para que inicie el nuevo día
        TimeManager.Instance.StartNewDay();
    }

}
