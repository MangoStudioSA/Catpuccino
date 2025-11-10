using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    static SaveDataManager Instance;

    public int currentSlot = 1;
    public int currentDay = 1;
    public int money = 0;
    public int rated = 0;
    public int score = 0;

    public Button slot1Button;
    public Button slot2Button;
    public Button slot3Button;

    public GameObject del1Button;
    public GameObject del2Button;
    public GameObject del3Button;

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
        slot1Button = GameObject.FindGameObjectWithTag("slot1").GetComponent<Button>();
        slot2Button = GameObject.FindGameObjectWithTag("slot2").GetComponent<Button>();
        slot3Button = GameObject.FindGameObjectWithTag("slot3").GetComponent<Button>();

        del1Button = GameObject.FindGameObjectWithTag("del1");
        del2Button = GameObject.FindGameObjectWithTag("del2");
        del3Button = GameObject.FindGameObjectWithTag("del3");

        slot1Button.onClick.AddListener(Slot1);
        slot2Button.onClick.AddListener(Slot2);
        slot3Button.onClick.AddListener(Slot3);

        if (PlayerPrefs.HasKey("Slot1_Day"))
        {
            slot1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Partida 1 (Día " + PlayerPrefs.GetInt("Slot1_Day") + ")";
            del1Button.GetComponent<Button>().onClick.AddListener(Del1);
        }
        else
        {
            del1Button.SetActive(false);
        }

        if (PlayerPrefs.HasKey("Slot2_Day"))
        {
            slot2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Partida 2 (Día " + PlayerPrefs.GetInt("Slot2_Day") + ")";
            del2Button.GetComponent<Button>().onClick.AddListener(Del2);
        }
        else
        {
            del2Button.SetActive(false);
        }

        if (PlayerPrefs.HasKey("Slot3_Day"))
        {
            slot3Button.GetComponentInChildren<TextMeshProUGUI>().text = "Partida 3 (Día " + PlayerPrefs.GetInt("Slot3_Day") + ")";
            del3Button.GetComponent<Button>().onClick.AddListener(Del3);
        }
        else
        {
            del3Button.SetActive(false);
        }
    }

    public void SaveGame()
    {
        switch (currentSlot)
        {
            case 1:
                PlayerPrefs.SetInt("Slot1_Day", currentDay);
                PlayerPrefs.SetInt("Slot1_Money", money);
                PlayerPrefs.SetInt("Slot1_Rated", rated);
                PlayerPrefs.SetInt("Slot1_Score", score);
                break;

            case 2:
                PlayerPrefs.SetInt("Slot2_Day", currentDay);
                PlayerPrefs.SetInt("Slot2_Money", money);
                PlayerPrefs.SetInt("Slot2_Rated", rated);
                PlayerPrefs.SetInt("Slot2_Score", score);
                break;

            case 3:
                PlayerPrefs.SetInt("Slot3_Day", currentDay);
                PlayerPrefs.SetInt("Slot3_Money", money);
                PlayerPrefs.SetInt("Slot3_Rated", rated);
                PlayerPrefs.SetInt("Slot3_Score", score);
                break;
        }
    }

    public int LoadDay()
    {
        switch (currentSlot)
        {
            case 1:
                if (PlayerPrefs.HasKey("Slot1_Day"))
                {
                    return PlayerPrefs.GetInt("Slot1_Day");
                }
                break;

            case 2:
                if (PlayerPrefs.HasKey("Slot2_Day"))
                {
                    return PlayerPrefs.GetInt("Slot2_Day");
                }
                break;

            case 3:
                if (PlayerPrefs.HasKey("Slot3_Day"))
                {
                    return PlayerPrefs.GetInt("Slot3_Day");
                }
                break;
        }

        return 1;
    }

    public int LoadMoney()
    {
        switch (currentSlot)
        {
            case 1:
                if (PlayerPrefs.HasKey("Slot1_Money"))
                {
                    return PlayerPrefs.GetInt("Slot1_Money");
                }
                break;

            case 2:
                if (PlayerPrefs.HasKey("Slot2_Money"))
                {
                    return PlayerPrefs.GetInt("Slot2_Money");
                }
                break;

            case 3:
                if (PlayerPrefs.HasKey("Slot3_Money"))
                {
                    return PlayerPrefs.GetInt("Slot3_Money");
                }
                break;
        }

        return 0;
    }

    public int LoadScore()
    {
        switch (currentSlot)
        {
            case 1:
                if (PlayerPrefs.HasKey("Slot1_Score"))
                {
                    return PlayerPrefs.GetInt("Slot1_Score");
                }
                break;

            case 2:
                if (PlayerPrefs.HasKey("Slot2_Score"))
                {
                    return PlayerPrefs.GetInt("Slot2_Score");
                }
                break;

            case 3:
                if (PlayerPrefs.HasKey("Slot3_Score"))
                {
                    return PlayerPrefs.GetInt("Slot3_Score");
                }
                break;
        }

        return 0;
    }

    public int LoadRated()
    {
        switch (currentSlot)
        {
            case 1:
                if (PlayerPrefs.HasKey("Slot1_Rated"))
                {
                    return PlayerPrefs.GetInt("Slot1_Rated");
                }
                break;

            case 2:
                if (PlayerPrefs.HasKey("Slot2_Rated"))
                {
                    return PlayerPrefs.GetInt("Slot2_Rated");
                }
                break;

            case 3:
                if (PlayerPrefs.HasKey("Slot3_Rated"))
                {
                    return PlayerPrefs.GetInt("Slot3_Rated");
                }
                break;
        }

        return 0;
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        PlayerPrefs.SetInt("LastSlot", currentSlot);
        sceneLoader.LoadGame();
    }

    public void DelGame(int slot)
    {
        switch (slot)
        {
            case 1:
                PlayerPrefs.DeleteKey("Slot1_Day");
                PlayerPrefs.DeleteKey("Slot1_Money");
                del1Button.SetActive(false);
                slot1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Nueva partida";
                break;

            case 2:
                PlayerPrefs.DeleteKey("Slot2_Day");
                PlayerPrefs.DeleteKey("Slot2_Money");
                del2Button.SetActive(false);
                slot2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Nueva partida";
                break;

            case 3:
                PlayerPrefs.DeleteKey("Slot3_Day");
                PlayerPrefs.DeleteKey("Slot3_Money");
                del3Button.SetActive(false);
                slot3Button.GetComponentInChildren<TextMeshProUGUI>().text = "Nueva partida";
                break;
        }
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

    public void Del1()
    {
        DelGame(1);
    }

    public void Del2()
    {
        DelGame(2);
    }

    public void Del3()
    {
        DelGame(3);
    }
}
