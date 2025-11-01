using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentDay = 0;
    public int money = 0;
    public int basicCoins = 0;
    public int premiumCoins = 0;
    public int totalCustomersServed = 0;
    public float customersSatisfaction = 0;

    public PlayerData() { }
    public PlayerData(int day, int money)
    {
        this.currentDay = day;
        this.money = money;
    }
}
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public PlayerData data = new();
    private string savePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/playerdata.json";
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int amount)
    {
        data.money += amount;
        SaveData();
    }
    public void AddPremiumCoins(int amount)
    {
        data.premiumCoins += amount;
        SaveData();
    }
    public void AddBasicCoins(int amount)
    {
        data.basicCoins += amount;
        SaveData();
    }
    public void AddServedCustomers(int amount)
    {
        data.totalCustomersServed = amount;
        SaveData();
    }
    public void AddSatisfaction(float amount)
    {
        data.customersSatisfaction += amount;
        SaveData();
    }

    public bool SpendMoney(int amount)
    {
        if (data.money >= amount)
        {
            data.money -= amount;
            SaveData();
            return true;
        }

        Debug.LogWarning("No hay suficiente dinero");
        return false;
    }

    public bool SpendBasicCoins(int amount)
    {
        if (data.basicCoins >= amount)
        {
            data.basicCoins -= amount;
            SaveData();
            return true;
        }
        Debug.LogWarning("No hay suficientes monedas del café.");
        return false;
    }

    public bool SpendPremiumCoins(int amount)
    {
        if (data.premiumCoins >= amount)
        {
            data.premiumCoins -= amount;
            SaveData();
            return true;
        }
        Debug.LogWarning("No hay suficientes croquetas doradas.");
        return false;
    }

    public void NextDay()
    {
        data.currentDay++;
        SaveData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Datos guardados en: " + savePath);
    }

    public void ResetPlayerData()
    {
        data = new PlayerData
        {
            basicCoins = 0,
            money = 0,
            currentDay = 0
        };
        SaveData();
        Debug.Log("Datos reiniciados");
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Datos cargados correctamente");
        }
        else
        {
            data = new PlayerData(0, 0);
            SaveData();
        }
    }

    public void ResetData()
    {
        data = new PlayerData(0, 0);
        SaveData();
    }
}
