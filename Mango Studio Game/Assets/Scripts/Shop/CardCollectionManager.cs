using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform contentParent;
    public GameObject cardPrefab;   // Prefab de una carta

    [Header("Sprites por rareza")]
    public Sprite[] basicCards;
    public Sprite[] intermediateCards;
    public Sprite[] rareCards;
    public Sprite[] legendaryCards;

    [Header("Color cartas bloqueadas")]
    public Color lockedColor = Color.gray;                     // Cartas bloqueadas

   
    void OnEnable()
    {
        GenerateCollection();
    }

    public void HasUnlockedSkins()
    {
        bool gotVasoCards = PlayerDataManager.instance.HasCard("CartaSkinVaso1") && PlayerDataManager.instance.HasCard("CartaSkinVaso2");
        bool gotTazaCard = PlayerDataManager.instance.HasCard("CartaSkinTaza");

        if (gotTazaCard)
        {
            SkinManager.instance.UnlockCupSkin("PremiumTaza");
        }

        if (gotVasoCards)
        {
            SkinManager.instance.UnlockVasoSkin("PremiumVaso");
        }
    }

    public void GenerateCollection()
    {
        // Limpiar si ya existían
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        HashSet<string> unlockedCards = null;

        if (PlayerDataManager.instance != null && PlayerDataManager.instance.GetUnlockedCards() != null)
        {
            unlockedCards = PlayerDataManager.instance.GetUnlockedCards();
        }
        else
        {
            unlockedCards= new HashSet<string>();
        }

        // Cargar todas las cartas 
        List<Sprite> allCards = new();
        allCards.AddRange(basicCards);
        allCards.AddRange(intermediateCards);
        allCards.AddRange(rareCards);
        allCards.AddRange(legendaryCards);

        foreach (var sprite in allCards)
        {
            GameObject newCard = Instantiate(cardPrefab, contentParent);
            Image cardImage = newCard.GetComponentInChildren<Image>();

            bool isUnlocked = unlockedCards.Contains(sprite.name);

            if (isUnlocked)
            {
                cardImage.sprite = sprite;
                cardImage.color = Color.white;
            }
            else
            {
                cardImage.color = lockedColor;
            }
        }
    }
}
