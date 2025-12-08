using UnityEngine;

public class OrderPanel : MonoBehaviour
{
    private AudioSource audioSource;
    private CoffeeGameManager coffeeManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Buscamos el manager automáticamente
        coffeeManager = FindFirstObjectByType<CoffeeGameManager>();
    }

    private void OnEnable()
    {
        PlayOrderSound();
    }

    public void PlayOrderSound()
    {
        if (coffeeManager == null) return;

        // 1. Pedimos el clip neutral del cliente que esté en la cola
        AudioClip voiceClip = coffeeManager.GetCurrentOrderingCustomerVoice();

        // 2. Si existe, lo reproducimos
        if (voiceClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(voiceClip);
        }
    }
}
