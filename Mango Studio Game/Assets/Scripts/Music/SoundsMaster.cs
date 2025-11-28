using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsMaster : MonoBehaviour
{
    public static SoundsMaster Instance;

    [SerializeField] private AudioSource audioSource_SFX;
    [SerializeField] private AudioSource audioSource_SoundTrack;
    string loadedScene;

    [Header("Sonidos generales")]
    [SerializeField] AudioClip sfx_menu;
    [SerializeField] AudioClip sfx_game;
    [SerializeField] AudioClip sfx_clickMenu;
    [SerializeField] AudioClip sfx_entregar;
    [SerializeField] AudioClip sfx_takeNote;
    [SerializeField] AudioClip sfx_finDia;

    [Header("Sonidos cafe")]
    [SerializeField] AudioClip sfx_clickCoffeeAmountMachine;
    [SerializeField] AudioClip sfx_coffeeMachine;
    [SerializeField] AudioClip sfx_cantidadCafeEchada;
    [SerializeField] AudioClip sfx_echarCafe;
    [SerializeField] AudioClip sfx_molerCafe;
    [SerializeField] AudioClip sfx_espumador;

    [Header("Sonidos mecanicas")]
    [SerializeField] AudioClip sfx_echarLiquido;
    [SerializeField] AudioClip sfx_cogerHielo;
    [SerializeField] AudioClip sfx_echarHielo;
    [SerializeField] AudioClip sfx_dejarHielo;
    [SerializeField] AudioClip sfx_dejarCogerObj;
    [SerializeField] AudioClip sfx_cuchara;
    [SerializeField] AudioClip sfx_azucar;
    [SerializeField] AudioClip sfx_papelera;

    [Header("Sonidos envases")]
    [SerializeField] AudioClip sfx_cogerPlato;
    [SerializeField] AudioClip sfx_cogerVaso;
    [SerializeField] AudioClip sfx_cogerTaza;
    [SerializeField] AudioClip sfx_cogerBolsa;

    [Header("Sonidos comida")]
    [SerializeField] AudioClip sfx_comida;
    [SerializeField] AudioClip sfx_microondas;

    private Dictionary<string, AudioSource> audioSources = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //Se añade al método DontDestroyOnLoad para que se preserve en las escenas y así se pueda usar el método donde se necesite
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loadedScene = SceneManager.GetActiveScene().name;
        if (loadedScene == "MainMenu")
        {
            PlayMusic(sfx_menu);
        }
        else if (loadedScene == "Game")
        {
            PlayMusic(sfx_game);
        }

        CreateAudioSource("CoffeeMachine", sfx_coffeeMachine, true);
        CreateAudioSource("Espumador", sfx_espumador, true);
        CreateAudioSource("Microondas", sfx_microondas, true);
    }

    private void CreateAudioSource(string key, AudioClip clip, bool loop = false)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.playOnAwake = false;

        audioSources.Add(key, source);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newScene = scene.name;
        if (loadedScene == newScene) return; // Evitar recargar si es la misma escena

        loadedScene = newScene;

        if (loadedScene == "MainMenu")
        {
            PlayMusic(sfx_menu);
        }
        else if (loadedScene == "Game")
        {
            PlayMusic(sfx_game);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource_SoundTrack.clip == clip) return; // Ya está sonando

        audioSource_SoundTrack.Stop();
        audioSource_SoundTrack.clip = clip;
        audioSource_SoundTrack.Play();
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource_SFX.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Se intentó reproducir un AudioClip nulo.");
        }
    }

    public void PlaySound_Menu()
    {
        PlaySound(sfx_menu);
    }

    public void PlaySound_Game()
    {
        PlaySound(sfx_game);
    }

    public void PlaySound_ClickMenu()
    {
        PlaySound(sfx_clickMenu);
    }

    public void PlaySound_TakeOrderNote()
    {
        PlaySound(sfx_takeNote);
    }

    public void PlaySound_Entregar()
    {
        PlaySound(sfx_entregar);
    }

    public void PlaySound_FinDia()
    {
        PlaySound(sfx_finDia);
    }

    public void PlaySound_CoffeeAmountReady()
    {
        PlaySound(sfx_cantidadCafeEchada);
    }

    public void PlaySound_CoffeeAmountMachine()
    {
        PlaySound(sfx_clickCoffeeAmountMachine);
    }

    public void PlaySound_MolerCafe()
    {
        PlaySound(sfx_molerCafe);
    }

    public void PlaySound_EcharCafe()
    {
        PlaySound(sfx_echarCafe);
    }

    public void PlaySound_EcharLiquido()
    {
        PlaySound(sfx_echarLiquido);
    }

    public void PlaySound_CogerDejarObj()
    {
        PlaySound(sfx_dejarCogerObj);
    }

    public void PlaySound_CogerHielo()
    {
        PlaySound(sfx_cogerHielo);
    }

    public void PlaySound_EcharHielo()
    {
        PlaySound(sfx_echarHielo);
    }

    public void PlaySound_DejarHielo()
    {
        PlaySound(sfx_dejarHielo);
    }

    public void PlaySound_Cuchara()
    {
        PlaySound(sfx_cuchara);
    }

    public void PlaySound_Azucar()
    {
        PlaySound(sfx_azucar);
    }

    public void PlaySound_Papelera()
    {
        PlaySound(sfx_papelera);
    }

    public void PlaySound_TakePlate()
    {
        PlaySound(sfx_cogerPlato);
    }

    public void PlaySound_TakeVase()
    {
        PlaySound(sfx_cogerVaso);
    }

    public void PlaySound_TakeCup()
    {
        PlaySound(sfx_cogerTaza);
    }

    public void PlaySound_TakeBag()
    {
        PlaySound(sfx_cogerBolsa);
    }

    public void PlaySound_TakeFood()
    {
        PlaySound(sfx_comida);
    }

    public void PlayAudio(string key)
    {
        if (audioSources.TryGetValue(key, out AudioSource source))
        {
            if (!source.isPlaying)
                source.Play();
        }
    }

    public void StopAudio(string key)
    {
        if (audioSources.TryGetValue(key, out AudioSource source))
        {
            source.Stop();
        }
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
