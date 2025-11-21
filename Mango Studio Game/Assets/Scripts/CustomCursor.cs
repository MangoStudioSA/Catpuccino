using UnityEngine;

// Clase utilizada para vincular la textura del cursor diseñada
public class CustomCursor : MonoBehaviour
{
    [Header("Referencias")]
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Singleton para que se mantenga entre escenas
    }
    void Start()
    {
        Cursor.visible = true; // Mostrar cursor
        Cursor.lockState = CursorLockMode.None; // No bloquear cursor

        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto); // Modificar cursor
        Debug.Log("Cursor personalizado aplicado correctamente.");
    }
}
