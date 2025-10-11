using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int monedas = 100; // Inicio 100 monedas


    private int totalSatisfactionScore = 0;
    private int customersRated = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    void Start()
    {
        HUDManager.Instance.UpdateMonedas(monedas);
    }

    public void AnadirMonedas(int cantidad)
    {
        monedas += cantidad;
        HUDManager.Instance.UpdateMonedas(monedas);
        Debug.Log("Total de monedas ahora: " + monedas);
    }

    public void AddSatisfactionPoint(int score)
    {
        totalSatisfactionScore += score;
        customersRated++;

        // Calculamos la media. La puntuación máxima es 50, así que la convertimos a un % (0-100).
        float averageSatisfaction = ((float)totalSatisfactionScore / customersRated) / 50f * 100f;

        // Le pasamos la nueva media al HUD para que la muestre
        HUDManager.Instance.UpdateSatisfaccion(averageSatisfaction);
        Debug.Log($"Satisfacción media ahora: {averageSatisfaction:F1}%");
    }
}