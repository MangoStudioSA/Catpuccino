using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Gestion del guardado de monedas de la tienda y de las cartas
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
        Debug.Log("PlayerDataManager Awake. Coins: " + data.basicCoins + "/" + data.premiumCoins);

        //RemoveUnlockedCard("CartaLatteArt");
        //AddLockedCard("CartaLatteArt");
    }

    // Funcion para añadir monedas premium
    public void AddPremiumCoins(int amount)
    {
        data.premiumCoins += amount;
        HUDManager.Instance.UpdatePremiumCoins(data.premiumCoins);
        SaveData();
    }
    // Funcion para añadir monedas basicas
    public void AddBasicCoins(int amount)
    {
        data.basicCoins += amount;
        HUDManager.Instance.UpdateBasicCoins(data.basicCoins);
        SaveData();
    }
    // Funcion para gastar monedas basicas
    public bool SpendBasicCoins(int amount)
    {
        if (data.basicCoins >= amount)
        {
            data.basicCoins -= amount;
            SaveData();
            HUDManager.Instance.UpdateBasicCoins(data.basicCoins);
            return true;
        }
        Debug.LogWarning("No hay suficientes monedas del café.");
        return false;
    }
    // Funcion para gastar monedas premium
    public bool SpendPremiumCoins(int amount)
    {
        if (data.premiumCoins >= amount)
        {
            data.premiumCoins -= amount;
            SaveData();
            HUDManager.Instance.UpdatePremiumCoins(data.premiumCoins);
            return true;
        }
        Debug.LogWarning("No hay suficientes croquetas doradas.");
        return false;
    }
    // Funcion para añadir cartas
    public void AddCard(Sprite cardSprite)
    {
        string cardID = cardSprite.name;

        if (!data.unlockedCards.Contains(cardID))
        {
            data.unlockedCards.Add(cardID);
            data.SyncListFromSet();
            SaveData();
            Debug.Log($"Carta añadida: {cardID}");
        }
    }

    public bool HasCard(string cardName)
    {
        return data.unlockedCards.Contains(cardName);
    }
    // Funcion para obtener las cartas desbloqueadas
    public HashSet<string> GetUnlockedCards()
    {
        if (data?.unlockedCards == null)
            return new HashSet<string>();

        // Se comprueba que los nombres esten bien
        return new HashSet<string>(
            data.unlockedCards
                .Where(c => !string.IsNullOrWhiteSpace(c)) // Ignora entradas vacias/nulas
                .Select(c => c.Trim())
                .Distinct()                                // Evita duplicados
        );
    }
    // Funcion para guardar los datos
    public void SaveData()
    {
        if (data == null) return;

        data.SyncListFromSet();
        string json = JsonUtility.ToJson(data, true);

        try
        {
            File.WriteAllText(savePath, json);
            Debug.Log("Datos guardados en archivo: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error guardando en archivo:" + e.Message);
        }

        PlayerPrefs.SetString("PlayerDataBackUp", json);
        PlayerPrefs.Save();
        Debug.Log("Copia de respaldo guardado en PlayerPrefs");
    }
    // Funcion para cargar los datos
    public void LoadData()
    {
        bool loaded = false;

        // Se carga desde archivo JSOn
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                data = JsonUtility.FromJson<PlayerData>(json);
                loaded = true;
                Debug.Log("Datos cargados desde archivo JSON.");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error cargando archivo JSON: " + e.Message);
            }
        }

        // Se intenta desde playerprefs
        if (!loaded && PlayerPrefs.HasKey("PlayerDataBackUp"))
        {
            string json = PlayerPrefs.GetString("PlayerDataBackUp");
            data = JsonUtility.FromJson<PlayerData>(json);
            loaded = true;
            Debug.Log("Datos cargados desde PlayerPrefs (respaldo).");
            SaveData();
        }
        
        if (!loaded)
        {
            data = new PlayerData();
            SaveData();
        }
        data.InitializeUnlockedCards();
    }

    // Borrar una carta
    public void RemoveUnlockedCard(string cardName)
    {
        if (data == null || data.unlockedCards == null) return;

        if (data.unlockedCards.Remove(cardName))
        {
            data.SyncListFromSet();
            SaveData();
            Debug.Log($"Carta '{cardName}' eliminada correctamente.");
        }
        else
        {
            Debug.LogWarning($"Carta '{cardName}' no estaba desbloqueada.");
        }
    }

    // Añadir una carta
    public void AddLockedCard(string cardName)
    {
        if (data == null || data.unlockedCards == null) return;

        if (data.unlockedCards.Add(cardName))
        {
            data.SyncListFromSet();
            SaveData();
            Debug.Log($"Carta '{cardName}' añadida correctamente.");
        }
    }
}
