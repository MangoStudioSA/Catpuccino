using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    static SaveDataManager Instance;

    public int currentSlot = 1;
    public int currentDay = 1;

    public Button slot1Button;
    public Button slot2Button;
    public Button slot3Button;

    SceneLoader sceneLoader;

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

        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("LastSlot"))
        {
            currentSlot = PlayerPrefs.GetInt("LastSlot");
        }
    }

    public void CheckButtons()
    {
        if (PlayerPrefs.HasKey("Slot1_Day"))
        {
            slot1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Partida 1 (Día " + PlayerPrefs.GetInt("Slot1_Day") + ")";
        }

        if (PlayerPrefs.HasKey("Slot2_Day"))
        {
            slot2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Partida 2 (Día " + PlayerPrefs.GetInt("Slot2_Day") + ")";
        }

        if (PlayerPrefs.HasKey("Slot3_Day"))
        {
            slot3Button.GetComponentInChildren<TextMeshProUGUI>().text = "Partida 3 (Día " + PlayerPrefs.GetInt("Slot3_Day") + ")";
        }
    }

    public void SaveGame()
    {
        switch (currentSlot)
        {
            case 1:
                PlayerPrefs.SetInt("Slot1_Day", currentDay);
                break;

            case 2:
                PlayerPrefs.SetInt("Slot2_Day", currentDay);
                break;

            case 3:
                PlayerPrefs.SetInt("Slot3_Day", currentDay);
                break;
        }
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        PlayerPrefs.SetInt("LastSlot", currentSlot);
        sceneLoader.LoadGame();
    }

    public void Slot1()
    {
        LoadGame(1);
    }

    public void Slot2()
    {
        LoadGame(2);
    }

    public void Slot3()
    {
        LoadGame(3);
    }
}
