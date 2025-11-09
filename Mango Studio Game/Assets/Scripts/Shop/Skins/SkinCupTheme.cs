using UnityEngine;

[System.Serializable]
public class SkinCupTheme
{
    public string skinName;
    public bool isUnlocked;

    [Header("Sprites tazas")]
    public Sprite tazaEspresso;
    public Sprite tazaAmericanoLungo;
    public Sprite tazaMocaIrishLatteCapucino;
    public Sprite tazaMachiatoBombon;
    public Sprite tazaVienes;
    public Sprite tazaFrappe;

    [Header("Sprites Tazas + Plato")]
    public Sprite tazaEspressoP;
    public Sprite tazaAmericanoLungoP;
    public Sprite tazaMocaIrishLatteCapucinoP;
    public Sprite tazaMachiatoBombonP;
    public Sprite tazaVienesP;
    public Sprite tazaFrappeP;

    [Header("Sprites Extras")]
    public Sprite tazaSinCafe;
    public Sprite tazaSinCafeP;
}
