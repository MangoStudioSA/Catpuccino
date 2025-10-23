using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursorTexture;
    [SerializeField] Texture2D tazaCursorTexture;
    [SerializeField] Texture2D filtroCursorTexture;
    [SerializeField] Texture2D cucharaCursorTexture;
    [SerializeField] Texture2D hieloCucharaCursorTexture;
    [SerializeField] Texture2D tapaCursorTexture;
    [SerializeField] Texture2D aguaCursorTexture;
    [SerializeField] Texture2D lecheCursorTexture;

    [SerializeField] Vector2 hotSpotDefault = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTaza = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotFiltro = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotHieloCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTapaCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotAgua = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotLeche = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)


    [SerializeField] MinigameInput miniGameInput; //para poder usar tazaIsThere       


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
        if (miniGameInput.tazaInHand == true)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
            miniGameInput.tazaInHand = false;
        }
        else if (miniGameInput.tazaIsThere == false && miniGameInput.tazaInHand == false)
        {
            Cursor.SetCursor(tazaCursorTexture, hotSpotTaza, CursorMode.Auto);
            miniGameInput.tazaInHand = true;
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
    public void TakeCuchara()
    {
        //tenia problemas de sincronizacion asi que he tenido que hacerlo asi, tiene en cuenta a variable antigua en lugar de la nueva, por que se comprueba aqui si es true antes de lo que
        //se pone en true en el otro script, se que es un poco chapuza pero funciona y yo soy artista no programadora :)
        if (miniGameInput.cucharaInHand == true && miniGameInput.coverInHand == false && miniGameInput.iceInHand==false && miniGameInput.waterInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(cucharaCursorTexture, hotSpotCuchara, CursorMode.Auto);
        }

        if (miniGameInput.cucharaInHand == false && miniGameInput.coverInHand == false && miniGameInput.iceInHand == false && miniGameInput.waterInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeMilk()
    {
        if (miniGameInput.milkInHand == true && miniGameInput.waterInHand == false && miniGameInput.coverInHand == false && miniGameInput.cucharaInHand == false && miniGameInput.iceInHand == false)
        {
            Cursor.SetCursor(aguaCursorTexture, hotSpotAgua, CursorMode.Auto);
        }

        if (miniGameInput.milkInHand == false && miniGameInput.waterInHand == false && miniGameInput.coverInHand == false && miniGameInput.cucharaInHand == false && miniGameInput.iceInHand == false)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeWater()
    {
        if (miniGameInput.waterInHand == true && miniGameInput.coverInHand == false && miniGameInput.cucharaInHand == false && miniGameInput.iceInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(aguaCursorTexture, hotSpotAgua, CursorMode.Auto);
        }

        if (miniGameInput.waterInHand == false && miniGameInput.coverInHand == false && miniGameInput.cucharaInHand == false && miniGameInput.iceInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeHielo()
    { 
        if(miniGameInput.iceInHand == true && miniGameInput.cucharaInHand == false && miniGameInput.coverInHand == false && miniGameInput.waterInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(hieloCucharaCursorTexture, hotSpotHieloCuchara, CursorMode.Auto);
        }

        if (miniGameInput.iceInHand == false && miniGameInput.cucharaInHand == false && miniGameInput.coverInHand == false && miniGameInput.waterInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeCover()
    {
        if (miniGameInput.coverInHand == true && miniGameInput.cucharaInHand == false && miniGameInput.iceInHand == false && miniGameInput.waterInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(tapaCursorTexture, hotSpotTapaCuchara, CursorMode.Auto);
        }

        if (miniGameInput.coverInHand == false && miniGameInput.cucharaInHand == false && miniGameInput.iceInHand == false && miniGameInput.waterInHand == false && miniGameInput.milkInHand == false)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }
}
