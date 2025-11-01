using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public int monedas; // Inicio monedas

    public PlayerDataManager playerDataManager;

    private int totalSatisfactionScore = 0;
    private int customersRated = 0;
    private int servedCustomers = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
        playerDataManager = FindFirstObjectByType<PlayerDataManager>();
    }

    void Start()
    {
        HUDManager.Instance.UpdateMonedas(monedas);
        playerDataManager.AddMoney(monedas);
    }

    public void AnadirMonedas(int cantidad)
    {
        monedas += cantidad;
        HUDManager.Instance.UpdateMonedas(monedas);
        playerDataManager.AddMoney(monedas);
        Debug.Log("Total de monedas ahora: " + monedas);
    }

    public void AddServedCustomers(int cantidad)
    {
        servedCustomers = cantidad;
        playerDataManager.AddServedCustomers(cantidad);
        Debug.Log("Total de clientes servidos ahora: " + cantidad);
    }

    public void AddSatisfactionPoint(int score)
    {
        totalSatisfactionScore += score;
        customersRated++;

        // Calculamos la media. La puntuación máxima es 50, así que la convertimos a un % (0-100).
        float averageSatisfaction = ((float)totalSatisfactionScore / customersRated);

        // Le pasamos la nueva media al HUD para que la muestre
        HUDManager.Instance.UpdateSatisfaccion(averageSatisfaction);
        playerDataManager.AddSatisfaction(averageSatisfaction);
        Debug.Log($"Satisfacción media ahora: {averageSatisfaction:F1}%");
    }
}