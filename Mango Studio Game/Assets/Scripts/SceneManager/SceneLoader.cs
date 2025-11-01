using UnityEngine;
using UnityEngine.SceneManagement;

// Script encargado de realizar los cambios entre escenas
public class SceneLoader : MonoBehaviour
{
    [Header("SceneLoader")]
    public static SceneLoader Instance; // Instancia del SceneLoader en la escena
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
        PlayerDataManager.instance.ResetPlayerData();
        SceneManager.LoadScene("Game"); // Cargar escena juego
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop"); // Cargar escena tienda
        Debug.Log("Cargando tienda");
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
