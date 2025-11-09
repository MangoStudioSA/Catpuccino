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
    [SerializeField] AudioClip sfx_menu;
    [SerializeField] AudioClip sfx_game;
    [SerializeField] AudioClip sfx_clickMenu;
    [SerializeField] AudioClip sfx_clickCoffeeMachine;
    [SerializeField] AudioClip sfx_coffee;
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

    public void PlaySound_CoffeeReady()
    {
        PlaySound(sfx_coffee);
    }

    public void PlaySound_CoffeeMachine()
    {
        PlaySound(sfx_clickCoffeeMachine);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
