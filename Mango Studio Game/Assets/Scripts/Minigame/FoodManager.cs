using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [Header("GameObjects comida")]
    private Dictionary<CakeType, GameObject> cakes = new Dictionary<CakeType, GameObject>();
    private Dictionary<CookieType, GameObject> cookies = new Dictionary<CookieType, GameObject>();
    private Dictionary<MufflinType, GameObject> mufflins = new Dictionary<MufflinType, GameObject>();

    public GameObject BZanahoria, BMantequilla, BChocolate, BRedVelvet;
    public GameObject GChocolate, GChocolateB, GMantequilla;
    public GameObject MArandano, MCereza, MPistacho, MDulceLeche;

    [Header("Cursores comida")]
    private Dictionary<CakeType, Texture2D> cakeCursors = new Dictionary<CakeType, Texture2D>();
    private Dictionary<CookieType, Texture2D> cookiesCursors = new Dictionary<CookieType, Texture2D>();
    private Dictionary<MufflinType, Texture2D> mufflinsCursors = new Dictionary<MufflinType, Texture2D>();

    public Texture2D BZanahoriaCursor, BMantequillaCursor, BChocolateCursor, BRedVelvetCursor;
    public Texture2D GChocolateCursor, GChocolateBCursor, GMantequillaCursor;
    public Texture2D MArandanoCursor, MCerezaCursor, MPistachoCursor, MDulceLecheCursor;


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
}
