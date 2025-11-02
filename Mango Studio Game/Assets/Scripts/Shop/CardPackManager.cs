using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPackManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject packPanel; // Panel 
    public RectTransform packRect; // Sobre
    public Image cardImage; // Imagen de la carta
    public TextMeshProUGUI cardText; // Texto de la carta
    public Button openButton; // Boton de abrir sobre
    public Button closeButton; // Boton de cerrar panel

    [Header("Colores rarezas")]
    public Color basicColor = Color.blue;
    public Color intermediateColor = Color.green;
    public Color rareColor = Color.magenta;
    public Color legendaryColor = Color.yellow;

    [Header("Configuración animación")]
    public float dropDuration = 2f; // Duracion del movimiento
    public float packMoveDistance = 1200f; // Distancia que baja el sobre

    private bool isOpening = false;
    private string pendindPackType = ""; // Guarda el tipo de sobre que se va a abrir

    public enum CardRarity { Basic, Intermediate, Rare, Legendary }

    public void Start()
    {
        packPanel.SetActive(false);
        cardImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(false);
        openButton.onClick.AddListener(OpenPack);
        packRect.anchoredPosition = Vector2.zero;
        isOpening = false;
        pendindPackType = "";
        cardText.text = "";
        closeButton.interactable = false;
    }

    public void ShowPackPanel(string packType)
    {
        if (isOpening) return;

        pendindPackType = packType;
        packPanel.SetActive(true);
        cardImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(true);

        packRect.anchoredPosition = Vector2.zero;   
    }

    public void OpenPack()
    {
        if (isOpening || string.IsNullOrEmpty(pendindPackType)) return;
        StartCoroutine(OpenPackAnimation());
    }

    private IEnumerator OpenPackAnimation()
    {
        closeButton.interactable = false;
        isOpening = true;
        openButton.gameObject.SetActive(false);

        CardRarity rarity = DeterminateRarity(pendindPackType);
        string cardName = GenerateCardName(rarity); 

        // Reiniciar posicion carta
        packRect.anchoredPosition = new Vector2(0, 200f);
        cardImage.gameObject.SetActive(false);
        cardText.text = "";

        // Animación caida de sobre
        Vector2 startPos = packRect.anchoredPosition;
        Vector2 targetPos = startPos - new Vector2(0, packMoveDistance);
        float elapsed = 0f;

        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dropDuration);
            packRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Mostrar la carta
        cardImage.gameObject.SetActive(true);
        cardText.text = cardName;

        switch (rarity)
        {
            case CardRarity.Basic: cardImage.color = basicColor; break;
            case CardRarity.Intermediate: cardImage.color = intermediateColor; break;
            case CardRarity.Rare: cardImage.color = rareColor; break;
            case CardRarity.Legendary: cardImage.color = legendaryColor; break;
        }

        yield return new WaitForSeconds(1.5f);
        isOpening = false;
        closeButton.interactable = true;
    }

    private CardRarity DeterminateRarity(string packType)
    {
        float roll = Random.value;

        // Se asignan las probabilidades de rareza segun el tipo de sobre
        if (packType == "basic") // Sobre basico
        {
            if (roll < 0.93f) return CardRarity.Basic;
            else return CardRarity.Intermediate;
        }
        else // Sobre premium
        {
            if (roll < 0.5f) return CardRarity.Intermediate;
            if (roll < 0.92f) return CardRarity.Rare;
            return CardRarity.Legendary;
        }
    }

    private string GenerateCardName(CardRarity rarity)
    {
        // Se asignan los nombres de las cartas con sus rarezas
        string[] basicNames = { "Latte cat", "Espresso cat", "Lungo cat", "Capuccino cat", "Americano cat" };
        string[] interNames = { "Bombón cat", "Macchiatto cat", "Frappé cat" };
        string[] rareNames = { "Irish cat", "Vienés cat" };
        string[] legNames = { "Mocca cat" };

        // Se genera una carta aleatoria de la rareza seleccionada
        switch (rarity)
        {
            case CardRarity.Basic: return basicNames[Random.Range(0, basicNames.Length)];
            case CardRarity.Intermediate: return interNames[Random.Range(0, interNames.Length)];
            case CardRarity.Rare: return rareNames[Random.Range(0, rareNames.Length)];
            case CardRarity.Legendary: return legNames[Random.Range(0, legNames.Length)];
            default: return string.Empty;
        }
    }

}
