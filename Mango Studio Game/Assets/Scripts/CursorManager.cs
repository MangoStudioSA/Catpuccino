using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [Header("Texturas cursores cafe")]
    [SerializeField] Texture2D defaultCursorTexture;
    [SerializeField] Texture2D tazaCursorTexture;
    [SerializeField] Texture2D vasoCursorTexture;
    [SerializeField] Texture2D filtroCursorTexture;
    [SerializeField] Texture2D cucharaCursorTexture;
    [SerializeField] Texture2D hieloCucharaCursorTexture;
    [SerializeField] Texture2D tapaCursorTexture;
    [SerializeField] Texture2D aguaCursorTexture;
    [SerializeField] Texture2D lecheCursorTexture;
    [SerializeField] Texture2D tazaLecheCursorTexture;
    [SerializeField] Texture2D lecheCondensadaCursorTexture;
    [SerializeField] Texture2D cremaCursorTexture;
    [SerializeField] Texture2D chocolateCursorTexture;
    [SerializeField] Texture2D whiskeyCursorTexture;
    [SerializeField] Texture2D hieloCucharaVaciaCursorTexture;

    [Header("Texturas cursores comida")]
    [SerializeField] Texture2D platoCursorTexture;
    [SerializeField] Texture2D bolsaLlevarCursorTexture;

    [Header("HotSpots cursores cafe")]
    [SerializeField] Vector2 hotSpotDefault = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTaza = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotVaso = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotFiltro = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotHieloCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTapaCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotAgua = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotLeche = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotTazaLeche = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotLecheCondensada = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCrema = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotChocolate = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotWhiskey = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)

    [Header("HotSpots cursores comida")]
    [SerializeField] Vector2 hotSpotPlato = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotBolsaLlevar = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)


    [SerializeField] MinigameInput miniGameInput; //para poder usar referencias a los vasos y tazas       
    public FoodManager foodManager;
    public TutorialManager tutorialManager;
    public FoodMinigameInput foodMinigameInput;

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
        if (!miniGameInput.tazaInHand && !miniGameInput.tazaIsInCafetera && !miniGameInput.vasoInHand && !miniGameInput.platoTazaInHand)
        {
            miniGameInput.tazaInHand = true;
            Cursor.SetCursor(tazaCursorTexture, hotSpotTaza, CursorMode.Auto);
        } 
        else if (miniGameInput.tazaInHand)
        {
            miniGameInput.tazaInHand = false;
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    // Gestionar coger el vaso del estante
    public void TakeVasoFromShelf()
    {
        if (!miniGameInput.vasoInHand && !miniGameInput.vasoIsInCafetera && !miniGameInput.tazaInHand && !miniGameInput.platoTazaInHand)
        {
            miniGameInput.vasoInHand = true;
            Cursor.SetCursor(vasoCursorTexture, hotSpotVaso, CursorMode.Auto);
        } 
        else if (miniGameInput.vasoInHand)
        {
            miniGameInput.vasoInHand = false;
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }


    // Gestionar coger la taza de leche
    public void TakeTazaWithMilk()
    {
        if (!miniGameInput.tazaMilkInHand && !miniGameInput.TengoOtroObjetoEnLaMano())
        {
            miniGameInput.tazaMilkInHand = true;
            Cursor.SetCursor(tazaLecheCursorTexture, hotSpotTazaLeche, CursorMode.Auto);
            miniGameInput.buttonManager.cogerTazaLecheButton.image.enabled = false;
        }
    }

    // Gestionar coger el plato para la taza del estante
    public void TakePlatoCupFromShelf()
    {
        if (miniGameInput.platoTazaIsInTable) return;
        if (miniGameInput.TengoOtroObjetoEnLaMano() || miniGameInput.tazaInHand || miniGameInput.vasoInHand) return;

        if (!miniGameInput.platoTazaInHand)
        {
            miniGameInput.platoTazaInHand = true;
            Cursor.SetCursor(platoCursorTexture, hotSpotPlato, CursorMode.Auto);
        }
        else if (miniGameInput.platoTazaInHand)
        {
            miniGameInput.platoTazaInHand = false;
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    // Gestionar coger el plato del estante
    public void TakePlatoFromShelf()
    {
        if (foodMinigameInput.platoIsInEncimera)
            return;

        if (!foodMinigameInput.platoInHand)
        {
            foodMinigameInput.platoInHand = true;
            Cursor.SetCursor(platoCursorTexture, hotSpotPlato, CursorMode.Auto);
        }
    }

    // Gestionar coger la bolsa para llevar del estante
    public void TakeCarryBagFromShelf()
    {
        if (foodMinigameInput.carryBagIsInEncimera)
            return;

        if (!foodMinigameInput.carryBagInHand)
        {
            foodMinigameInput.carryBagInHand = true;
            Cursor.SetCursor(bolsaLlevarCursorTexture, hotSpotBolsaLlevar, CursorMode.Auto);
        }
    }

    // El boton llamara a una de las 3 funciones segun el tipo de comida que se trate (asociado al index del tipo de cada una)
    public void TakeCakeByInt(int index)
    {
        TakeFood(FoodCategory.bizcocho, (CakeType)index);
    }

    public void TakeCookieByInt(int index)
    {
        TakeFood(FoodCategory.galleta, (CookieType)index);
    }

    public void TakeMufflinByInt(int index)
    {
        TakeFood(FoodCategory.mufflin, (MufflinType)index);
    }

    // Gestionar coger la comida del estante
    public void TakeFood(FoodCategory category, object type)
    {
        if (foodMinigameInput.foodInHand || foodMinigameInput.platoInHand)
            return;

        foodMinigameInput.foodInHand = true;
        foodMinigameInput.foodCategoryInHand = category;
        foodMinigameInput.foodTypeInHand = (int)type;
        foodMinigameInput.ActualizarBotonCogerComida();

        Texture2D cursor = foodManager.GetFoodCursor(category, type);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        Debug.Log($"Comina en mano: {category}{type}");

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 3)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }

    // Gestionar cursor comida
    public void UpdateCursorFood(bool dejandoComida, FoodCategory category, object type)
    {
        Texture2D cursor = foodManager.GetFoodCursor(category, type);

        if (dejandoComida)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
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

    // Gestionar cursor taza con leche
    public void UpdateCursorTazaMilk(bool dejandoTazaLeche)
    {
        if (miniGameInput.TengoOtroObjetoEnLaMano())
            return;

        if (dejandoTazaLeche)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(tazaLecheCursorTexture, hotSpotTazaLeche, CursorMode.Auto);
        }
    }

    // Gestionar cursor plato
    public void UpdateCursorPlato(bool dejandoPlato)
    {
        if (miniGameInput.TengoOtroObjetoEnLaMano())
            return;

        if (dejandoPlato)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(platoCursorTexture, hotSpotPlato, CursorMode.Auto);
        }
    }

    // Gestionar cursor bolsa para llevar
    public void UpdateCursorCarryBag(bool dejandoBolsa)
    {
        if (miniGameInput.TengoOtroObjetoEnLaMano())
            return;

        if (dejandoBolsa)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(bolsaLlevarCursorTexture, hotSpotBolsaLlevar, CursorMode.Auto);
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

    public void ChangeHieloSpoon()
    {
        if (miniGameInput.iceInHand && miniGameInput.countIce > 0)
        {
            Cursor.SetCursor(hieloCucharaVaciaCursorTexture, hotSpotHieloCuchara, CursorMode.Auto);
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

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
    }
}
