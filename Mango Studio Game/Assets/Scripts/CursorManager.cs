using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

// Clase encargada de gestionar el cambio de cursor con las comidas
public class CursorManager : MonoBehaviour
{
    [Header("Textura y hotspot cursores")]
    [SerializeField] Texture2D defaultCursorTexture;
    [SerializeField] Vector2 hotSpotDefault = Vector2.zero; //el punto que hace click en si del cursor (ahora mismo arriba izquierda)

    [Header("Referencias")]
    public FoodManager foodManager;
    public TutorialManager tutorialManager;
    public FoodMinigameInput foodMinigameInput;

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
        TakeFood(FoodCategory.cupcake, (CupcakeType)index);
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

        if (category == FoodCategory.cupcake || category == FoodCategory.galleta)
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
}
