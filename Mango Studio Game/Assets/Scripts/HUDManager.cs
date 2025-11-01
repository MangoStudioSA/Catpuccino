using UnityEngine;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public CoffeeUnlockerManager coffeeUnlockManager;
    public FoodUnlockerManager foodUnlockerManager;
    public TimeManager timeManager;
    public static HUDManager Instance { get; private set; }

    [SerializeField] private RectTransform notePanelAvailableItems;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI availableElementsTxt;

    [SerializeField] private GameObject unlockedCanvas;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool useUnscaledTime = false;
    [SerializeField] private TextMeshProUGUI unlockedElementsTxt;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;

    private bool isVisible = false;
    private CanvasGroup unlockedCanvasGroup;
    private Coroutine unlockedCoroutine;
    private Coroutine slideCoroutine;

    public TextMeshProUGUI textoSatisfaccion;
    public TextMeshProUGUI textoMonedas;
    [SerializeField] private TextMeshProUGUI basicCoinsTxt;
    [SerializeField] private TextMeshProUGUI premiumCoinsTxt;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        //ShowAvailableElements();

        visiblePos = notePanelAvailableItems.anchoredPosition;
        hiddenPos = new Vector2(visiblePos.x, visiblePos.y - notePanelAvailableItems.rect.height);
        
        notePanelAvailableItems.anchoredPosition = hiddenPos;
        notePanelAvailableItems.gameObject.SetActive(false);

        unlockedCanvasGroup = unlockedCanvas.GetComponent<CanvasGroup>();
        if (unlockedCanvasGroup == null )
        {
            unlockedCanvasGroup = unlockedCanvas.AddComponent<CanvasGroup>();
        }

        unlockedCanvasGroup.alpha = 0f;
        unlockedCanvasGroup.interactable = false;
        unlockedCanvasGroup.blocksRaycasts = false;
        unlockedCanvas.SetActive(false);
    }

    public void UpdateMonedas(int cantidad)
    {
        textoMonedas.text = cantidad + " $";
    }

    public void UpdateSatisfaccion(float cantidad)
    {
        // "F0" formatea el número para que no tenga decimales
        textoSatisfaccion.text = $"Satisfacción: {cantidad:F0}%";
    }

    public void UpdateBasicCoins(int cantidad)
    {
        basicCoinsTxt.text = $"Monedas básicas: {cantidad}";
    }
    public void UpdatePremiumCoins(int cantidad)
    {
        premiumCoinsTxt.text = $"Monedas premium: {cantidad}";
    }

    // Funcion encargada de mostrar por pantalla los cafes disponibles del dia actual
    public void ShowAvailableElements()
    {
        int day = timeManager.currentDay;
        CoffeeType[] availableCoffees = coffeeUnlockManager.GetAvailableCoffees(day);
        FoodCategory[] availableFood = foodUnlockerManager.GetAvailableFood(day);

        string message = "Disponible en la carta: \n";

        if (availableCoffees.Length > 0)
        {
            string coffeesList = string.Join(", ", availableCoffees); // Se separa cada cafe por ","
            message += $"Cafés: {coffeesList}. \n";
        } else
        {
            message += "No hay cafes disponibles. \n";
        }

        if (availableFood.Length > 0)
        {
            string foodList = string.Join(", ", availableFood); // Se separa cada comida por ","
            message += $"Comidas: {foodList}.";
        }
        else
        {
            message += "No hay comida disponible.";
        }

        UpdateAvailableText(message);
    }

    // Funcion encargada de mostrar por pantalla los cafes desbloqueados en el dia actual
    public void ShowUnlockedElements()
    {
        int day = timeManager.currentDay;
        CoffeeType[] unlockedCoffees = coffeeUnlockManager.GetUnlockedCoffees(day);
        FoodCategory[] unlockedFood = foodUnlockerManager.GetUnlockedFood(day);

        string message = "¡Has desbloqueado nuevos cafés y/o postres! \n";

        if (unlockedCoffees.Length > 0)
        {
            string unlockedCoffeesList = string.Join(", ", unlockedCoffees); // Se separa cada cafe por ","
            message += $"Cafés de tipo {unlockedCoffeesList}. \n";
        }
        else
        {
            message += "";
        }

        if (unlockedFood.Length > 0)
        {
            string unlockedFoodList = string.Join(", ", unlockedFood); // Se separa cada comida por ","
            message += $"Postre a hornear: {unlockedFoodList}. \n";
        }
        else
        {
            message += "";
        }

        unlockedElementsTxt.text = message;

        if (unlockedCoroutine != null) StopCoroutine(unlockedCoroutine);

        unlockedCoroutine = StartCoroutine(HideUnlockedCanvasAfterTime());
    }

    private IEnumerator HideUnlockedCanvasAfterTime()
    {
        unlockedCanvas.SetActive(true);
        unlockedCanvasGroup.alpha = 0f;
        unlockedCanvasGroup.interactable = false;
        unlockedCanvasGroup.blocksRaycasts = false;

        float elapsed = 0f;
        float dt;

        // Efecto fade in
        elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            dt = useUnscaledTime ? Time.unscaledTime : Time.deltaTime;
            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            unlockedCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        unlockedCanvasGroup.alpha = 1f;
        unlockedCanvasGroup.interactable = true;
        unlockedCanvasGroup.blocksRaycasts = true;

        // Tiempo en espera
        float wait = 0f;
        while ( wait < displayDuration)
        {
            dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            wait += dt;
            yield return null;
        }

        unlockedCanvasGroup.interactable = false;
        unlockedCanvasGroup.blocksRaycasts = false;

        // Efecto fade out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            unlockedCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        unlockedCanvasGroup.alpha = 0f;
        unlockedCanvas.SetActive(false);
        unlockedCoroutine = null;
    }

    public void ResetNote()
    {
        notePanelAvailableItems.gameObject.SetActive(false);
    }

    public void UpdateAvailableText(string message)
    {
        availableElementsTxt.text = message;
    }
    public void ToggleNote()
    {
        isVisible = !isVisible;
        notePanelAvailableItems.gameObject.SetActive(true);

        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        slideCoroutine = StartCoroutine(SlideNote(isVisible));
    }
    private IEnumerator SlideNote(bool show)
    {
        Vector2 start = notePanelAvailableItems.anchoredPosition;
        Vector2 end = show ? visiblePos : hiddenPos;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            notePanelAvailableItems.anchoredPosition = Vector2.Lerp(start, end, t); 
            yield return null;
        }

        notePanelAvailableItems.anchoredPosition = end;

        if (!show)
        {
            notePanelAvailableItems.gameObject.SetActive(false);
        }
    }
}