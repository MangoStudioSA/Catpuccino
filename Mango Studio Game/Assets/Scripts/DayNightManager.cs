using UnityEngine;

// Clase encargada de gestionar el ciclo de dia/noche
public class DayNightManager : MonoBehaviour
{
    public TimeManager timeManager;

    [Header("Configuración")]
    public float horaInicio = 7f;
    public float horaFin = 20f;

    void Update()
    {
        if (timeManager == null) return;

        // Se accede a la hora actual
        float horaActual = timeManager.GetCurrentHour();

        // Se calcula el porcentaje del dia (0.0 a 1.0)
        // 7:00 -> 0
        // 13:30 -> 0.5
        // 20:00 -> 1
        float porcentaje = Mathf.InverseLerp(horaInicio, horaFin, horaActual);

        // Se crea la rotacion (0-180 grados)
        float angulo = Mathf.Lerp(0f, 180f, porcentaje);

        // Se aplica la rotacion
        transform.rotation = Quaternion.Euler(angulo, -90f, 0f);
    }
}
