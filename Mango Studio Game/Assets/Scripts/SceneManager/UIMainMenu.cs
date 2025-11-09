using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Clase encargada de gestionar los paneles del menu principal
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject contactPanel;
    [SerializeField] GameObject slotsPanel;
    private CanvasGroup mainMenuCanvasGroup;
    private SaveDataManager saveDataManager;
    SceneLoader loader;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mainMenuCanvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
        saveDataManager = FindFirstObjectByType<SaveDataManager>();
        loader = FindFirstObjectByType<SceneLoader>();

        loader.playButton = GameObject.FindGameObjectWithTag("play").GetComponent<Button>();
        loader.playButton.onClick.AddListener(loader.LoadGame);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
    }

    public void OpenSlots()
    {
        slotsPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
        saveDataManager.CheckButtons();
    }

    public void OpenContact()
    {
        contactPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
    }

    public void VolverMenuPrincipal()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
        }
        if (contactPanel.activeSelf)
        {
            contactPanel.SetActive(false);
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
        }
        if (slotsPanel.activeSelf)
        {
            slotsPanel.SetActive(false);
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego");
    }

    // Funcion para reproducir sonido 
    public void OnMouseEnterSound()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
    }
}
