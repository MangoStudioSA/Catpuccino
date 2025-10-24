using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursorTexture;
    [SerializeField] Texture2D tazaCursorTexture;
    [SerializeField] Texture2D vasoCursorTexture;
    [SerializeField] Texture2D filtroCursorTexture;
    [SerializeField] Texture2D cucharaCursorTexture;
    [SerializeField] Texture2D hieloCucharaCursorTexture;
    [SerializeField] Texture2D tapaCursorTexture;
    [SerializeField] Texture2D aguaCursorTexture;
    [SerializeField] Texture2D lecheCursorTexture;
    [SerializeField] Texture2D lecheCondensadaCursorTexture;
    [SerializeField] Texture2D cremaCursorTexture;
    [SerializeField] Texture2D chocolateCursorTexture;
    [SerializeField] Texture2D whiskeyCursorTexture;

    [SerializeField] Vector2 hotSpotDefault = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTaza = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotVaso = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotFiltro = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotHieloCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTapaCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotAgua = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotLeche = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotLecheCondensada = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCrema = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotChocolate = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotWhiskey = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)

    [SerializeField] MinigameInput miniGameInput; //para poder usar referencias a los vasos y tazas       


    void Start()
    {
        //ocultamos cursor por defecto
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

    // Gestionar coger la taza del estante
    public void TakeTazaFromShelf()
    {
        if (!miniGameInput.tazaInHand && !miniGameInput.tazaIsInCafetera && !miniGameInput.vasoInHand)
        {
            miniGameInput.tazaInHand = true;
            Cursor.SetCursor(tazaCursorTexture, hotSpotTaza, CursorMode.Auto);
        }
    }

    // Gestionar coger el vaso del estante
    public void TakeVasoFromShelf()
    {
        if (!miniGameInput.vasoInHand && !miniGameInput.vasoIsInCafetera && !miniGameInput.tazaInHand)
        {
            miniGameInput.vasoInHand = true;
            Cursor.SetCursor(vasoCursorTexture, hotSpotVaso, CursorMode.Auto);
        }
    }

    // Gestionar cursor taza
    public void UpdateCursorTaza(bool dejandoTaza)
    {
        if (miniGameInput.TengoOtroObjetoEnLaMano())
            return;

        if (dejandoTaza)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(tazaCursorTexture, hotSpotTaza, CursorMode.Auto);
        }
    }

    // Gestionar cursor vaso
    public void UpdateCursorVaso(bool dejandoVaso)
    {
        if (miniGameInput.TengoOtroObjetoEnLaMano())
            return;

        if (dejandoVaso)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(vasoCursorTexture, hotSpotVaso, CursorMode.Auto);
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
        if (miniGameInput.cucharaInHand && !miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.iceInHand && !miniGameInput.milkInHand 
            && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(cucharaCursorTexture, hotSpotCuchara, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeMilk()
    {
        if (miniGameInput.milkInHand && !miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand 
            && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(lecheCursorTexture, hotSpotLeche, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeWater()
    {
        if (miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand && !miniGameInput.milkInHand 
            && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(aguaCursorTexture, hotSpotAgua, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeCondensedMilk()
    {
        if (miniGameInput.condensedMilkInHand && !miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand 
            && !miniGameInput.milkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(lecheCondensadaCursorTexture, hotSpotLecheCondensada, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeCream()
    {
        if (miniGameInput.creamInHand && !miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand
            && !miniGameInput.milkInHand && !miniGameInput.condensedMilkInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(cremaCursorTexture, hotSpotCrema, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeChocolate()
    {
        if (miniGameInput.chocolateInHand && !miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand 
            && !miniGameInput.milkInHand && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(chocolateCursorTexture, hotSpotChocolate, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeWhiskey()
    {
        if (miniGameInput.whiskeyInHand && !miniGameInput.waterInHand && !miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand 
            && !miniGameInput.milkInHand && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand)
        {
            Cursor.SetCursor(whiskeyCursorTexture, hotSpotWhiskey, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeHielo()
    { 
        if(miniGameInput.iceInHand && !miniGameInput.cucharaInHand && !miniGameInput.coverInHand && !miniGameInput.waterInHand && !miniGameInput.milkInHand 
            && !miniGameInput.condensedMilkInHand  && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(hieloCucharaCursorTexture, hotSpotHieloCuchara, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeCover()
    {
        if (miniGameInput.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand && !miniGameInput.waterInHand && !miniGameInput.milkInHand 
            && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(tapaCursorTexture, hotSpotTapaCuchara, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }
}
