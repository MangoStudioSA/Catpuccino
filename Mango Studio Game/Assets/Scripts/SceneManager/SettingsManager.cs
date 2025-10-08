using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Toggle fullscreenToggle;

    [SerializeField] private Image brightnessOverlay;

    private void Start()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        AdjustBrightness(PlayerPrefs.GetFloat("Brightness", 1f));
        AdjustVolume(PlayerPrefs.GetFloat("Volume", 1f));

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(AdjustVolume);
        }
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        }
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }

    private void OnLevelWasLoaded()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        // Aplicar el brillo guardado al iniciar
        AdjustBrightness(PlayerPrefs.GetFloat("Brightness", 1f)); // Valor por defecto 1f (brillo total)
        AdjustVolume(PlayerPrefs.GetFloat("Volume", 1f));
    }

    public void AdjustBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            float overlayAlpha = 1f - brightnessSlider.value;
            brightnessOverlay.color = new Color(0, 0, 0, overlayAlpha);
        }
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
    }

    public void AdjustVolume(float value)
    {
        // Ajustar el volumen global
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value); // Guardar configuración
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0); // Guardar configuración
    }

    public void LoadSettings()
    {
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1f); // Brillo por defecto = 1
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
        }
        AdjustBrightness(savedBrightness);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1; // Por defecto pantalla completa
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
        }
        SetFullscreen(isFullscreen);
    }

    public void OnMouseEnterSound()
    {
        //SoundsMaster.Instance.PlaySound_Lever();
    }
}
