using System.Collections;
using TMPro;
using UnityEngine;

// Mostrar un panel que se activa/desactiva con la informacion de los sobres 
public class InfoUI : MonoBehaviour
{
    [SerializeField] private RectTransform noteInfo;
    [SerializeField] private TextMeshProUGUI infoTxt;
    [SerializeField] private float slideDuration = 0.5f;
    
    // Posiciones del panel
    private Vector2 hiddenPos;
    private Vector2 visiblePos;

    private bool isVisible = false;

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
        isVisible = !isVisible;
        noteInfo.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(SlideNote(isVisible));    
    }

    // Corrutina para que se muestre el panel con animacion de subida o bajada
    private IEnumerator SlideNote (bool show)
    {
        Vector2 start = noteInfo.anchoredPosition; // posicion inicial
        Vector2 end = show ? visiblePos : hiddenPos; // posicion final

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            noteInfo.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        noteInfo.anchoredPosition = end;

        if (!show)
            noteInfo.gameObject.SetActive(false);
    }
}
