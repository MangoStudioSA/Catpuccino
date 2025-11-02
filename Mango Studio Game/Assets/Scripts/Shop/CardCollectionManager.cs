using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform contentParent;
    public GameObject cardPrefab;   // Prefab de una carta

    [Header("Colores por rareza")]
    public Color basicColor = new(0.4f, 0.6f, 1f);       // Azul claro
    public Color intermediateColor = new(0.3f, 0.9f, 0.5f); // Verde
    public Color rareColor = new(0.8f, 0.4f, 1f);        // Morado
    public Color legendaryColor = new(1f, 0.9f, 0.4f);   // Dorado
    public Color lockedColor = Color.gray;                     // Cartas bloqueadas

    // Simulación de las cartas (nombre + rareza)
    private List<(string name, string rarity)> allCards = new List<(string, string)>
    {
        ("Latte cat", "Basic"),
        ("Espresso cat", "Basic"),
        ("Lungo cat", "Basic"),
        ("Capuccino cat", "Basic"),
        ("Americano cat", "Basic"),
        ("Bombón cat", "Intermediate"),
        ("Macchiato cat", "Intermediate"),
        ("Frappé cat", "Intermediate"),
        ("Irish cat", "Rare"),
        ("Vienés cat", "Rare"),
        ("Mocca cat", "Legendary"),
    };

    void OnEnable()
    {
        GenerateCollection();
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

        foreach (var card in allCards)
        {
            GameObject newCard = Instantiate(cardPrefab, contentParent);
            Image cardImage = newCard.GetComponentInChildren<Image>();
            TextMeshProUGUI cardText = newCard.GetComponentInChildren<TextMeshProUGUI>();

            bool isUnlocked = unlockedCards.Contains(card.name);

            cardText.text = card.name;

            if (isUnlocked)
            {
                cardImage.color = GetColorByRarity(card.rarity);
                cardText.color = Color.white;
            }
            else
            {
                cardImage.color = lockedColor;
                cardText.color = Color.black;
            }
        }
    }

    private Color GetColorByRarity(string rarity)
    {
        switch (rarity)
        {
            case "Basic": return basicColor;
            case "Intermediate": return intermediateColor;
            case "Rare": return rareColor;
            case "Legendary": return legendaryColor;
            default: return Color.white;
        }
    }
}
