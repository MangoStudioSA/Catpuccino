using System.Collections;
using UnityEngine;

// Clase encargada de gestionar la animacion del menu de seleccion de gatos
public class CatsMenu : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private CatSelectionUI catSelectionUI;

    [Header("Configuracion")]
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float widthButton = 50f;

    private Vector2 posOpen;
    private Vector2 posClose;
    private bool isOpen = false;
    private Coroutine animation;

    void Start()
    {
        // Posicion inicial - panel abierto
        posOpen = panelRectTransform.anchoredPosition;

        // Se calcula el ancho total de la imagen
        float totalWidth = panelRectTransform.rect.width;

        // Se calcula cuanto esconder
        float moveDistance = totalWidth - widthButton;

        // Se suman las posiciones porque el pivot esta a la derecha
        posClose = new Vector2(posOpen.x + moveDistance, posOpen.y);

        panelRectTransform.anchoredPosition = posClose;
    }

    // Funcion para mostrar/ocultar el menu
    public void ToggleMenu()
    {
        isOpen = !isOpen;
        SoundsMaster.Instance.PlaySound_ClickMenu();

        if (animation != null) StopCoroutine(animation);
        animation = StartCoroutine(MovePanel(isOpen));
    }

    // Corrutina para mostrar/ocultar el menu
    private IEnumerator MovePanel(bool open)
    {
        if (open && catSelectionUI != null) catSelectionUI.RefreshMenu();

        Vector2 start = panelRectTransform.anchoredPosition;
        Vector2 end = open ? posOpen : posClose;

        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            panelRectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        panelRectTransform.anchoredPosition = end;
    }
}
