using UnityEngine;
using TMPro; 

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    public TextMeshProUGUI textoSatisfaccion;

    public TextMeshProUGUI textoMonedas;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    public void UpdateMonedas(int cantidad)
    {
        textoMonedas.text = cantidad + " $";
    }

    public void UpdateSatisfaccion(float cantidad)
    {
        // "F0" formatea el n�mero para que no tenga decimales
        textoSatisfaccion.text = $"Satisfacci�n: {cantidad:F0}%";
    }
}