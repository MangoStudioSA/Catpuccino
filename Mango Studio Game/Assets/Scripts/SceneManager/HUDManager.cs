using UnityEngine;
using TMPro; 

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    public TextMeshProUGUI textoMonedas;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    public void UpdateMonedas(int cantidad)
    {
        textoMonedas.text = cantidad.ToString();
    }
}