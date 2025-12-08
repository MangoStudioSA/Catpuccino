using UnityEngine;
using UnityEngine.Audio;

public class DeliveryPanel : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip clipPendiente; // Variable para guardar el sonido si el panel está cerrado

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Autocrear si falta para evitar errores
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        // 1. Sonido de la caja (Siempre suena al abrir)
        if (SoundsMaster.Instance != null)
        {
            SoundsMaster.Instance.PlaySound_CajaRegistradora();
        }

        // 2. Si había una voz pendiente de sonar, la tocamos ahora
        if (clipPendiente != null)
        {
            audioSource.PlayOneShot(clipPendiente);
            clipPendiente = null; // Ya sonó, limpiamos
        }
    }

    // FUNCIÓN PÚBLICA PARA LLAMAR DESDE EL MANAGER
    public void ReproducirVoz(AudioClip clip)
    {
        if (clip == null) return;

        if (gameObject.activeInHierarchy)
        {
            // CASO A: El panel ya está visible en pantalla -> Suena ya
            if (audioSource != null) audioSource.PlayOneShot(clip);
        }
        else
        {
            // CASO B: El panel está apagado -> Lo guardamos para el OnEnable
            clipPendiente = clip;
        }
    }
}
