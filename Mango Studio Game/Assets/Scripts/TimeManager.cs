using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI requiredText;
    [SerializeField] private TextMeshProUGUI earnedText;
    [SerializeField] private TextMeshProUGUI endButtonText;

    [Header("Configuración del Tiempo")]
    [SerializeField] private float secondsPerGameMinute = 1f;
    [SerializeField] private int startHour = 7;
    [SerializeField] private int endHour = 20;
    [SerializeField] private int endMinutes = 20;

    [Header("Configuración de facturas")]
    [SerializeField] private int requiredBase = 500;
    [SerializeField] private int requiredIncrement = 250;


    private float currentTimeInSeconds;
    private int currentDay = 0;

    public bool IsOpen { get; private set; }
    private bool isDayEnding = false; // Flag para evitar que la corrutina se lance múltiples veces

    private GameUIManager gameUIManager;
    private CustomerManager customerManager;
    private GameManager gameManager;
    private SceneUIManager sceneUIManager;

    private int required = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    void Start()
    {
        gameUIManager = FindObjectOfType<GameUIManager>();
        customerManager = FindObjectOfType<CustomerManager>();
        gameManager = FindObjectOfType<GameManager>();
        sceneUIManager = FindObjectOfType<SceneUIManager>();
        StartNewDay();
    }

    void Update()
    {
        if (!IsOpen) return;

        currentTimeInSeconds += (60 / secondsPerGameMinute) * Time.deltaTime;

        float currentHour = GetCurrentHour();
        float currentMinute = GetCurrentMinute();

        // Si se cumple la hora de cierre Y el proceso de fin de día no ha empezado ya
        if (!isDayEnding && (currentHour > endHour || (currentHour == endHour && currentMinute >= endMinutes)) && gameUIManager.orderScreen == false)
        {
            StartCoroutine(EndDaySequence());
        }

        UpdateTimeUI();
    }

    public void StartNewDay()
    {
        if (gameManager.monedas <= required && currentDay != 0)
        {
            sceneUIManager.EndGameMenu();
        }

        // Llamamos a la función de reinicio ANTES de hacer cualquier otra cosa
        if (customerManager != null)
        {
            customerManager.ResetForNewDay();
        }

        currentDay++;
        currentTimeInSeconds = startHour * 3600;
        IsOpen = true;
        isDayEnding = false;

        gameManager.monedas -= required;
        HUDManager.Instance.UpdateMonedas(gameManager.monedas);

        required = requiredBase + (currentDay - 1) * requiredIncrement + (int)Random.Range(-50, 50);

        if (gameUIManager != null)
        {
            gameUIManager.ShowGamePanel();
        }

        Debug.Log($"--- DÍA {currentDay} --- \nLa cafetería ha abierto.");
    }

    private IEnumerator EndDaySequence()
    {
        isDayEnding = true; // Marcamos que el fin de día ha comenzado
        IsOpen = false;
        Debug.Log("¡Hora de cerrar! Mostrando resumen del día.");

        if (gameUIManager != null)
        {
            gameUIManager.ShowEndOfDayPanel();
            requiredText.text = "Debes pagar " + required + "$ para cubrir los gastos del local";
        }

        if (gameManager.monedas >= required)
        {
            earnedText.text = "Hoy has ganado " + gameManager.monedas + "$, tienes suficiente para pagar las facturas pendientes";
            endButtonText.text = "Siguiente día";
        }
        else
        {
            earnedText.text = "Hoy has ganado " + gameManager.monedas + "$, no tienes suficiente para pagar las facturas pendientes";
            endButtonText.text = "Finalizar partida";
        }

        // La corrutina ahora simplemente termina aquí.
        // Ya no espera ni llama a StartNewDay().
        yield return null;
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(GetCurrentHour());
            int minutes = Mathf.FloorToInt(GetCurrentMinute());
            timeText.text = $"{hours:D2}:{minutes:D2}";
        }
    }

    private float GetCurrentHour() { return currentTimeInSeconds / 3600f; }
    private float GetCurrentMinute() { return (currentTimeInSeconds % 3600f) / 60f; }
}