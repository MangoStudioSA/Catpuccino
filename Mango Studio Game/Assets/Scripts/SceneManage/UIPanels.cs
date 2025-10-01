using UnityEngine;

// Script encargado de activar/desactivar los paneles de los menus
public class UILoader : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject mainMenuPanel; // Panel UI del menu principal
    public GameObject optionsMainPanel; // Panel UI del menu de opciones (accediendo desde el menu principal)
    public GameObject optionsGamePanel; // Panel UI del menu de opciones (accediendo desde el juego)
    public GameObject contactPanel; // Panel UI del menu de contacto

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false); // Desactivar UI menu principal
        optionsMainPanel.SetActive(true); // Activar UI menu opciones
    }

    public void CloseOptions()
    {
        optionsMainPanel.SetActive(false); // Desactivar UI menu opciones
        mainMenuPanel.SetActive(true); // Activar UI menu principal
    }

    public void OpenContact()
    {
        mainMenuPanel.SetActive(false); // Desactivar UI menu principal
        contactPanel.SetActive(true); // Activar UI menu contacto
    }

    public void CloseContact()
    {
        contactPanel.SetActive(false); // Desactivar UI menu contacto
        mainMenuPanel.SetActive(true); // Activar UI menu principal
    }

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
}
