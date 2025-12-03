using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [Header("Texturas cursores cafe")]
    [SerializeField] Texture2D defaultCursorTexture;
    [SerializeField] Texture2D cucharaCursorTexture;
    [SerializeField] Texture2D cremaCursorTexture;
    [SerializeField] Texture2D hieloCucharaCursorTexture;
    [SerializeField] Texture2D hieloCucharaVaciaCursorTexture;

    [Header("HotSpots cursores cafe")]
    [SerializeField] Vector2 hotSpotDefault = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotHieloCuchara = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)
    [SerializeField] Vector2 hotSpotCrema = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)

    [SerializeField] MinigameInput miniGameInput; //para poder usar referencias a los vasos y tazas       
    public FoodManager foodManager;
    public TutorialManager tutorialManager;
    public FoodMinigameInput foodMinigameInput;
    public CoffeeContainerManager coffeeContainerManager;

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

        MiniGameSoundManager.instance.PlayTakeFood();
        foodMinigameInput.foodInHand = true;
        foodMinigameInput.foodCategoryInHand = category;
        foodMinigameInput.foodTypeInHand = (int)type;
        foodMinigameInput.ActualizarBotonCogerComida();

        if (category == FoodCategory.mufflin || category == FoodCategory.galleta)
            foodManager.TakeFood(category, (int)type);

        if (category == FoodCategory.bizcocho)
            foodManager.TakeCakeSlice((CakeType)type);

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

    public void TakeCuchara()
    {
        if (miniGameInput.cucharaInHand && !miniGameInput.waterInHand && !coffeeContainerManager.coverInHand && !miniGameInput.iceInHand && !miniGameInput.milkInHand 
            && !miniGameInput.condensedMilkInHand && !miniGameInput.creamInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand)
        {
            Cursor.SetCursor(cucharaCursorTexture, hotSpotCuchara, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano())
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeCream()
    {
        if (miniGameInput.creamInHand && !miniGameInput.filtroInHand && !miniGameInput.waterInHand && !coffeeContainerManager.coverInHand && !miniGameInput.cucharaInHand && !miniGameInput.iceInHand
            && !miniGameInput.milkInHand && !miniGameInput.condensedMilkInHand && !miniGameInput.chocolateInHand && !miniGameInput.whiskeyInHand && !miniGameInput.tazaMilkInHand)
        {
            Cursor.SetCursor(cremaCursorTexture, hotSpotCrema, CursorMode.Auto);
        }

        if (!miniGameInput.TengoOtroObjetoEnLaMano() && !miniGameInput.tazaMilkInHand && !miniGameInput.filtroInHand)
        {
            Cursor.SetCursor(defaultCursorTexture, hotSpotDefault, CursorMode.Auto);
        }
    }

    public void TakeHielo()
    { 
        if(miniGameInput.iceInHand && !miniGameInput.cucharaInHand && !coffeeContainerManager.coverInHand && !miniGameInput.waterInHand && !miniGameInput.milkInHand 
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
}
