using UnityEngine;
using UnityEngine.UI;

// Clase encargada de generar la imagen de la taza/vaso que sigue al cursor
public class DragController : MonoBehaviour
{
    [Header("Referencias")]
    public static DragController Instance;
    public RectTransform dragIcon;
    public Image dragImage;

    private bool dragging = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dragIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!dragging) return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)dragIcon.parent,
            Input.mousePosition,
            null,                // null = funciona en overlay/screen space
            out mousePos
        );

        dragIcon.anchoredPosition = mousePos;
    }

    // Funcion encargada de mostrar el sprite para arrastrar
    public void StartDragging(Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogError("[StartDragging] Sprite = NULL");
            return;
        }

        dragImage.sprite = sprite;
        dragging = true;
        dragIcon.gameObject.SetActive(true);
    }

    // Funcion encargada de desactivar el sprite
    public void StopDragging()
    {
        dragging = false;
        dragIcon.gameObject.SetActive(false);
        dragImage.sprite = null;
    }
}
