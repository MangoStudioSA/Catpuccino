using System.Collections;
using System.Collections.Generic;
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
    public GameObject emptyCardAnimObject;
    public RectTransform packRect; // Sobre
    public Image cardImage; // Imagen de la carta
    public Image packImage; // Imagen del sobre
    public Button openButton; // Boton de abrir sobre
    public Button closeButton; // Boton de cerrar panel
    public Sprite basicPackSprite; // Sprite sobre basico
    public Sprite premiumPackSprite; // Sprite sobre premium

    [Header("Referencias animacion")]
    public float timeToShowCard = 6f; // Tiempo animacion sobre
    public float emptyCardAnimDuration = 1.2f; // Tiempo animacion carta vacia
    public float timeReadingCard = 2f; // Tiempo que se queda la carta en pantalla

    [Header("Referencias Animaciones")]
    public GameObject basicAnimObject;   
    public GameObject premiumAnimObject;

    [Header("Carta vacia")]
    public Sprite emptyCardSprite;

    [Header("Sprites por rareza")]
    public Sprite[] basicCards;       
    public Sprite[] intermediateCards;
    public Sprite[] rareCards;        
    public Sprite[] legendaryCards;   

    private bool isOpening = false;
    private string pendindPackType = ""; // Guarda el tipo de sobre que se va a abrir
    private Animator emptyCardAnimator;

    public enum CardRarity { Basic, Intermediate, Rare, Legendary }

    private enum CardType { Empty, Collectable }

    private class CardResult
    {
        public CardType type;
        public Sprite sprite;
    }

    public void Start()
    {

        if (emptyCardAnimObject != null)
        {
            emptyCardAnimator = emptyCardAnimObject.GetComponent<Animator>();

            if (emptyCardAnimator == null)
                emptyCardAnimator = emptyCardAnimObject.GetComponentInChildren<Animator>();

            if (emptyCardAnimator != null)
            {
                emptyCardAnimator.updateMode = AnimatorUpdateMode.UnscaledTime; // Unescaled time porque esta el tiempo parado
                emptyCardAnimator.enabled = false;
            }
        }

        // Comprobaciones iniciales
        packPanel.SetActive(false);
        if (basicAnimObject != null) basicAnimObject.SetActive(false);
        if (premiumAnimObject != null) premiumAnimObject.SetActive(false);
        if (emptyCardAnimObject != null) emptyCardAnimObject.SetActive(false);
        
        cardImage.gameObject.SetActive(false);
        openButton.onClick.AddListener(OpenPack);
        closeButton.interactable = false;
    }

    // Funcion que muestra el panel y asocia el tipo de sobre que se va a abrir
    public void PreparePack(string packType)
    {
        pendindPackType = packType;
        packPanel.SetActive(true);
        openButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);

        // Reset visual
        cardImage.gameObject.SetActive(false);
        if (emptyCardAnimObject != null) emptyCardAnimObject.SetActive(false);

        packImage.gameObject.SetActive(true);

        if (packType == "basic") packImage.sprite = basicPackSprite;
        else if (packType == "premium") packImage.sprite = premiumPackSprite;
    }

    // Se ejecuta la corrutina de abrir sobre
    public void OpenPack()
    {
        if (isOpening || string.IsNullOrEmpty(pendindPackType)) return;

        isOpening = true;
        openButton.gameObject.SetActive(false);

        List<CardResult> cardsToReveal = GeneratePackContent(pendindPackType);
        StartCoroutine(OpenPackSequence(cardsToReveal));
    }

    // Funcion para generar el contenido de cartas del sobre
    private List<CardResult> GeneratePackContent(string packType)
    {
        List<CardResult> results = new();

        // Siempre sale primero una carta vacia
        results.Add(new CardResult { type = CardType.Empty, sprite = emptyCardSprite });

        if (packType == "basic")
        {
            float chance = Random.Range(0f, 100f);

            if (chance >= 90f) // 90 - 100 (10%): 2 cartas vacias + 1 coleccionable
            {
                results.Add(new CardResult { type = CardType.Empty, sprite = emptyCardSprite });

                // Y añadimos la coleccionable final
                CardRarity rarity = DeterminateRarity("basic");
                Sprite s = GetRandomCardSprite(rarity);
                results.Add(new CardResult { type = CardType.Collectable, sprite = s });
            }
            else if (chance >= 60f) // 60 - 90 (30%): carta vacia + 1 coleccionable
            {
                CardRarity rarity = DeterminateRarity("basic");
                Sprite s = GetRandomCardSprite(rarity);
                results.Add(new CardResult { type = CardType.Collectable, sprite = s });
            }
            // 0 - 60 (60%): carta vacia
        }
        else if (packType == "premium") // Premium: carta vacia + coleccionable
        {
            CardRarity rarity = DeterminateRarity("premium");
            Sprite s = GetRandomCardSprite(rarity);
            results.Add(new CardResult { type = CardType.Collectable, sprite = s });
        }

        return results;
    }

    // Corrutina que ejecuta las animaciones

    private IEnumerator OpenPackSequence(List<CardResult> cards)
    {
        packImage.gameObject.SetActive(false);

        // Animacion sobre
        GameObject currentAnim = (pendindPackType == "basic") ? basicAnimObject : premiumAnimObject;
        if (currentAnim != null)
        {
            currentAnim.SetActive(true);
            currentAnim.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        yield return new WaitForSecondsRealtime(timeToShowCard);
        if (currentAnim != null) currentAnim.SetActive(false);

        // Bucle de cartas
        for (int i = 0; i < cards.Count; i++)
        {
            CardResult cardData = cards[i];

            // Se muestra la carta
            if (cardData.type == CardType.Empty)
            {
                // Si es una carta vacia se hace la animacion
                if (emptyCardAnimObject != null)
                {
                    cardImage.gameObject.SetActive(false);
                    emptyCardAnimObject.SetActive(true);
                    if (emptyCardAnimator != null)
                    {
                        emptyCardAnimator.enabled = true;
                        emptyCardAnimator.Play(0, -1, 0f);
                        emptyCardAnimator.Update(0f);
                    }
                    yield return new WaitForSecondsRealtime(emptyCardAnimDuration);
                    emptyCardAnimObject.SetActive(false);
                }

                // Se activa la imagen fija con el sprite de carta vacia y se espera
                cardImage.sprite = emptyCardSprite;
                cardImage.color = Color.white;
                cardImage.gameObject.SetActive(true);

                yield return new WaitForSecondsRealtime(timeReadingCard);
            }
            else
            {
                // Carta coleccionable
                if (emptyCardAnimObject != null) emptyCardAnimObject.SetActive(false);

                cardImage.sprite = cardData.sprite;
                cardImage.gameObject.SetActive(true);

                // Efecto fade In
                cardImage.color = new Color(1, 1, 1, 0);
                float fadeTime = 0.5f;
                float elapsed = 0f;
                while (elapsed < fadeTime)
                {
                    elapsed += Time.unscaledDeltaTime;
                    cardImage.color = new Color(1, 1, 1, elapsed / fadeTime);
                    yield return null;
                }
                cardImage.color = Color.white;

                PlayerDataManager.instance.AddCard(cardData.sprite); // Se guarda la carta obtenida
                yield return new WaitForSecondsRealtime(timeReadingCard);
            }

            // Se comprueba si la carta es la ultima de la lista generada
            bool isLastCard = (i == cards.Count - 1);

            // Si no es la ultima carta se reproduce el efecto de fade out
            if (!isLastCard)
            {
                float fadeOutTime = 0.3f;
                float elapsedOut = 0f;
                while (elapsedOut < fadeOutTime)
                {
                    elapsedOut += Time.unscaledDeltaTime;
                    float alpha = 1f - (elapsedOut / fadeOutTime);
                    cardImage.color = new Color(1, 1, 1, alpha);
                    yield return null;
                }
                cardImage.gameObject.SetActive(false);
            }
            // Si era la ultima carta no se reproduce el efecto de fade out
        }

        isOpening = false;
        closeButton.gameObject.SetActive(true);
        closeButton.interactable = true;

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
