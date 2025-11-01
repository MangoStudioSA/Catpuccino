using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsMaster : MonoBehaviour
{
    public static SoundsMaster Instance;

    AudioSource audioSource_SFX;
    AudioSource audioSource_SoundTrack;
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        string checkScene;
        checkScene = SceneManager.GetActiveScene().name;
        if(loadedScene != checkScene)
        {
            loadedScene = checkScene;
            if (loadedScene == "MainMenu")
            {
                FindAudioSources();
                audioSource_SoundTrack.clip = sfx_menu;
                audioSource_SoundTrack.Play();
            }
            else if (loadedScene == "Game")
            {
                FindAudioSources();
                audioSource_SoundTrack.clip = sfx_game;
                audioSource_SoundTrack.Play();
            }
        }
    }

    private void Start()
    {
        FindAudioSources();
        loadedScene = SceneManager.GetActiveScene().name;
        if (loadedScene == "MainMenu")
        {
            FindAudioSources();
            audioSource_SoundTrack.clip = sfx_menu;
            audioSource_SoundTrack.Play();
        }
        else if (loadedScene == "Game")
        {
            FindAudioSources();
            audioSource_SoundTrack.clip = sfx_game;
            audioSource_SoundTrack.Play();
        }
    }

    void FindAudioSources()
    {
        audioSource_SFX = FindFirstObjectByType<SFXSource>().GetComponent<AudioSource>();
        audioSource_SoundTrack = FindFirstObjectByType<SoundTrackSource>().GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)
    {
        FindAudioSources();
        audioSource_SFX.PlayOneShot(clip);
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
}
