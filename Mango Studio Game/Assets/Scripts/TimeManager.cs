using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI requiredText;
    [SerializeField] private TextMeshProUGUI earnedText;
    [SerializeField] private TextMeshProUGUI endButtonText;

    [Header("Configuraci�n del Tiempo")]
    [SerializeField] private float secondsPerGameMinute = 1f;
    [SerializeField] private int startHour = 7;
    [SerializeField] private int endHour = 20;
    [SerializeField] private int endMinutes = 20;

    [Header("Configuraci�n de facturas")]
    [SerializeField] private int requiredBase = 50;
    [SerializeField] private int requiredIncrement = 20;


    private float currentTimeInSeconds;
    public int currentDay = 0;

    public bool IsOpen { get; private set; }
    private bool isDayEnding = false; // Flag para evitar que la corrutina se lance m�ltiples veces

    private GameUIManager gameUIManager;
    private CustomerManager customerManager;
    private GameManager gameManager;
    private SceneUIManager sceneUIManager;
    private ButtonUnlockManager buttonUnlockManager;

    private int requiredMoney = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        buttonUnlockManager = FindFirstObjectByType<ButtonUnlockManager>();
        if (buttonUnlockManager == null)
        {
            Debug.LogError("No se encontro buttonunlockmanager en la escena");
        }
    }

    void Start()
    {
        gameUIManager = FindFirstObjectByType<GameUIManager>();
        customerManager = FindFirstObjectByType<CustomerManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        sceneUIManager = FindFirstObjectByType<SceneUIManager>();
        StartNewDay();
    }

    void Update()
    {
        if (!IsOpen) return;

        currentTimeInSeconds += (60 / secondsPerGameMinute) * Time.deltaTime;

        float currentHour = GetCurrentHour();
        float currentMinute = GetCurrentMinute();

        // Si se cumple la hora de cierre Y el proceso de fin de d�a no ha empezado ya
        if (!isDayEnding && (currentHour > endHour || (currentHour == endHour && currentMinute >= endMinutes)) && gameUIManager.orderScreen == false)
        {
            StartCoroutine(EndDaySequence());
        }

        UpdateTimeUI();
    }

    public void StartNewDay()
    {
        if (gameManager.monedas <= requiredMoney && currentDay != 0)
        {
            sceneUIManager.EndGameMenu();
        }

        // Llamamos a la funci�n de reinicio ANTES de hacer cualquier otra cosa
        if (customerManager != null)
        {
            customerManager.ResetForNewDay();
        }

        currentDay++;
        currentTimeInSeconds = startHour * 3600;
        IsOpen = true;
        isDayEnding = false;

        gameManager.monedas -= requiredMoney;
        HUDManager.Instance.UpdateMonedas(gameManager.monedas);

        GameProgressManager.Instance.UpdateMechanicsForDay(currentDay);
        buttonUnlockManager.RefreshButtons();


        requiredMoney = requiredBase + (currentDay - 1) * requiredIncrement + (int)Random.Range(-50, 50);

        if (gameUIManager != null)
        {
            gameUIManager.ShowGamePanel();
        }

        currentDayText.text = $"D�a {currentDay:F0}"; // Se muestra el dia actual
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.ShowAvailableCoffees();
            HUDManager.Instance.ShowUnlockedCoffees();
        }

        Debug.Log($"--- D�A {currentDay} --- \nLa cafeter�a ha abierto.");
    }


    private IEnumerator EndDaySequence()
    {
        isDayEnding = true; // Marcamos que el fin de d�a ha comenzado
        IsOpen = false;
        Debug.Log("�Hora de cerrar! Mostrando resumen del d�a.");

        if (gameUIManager != null)
        {
            gameUIManager.ShowEndOfDayPanel();
            requiredText.text = "Debes pagar " + requiredMoney + "$ para cubrir los gastos del local";
        }

        if (gameManager.monedas >= requiredMoney)
        {
            earnedText.text = "Hoy has ganado " + gameManager.monedas + "$, tienes suficiente para pagar las facturas pendientes";
            endButtonText.text = "Siguiente d�a";
        }
        else
        {
            earnedText.text = "Hoy has ganado " + gameManager.monedas + "$, no tienes suficiente para pagar las facturas pendientes";
            endButtonText.text = "Finalizar partida";
        }

        // La corrutina ahora simplemente termina aqu�.
        // Ya no espera ni llama a StartNewDay().
        yield return null;
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(GetCurrentHour());
            int minutes = Mathf.FloorToInt(GetCurrentMinute());
            timeText.text = $"{hours:D2}:{minutes:D2}h";
        }
    }

    private float GetCurrentHour() { return currentTimeInSeconds / 3600f; }
    private float GetCurrentMinute() { return (currentTimeInSeconds % 3600f) / 60f; }
}