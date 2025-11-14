using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

// Clase encargada del stock de galletas y mufflins
[System.Serializable]
public class FoodObjects
{
    public FoodCategory category;
    public int type; // Indice del tipo dentro de la categoría
    public GameObject[] foodInstances; // GameObjects visibles 
    [HideInInspector] public int currentIndex = 0; // Siguiente disponible

    public bool IsDepleted()
    {
        return currentIndex >= foodInstances.Length;
    }

    // Reiniciar el stock visual
    public void Reset()
    {
        foreach (var obj in foodInstances)
            obj.SetActive(true);
        currentIndex = 0;
    }
}

// Clase encargada de gestionar la comida y sus cursores
public class FoodManager : MonoBehaviour
{
    #region Variables
    // Diccionarios gameobjects comida
    private Dictionary<CakeType, GameObject> cakes = new();
    private Dictionary<CookieType, GameObject> cookies = new();
    private Dictionary<MufflinType, GameObject> mufflins = new();

    // Diccionarios cursores comida
    private Dictionary<CakeType, Texture2D> cakeCursors = new();
    private Dictionary<CookieType, Texture2D> cookiesCursors = new();
    private Dictionary<MufflinType, Texture2D> mufflinsCursors = new();

    [Header("GameObjects comida")]
    public GameObject BZanahoria;
    public GameObject BMantequilla;
    public GameObject BChocolate;
    public GameObject BRedVelvet;
    public GameObject GChocolate;
    public GameObject GChocolateB;
    public GameObject GMantequilla;
    public GameObject MArandano;
    public GameObject MCereza;
    public GameObject MPistacho;
    public GameObject MDulceLeche;

    [Header("Cursores comida")]
    public Texture2D BZanahoriaCursor;
    public Texture2D BMantequillaCursor;
    public Texture2D BChocolateCursor;
    public Texture2D BRedVelvetCursor;
    public Texture2D GChocolateCursor;
    public Texture2D GChocolateBCursor;
    public Texture2D GMantequillaCursor;
    public Texture2D MArandanoCursor;
    public Texture2D MCerezaCursor;
    public Texture2D MPistachoCursor;
    public Texture2D MDulceLecheCursor;

    [Header("Stocks de galletas y mufflins")]
    public FoodObjects[] foodStocks;

    [Header("Stocks de bizcochos")]
    public CakeSpriteStock[] cakeSpriteStocks;
    #endregion

    private void Awake()
    {
        // GameObjects
        cakes.Add(CakeType.chocolate, BChocolate);
        cakes.Add(CakeType.mantequilla, BMantequilla);
        cakes.Add(CakeType.RedVelvet, BRedVelvet);
        cakes.Add(CakeType.zanahoria, BZanahoria);

        cookies.Add(CookieType.chocolate, GChocolate);
        cookies.Add(CookieType.blanco, GChocolateB);
        cookies.Add(CookieType.mantequilla, GMantequilla);

        mufflins.Add(MufflinType.cereza, MCereza);
        mufflins.Add(MufflinType.pistacho, MPistacho);
        mufflins.Add(MufflinType.arandanos, MArandano);
        mufflins.Add(MufflinType.dulceLeche, MDulceLeche);

        // Cursores
        cakeCursors.Add(CakeType.chocolate, BChocolateCursor);
        cakeCursors.Add(CakeType.mantequilla, BMantequillaCursor);
        cakeCursors.Add(CakeType.RedVelvet, BRedVelvetCursor);
        cakeCursors.Add(CakeType.zanahoria, BZanahoriaCursor);

        cookiesCursors.Add(CookieType.chocolate, GChocolateCursor);
        cookiesCursors.Add(CookieType.blanco, GChocolateBCursor);
        cookiesCursors.Add(CookieType.mantequilla, GMantequillaCursor);

        mufflinsCursors.Add(MufflinType.cereza, MCerezaCursor);
        mufflinsCursors.Add(MufflinType.pistacho, MPistachoCursor);
        mufflinsCursors.Add(MufflinType.arandanos, MArandanoCursor);
        mufflinsCursors.Add(MufflinType.dulceLeche, MDulceLecheCursor);
    }

    // Coger gameobjects
    public GameObject GetFoodObject(FoodCategory category, object type)
    {
        switch (category)
        {
            case FoodCategory.bizcocho:
                return cakes.TryGetValue((CakeType)type, out GameObject cake) ? cake : null;

            case FoodCategory.galleta:
                return cookies.TryGetValue((CookieType)type, out GameObject cookie) ? cookie : null;

            case FoodCategory.mufflin:
                return mufflins.TryGetValue((MufflinType)type, out GameObject mufflin) ? mufflin : null;

            default:
                return null;
        }
    }

    // Asociar cursores
    public Texture2D GetFoodCursor(FoodCategory category, object type)
    {
        switch (category)
        {
            case FoodCategory.bizcocho:
                if (cakeCursors.TryGetValue((CakeType)type, out Texture2D cakeCursor))
                    return cakeCursor;
                break;

            case FoodCategory.galleta:
                if (cookiesCursors.TryGetValue((CookieType)type, out Texture2D cookieCursor))
                    return cookieCursor;
                break;

            case FoodCategory.mufflin:
                if (mufflinsCursors.TryGetValue((MufflinType)type, out Texture2D mufflinCursor))
                    return mufflinCursor;
                break;
        }
        return null;
    }

    // Restar stock galletas/mufflins
    public bool TakeFood(FoodCategory category, int type)
    {
        FoodObjects food = System.Array.Find(foodStocks, f => f.category == category && f.type == type);
        if (food == null) return false;

        if (food.currentIndex >= food.foodInstances.Length)
            return false; // ya no quedan objetos disponibles

        // Ocultar el siguiente objeto disponible
        food.foodInstances[food.currentIndex].SetActive(false);
        food.currentIndex++;

        return true;
    }

    // Resetear stock galletas/mufflins
    public void ResetDepletedFood()
    {
        foreach (var stock in foodStocks)
        {
            if (stock.IsDepleted())
            {
                stock.Reset(); // Solo reinicia los que estaban completamente agotados
            }
        }
    }

    // Restar stock bizcochos
    public bool TakeCakeSlice(CakeType type)
    {
        CakeSpriteStock stock =
            System.Array.Find(cakeSpriteStocks, f => f.cakeType == type);

        if (stock == null) return false;

        return stock.ConsumeStage();
    }

    // Resetear stock bizcochos
    public void ResetDepletedCakes()
    {
        foreach (var stock in cakeSpriteStocks)
        {
            if (stock.IsDepleted())
                stock.Reset();
        }
    }
}
