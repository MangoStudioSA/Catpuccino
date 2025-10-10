using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int monedas = 100; // Inicio 100 monedas

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
}