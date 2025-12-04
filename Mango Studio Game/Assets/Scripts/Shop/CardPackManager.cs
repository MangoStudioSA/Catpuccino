using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Gestionar abrir sobre
public class CardPackManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public CatCafeManager catCafeManager;
    public CatSelectionUI catSelectionUI;
    public GameObject packPanel; // Panel 
    public RectTransform packRect; // Sobre
    public Image cardImage; // Imagen de la carta
    public Image packImage; // Imagen del sobre
    public Button openButton; // Boton de abrir sobre
    public Button closeButton; // Boton de cerrar panel
    public Sprite basicPackSprite; // Sprite sobre basico
    public Sprite premiumPackSprite; // Sprite sobre premium

    [Header("Referencias animacion")]
    public float timeToShowCard = 2.2f;

    [Header("Referencias Animaciones Full Screen")]
    public GameObject basicAnimObject;   
    public GameObject premiumAnimObject;

    [Header("Sprites por rareza")]
    public Sprite[] basicCards;       
    public Sprite[] intermediateCards;
    public Sprite[] rareCards;        
    public Sprite[] legendaryCards;   

    private bool isOpening = false;
    private string pendindPackType = ""; // Guarda el tipo de sobre que se va a abrir

    public enum CardRarity { Basic, Intermediate, Rare, Legendary }

    // Se resetean todas las variables cada vez que se ejecute
    public void Start()
    {
        packPanel.SetActive(false);
        if (basicAnimObject != null) basicAnimObject.SetActive(false);
        if (premiumAnimObject != null) premiumAnimObject.SetActive(false);

        cardImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        openButton.onClick.AddListener(OpenPack);

        packRect.anchoredPosition = Vector2.zero;
        isOpening = false;
        pendindPackType = "";
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
        cardImage.gameObject.SetActive(false);

        if (basicAnimObject != null) basicAnimObject.SetActive(false);
        if (premiumAnimObject != null) premiumAnimObject.SetActive(false);

        packImage.gameObject.SetActive(true);
        packRect.anchoredPosition = Vector2.zero;

        if (packType == "basic")
        {
            packImage.sprite = basicPackSprite;
        }
        else if (packType == "premium")
        {
            packImage.sprite = premiumPackSprite;
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
        Sprite cardSprite = GetRandomCardSprite(rarity);

        StartCoroutine(OpenPackAnimation(cardSprite));
    }

    private IEnumerator OpenPackAnimation(Sprite cardSprite)
    {
        packImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(false);

        if (pendindPackType == "basic" && basicAnimObject != null)
        {
            basicAnimObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            basicAnimObject.SetActive(true);
        }
        else if (pendindPackType == "premium" && premiumAnimObject != null)
        {
            premiumAnimObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            premiumAnimObject.SetActive(true);
        }

        // Esperar el tiempo configurado
        yield return new WaitForSecondsRealtime(timeToShowCard);

        if (basicAnimObject != null) basicAnimObject.SetActive(false);
        if (premiumAnimObject != null) premiumAnimObject.SetActive(false);

        // Mostrar la carta y su color
        cardImage.sprite = cardSprite;
        cardImage.gameObject.SetActive(true);

        float fadeDuration = 1.2f;
        float fadeElapsed = 0f;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(fadeElapsed / fadeDuration);

            cardImage.color = new Color (1,1,1, alpha);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.25f);

        isOpening = false;
        closeButton.gameObject.SetActive(true);
        closeButton.interactable = true;

        PlayerDataManager.instance.AddCard(cardSprite);
        if (catSelectionUI != null) catSelectionUI.RefreshMenu();
    }

    // Se determina la rareza de la carta dependiendo del sobre abierto
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

    // Se genera un sprite random de la rareza indicada
    private Sprite GetRandomCardSprite(CardRarity rarity)
    {
        Sprite[] sourceArray = null;

        switch (rarity)
        {
            case CardRarity.Basic: sourceArray = basicCards; break;
            case CardRarity.Intermediate: sourceArray = intermediateCards; break;
            case CardRarity.Rare: sourceArray = rareCards; break;
            case CardRarity.Legendary: sourceArray = legendaryCards; break;
        }

        if (sourceArray == null || sourceArray.Length == 0)
        {
            Debug.LogWarning("No hay sprite asignados para la rareza: " + rarity);
            return null;
        }
        return sourceArray[Random.Range(0, sourceArray.Length)];
    }

}
