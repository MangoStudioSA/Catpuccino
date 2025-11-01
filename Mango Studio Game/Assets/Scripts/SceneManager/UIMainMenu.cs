using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject contactPanel;
    private CanvasGroup mainMenuCanvasGroup;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mainMenuCanvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
        //SoundMaster.Instance.PlaySound_Menu();
    }

    public void OpenContact()
    {
        contactPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
        //SoundMaster.Instance.PlaySound_Menu();
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
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego");
    }

    public void OnMouseEnterSound()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
    }
}
