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
    public Image packImage; // Imagen de la carta
    public TextMeshProUGUI cardText; // Texto de la carta
    public Button openButton; // Boton de abrir sobre
    public Button closeButton; // Boton de cerrar panel

    [Header("Colores rarezas de sobres")]
    public Color basicPackColor; 
    public Color premiumPackColor; 

    [Header("Colores rarezas de carta")]
    public Color basicColor;       
    public Color intermediateColor;
    public Color rareColor;        
    public Color legendaryColor;   

    [Header("Configuración animación")]
    public float dropDuration = 2f; // Duracion del movimiento
    public float packMoveDistance = 1200f; // Distancia que baja el sobre

    private bool isOpening = false;
    private string pendindPackType = ""; // Guarda el tipo de sobre que se va a abrir

    public enum CardRarity { Basic, Intermediate, Rare, Legendary }

    // Se resetean todas las variables cada vez que se ejecute
    public void Start()
    {
        packPanel.SetActive(false);
        cardImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        openButton.onClick.AddListener(OpenPack);
        packRect.anchoredPosition = Vector2.zero;
        isOpening = false;
        pendindPackType = "";
        cardText.text = "";
        closeButton.interactable = false;
    }

    // Se muestra el panel y se asocia el tipo de sobre que se va a abrir
    public void PreparePack(string packType)
    {
        pendindPackType = packType;
        packPanel.SetActive(true);

        if(openButton != null)
            openButton.gameObject.SetActive(true);

        closeButton.gameObject.SetActive(false);
        closeButton.interactable = false;
        cardImage.gameObject.SetActive(false);
        packRect.anchoredPosition = Vector2.zero;   

        if (packType == "basic")
        {
            packImage.color = basicPackColor;
        }
        else if (packType == "premium")
        {
            packImage.color = premiumPackColor;
        }
        Debug.Log("Tipo de sobre abierto: " + packType);
    }

    // Se ejecuta la corrutina de abrir sobre
    public void OpenPack()
    {
        if (isOpening || string.IsNullOrEmpty(pendindPackType)) return;

        isOpening = true;
        if (openButton != null)
            openButton.gameObject.SetActive(false);

        CardRarity rarity = DeterminateRarity(pendindPackType);
        string cardName = GenerateCardName(rarity);

        StartCoroutine(OpenPackAnimation(rarity, cardName));

        PlayerDataManager.instance.AddCard(cardName);
    }

    private IEnumerator OpenPackAnimation(CardRarity rarity, string cardName)
    {
        // Resetear la carta y determinar su rareza y nombre
        isOpening = true;
        openButton.gameObject.SetActive(false);

        // Reiniciar posicion carta
        packRect.anchoredPosition = new Vector2(0, 200f);
        cardImage.gameObject.SetActive(false);
        cardText.text = "";

        // Animación caida de sobre
        Vector2 startPos = packRect.anchoredPosition;
        Vector2 targetPos = startPos - new Vector2(0, packMoveDistance);
        float elapsed = 0f;

        // Animacion de caida del sobre
        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dropDuration);
            packRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Mostrar la carta y su color
        cardImage.gameObject.SetActive(true);
        cardText.text = cardName;

        switch (rarity)
        {
            case CardRarity.Basic: cardImage.color = basicColor; break;
            case CardRarity.Intermediate: cardImage.color = intermediateColor; break;
            case CardRarity.Rare: cardImage.color = rareColor; break;
            case CardRarity.Legendary: cardImage.color = legendaryColor; break;
        }

        // Fade in carta - se empieza con alpha en 0
        Color imgColor = cardImage.color;
        Color textColor = cardText.color;
        imgColor.a = 0f;
        textColor.a = 0f;
        cardImage.color = imgColor;
        cardText.color = textColor;

        float fadeDuration = 1.2f;
        float fadeElapsed = 0f;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeElapsed / fadeDuration);

            imgColor.a = alpha;
            textColor.a = alpha;

            cardImage.color = imgColor;
            cardText.color = textColor;

            yield return null;
        }

        yield return new WaitForSeconds(1.25f);
        isOpening = false;
        closeButton.gameObject.SetActive(true);
        closeButton.interactable = true;
        PlayerDataManager.instance.AddCard(cardName);

        var collection = FindFirstObjectByType<CardCollectionManager>();
        if (collection != null)
            collection.GenerateCollection();
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
            if (roll < 0.6f) return CardRarity.Intermediate;
            if (roll < 0.95f) return CardRarity.Rare;
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
