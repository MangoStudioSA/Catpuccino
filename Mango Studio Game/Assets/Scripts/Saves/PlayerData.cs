using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int basicCoins = 0;
    public int premiumCoins = 0;

    public List<string> unlockedCardsList;

    [System.NonSerialized]
    public HashSet<string> unlockedCards = new();

    public PlayerData() 
    {
        basicCoins = 0;
        premiumCoins = 0;
    }

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
