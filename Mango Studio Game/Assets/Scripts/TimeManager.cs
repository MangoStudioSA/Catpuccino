using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI requiredText;
    [SerializeField] private TextMeshProUGUI earnedText;
    [SerializeField] private TextMeshProUGUI endButtonText;

    [Header("Configuración del Tiempo")]
    [SerializeField] public float secondsPerGameMinute;
    [SerializeField] private float secondsPerGameMinuteBase;
    [SerializeField] private float timeDecay;
    [SerializeField] private int startHour;
    [SerializeField] private int endHour;
    [SerializeField] private int endMinutes;

    [Header("Economia")]
    [SerializeField][Range(0.1f, 1f)] private float difficultyPercentage = 0.85f;

    [Header("Configuración de facturas")]
    [SerializeField] private float requiredIncrement;

    private float currentTimeInSeconds;
    public int currentDay;
    public event Action<int> onDayStarted;

    public bool IsOpen { get; private set; }
    private bool isDayEnding = false; // Flag para evitar que la corrutina se lance múltiples veces

    private GameUIManager gameUIManager;
    private CustomerManager customerManager;
    private SceneUIManager sceneUIManager;
    private ButtonUnlockManager buttonUnlockManager;
    public CoffeeRecipesManager coffeeRecipesManager;
    public CoffeeGameManager coffeeGameManager;
    public CoffeeUnlockerManager coffeeUnlockerManager;
    public HUDManager HUDmanager;
    public PlayerDataManager playerDataManager;
    private TutorialManager tutorialManager;
    private SaveDataManager saveDataManager;

    // Variables para las facturas
    private float averageTicketPrice = 3f;
    private int requiredMoney = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        buttonUnlockManager = FindFirstObjectByType<ButtonUnlockManager>();
        playerDataManager = FindFirstObjectByType<PlayerDataManager>();

        if (buttonUnlockManager == null)
        {
            Debug.LogError("No se encontro buttonunlockmanager en la escena");
        }
    }

    void Start()
    {
        gameUIManager = FindFirstObjectByType<GameUIManager>();
        customerManager = FindFirstObjectByType<CustomerManager>();
        sceneUIManager = FindFirstObjectByType<SceneUIManager>();
        tutorialManager = FindFirstObjectByType<TutorialManager>();
        saveDataManager = FindFirstObjectByType<SaveDataManager>();
        StartNewDay();
    }

    void Update()
    {
        if (!IsOpen) return;

        if (!tutorialManager.isRunningT1 && !tutorialManager.isRunningT2 && !tutorialManager.isRunningT3)
        {
            currentTimeInSeconds += (60 / secondsPerGameMinute) * Time.deltaTime;
        }

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
        // Comprobacion de game over y pago de facturas
        if (GameManager.Instance.monedas < requiredMoney && currentDay != 0)
        {
            saveDataManager.rated = GameManager.Instance.customersRated;
            saveDataManager.score = GameManager.Instance.totalSatisfactionScore;
            sceneUIManager.EndGameMenu();
            return;
        }

        // Restar dinero de facturas si no es el 1 dia
        if (currentDay > 0)
        {
            GameManager.Instance.monedas -= requiredMoney;
            HUDManager.Instance.UpdateMonedas(GameManager.Instance.monedas);
        }
           
        // Guardar y cargar datos
        if (currentDay > 0)
        {
            saveDataManager.currentDay = currentDay + 1;
            saveDataManager.money = GameManager.Instance.monedas;
            saveDataManager.rated = GameManager.Instance.customersRated;
            saveDataManager.score = GameManager.Instance.totalSatisfactionScore;
            saveDataManager.SaveGame();
        }

        // Se resetean los clientes
        if (customerManager != null) customerManager.ResetForNewDay();

        // Se aumenta el dia
        currentDay++;
        coffeeGameManager.customersServed = 0;

        if (SpecialReward.instance != null) SpecialReward.instance.CheckDayCondition();
        
        // Se cargan los datos del jugador y se actualiza la UI
        currentDay = saveDataManager.LoadDay();
        GameManager.Instance.monedas = saveDataManager.LoadMoney();
        GameManager.Instance.customersRated = saveDataManager.LoadRated();
        GameManager.Instance.totalSatisfactionScore = saveDataManager.LoadScore();
        
        if (GameManager.Instance.customersRated > 0)
        {
            float averageSatisfaction = ((float)GameManager.Instance.totalSatisfactionScore / GameManager.Instance.customersRated);
            HUDManager.Instance.UpdateSatisfaccion(averageSatisfaction);
        } 
        HUDManager.Instance.UpdateMonedas(GameManager.Instance.monedas);

        // Calcular velocidad del dia y resetear variables
        secondsPerGameMinute = secondsPerGameMinuteBase + timeDecay * (currentDay - 1);
        
        currentTimeInSeconds = startHour * 3600;
        IsOpen = true;
        isDayEnding = false;

        // Actualizar UI de monedas basicas y premium
        HUDManager.Instance.UpdateUI();

        // Actualizar mecanicas y elementos disponibles para el dia actual
        GameProgressManager.Instance.UpdateMechanicsForDay(currentDay);
        FindFirstObjectByType<FoodManager>().ApplyFoodUnlocks();
        buttonUnlockManager.RefreshButtons();

        // Calculo facturas
        float totalGameMinutes = (endHour - startHour) * 60f + endMinutes;
        float realSecondsDuration = totalGameMinutes * secondsPerGameMinute;

        float t = Mathf.Clamp01((float)(currentDay - 1) / 6f);
        float dynamicEstimate = Mathf.Lerp(80f, 50f, t);

        // Capacidad maxima teorica
        int maxPossibleCustomers = Mathf.FloorToInt(realSecondsDuration / dynamicEstimate);
        // Precio estimado de la comida + cafe
        float currentAvgTicket = averageTicketPrice + (currentDay * 0.2f);
       
        // Calculo base 
        float calculatedBill = maxPossibleCustomers * currentAvgTicket * difficultyPercentage;

        // Variacion
        int randomVar = UnityEngine.Random.Range(-5, 15);
        requiredMoney = Mathf.RoundToInt(calculatedBill) + randomVar;
        
        int minimumFloor = 15 + ((currentDay - 1) * 12);
        requiredMoney = Mathf.Max(minimumFloor, requiredMoney);

        if (currentDay == 1)
        {
            requiredMoney = Mathf.Clamp(requiredMoney, 10, 30);
        }

        // Se muestra el dia actual y se resetea la pizarra con la carta del dia
        if (gameUIManager != null) gameUIManager.ShowGamePanel();
        currentDayText.text = $"Día {currentDay:F0}";
        HUDmanager.ResetNote();

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.ShowAvailableElements();
            HUDManager.Instance.ShowUnlockedElements();
        }
            
        Debug.Log($"--- DÍA {currentDay} --- \nLa cafetería ha abierto.");
        onDayStarted?.Invoke(currentDay);
    }

    // Funcion para las facturas al final del dia
    private IEnumerator EndDaySequence()
    {
        isDayEnding = true; // Marcamos que el fin de día ha comenzado
        IsOpen = false;
        Debug.Log("¡Hora de cerrar! Mostrando resumen del día.");

        if (gameUIManager != null)
        {
            gameUIManager.ShowEndOfDayPanel();
            int t = requiredMoney;
            int l = UnityEngine.Random.Range(t / 8, t / 2);
            t -= l;
            int a = UnityEngine.Random.Range(t / 8, t / 2);
            t -= a;
            int c = UnityEngine.Random.Range(t / 8, t / 2);
            t -= c;

            requiredText.text = "Los gastos de hoy son:\n - Luz y electricidad: " + l + "$\n - Agua: " + a + "$\n- Café y suministros: " + c + "$\n- Mantenimiento: " + t + "$\nTOTAL: " + requiredMoney + "$";
        }

        if (GameManager.Instance.monedas >= requiredMoney)
        {
            PlayerDataManager.instance.AddBasicCoins(50);
            earnedText.text = "En total, tienes ingresados " + GameManager.Instance.monedas + "$. Puedes pagar las facturas pendientes.";
            endButtonText.text = "Siguiente día";
        }
        else
        {
            earnedText.text = "En total, tienes ingresados " + GameManager.Instance.monedas + "$. No puedes pagar las facturas pendientes.";
            endButtonText.text = "Finalizar partida";
        }

        yield return null;
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(GetCurrentHour());
            int minutes = Mathf.FloorToInt(GetCurrentMinute());

            if (minutes>=50) minutes = 50;
            else if (minutes>=40) minutes = 40;
            else if (minutes>=30) minutes = 30;
            else if (minutes >= 20) minutes = 20;
            else if (minutes >= 10) minutes = 10;
            else minutes = 0;
            
            timeText.text = $"{hours:D2}:{minutes:D2}h";
        }
    }

    public float GetCurrentHour() { return currentTimeInSeconds / 3600f; }
    private float GetCurrentMinute() { return (currentTimeInSeconds % 3600f) / 60f; }
}