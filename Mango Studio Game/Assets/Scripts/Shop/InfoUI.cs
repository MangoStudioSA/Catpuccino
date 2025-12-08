using System.Collections;
using TMPro;
using UnityEngine;

// Mostrar un panel que se activa/desactiva con la informacion de los sobres 
public class InfoUI : MonoBehaviour
{
    [SerializeField] private RectTransform noteInfo;
    [SerializeField] private float slideDuration = 1.5f;
    
    // Posiciones del panel
    private Vector2 hiddenPos;
    private Vector2 visiblePos;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        visiblePos = noteInfo.anchoredPosition;

        hiddenPos = new Vector2(visiblePos.x, visiblePos.y - noteInfo.rect.height);
        noteInfo.anchoredPosition = hiddenPos;
        noteInfo.gameObject.SetActive(false);
    }

    // Funcion para activar/desactivar el panel
    public void ToggleNote()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        SoundsMaster.Instance.PlaySound_ClickMenu();

        noteInfo.gameObject.SetActive(true);
        bool show = noteInfo.anchoredPosition.y < (visiblePos.y - 1f);

        if (show)
            currentCoroutine = StartCoroutine(SlideNote(visiblePos, true)); // Subir
        else
            currentCoroutine = StartCoroutine(SlideNote(hiddenPos, false));   
    }

    // Corrutina para que se muestre el panel con animacion de subida o bajada
    private IEnumerator SlideNote (Vector2 target, bool show)
    {
        Vector2 start = noteInfo.anchoredPosition; // posicion inicial

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            noteInfo.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        noteInfo.anchoredPosition = target;

        if (!show)
            noteInfo.gameObject.SetActive(false);
    }
}
