using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public PlayerData data = new();
    private string savePath;

    private List<string> unlockedCards = new();

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

    public void AddCard(string cardName)
    {
        if (!data.unlockedCards.Contains(cardName))
        {
            data.unlockedCards.Add(cardName);
            data.SyncListFromSet();
            SaveData();
            Debug.Log($"Carta añadida: {cardName}");
        }
    }

    public HashSet<string> GetUnlockedCards()
    {
        return new HashSet<string>(data.unlockedCards);
    }

    public void NextDay()
    {
        data.currentDay++;
        SaveData();
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

    public void SaveData()
    {
        if (data == null) return;

        data.SyncListFromSet();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Datos guardados en: " + savePath);
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

        data.InitializeUnlockedCards();
    }

    public void ResetData()
    {
        data = new PlayerData(0, 0);
        SaveData();
    }
}
