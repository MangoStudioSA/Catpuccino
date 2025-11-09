using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script principal encargado de gestionar los cambios de escena
public class SceneLoader : MonoBehaviour
{
    [Header("SceneLoader")]
    public static SceneLoader Instance; // Instancia del SceneLoader en la escena

    public Button playButton;
    void Awake()
    {
        if (Instance == null) // Patron Singleton para mantener la informacion entre escenas
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas
        }
        else
        {
            Destroy(gameObject); // Si ya hay un SceneLoader en escena se destruyen los sobrantes
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cargar escena menu principal
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game"); // Cargar escena juego
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene("Game"); // Cargar escena juego
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver"); // Cargar escena fin del juego
    }

    public void QuitGame()
    {
        Application.Quit(); // Salir del juego
        Debug.Log("Saliendo del juego");
    }

}
