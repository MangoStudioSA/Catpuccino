using UnityEngine;

// Script encargado de gestionar la UI de las pantallas haciendo llamadas al SceneLoader
public class SceneUIManager : MonoBehaviour
{
    public void GameScene() // Llama a la funcion de cargar el juego del sceneloader de la escena
    {
        SceneLoader.Instance.LoadGame();
    }

    public void NewGameScene() // Llama a la funcion de cargar el juego del sceneloader de la escena
    {
        SceneLoader.Instance.LoadNewGame();
    }

    public void ShopScene() // Llama a la funcion de cargar la tienda del sceneloader de la escena
    {
        SceneLoader.Instance.LoadShop(); 
    }

    public void EndGameMenu() // Llama a la funcion de cargar menu game over del sceneloader de la escena
    {
        SceneLoader.Instance.LoadGameOver();
    }

    public void ReturnMenu() // Llama a la funcion de cargar menu principal del sceneloader de la escena
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void ExitGame() // Llama a la funcion de cerrar el juego del sceneloader de la escena
    {
        SceneLoader.Instance.QuitGame();
    }
}
