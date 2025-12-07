using System.Collections;
using UnityEngine;

// Clase encargada de gestionar el clima del juego (lluvia/sol)
public class WeatherController : MonoBehaviour
{
    [Header("Referencias")]
    public ParticleSystem rainSystem;
    public Light sunLight;
    public TimeManager timeManager;

    [Header("Material suelo")]
    public MeshRenderer rendererFloor;
    private Material materialFloor;

    [Header("Duracion clima")]
    public Vector2 sunnyHours = new Vector2(4f, 8f);
    public Vector2 rainyHours = new Vector2(2f, 3.5f);

    [Header("Probabilidad")]
    [Range(0, 100)]
    public float rainProbability = 30f;

    [Header("Clima soleado")]
    public float sunIntensitySunny = 1.5f;
    public Color sunColorSunny = new(1f, 0.95f, 0.8f);
    public float ambientIntensitySunny = 1.0f;
    [Range(0f, 1f)] public float sueloSeco = 0.2f;

    [Header("Clima lluvioso")]
    public float sunIntensityRainy = 0.2f;
    public Color sunColorRainy = new(0.25f, 0.3f, 0.4f);
    public float ambientIntensityRainy = 0.3f;
    [Range(0f, 1f)] public float sueloMojado = 0.9f;

    [Header("Sonidos clima")]
    public AudioClip rainClip;
    public AudioClip ambientClip;
    [Range(0f, 1f)] public float maxVolume = 0.5f;

    private AudioSource rainSource;
    private AudioSource ambientSource;

    private float normalIntensitySun; // Brillo normal del sol
    private float transitionVel = 0.5f; // Suavidad del cambio de luz

    private void Start()
    {
        if (timeManager == null) timeManager = FindFirstObjectByType<TimeManager>();

        // Material del suelo
        if (rendererFloor != null)
        {
            materialFloor = rendererFloor.material;
            materialFloor.SetFloat("_Smoothness", sueloSeco);
        }

        // Sonidos 
        rainSource = CreateWeatherAudioSource("RainSource", rainClip);
        ambientSource = CreateWeatherAudioSource("AmbientSource", ambientClip);

        rainSource.volume = 0f;
        rainSource.Play();

        ambientSource.volume = maxVolume;
        ambientSource.Play();

        // Se empieza sin lluvia
        if (rainSystem != null) rainSystem.Stop();

        // Se inicia el ciclo de clima
        StartCoroutine(WeatherCicle());
    }

    // Funcion para gestionar el sonido
    AudioSource CreateWeatherAudioSource(string name, AudioClip clip)
    {
        GameObject audioObj = new GameObject(name);
        audioObj.transform.parent = this.transform;
        AudioSource source = audioObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        return source;
    }

    // Funcion para calcular el tiempo a esperar
    float CalculateWeatherTime(float gameHours)
    {
        if (timeManager == null) return gameHours;
        return gameHours * 60f * timeManager.secondsPerGameMinute;
    }

    // Corrutina que gestiona los 2 ciclos del clima
    IEnumerator WeatherCicle()
    {
        // Empieza soleado
        yield return StartCoroutine(ChangeWeather(false));

        float waitingHours = Random.Range(sunnyHours.x, sunnyHours.y);
        yield return new WaitForSeconds(waitingHours);

        while (true)
        {
            float chance = Random.Range(0f, 100f);

            if (chance < rainProbability)
            {
                // Lluvia
                if (rainSystem != null) rainSystem.Play();
                yield return StartCoroutine(ChangeWeather(true));

                waitingHours = Random.Range(rainyHours.x, rainyHours.y);
                yield return new WaitForSeconds(CalculateWeatherTime(waitingHours));
            }
            else
            {
                // Soleado
                if (rainSystem != null) rainSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                yield return StartCoroutine(ChangeWeather(false));

                waitingHours = Random.Range(sunnyHours.x, sunnyHours.y);
                yield return new WaitForSeconds(CalculateWeatherTime(waitingHours));
            }
        }
    }

    // Corrutina para cambiar el clima
    IEnumerator ChangeWeather(bool isRain)
    {
        float intensityTarget = isRain ? sunIntensityRainy : sunIntensitySunny;
        Color colorTarget = isRain ? sunColorRainy : sunColorSunny;
        float ambientTarget = isRain ? ambientIntensityRainy : ambientIntensitySunny;

        float startIntensidad = sunLight.intensity;
        Color startColor = sunLight.color;
        float startAmbiente = RenderSettings.ambientIntensity;

        // Material del suelo
        float targetSuelo = isRain ? sueloMojado : sueloSeco;
        float currentSuelo = materialFloor != null ? materialFloor.GetFloat("_Smoothness") : 0f;
        float timer = 0f;

        // Sonidos 
        float targetRainVol = isRain ? maxVolume : 0f;
        float targetAmbientVol = isRain ? 0f : maxVolume;

        float startRainVol = rainSource.volume;
        float startAmbientVol = ambientSource.volume;

        while (timer < 1f)
        {
            timer += Time.deltaTime * transitionVel;

            // Luz
            sunLight.intensity = Mathf.Lerp(startIntensidad, intensityTarget, timer);
            sunLight.color = Color.Lerp(startColor, colorTarget, timer);
            RenderSettings.ambientIntensity = Mathf.Lerp(startAmbiente, ambientTarget, timer);

            // Brillo suelo
            if (materialFloor != null)
            {
                float nuevoBrillo = Mathf.Lerp(currentSuelo, targetSuelo, timer);
                materialFloor.SetFloat("_Smoothness", nuevoBrillo);
            }

            if (rainSource != null)
                rainSource.volume = Mathf.Lerp(startRainVol, targetRainVol, timer);

            if (ambientSource != null)
                ambientSource.volume = Mathf.Lerp(startAmbientVol, targetAmbientVol, timer);

            yield return null;
        }

        sunLight.intensity = intensityTarget;
        sunLight.color = colorTarget;
        RenderSettings.ambientIntensity = ambientTarget;
        if (materialFloor != null) materialFloor.SetFloat("_Smoothness", targetSuelo); // Material suelo

        // Sonido
        if (rainSource != null) rainSource.volume = targetRainVol;
        if (ambientSource != null) ambientSource.volume = targetAmbientVol;
    }
}
