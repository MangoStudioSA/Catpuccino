using System.Collections.Generic;
using UnityEngine;

// Clase auxiliar para guardar los datos de la tienda y las cartas del jugador
[System.Serializable]
public class PlayerData
{
    public int basicCoins = 0;
    public int premiumCoins = 0;
    public bool day7RewardClaimed = false;
    public bool tutorial1Completed;
    public bool tutorial2Completed;
    public bool tutorial3Completed;

    public List<string> unlockedCardsList;

    [System.NonSerialized]
    public HashSet<string> unlockedCards = new();

    public PlayerData() 
    {
        basicCoins = 0;
        premiumCoins = 0;
    }

    // Funciones que inicializan las cartas desbloqueadas
    public void InitializeUnlockedCards()
    {
        if (unlockedCardsList == null) 
            unlockedCardsList = new List<string>();

        unlockedCards = new HashSet<string>(unlockedCardsList);
    }

    public void SyncListFromSet()
    {
        unlockedCardsList = new List<string>(unlockedCards);
    }
}
