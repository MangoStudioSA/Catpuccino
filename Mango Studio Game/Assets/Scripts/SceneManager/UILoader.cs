using UnityEngine;

// Script encargado de activar/desactivar los paneles de los menus
public class UILoader : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject optionsGamePanel; // Panel UI del menu de opciones (accediendo desde el juego)
    public GameObject roomPanel;
    public GameObject dialoguePanel;

    public void OpenGameOptions()
    {
        optionsGamePanel.SetActive(true); // Activar UI menu opciones desde el juego
        Time.timeScale = 0.0f;
    }

    public void CloseGameOptions()
    {
        optionsGamePanel.SetActive(false); // Desactivar UI menu opciones desde el juego
        Time.timeScale = 1.0f;
    }

    public void OpenDialogue()
    {
        roomPanel.SetActive(false); // Desactivar UI menu principal
        dialoguePanel.SetActive(true); // Activar UI menu opciones
    }
}
