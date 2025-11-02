using System.Collections.Generic;
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
    public List<string> unlockedCardsList;

    [System.NonSerialized]
    public HashSet<string> unlockedCards = new();

    public PlayerData() { }
    public PlayerData(int day, int money)
    {
        this.currentDay = day;
        this.money = money;
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
