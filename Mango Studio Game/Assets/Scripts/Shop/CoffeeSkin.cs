using UnityEngine;

[CreateAssetMenu(fileName = "NuevaSkin", menuName = "Cafeteria/Skin")]
public class CoffeeSkin : ScriptableObject
{
    [Header("Sprites tazas y vasos base")]
    public Sprite vasoConTapa;
    public Sprite vasoSinTapa;
    public Sprite tapaVaso;
    public Sprite tazaSinCafe;
    public Sprite tazaSinCafeP;
    public Sprite platoTaza;

    [Header("Sprites mecánicas")]
    public Sprite tazaNWater;
    public Sprite tazaNMilk;
    public Sprite tazaNWhiskey;
    public Sprite tazaNChocolate;

    [Header("Sprites mecánicas + plato")]
    public Sprite tazaNWaterP;
    public Sprite tazaNMilkP;
    public Sprite tazaNWhiskeyP;
    public Sprite tazaNChocolateP;

    [Header("Sprites cafés tazas")]
    public Sprite tazaNEspresso;
    public Sprite tazaNAmericanoLungo;
    public Sprite tazaNMocaIrishLatte;
    public Sprite tazaNCappuccino;
    public Sprite tazaNMachiatoBombon;
    public Sprite tazaNVienes;
    public Sprite tazaNFrappe;

    [Header("Sprites cafés tazas + dibujo")]
    public Sprite tazaNMocaIrishLatteD;
    public Sprite tazaNMachiatoBombonD;
    public Sprite tazaNVienesD;

    [Header("Sprites cafés tazas + plato")]
    public Sprite tazaNEspressoP;
    public Sprite tazaNAmericanoLungoP;
    public Sprite tazaNMocaIrishLatteP;
    public Sprite tazaNCappuccinoP;
    public Sprite tazaNMachiatoBombonP;
    public Sprite tazaNVienesP;
    public Sprite tazaNFrappeP;

    [Header("Sprites cafés tazas + dibujo + plato")]
    public Sprite tazaNMocaIrishLatteDP;
    public Sprite tazaNMachiatoBombonDP;
    public Sprite tazaNVienesDP;

    [Header("Sprites cafés vasos")]
    public Sprite vasoNEspresso;
    public Sprite vasoNAmericanoLungo;
    public Sprite vasoNMocaIrishLatte;
    public Sprite vasoNCappuccino;
    public Sprite vasoNMachiatoBombon;
    public Sprite vasoNVienes;
    public Sprite vasoNFrappe;

    [Header("Sprites cafés vasos + dibujo")]
    public Sprite vasoNMocaIrishLatteD;
    public Sprite vasoNMachiatoBombonD;
    public Sprite vasoNVienesD;
}
