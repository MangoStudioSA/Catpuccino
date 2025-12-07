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

    // Funcion para actualizar el volumen
    private void UpdateSound()
    {
        if (WeatherController.Instance == null) return;

        float volumenObjetivo = 1f; // Exterior

        if (panelesSilenciososAbiertos > 0)
        {
            volumenObjetivo = 0f; 
        }
        else if (openPanels > 0)
        {
            volumenObjetivo = WeatherController.Instance.volumenInterior;
        }

        WeatherController.Instance.SetIndoorVolume(volumenObjetivo);
    }

    private void OnDestroy()
    {
        openPanels = 0;
        panelesSilenciososAbiertos = 0;
    }
}
