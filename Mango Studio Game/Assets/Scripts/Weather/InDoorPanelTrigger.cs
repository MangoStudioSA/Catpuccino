using UnityEngine;

public class InDoorPanelTrigger : MonoBehaviour
{
    public bool silenciarPorCompleto = false;
    public static int openPanels = 0;
    public static int panelesSilenciososAbiertos = 0;

    private void OnEnable()
    {
        openPanels++;
        if (silenciarPorCompleto) panelesSilenciososAbiertos++;

        UpdateSound();
    }

    private void OnDisable()
    {
        openPanels--;
        if (silenciarPorCompleto) panelesSilenciososAbiertos--;

        // Seguridad para que no baje de 0
        if (openPanels < 0) openPanels = 0;
        if (panelesSilenciososAbiertos < 0) panelesSilenciososAbiertos = 0;

        UpdateSound();
    }

    private void UpdateSound()
    {
        if (WeatherController.Instance == null) return;

        float volumenObjetivo = 1f; // Por defecto: Exterior (100%)

        // LÓGICA DE PRIORIDAD
        if (panelesSilenciososAbiertos > 0)
        {
            // PRIORIDAD 1: Si hay AL MENOS UN panel de silencio total abierto
            volumenObjetivo = 0f;
        }
        else if (openPanels > 0)
        {
            // PRIORIDAD 2: Si hay paneles normales abiertos (pero ninguno silencioso)
            // Usamos la variable que definimos en el WeatherController
            volumenObjetivo = WeatherController.Instance.volumenInterior;
        }

        // Enviamos la orden al controlador
        WeatherController.Instance.SetIndoorVolume(volumenObjetivo);
    }

    private void OnDestroy()
    {
        // Reseteo de seguridad al cambiar de escena
        openPanels = 0;
        panelesSilenciososAbiertos = 0;
    }
}
