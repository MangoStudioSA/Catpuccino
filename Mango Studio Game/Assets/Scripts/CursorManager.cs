using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursorTexture;
    public Texture2D tazaCursorTexture;
    public Texture2D filtroCursorTexture;

    public Vector2 hotSpotDefault = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    public Vector2 hotSpotTaza = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    public Vector2 hotSpotFiltro = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)

    public MinigameInput miniGameInput; //para poder usar tazaIsThere       


    void Start()
    {
        //ocultamos cursor por defecto de so
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //aplicar cursor personalizado
        if (defaultCursorTexture != null)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
            Debug.Log("Cursor personalizado aplicado correctamente.");
        }
        else
        {
            Debug.LogError("ERROR: No se ha asignado una textura para el cursor. Se usará el cursor por defecto.");
        }
    }

    public void TakeTaza()
    {
        if (miniGameInput.tazaIsThere == false)
        {
            Cursor.SetCursor(tazaCursorTexture, hotSpotTaza, CursorMode.Auto);
        }
    }
    public void PutTaza()
    {
        if (miniGameInput.tazaIsThere == false)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeFiltro()
    {
        Cursor.SetCursor(filtroCursorTexture, hotSpotFiltro, CursorMode.Auto);
    }
    public void PutFiltro()
    {
        Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
    }
}
